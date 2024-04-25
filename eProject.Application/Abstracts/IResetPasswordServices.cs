using eProject.Application.DTOs.ResetPassword;
using eProject.Domain.Entities;

namespace eProject.Application.Abstracts
{
    public interface IResetPasswordServices
    {
        Task UpdateAsync(ResetPassword model);
        Task<ResetPassword> InsertAsync(ResetPassword entity);
        Task<ResetPasswordDTO> GetByCodeAsync(Guid code);
        Task<bool> CheckDuplicatePass(string pass);
    }
}
