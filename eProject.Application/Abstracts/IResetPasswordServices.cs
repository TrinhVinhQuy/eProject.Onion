using eProject.Application.DTOs.ResetPassword;
using eProject.Domain.Entities;

namespace eProject.Application.Abstracts
{
    public interface IResetPasswordServices
    {
        Task UpdateAsync(ResetPasswordDTO model);
        Task<ResetPassword> InsertAsync(ResetPassword entity);
        Task<ResetPasswordDTO> GetByCodeAsync(string code);
    }
}
