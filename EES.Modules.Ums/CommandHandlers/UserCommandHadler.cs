using EES.Infrastructure.Bus;
using EES.Infrastructure.Commons;
using EES.Infrastructure.DataBase.Repository;
using EES.Infrastructure.Enums;
using EES.Infrastructure.Service;
using EES.Modules.Ums.Commands;
using EES.Modules.Ums.Commands.Role;
using EES.Modules.Ums.Commands.User;
using EES.Modules.Ums.Commands.UserRoles;
using EES.Modules.Ums.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EES.Modules.Ums.CommandHandlers
{
    /// <summary>
    /// 用户管理
    /// </summary>
    public class UserCommandHadler : ServerBase,
                                    IRequestHandler<CreateUserCmd, CommandResponse>,
                                    IRequestHandler<ModifyPasswordCmd, CommandResponse>,
                                    IRequestHandler<ModifyUserCmd, CommandResponse>,
                                    IRequestHandler<DeleteUserCmd, CommandResponse>,
                                    IRequestHandler<CheckUserIdsCmd, CommandResponse>,
                                    IRequestHandler<ResetPasswordCmd,CommandResponse>
    {
        private readonly IRepository<User> _userRepository;

        private readonly IRepository<Department> _departmentRepository;

        private readonly IRepository<Role> _roleRepository;

        private readonly ILogger<UserCommandHadler> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="userRepository"></param>
        /// <param name="logger"></param>
        /// <param name="departmentRepository"></param>
        /// <param name="roleRepository"></param>
        /// <param name="handler"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public UserCommandHadler(IRepository<User> userRepository, ILogger<UserCommandHadler> logger, IRepository<Department> departmentRepository, IRepository<Role> roleRepository, IMediatorHandler handler) : base(handler)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _departmentRepository = departmentRepository ?? throw new ArgumentNullException(nameof(departmentRepository));
            _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
        }

        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<CommandResponse> Handle(CreateUserCmd request, CancellationToken cancellationToken)
        {
            var isAccountDuplicate = await _userRepository.Query().AnyAsync(t => t.Account == request.Account, cancellationToken: cancellationToken);

            if (isAccountDuplicate)
            {
                return CommandResponse.Fail(BusinessError.该账号已被注册);
            }

            var checkRoleCmd = new CheckRoleIdsCmd() { RoleIds = request.RoleIds };

            var checkRoleRes = await SendCommandAsync(checkRoleCmd);

            if (checkRoleRes != null && checkRoleRes.Status)
            {
                var res = bool.Parse(checkRoleRes.Data.ToString());

                if (!res)
                {
                    return CommandResponse.Fail(BusinessError.角色数据错误);
                }
            }
            else
            {
                CommandResponse.Fail(BusinessError.服务器异常);
            }

            if (!await CheckDepartmentExist(request.DepartmenId))
            {
                return CommandResponse.Fail(BusinessError.该部门不存在);
            }

            var user = new User(request.Account, request.Password, request.Name, request.Mail, request.Phone,  request.Remark, request.ImageUrl,request.SignatureUrl , request.DepartmenId, Accessor.Id, Accessor.Name, request.Language);

            await _userRepository.AddAsync(user);

            _userRepository.SaveChanges();

            var cmd = new CreateUserRolesCmd(user.Id, request.RoleIds);

            return await SendCommandAsync(cmd); //添加用户-角色的绑定关系        
        }

        /// <summary>
        /// 检查部门是否存在
        /// </summary>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        private async Task<bool> CheckDepartmentExist(long departmentId)
        {
            return await _departmentRepository.Query().AnyAsync(t => t.Id == departmentId);
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<CommandResponse> Handle(ModifyPasswordCmd request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.Query().FirstOrDefaultAsync(t => t.Id == Accessor.Id, cancellationToken: cancellationToken);

            if (user == null)
            {
                return CommandResponse.Fail(BusinessError.用户不存在);
            }

            if (!user.IsAvailable())
            {
                return CommandResponse.Fail(BusinessError.用户账号已被禁用);
            }

            if (!user.VerifyPassword(request.OldPassword))
            {
                return CommandResponse.Fail(BusinessError.密码错误);
            }

            user.ModifyPassword(request.NewPassword);

            return CommandResponse.Success();
        }

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<CommandResponse> Handle(ModifyUserCmd request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.Query().FirstOrDefaultAsync(t => t.Id == request.UserId, cancellationToken: cancellationToken);

            if (user == null)
            {
                return CommandResponse.Fail(BusinessError.用户不存在);
            }

            if (!await CheckDepartmentExist(request.DepartmenId))
            {
                return CommandResponse.Fail(BusinessError.该部门不存在);
            }

            user.ModifyBaseInfo(request.Name, request.Mail, request.Phone,  request.Remark, request.ImageUrl, request.SignatureUrl,request.DepartmenId, request.Language, request.State, Accessor.Id, Accessor.Name);

            var cmd = new CreateUserRolesCmd(user.Id, request.RoleIds);

            return await SendCommandAsync(cmd);
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<CommandResponse> Handle(DeleteUserCmd request, CancellationToken cancellationToken)
        {
            await _userRepository.Query().Where(t => t.Id == request.UserId).ExecuteDeleteAsync(cancellationToken: cancellationToken);

            var cmd = new DeleteUserRolesCmd(request.UserId);

            return await SendCommandAsync(cmd);
        }

        /// <summary>
        /// 检查用户ID
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<CommandResponse> Handle(CheckUserIdsCmd request, CancellationToken cancellationToken)
        {
            if (request.UserIds == null || !request.UserIds.Any())
            {
                return CommandResponse.Success(data: false);
            }

            var allUserIds = await _userRepository.Query().Select(x => x.Id).ToListAsync(cancellationToken: cancellationToken);

            return CommandResponse.Success(data: request.UserIds.All(allUserIds.Contains));
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<CommandResponse> Handle(ResetPasswordCmd request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.Query().FirstOrDefaultAsync(t => t.Id == request.UserId, cancellationToken: cancellationToken);

            if (user == null)
            {
                return CommandResponse.Fail(BusinessError.用户不存在);
            }

            if (!user.IsAvailable())
            {
                return CommandResponse.Fail(BusinessError.用户账号已被禁用);
            }

            user.ResetPassword(request.Password, Accessor.Id, Accessor.Name);

            return CommandResponse.Success(user);
        }
    }
}
