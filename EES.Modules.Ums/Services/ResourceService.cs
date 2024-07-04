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
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Modules.Ums.Services
{
    /// <summary>
    /// 资源服务(菜单，数据权限资源，功能权限资源)
    /// </summary>
    public class ResourceService : ServerBase, IResourceService
    {
        private readonly IRepository<Entities.Menu> _menuRepository;

        private readonly IRepository<Entities.Function> _functionRepository;

        private readonly IRepository<Entities.Data> _dataRepository;

        private readonly ILogger<ResourceService> _logger;

        private readonly IMapper _mapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="menuRepository"></param>
        /// <param name="functionRepository"></param>
        /// <param name="dataRepository"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        /// <param name="mediatorHandler"></param>
        public ResourceService(IRepository<Menu> menuRepository, IRepository<Function> functionRepository, IRepository<Entities.Data> dataRepository, ILogger<ResourceService> logger, IMapper mapper, IMediatorHandler mediatorHandler) : base(mediatorHandler)
        {
            _menuRepository = menuRepository;
            _functionRepository = functionRepository;
            _dataRepository = dataRepository;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// 获取数据权限资源
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<QueryResponse<DataDto>> GetDataAsync(long id)
        {
            var resource = await _dataRepository.NoTrackingQuery().FirstOrDefaultAsync(t => t.Id == id);


            return resource is null ? QueryResponse<DataDto>.Fail(BusinessError.数据资源不存在)
                                   : QueryResponse<DataDto>.Success(_mapper.Map<DataDto>(resource));


        }

        /// <summary>
        /// 获取功能权限资源树
        /// </summary>
        /// <returns></returns>        
        public async Task<QueryResponse<AllDataDto>> GetDataTreeAsync()
        {
            var datas = await _dataRepository.NoTrackingQuery().ToListAsync();

            var dtoList = _mapper.Map<IEnumerable<ShortDataDto>>(datas);

            AllDataDto dto = new();

            GetDataTree(dtoList, ref dto);

            return QueryResponse<AllDataDto>.Success(dto);
        }

        /// <summary>
        /// 获取功能权限资源详细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<QueryResponse<FunctionDto>> GetFunctionAsync(long id)
        {
            var resource = await _functionRepository.NoTrackingQuery().FirstOrDefaultAsync(t => t.Id == id);

            return resource is null ? QueryResponse<FunctionDto>.Fail(BusinessError.功能资源不存在)
                                : QueryResponse<FunctionDto>.Success(_mapper.Map<FunctionDto>(resource));
        }

        /// <summary>
        /// 获取功能权限资源树
        /// </summary>
        /// <returns></returns>
        public async Task<QueryResponse<IEnumerable<TreeItem<FunctionDto>>>> GetFunctionTreeAsync()
        {
            var resources = await _functionRepository.NoTrackingQuery().ToListAsync();

            var tree = _mapper.Map<IEnumerable<FunctionDto>>(resources).GenerateTree(x => x.Id, x => x.ParentId, (long)0);

            return QueryResponse<IEnumerable<TreeItem<FunctionDto>>>.Success(tree);
        }


        /// <summary>
        /// 获取菜单详细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<QueryResponse<MenuDto>> GetMenuAsync(long id)
        {
            var menu = await _menuRepository.NoTrackingQuery().FirstOrDefaultAsync(t => t.Id == id);

            if (menu == null)
            {
                return QueryResponse<MenuDto>.Fail(BusinessError.资源不存在);
            }

            var dto = _mapper.Map<MenuDto>(menu);

            return QueryResponse<MenuDto>.Success(dto);
        }

        /// <summary>
        /// 以树状结构返回指定资源以及其全部子资源信息
        /// </summary>
        /// <param name="menuType"></param>
        /// <param name="rootId"></param>
        /// <returns></returns>f
        public async Task<QueryResponse<IEnumerable<TreeItem<MenuDto>>>> GetMenuTreeAsync(MenuType menuType, long rootId = 0)
        {
            var menus = await _menuRepository.NoTrackingQuery().Where(t => t.Type == menuType).ToListAsync();

            var treeItems = _mapper.Map<IEnumerable<MenuDto>>(menus).GenerateTree(x => x.Id, x => x.ParentId, rootId);

            return QueryResponse<IEnumerable<TreeItem<MenuDto>>>.Success(treeItems);
        }

        private static void GetDataTree(IEnumerable<ShortDataDto> source, ref AllDataDto dto)
        {
            List<TreeItem<ShortDataDto>> Factories = new();

            List<TreeItem<ShortDataDto>> Stories = new();

            List<TreeItem<ShortDataDto>> Materials = new();

            foreach (var item in source.Where(t => t.Category == Enums.DataCategory.工厂))
            {
                Factories.Add(new TreeItem<ShortDataDto>
                {
                    Item = item,
                    Children = GetChildren(source, t => t.ParentId == item.ObjectId && t.Category == Enums.DataCategory.车间 && t.Id != item.Id)
                });

                Stories.Add(new TreeItem<ShortDataDto>
                {
                    Item = item,
                    Children = GetChildren(source, t => t.ParentId == item.ObjectId && t.Category == Enums.DataCategory.仓库 && t.Id != item.Id)
                });

            }

            foreach (var item in source.Where(t => t.Category == Enums.DataCategory.物料类型))
            {
                Materials.Add(new TreeItem<ShortDataDto>
                {
                    Item = item,
                    Children = GetChildren(source, t => t.ParentId == item.ObjectId && t.Category == Enums.DataCategory.物料类型 && t.Id != item.Id)
                });
            }


            dto.Factories = Factories;
            dto.Stores = Stories;
            dto.Materials = Materials;

        }

        private static IEnumerable<TreeItem<T>> GetChildren<T>(IEnumerable<T> source, Func<T, bool> predicate)
        {
            foreach (var item in source.Where(predicate))
            {
                yield return new TreeItem<T>
                {
                    Item = item,
                    Children = new List<TreeItem<T>>()
                    /*
                       tree的节点和子节点分别来自不同的表时，很大可能性出现 node.id == node.pid  或者  node1.id== node2.pid 的情况，容易陷入死循环。
                       因为数据权限只有两层，这里强制先写死
                     */
                };
            }
        }
    }
}
