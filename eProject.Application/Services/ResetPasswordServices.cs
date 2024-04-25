using AutoMapper;
using eProject.Application.Abstracts;
using eProject.Application.DTOs.ResetPassword;
using eProject.Domain.Abstracts;
using eProject.Domain.Entities;

namespace eProject.Application.Services
{
    public class ResetPasswordServices : IResetPasswordServices
    {
        private readonly IRepositoryBase<ResetPassword> _resetRepository;
        private readonly IMapper _mapper;
        public ResetPasswordServices(IRepositoryBase<ResetPassword> resetRepository, 
            IMapper mapper)
        {
            _resetRepository = resetRepository;
            _mapper = mapper;
        }

        public async Task<bool> CheckDuplicatePass(string pass)
        {
            var _pass = await _resetRepository.GetAllAsync();
            if (_pass.Where(x=>x.Password == pass).Count()>1)
            {
                return true;
            }
            return false;
        }

        public async Task<ResetPasswordDTO> GetByCodeAsync(Guid code)
        {
            var resets = await _resetRepository.GetAllAsync();
            var reset = _mapper.Map<ResetPasswordDTO>(resets.FirstOrDefault(x=>x.Code == code));
            return reset;
        }

        public async Task<ResetPassword> InsertAsync(ResetPassword entity)
        {
            return await _resetRepository.InsertAsync(entity);
        }

        public async Task UpdateAsync(ResetPassword model)
        {
            var reset = await _resetRepository.GetByIdAsync(model.Id);
            reset.Password = model.Password;
            await _resetRepository.UpdateAsync(reset);
        }
    }
}
