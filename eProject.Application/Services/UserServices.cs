using AutoMapper;
using eProject.Application.Abstracts;
using eProject.Application.DTOs.User;
using eProject.Domain.Abstracts;
using eProject.Domain.Entities;

namespace eProject.Application.Services
{
    public class UserServices : IUserServices
    {
        private readonly IRepositoryBase<User> _userRepository;
        private readonly IRepositoryBase<Role> _roleRepository;
        private IMapper _mapper;
        public UserServices(IRepositoryBase<User> userRepository, IRepositoryBase<Role> roleRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserLoginDTO>> GetAllUserLoginAsync()
        {
            var _users = await _userRepository.GetAllAsync();
            var _usersActive = _mapper.Map<IEnumerable<UserLoginDTO>>(_users);
            var _roles = await _roleRepository.GetAllAsync();
            foreach (var user in _usersActive)
            {
                user.RoleName = _roles.First(x => x.Id == user.RoleId).Name;
            }
            return _usersActive;
        }

        public async Task<UserDetailDTO> GetUserDetailAsync(string email)
        {
            var users = await _userRepository.GetAllAsync();
            var user = users.FirstOrDefault(x => x.Email == email);

            if (user != null)
            {
                return _mapper.Map<UserDetailDTO>(user);
            }
            return null;
        }

        public async Task<User> InsertAsync(User user)
        {
            return await _userRepository.InsertAsync(user);
        }

        public async Task UpdateAsync(UserDetailDTO model)
        {
            var user = await _userRepository.GetByIdAsync(model.Id);
            _mapper.Map(model, user);
            await _userRepository.UpdateAsync(user);
        }
    }
}
