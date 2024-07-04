using AutoMapper;
using EES.Infrastructure.Bus;
using EES.Infrastructure.Commons;
using EES.Infrastructure.Data;
using EES.Infrastructure.DataBase.Repository;
using EES.Infrastructure.Enums;
using EES.Infrastructure.Service;
using EES.Modules.Ums.Dto;
using EES.Modules.Ums.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Modules.Ums.Services
{
    /// <summary>
    /// 部门查询服务
    /// </summary>
    public class DepartmentService : ServerBase, IDepartmentService
    {
        private readonly IRepository<Department> _departmentRepository;

        private readonly ILogger<DepartmentService> _logger;

        private readonly IMapper _mapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="departmentRepository"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        /// <param name="mediatorHandler"></param>
        public DepartmentService(IRepository<Department> departmentRepository, ILogger<DepartmentService> logger, IMapper mapper, IMediatorHandler mediatorHandler) : base(mediatorHandler)
        {
            _departmentRepository = departmentRepository;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// 查询部门详细信息
        /// </summary>
        /// <param name="departmentId">组织部门id</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<QueryResponse<DepartmentDto>> GetDepartmentAsync(long departmentId)
        {
            var department = await _departmentRepository.NoTrackingQuery().FirstOrDefaultAsync(t => t.Id == departmentId);

            if (department == null)
            {
                return QueryResponse<DepartmentDto>.Fail(BusinessError.该部门不存在);
            }

            var dto = _mapper.Map<DepartmentDto>(department);

            if (department.ParentId != 0)
            {
                var parent = await _departmentRepository.NoTrackingQuery().FirstOrDefaultAsync(t => t.Id == department.ParentId);

                dto.ParentName = parent.Name;
            }

            return QueryResponse<DepartmentDto>.Success(dto);
        }

        /// <summary>
        /// 以树状结构返回指定组织部门以及其全部子部门信息
        /// </summary>
        /// <param name="rootId">根节点(部门)id，获取全部则写0</param>
        /// <returns></returns>
        public async Task<QueryResponse<IEnumerable<TreeItem<DepartmentDto>>>> GetDepartmentTreeAsync(long rootId = 0)
        {
            var departments = await _departmentRepository.NoTrackingQuery().ToListAsync();

            var treeItems = _mapper.Map<IEnumerable<DepartmentDto>>(departments).GenerateTree(x => x.Id, x => x.ParentId, rootId);

            return QueryResponse<IEnumerable<TreeItem<DepartmentDto>>>.Success(treeItems);
        }
    }
}
