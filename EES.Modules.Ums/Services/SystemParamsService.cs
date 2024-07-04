using AutoMapper;
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
    /// 系统参数服务
    /// </summary>
    public class SystemParamsService : ISystemParamsService
    {
        private readonly IRepository<SystemParam> _systemParamRepository;

        private readonly ILogger<ParamService> _logger;

        private readonly IMapper _mapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="systemParamRepository"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public SystemParamsService(IRepository<SystemParam> systemParamRepository, ILogger<ParamService> logger, IMapper mapper)
        {
            _systemParamRepository = systemParamRepository;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// 查询模块下特定的配置信息
        /// </summary>
        /// <param name="module">模块</param>
        /// <param name="code">编码</param>
        /// <returns></returns>
        public async Task<SystemParamModel> GetSystemParamModelAsync(SystemModule module, string code)
        {
            var param = await _systemParamRepository.NoTrackingQuery().FirstOrDefaultAsync(t => t.Module == module && t.Code == code);

            return _mapper.Map<SystemParamModel>(param);
        }

        /// <summary>
        /// 获取模块下的全部配置
        /// </summary>
        /// <param name="module">模块</param>
        /// <param name="isIncludeDisabled">是否包含已经被禁用的</param>
        /// <returns></returns>
        public async Task<IEnumerable<SystemParamModel>> GetSystemParamModelsAsync(SystemModule module, bool isIncludeDisabled = false)
        {

            var list = await _systemParamRepository.NoTrackingQuery()
                      .Where(t => t.Module == module)
                      .WhereIf(!isIncludeDisabled, t => t.IsEnable)
                      .ToListAsync();

            return _mapper.Map<IEnumerable<SystemParamModel>>(list);

        }
    }
}
