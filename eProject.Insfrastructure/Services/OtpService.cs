using Microsoft.AspNetCore.Http;
using System.Net.Mail;

namespace eProject.Insfrastructure.Services
{
    public class OtpService
    {

        public string GenerateOtp(string email)
        {
            Random random = new Random();
            string otp = Convert.ToString(random.Next(100000, 1000000));
            try
            {
                // Gửi OTP qua email
                SendOtpEmail(email, otp);
                return otp;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        

        private void SendOtpEmail(string email, string otp)
        {
            // Mật khẩu ứng dụng OtpEmail : kemz hkfu jode ctfp

            //MailMessage message = new MailMessage("txvq0101@gmail.com", email, "Mã xác nhận Otp", randomNumber);
            MailMessage message = new MailMessage();
            message.From = new MailAddress("txvq0101@gmail.com");
            message.To.Add(email);
            message.Subject = "Your OTP";

            string htmlBody = "<h1>Your OTP Code</h1><p>Your OTP code is: " + otp + "</p>";
            message.Body = htmlBody;
            message.IsBodyHtml = true;

            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);

            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("txvq0101@gmail.com", "kemz hkfu jode ctfp");

            client.Send(message);
        }
    }
}
