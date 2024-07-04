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
    /// 系统参数和用户参数查询服务
    /// </summary>
    public class ParamService : ServerBase, IParamService
    {
        private readonly IRepository<UserParam> _userParamRepository;

        private readonly IRepository<SystemParam> _systemParamRepository;

        private readonly ILogger<ParamService> _logger;

        private readonly IMapper _mapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userParamRepository"></param>
        /// <param name="systemParamRepository"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        /// <param name="mediatorHandler"></param>
        public ParamService(IRepository<UserParam> userParamRepository, IRepository<SystemParam> systemParamRepository, ILogger<ParamService> logger, IMapper mapper, IMediatorHandler mediatorHandler) : base(mediatorHandler)
        {
            _userParamRepository = userParamRepository;
            _systemParamRepository = systemParamRepository;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// 根据ID获取系统参数信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<QueryResponse<SystemParamDto>> GetSystemParamAsync(long id)
        {
            var param = await _systemParamRepository.NoTrackingQuery().FirstOrDefaultAsync(t => t.Id == id);

            if (param is null)
            {
                return QueryResponse<SystemParamDto>.Fail(BusinessError.参数不存在);
            }

            return QueryResponse<SystemParamDto>.Success(_mapper.Map<SystemParamDto>(param));
        }

        /// <summary>
        /// 根据模块和编码获系统参数信息
        /// </summary>
        /// <param name="module"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<QueryResponse<SystemParamDto>> GetSystemParamAsync(SystemModule module, string code)
        {
            var param = await _systemParamRepository.NoTrackingQuery().FirstOrDefaultAsync(t => t.Module == module && t.Code == code);

            if (param is null)
            {
                return QueryResponse<SystemParamDto>.Fail(BusinessError.参数不存在);
            }

            return QueryResponse<SystemParamDto>.Success(_mapper.Map<SystemParamDto>(param));
        }

        /// <summary>
        /// 获取全部的系统参数(分页)
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="isGetTotalCount">是否返回总数量的信息</param>
        /// <returns></returns>
        public async Task<QueryResponse<PaginationModel<SystemParamDto>>> GetSystemParamsAsync(int pageIndex, int PageSize, bool isGetTotalCount = true)
        {
            var (totalCount, systemPsrams) = await _systemParamRepository.NoTrackingQuery().OrderBy(x => x.Id).GetPagingInTupleAsync(pageIndex, PageSize, isGetTotalCount);

            var pagingModel = new PaginationModel<SystemParamDto>
            {
                PageIndex = pageIndex,
                PageSize = PageSize,
                TotalCount = totalCount,
                Data = _mapper.Map<IEnumerable<SystemParamDto>>(systemPsrams)
            };

            return QueryResponse<PaginationModel<SystemParamDto>>.Success(pagingModel);
        }

        /// <summary>
        /// 根据ID获取当前用户的用户参数信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<QueryResponse<UserParamDto>> GetUserParamAsync(long id)
        {
            var param = await _userParamRepository.NoTrackingQuery().FirstOrDefaultAsync(t => t.Id == id);

            if (param is null)
            {
                return QueryResponse<UserParamDto>.Fail(BusinessError.参数不存在);
            }

            if (param.UserId != Accessor.Id)
            {
                return QueryResponse<UserParamDto>.Fail(BusinessError.无权查询此数据);
            }

            return QueryResponse<UserParamDto>.Success(_mapper.Map<UserParamDto>(param));
        }


        /// <summary>
        /// 根据类型和编码获取当前用户的用户参数信息
        /// </summary>
        /// <param name="type"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<QueryResponse<UserParamDto>> GetUserParamAsync(string type, string code)
        {
            var param = await _userParamRepository.NoTrackingQuery().FirstOrDefaultAsync(t => t.Type == type && t.Code == code && t.UserId == Accessor.Id);
            if (param is null)
            {
                return QueryResponse<UserParamDto>.Fail(BusinessError.参数不存在);
            }

            if (param.UserId != Accessor.Id)
            {
                return QueryResponse<UserParamDto>.Fail(BusinessError.无权查询此数据);
            }

            return QueryResponse<UserParamDto>.Success(_mapper.Map<UserParamDto>(param));
        }

        /// <summary>
        /// 获取用户全部的用户参数信息
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="isGetTotalCount"></param>
        /// <returns></returns>    
        public async Task<QueryResponse<PaginationModel<UserParamDto>>> GetUserParamsAsync(int pageIndex, int PageSize, bool isGetTotalCount = true)
        {
            var (totalCount, userPsrams) = await _userParamRepository.NoTrackingQuery().OrderBy(x => x.Id).GetPagingInTupleAsync(pageIndex, PageSize, isGetTotalCount);

            var pagingModel = new PaginationModel<UserParamDto>
            {
                PageIndex = pageIndex,
                PageSize = PageSize,
                TotalCount = totalCount,
                Data = _mapper.Map<IEnumerable<UserParamDto>>(userPsrams)
            };

            return QueryResponse<PaginationModel<UserParamDto>>.Success(pagingModel);
        }
    }
}
