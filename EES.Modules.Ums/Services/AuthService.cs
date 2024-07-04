using AutoMapper;
using EES.Infrastructure.Auth;
using EES.Infrastructure.Bus;
using EES.Infrastructure.Cache;
using EES.Infrastructure.Commons;
using EES.Infrastructure.DataBase.Repository;
using EES.Infrastructure.Enums;
using EES.Infrastructure.Jwt;
using EES.Infrastructure.Service;
using EES.Modules.Ums.Dto;
using EES.Modules.Ums.Entities;
using FreeRedis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Modules.Ums.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class AuthService : ServerBase, IAuthService
    {
        private readonly IRepository<User> _userRepository;

        private readonly IRepository<Role> _roleRepository;

        private readonly IRepository<Menu> _menuRepository;

        private readonly IRepository<Entities.Data> _dataRepository;

        private readonly IRepository<Function> _functionRepository;

        private readonly ILogger<AuthService> _logger;

        private readonly RedisClient _redisClient;

        private readonly IMapper _mapper;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="userRepository"></param>
        /// <param name="roleRepository"></param>
        /// <param name="menuRepository"></param>
        /// <param name="dataRepository"></param>
        /// <param name="functionRepository"></param>
        /// <param name="logger"></param>
        /// <param name="redisClient"></param>
        /// <param name="mapper"></param>
        /// <param name="mediatorHandler"></param>
        public AuthService(IRepository<User> userRepository, IRepository<Role> roleRepository, IRepository<Menu> menuRepository, IRepository<Entities.Data> dataRepository, IRepository<Function> functionRepository, ILogger<AuthService> logger, RedisClient redisClient, IMapper mapper, IMediatorHandler mediatorHandler) : base(mediatorHandler)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _menuRepository = menuRepository;
            _dataRepository = dataRepository;
            _functionRepository = functionRepository;
            _logger = logger;
            _redisClient = redisClient;
            _mapper = mapper;
        }


        /// <summary>
        /// 获取权限
        /// </summary>
        /// <returns></returns>
        public async Task<QueryResponse<AuthInfo>> GetAuthAsync()
        {

            string cacheKey = CacheKeyProvider.GetAuthCacheKey(Accessor.Id.ToString());

            var dto = GetAuthDtoCache(cacheKey);

            if (dto != null)
            {
                return QueryResponse<AuthInfo>.Success(dto);
            }

            var user = await _userRepository.NoTrackingQuery()
                      .Include(x => x.Roles)
                      .Include(x => x.Department)
                      .FirstOrDefaultAsync(t => t.Id == Accessor.Id);

            if (user is null)
            {
                return QueryResponse<AuthInfo>.Fail(BusinessError.用户不存在);
            }

            if (user.State == UserState.禁用)
            {
                return QueryResponse<AuthInfo>.Fail(BusinessError.用户账号已被禁用);
            }

            dto = new()
            {
                User = _mapper.Map<LoginUser>(user)
            };

            GetUserAuth(user.Roles.Select(x => x.Id).Distinct(), ref dto);

            if (dto.AuthPcMenus is null || !dto.AuthPcMenus.Any())
            {
                _logger.LogInformation("获取用户的PC菜单信息失败，UserId:{id},Name:{name}", dto.User.Id, dto.User.Name);
            }

            if (dto.AuthPdaMenus is null || !dto.AuthPdaMenus.Any())
            {
                _logger.LogInformation("获取用户的Pda菜单信息失败，UserId:{id},Name:{name}", dto.User.Id, dto.User.Name);
            }

            _redisClient.Set(cacheKey, JsonConvert.SerializeObject(dto));

            return QueryResponse<AuthInfo>.Success(dto);
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <param name="platform"></param>
        /// <returns></returns>
        public async Task<QueryResponse<TokenDto>> LoginAsync(string account, string password, string platform)
        {
            var user = await _userRepository.NoTrackingQuery()
                      .Include(x => x.Roles)
                      .Include(x => x.Department)
                      .FirstOrDefaultAsync(t => t.Account == account);
           
            if (user is null)
            {
                return QueryResponse<TokenDto>.Fail(BusinessError.用户不存在);
            }

            if (!user.VerifyPassword(password))
            {
                return QueryResponse<TokenDto>.Fail(BusinessError.密码错误);
            }

            if (user.State == UserState.禁用)
            {
                return QueryResponse<TokenDto>.Fail(BusinessError.用户账号已被禁用);
            }

            var token = JwtTokenProvider.GenerateToken(user.Id.ToString(), platform, CacheKeyProvider.TOKEN_EXPIRE_MINUTE);

            if (JwtTokenProvider.TryGetExpirationTimestamp(token, out int? exp))
            {
                var key = CacheKeyProvider.GetTokenCacheKey(platform.ToLower(), user.Id.ToString());

                _redisClient.Set(key, exp, CacheKeyProvider.TOKEN_EXPIRE_MINUTE * 60);
            }
            else
            {
                _logger.LogInformation("获取token 过期时间失败。token{token}", token);

                return QueryResponse<TokenDto>.Fail(BusinessError.操作失败);
            }

            string authCacheKey = CacheKeyProvider.GetAuthCacheKey(user.Id.ToString());
       
            var auth = new AuthInfo
            {
                User = _mapper.Map<LoginUser>(user)
            };

            GetUserAuth(user.Roles.Select(x => x.Id).Distinct(), ref auth);

            _redisClient.Set(authCacheKey, JsonConvert.SerializeObject(auth));

            return QueryResponse<TokenDto>.Success(new TokenDto { Authorization = $"Bearer {token}" });

        }


        /// <summary>
        /// 获取用户已被授权的资源
        /// </summary>
        /// <param name="roleIds"></param>
        /// <param name="auth"></param>
        private void GetUserAuth(IEnumerable<long> roleIds, ref AuthInfo auth)
        {

            var roles = _roleRepository.NoTrackingQuery()
                       .Include(x => x.RoleMenus)
                       .Include(x => x.RoleDatas)
                       .Include(x => x.RoleFunctions)
                       .Where(t => roleIds.Contains(t.Id))
                       .AsSplitQuery()
                       .ToList();

            GetAuthedMenus(roles.SelectMany(x => x.RoleMenus).Select(x => x.MenuId).Distinct(), ref auth);

            GetAuthedDatas(roles.SelectMany(x => x.RoleDatas).Select(x => x.DataId).Distinct(), ref auth);

            GetAuthedFunctions(roles.SelectMany(x => x.RoleFunctions).Select(x => x.FunctionId).Distinct(), ref auth);
        }

        /// <summary>
        /// 获取授权的菜单资源
        /// </summary>
        /// <param name="authedMenuIds"></param>
        /// <param name="auth"></param>
        /// <returns></returns>
        private void GetAuthedMenus(IEnumerable<long> authedMenuIds, ref AuthInfo auth)
        {
            if (authedMenuIds is null || !authedMenuIds.Any())
            {
                _logger.LogInformation("authedMenuIds is Empty");
                return;
            }


            var allMenus = _menuRepository.NoTrackingQuery().ToList();

            var _menus = _mapper.Map<IEnumerable<AuthMenuItem>>(allMenus.Where(t => authedMenuIds.Contains(t.Id))); //内存筛选过滤操作可能会更快一些

            auth.AuthPcMenus = _menus.Where(t => t.Type == MenuType.PC).GenerateTree(x => x.Id, x => x.ParentId, (long)0);

            auth.AuthPdaMenus = _menus.Where(t => t.Type == MenuType.PDA).GenerateTree(x => x.Id, x => x.ParentId, (long)0);        
           
            auth.AuthMfsMenus = _menus.Where(t => t.Type == MenuType.MFS).GenerateTree(x => x.Id, x => x.ParentId, (long)0);        
        }

        /// <summary>
        /// 获取授权的数据资源
        /// </summary>
        /// <param name="authedDataIds"></param>
        /// <param name="auth"></param>
        private void GetAuthedDatas(IEnumerable<long> authedDataIds, ref AuthInfo auth)
        {
            if (authedDataIds is null || !authedDataIds.Any())
            {
                return;
            }

            var allDatas = _dataRepository.NoTrackingQuery().AsEnumerable();

            var authedDatas = allDatas.Where(t => authedDataIds.Contains(t.Id));

            auth.AuthData = new()
            {
                Factories = _mapper.Map<IEnumerable<AuthDataItem>>(authedDatas.Where(t => t.Category == Enums.DataCategory.工厂)),

                Workshops = _mapper.Map<IEnumerable<AuthDataItem>>(authedDatas.Where(t => t.Category == Enums.DataCategory.车间)),

                Stories = _mapper.Map<IEnumerable<AuthDataItem>>(authedDatas.Where(t => t.Category == Enums.DataCategory.仓库)),

                Materials = _mapper.Map<IEnumerable<AuthDataItem>>(authedDatas.Where(t => t.Category == Enums.DataCategory.物料类型))
            };
        }

        /// <summary>
        /// 获取授权的功能资源
        /// </summary>
        /// <param name="authedFunctionIds"></param>
        /// <param name="auth"></param>
        private void GetAuthedFunctions(IEnumerable<long> authedFunctionIds, ref AuthInfo auth)
        {
            if (authedFunctionIds is null || !authedFunctionIds.Any())
            {
                return;
            }

            var allFunctions = _functionRepository.NoTrackingQuery().AsEnumerable();

            var authedFunctions = allFunctions.Where(t => authedFunctionIds.Contains(t.Id));

            auth.AuthFunctions = _mapper.Map<IEnumerable<AuthFunctionItem>>(authedFunctions);
        }



        private AuthInfo? GetAuthDtoCache(string key)
        {
            try
            {
                if (_redisClient.Exists(key))
                {
                    var info = _redisClient.Get(key);

                    _logger.LogInformation("缓存命中, Key:{key}", key);

                    if (!string.IsNullOrWhiteSpace(info))
                    {
                        return JsonConvert.DeserializeObject<AuthInfo>(info)!;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取用户权限缓存发生异常");
            }

            return null;

        }
        /// <summary>
        /// 加密登录
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <param name="platform"></param>
        /// <returns></returns>
        public async Task<QueryResponse<TokenDto>> LoginPwdAsync(string account, string password, string platform = "pc")
        {
            var user = await _userRepository.NoTrackingQuery()
                      .Include(x => x.Roles)
                      .Include(x => x.Department)
                      .FirstOrDefaultAsync(t => t.Account == account && t.Password == password);

            if (user is null)
            {
                return QueryResponse<TokenDto>.Fail(BusinessError.用户不存在);
            }

            //if (!user.VerifyPassword(password))
            //{
            //    return QueryResponse<TokenDto>.Fail(BusinessError.密码错误);
            //}

            if (user.State == UserState.禁用)
            {
                return QueryResponse<TokenDto>.Fail(BusinessError.用户账号已被禁用);
            }

            var token = JwtTokenProvider.GenerateToken(user.Id.ToString(), platform, CacheKeyProvider.TOKEN_EXPIRE_MINUTE);

            if (JwtTokenProvider.TryGetExpirationTimestamp(token, out int? exp))
            {
                var key = CacheKeyProvider.GetTokenCacheKey(platform.ToLower(), user.Id.ToString());

                _redisClient.Set(key, exp, CacheKeyProvider.TOKEN_EXPIRE_MINUTE * 60);
            }
            else
            {
                _logger.LogInformation("获取token 过期时间失败。token{token}", token);

                return QueryResponse<TokenDto>.Fail(BusinessError.操作失败);
            }

            string authCacheKey = CacheKeyProvider.GetAuthCacheKey(user.Id.ToString());

            var auth = new AuthInfo
            {
                User = _mapper.Map<LoginUser>(user)
            };

            GetUserAuth(user.Roles.Select(x => x.Id).Distinct(), ref auth);

            _redisClient.Set(authCacheKey, JsonConvert.SerializeObject(auth));

            return QueryResponse<TokenDto>.Success(new TokenDto { Authorization = $"Bearer {token}" });
        }
    }
}
