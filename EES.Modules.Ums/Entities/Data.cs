using EES.Infrastructure.Entities;
using EES.Modules.Ums.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Modules.Ums.Entities
{
    /// <summary>
    /// 数据权限
    /// </summary>
    public class Data : EntityBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="category"></param>
        /// <param name="objectId"></param>
        /// <param name="objectCode"></param>
        /// <param name="objectName"></param>
        /// <param name="parentId"></param>
        /// <param name="createUserId"></param>
        /// <param name="createUserName"></param>
        public Data(DataCategory category, long objectId, string objectCode, string objectName, long parentId, long createUserId, string createUserName) : base(createUserId, createUserName)
        {
            Category = category;
            ObjectId = objectId;
            ObjectCode = string.IsNullOrEmpty(objectCode) ? string.Empty : objectCode;
            ObjectName = string.IsNullOrEmpty(objectName) ? string.Empty : objectName;
            ParentId = parentId;
        }
        /// <summary>
        /// 
        /// </summary>
        protected Data() { }

        /// <summary>
        /// 1工厂  2仓库  3 物料类型  4 车间
        /// </summary>
        public DataCategory Category { get; private set; }
        /// <summary>
        /// 对应的数据id
        /// </summary>
        public long ObjectId { get; private set; }
        /// <summary>
        /// 对应数据的编码
        /// </summary>
        public string ObjectCode { get; private set; }
        /// <summary>
        /// 数据显示名字
        /// </summary>
        public string ObjectName { get; private set; }

        /// <summary>
        /// 所属上级的id。 对应到object_id。工厂的上级id为0，车间，仓库的上级id填写所属工厂的id。
        /// </summary>
        public long ParentId { get; private set; }

        /// <summary>
        /// 数据和角色绑定关系
        /// </summary>
        public IList<RoleDatas> RoleDatas { get; set; }


        /// <summary>
        /// 数据修改
        /// </summary>
        /// <param name="objectId">对应的数据id</param>
        /// <param name="objectCode">对应数据的编码</param>
        /// <param name="objectName">数据显示名字</param>
        /// <param name="parentId">所属上级的id。</param>
        /// <param name="updateUserId">修改者id</param>
        /// <param name="updateUserName">修改者名字</param>
        public void Modify(long objectId, string objectCode, string objectName, long parentId, long updateUserId, string updateUserName)
        {
            ObjectId = objectId;
            ObjectCode = string.IsNullOrEmpty(objectCode) ? string.Empty : objectCode;
            ObjectName = string.IsNullOrEmpty(objectName) ? string.Empty : objectName;
            ParentId = parentId;

            UpdateRecord(updateUserId, updateUserName);
        }
    }
}
