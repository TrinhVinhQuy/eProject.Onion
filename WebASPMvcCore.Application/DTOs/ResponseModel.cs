using WebASPMvcCore.Domain.Enums;

namespace WebASPMvcCore.Application.DTOs
{
    public class ResponseModel
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
        public ActionType Action { get; set; } = ActionType.Get;
    }
}
