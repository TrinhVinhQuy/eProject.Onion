using eProject.Application.DTOs.ViewModel;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace eProject.Insfrastructure.Services
{
    public class EmailService
    {
        public void EmailSend(SendEmailModel model)
        {
            // Mật khẩu ứng dụng OtpEmail : kemz hkfu jode ctfp

            MailMessage message = new MailMessage();
            message.From = new MailAddress("txvq0101@gmail.com");
            message.To.Add(model.ToEmail);
            message.Subject = model.Subject;
            //if (model.CCEmail != null)
            //{
            //    message.CC.Add(model.CCEmail);
            //}
            message.Body = model.HtmlBody;
            message.IsBodyHtml = true;

            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);

            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("txvq0101@gmail.com", "kemz hkfu jode ctfp");

            client.Send(message);
        }
        public bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Kiểm tra định dạng email bằng regex
                var regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
                return regex.IsMatch(email);
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
    }
}
