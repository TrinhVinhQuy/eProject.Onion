using WebASPMvcCore.Application.DTOs;

namespace WebASPMvcCore.Application.Abstracts
{
    public interface IAuthenticationService
    {
        Task<ResponseModel> CheckLogin(string username, string password, bool hasRemmeber);
    }
}
