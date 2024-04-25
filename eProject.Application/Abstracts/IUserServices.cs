using eProject.Application.DTOs.User;
using eProject.Domain.Entities;

namespace eProject.Application.Abstracts
{
    public interface IUserServices
    {
        Task<IEnumerable<UserLoginDTO>> GetAllUserLoginAsync();
        Task<UserDetailDTO> GetUserDetailAsync(string email);
        Task UpdateAsync(UserDetailDTO model);
        Task<User> InsertAsync(User user);
    }
}
