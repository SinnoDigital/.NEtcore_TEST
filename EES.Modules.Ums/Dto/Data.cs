using EES.Infrastructure.Data;
using EES.Modules.Ums.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Modules.Ums.Dto
{
    /// <summary>
    /// 数据资源
    /// </summary>
    public class DataDto
    {

        /// <summary>
        /// id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 1 工厂车间  2：工厂仓库  3：物料类型
        /// </summary>
        public DataCategory Category { get; set; }
        /// <summary>
        /// 1工厂  11 车间   21 仓库   31物料类型
        /// </summary>
        public DataSubCategory SubCategory { get; set; }
        /// <summary>
        /// 对应的数据id
        /// </summary>
        public long ObjectId { get; set; }
        /// <summary>
        /// 对应数据的编码
        /// </summary>
        public string ObjectCode { get; set; }
        /// <summary>
        /// 数据显示名字
        /// </summary>
        public string ObjectName { get; set; }

        /// <summary>
        /// 所属上级的id。 对应到object_id。工厂的上级id为0，车间，仓库的上级id填写所属工厂的id。
        /// </summary>
        public long ParentId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 创建者名称
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 最后一次更新时间
        /// </summary>
        public string UpdateTime { get; set; }

        /// <summary>
        /// 最后一次更新用户的名称
        /// </summary>
        public string UpdateUserName { get; set; }
    }

    /// <summary>
    /// 数据权限资源简要模型
    /// </summary>
    public class ShortDataDto
    {
        /// <summary>
        /// id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 1 工厂  2：工厂仓库  3：物料类型  4车间
        /// </summary>
        public DataCategory Category { get; set; }
        /// <summary>
        /// 对应的数据id
        /// </summary>
        public long ObjectId { get; set; }
        /// <summary>
        /// 对应数据的编码
        /// </summary>
        public string ObjectCode { get; set; }
        /// <summary>
        /// 数据显示名字
        /// </summary>
        public string ObjectName { get; set; }

        /// <summary>
        /// 所属上级的id。 对应到object_id。工厂的上级id为0，车间，仓库的上级id填写所属工厂的id。
        /// </summary>
        public long ParentId { get; set; }
    }


    /// <summary>
    /// 所有的数据权限资源数据
    /// </summary>
    public class AllDataDto
    {
      
        /// <summary>
        /// 工厂车间数据
        /// </summary>
        public IEnumerable<TreeItem<ShortDataDto>> Factories { get; set; }

        /// <summary>
        /// 仓库数据
        /// </summary>
        public IEnumerable<TreeItem<ShortDataDto>> Stores { get; set; }

        /// <summary>
        /// 物料类型数据
        /// </summary>
        public IEnumerable<TreeItem<ShortDataDto>> Materials { get; set; }
    }
}
