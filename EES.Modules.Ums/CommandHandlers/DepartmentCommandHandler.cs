using EES.Infrastructure.Bus;
using EES.Infrastructure.Commons;
using EES.Infrastructure.DataBase.Repository;
using EES.Infrastructure.Enums;
using EES.Infrastructure.Service;
using EES.Modules.Ums.Commands.Department;
using EES.Modules.Ums.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EES.Modules.Ums.CommandHandlers
{
    /// <summary>
    /// 部门管理
    /// </summary>
    public class DepartmentCommandHandler : ServerBase,
                                          IRequestHandler<CreateDepartmentCmd, CommandResponse>,
                                          IRequestHandler<ModifyDepartmentCmd, CommandResponse>,
                                          IRequestHandler<DeleteDepartmentCmd, CommandResponse>
    {
        private readonly IRepository<Department> _departmentRepository;

        private readonly IRepository<User> _userRepository;

        private ILogger<DepartmentCommandHandler> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="departmentRepository"></param>
        /// <param name="logger"></param>
        /// <param name="userRepository"></param>
        /// <param name="mediatorHandler"></param>
        public DepartmentCommandHandler(IRepository<Department> departmentRepository, ILogger<DepartmentCommandHandler> logger, IRepository<User> userRepository, IMediatorHandler mediatorHandler) : base(mediatorHandler)
        {
            _departmentRepository = departmentRepository;
            _logger = logger;
            _userRepository = userRepository;
        }

        /// <summary>
        /// 新建部门
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<CommandResponse> Handle(CreateDepartmentCmd request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Command：{command}", request.SerializeCommand());
            var isParentExist = await CheckParentDepartmentExistAsync(request.ParentId);
        
            if (!isParentExist)
            {
                return CommandResponse.Fail(BusinessError.上级部门不存在);
            }

            var isCodeExist = await CheckCodeExistAsync(request.Code);

            if (isCodeExist)
            {
                _logger.LogInformation("Command：{command},error:{error}", nameof(CreateDepartmentCmd),"部门编码重复");

                return CommandResponse.Fail(BusinessError.部门编码重复);
            }

            var department = new Department(request.ParentId, request.SortNumber, request.Code, request.Name, request.Description, request.ImageUrl, Accessor.Id, Accessor.Name);

            _departmentRepository.Add(department);

            return CommandResponse.Success();
        }

        /// <summary>
        /// 修改部门信息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<CommandResponse> Handle(ModifyDepartmentCmd request, CancellationToken cancellationToken)
        {
            var department = await _departmentRepository.Query().FirstOrDefaultAsync(t => t.Id == request.DepartmentId, cancellationToken: cancellationToken);

            if (department is null)
            {
                return CommandResponse.Fail(BusinessError.该部门不存在);
            }

            var isParentExist = await CheckParentDepartmentExistAsync(request.ParentId);

            if (!isParentExist)
            {
                return CommandResponse.Fail(BusinessError.上级部门不存在);
            }

            var isCodeExist = await CheckCodeExistAsync(request.Code,department.Id);

            if (isCodeExist)
            {
                return CommandResponse.Fail(BusinessError.部门编码重复);
            }

            department.Modify(request.ParentId, request.SortNumber, request.Code, request.Name, request.Description, request.ImageUrl, Accessor.Id, Accessor.Name);

            return CommandResponse.Success();
        }


        /// <summary>
        /// 删除部门
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<CommandResponse> Handle(DeleteDepartmentCmd request, CancellationToken cancellationToken)
        {
            /*
              1.部门下有子部门的，不能删，先手动把子部门删了。
              2.部门下有用户挂靠的，不能删。
             */
            var department = await _departmentRepository.Query().FirstOrDefaultAsync(t => t.Id == request.DepartmentId, cancellationToken: cancellationToken);

            if (department is null)
            {
                return CommandResponse.Fail(BusinessError.该部门不存在);
            }

            var isSonExist = await _departmentRepository.Query().AnyAsync(t => t.ParentId == department.Id, cancellationToken: cancellationToken);

            if (isSonExist)
            {
                return CommandResponse.Fail(BusinessError.请先删除部门下的子级部门);
            }

            var isUserExist = await _userRepository.Query().AnyAsync(t => t.DepartmenId == department.Id, cancellationToken: cancellationToken);

            if (isUserExist)
            {
                return CommandResponse.Fail(BusinessError.请先删除部门下的用户);
            }

            _departmentRepository.Delete(department);

            return CommandResponse.Success();
        }



        /// <summary>
        /// 检查上级部门是否存在
        /// </summary>
        /// <param name="parentDepartmentId"></param>
        /// <returns></returns>
        private async Task<bool> CheckParentDepartmentExistAsync(long parentDepartmentId)
        {
            return parentDepartmentId == 0 || await _departmentRepository.Query().AnyAsync(t => t.Id == parentDepartmentId);
        }

        /// <summary>
        /// 检查编码是否存在
        /// </summary>
        /// <param name="code">编码</param>
        /// <param name="excludedDepartmentId">被排除，不参与检查的部门的id</param>
        /// <returns></returns>
        private async Task<bool> CheckCodeExistAsync(string code, long excludedDepartmentId = 0)
        {
            return string.IsNullOrEmpty(code) || await _departmentRepository.Query().AnyAsync(t => t.Code == code && t.Id != excludedDepartmentId);
        }

       
    }
}
