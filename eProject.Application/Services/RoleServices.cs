using AutoMapper;
using eProject.Application.Abstracts;
using eProject.Domain.Abstracts;
using eProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eProject.Application.Services
{
    public class RoleServices : IRoleServices
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryBase<Role> _repositoryRole;
        public RoleServices(IMapper mapper,
                            IRepositoryBase<Role> repositoryRole)
        {
            _mapper = mapper;
            _repositoryRole = repositoryRole;
        }

        public async Task<List<Role>> GetAll()
        {
            var _role = await _repositoryRole.GetAllAsync();
            var result = _mapper.Map<List<Role>>(_role);
            return result;
        }
    }
}
