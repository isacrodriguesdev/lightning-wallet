using Microsoft.EntityFrameworkCore;
using EFCore.NamingConventions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Controller;
using Factory;
using System.Text;
using Microsoft.AspNetCore.Authentication.Certificate;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;

public static class App
{
  private static WebApplication _app;
  private static WebApplicationBuilder _builder;

  public static WebApplication CreateApp(string[] args)
  {
    var builder = WebApplication.CreateBuilder(args);
    _builder = builder;

    AddAuthentication();
    _builder.Services.AddAuthorization();
    _builder.Services.AddGrpc();

    _app = builder.Build();

    // _app.UseHsts();
    // _app.UseHttpsRedirection();
    _app.UseAuthentication();
    _app.UseAuthorization();

    _app.UseExceptionHandler(c => c.Run(async context =>
    {
      if (context.Features.Get<IExceptionHandlerFeature>()?.Error is not null)
      {
        var response = new { error = "Han error has ocorred" };
        context.Response.StatusCode = 400;

        await context.Response.WriteAsJsonAsync(response);
        return;
      }
    }));

    AddRoutes();
    return _app;
  }

  private static void AddAuthentication()
  {

    var getValueConfig = GetValueConfigFactory.Creator();

    _builder.Services.AddAuthentication(x =>
    {
      x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
      x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(x =>
    {
      x.RequireHttpsMetadata = false;
      x.SaveToken = true;
      x.TokenValidationParameters = new TokenValidationParameters
      {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = false,
        ValidateAudience = false,
        IssuerSigningKey = new SymmetricSecurityKey(
          Encoding.ASCII.GetBytes(getValueConfig.Execute<string>("SecurityKey"))
        ),
      };
    });
  }

  public static WebApplicationBuilder GetBuilder()
  {
    return _builder;
  }

  public static WebApplication GetApp()
  {
    return _app;
  }

  private static void AddRoutes()
  {
    _app.MapGrpcService<WalletServiceGrpc>();

    _app.MapPost("/user", CreateUserRouteAdapter.Router).AllowAnonymous();
    _app.MapPost("/login", LoginUserRouteAdapter.Router).AllowAnonymous();
    _app.MapPost("/confirmation_code", ConfirmationCodeRouteAdapter.Router).AllowAnonymous();

    _app.MapPost("/create_invoice", CreateInvoiceRouteAdapter.Router).RequireAuthorization();
    _app.MapPost("/send_payment", SendPaymentRouteAdapter.Router).RequireAuthorization();
    _app.MapGet("/wallet_info", GetWalletInfoRouteAdapter.Router).RequireAuthorization();
    _app.MapGet("/new_address", DepositRouteAdapter.Router).RequireAuthorization();
  }

  public static void Run()
  {
    _app.Run();
  }
}