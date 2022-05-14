using System.Configuration;
using System.IO;
using System.Net.Mail;
using Model;

public class MailService : IMailService
{
  public void SendCodeConfirmation(User user, string code)
  {

    MailMessage mailMessage = new MailMessage();

    mailMessage.From = new MailAddress("isacrodriguesdev@gmail.com");
    mailMessage.To.Add(new MailAddress(user.Email));
    mailMessage.IsBodyHtml = true;

    string body = String.Empty;
    string pathHtml = Path.Combine(Environment.CurrentDirectory, "Src", "Services", "Smtp", "Templates");
    using (StreamReader streamReader = new StreamReader(pathHtml + "/EmailCodeConfirmation.html"))
    {
      body = streamReader.ReadToEnd();
    }

    body = body.Replace("{name}", user.Name);
    body = body.Replace("{code}", code);

    mailMessage.Subject = "Código de confirmação NODE14";
    mailMessage.Body = body;

    using (var smtp = new SmtpClient())
    {
      smtp.Host = "smtp.gmail.com";
      smtp.Port = 587;
      smtp.EnableSsl = true;
      smtp.Credentials = new System.Net.NetworkCredential(
        "isacrodriguesdev@gmail.com",
        "79LyqPJf4WFbMmsi$m$w7gMtVjc*nARvmJL$2TnCwA%3yyFSMarh"
      );

      smtp.Send(mailMessage);
    }
  }
}
