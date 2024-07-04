namespace EES.Modules.Share.Dto
{
    /// <summary>
    /// 
    /// </summary>
    public class CustomConfigurationDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 类型名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 类型代码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 工厂id
        /// </summary>
        public long FactoryId { get; set; }

        /// <summary>
        /// 工厂名称
        /// </summary>
        public string FactoryName { get; set; }

        /// <summary>
        /// 工厂代码
        /// </summary>
        public string FactoryCode { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDelete { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<CustomConfigurationItemDto> Item { get; set; }
    }
}
