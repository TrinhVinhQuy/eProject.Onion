namespace eProject.Application.DTOs.ViewModel
{
    public class SendEmailModel
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string HtmlBody { get; set; }
        //public string? CCEmail { get; set; }
        //public string FromEmail { get; set; }
        //public string CodeEmail {  get; set; }
    }
}
