namespace EES.Modules.Share.Dto
{
    /// <summary>
    /// 
    /// </summary>
    public class CustomConfigurationItemDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 链接
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// 默认值
        /// </summary>
        public string DefaultValue { get; set; }

        /// <summary>
        /// 设定值
        /// </summary>
        public string SettingValue { get; set; }

        /// <summary>
        /// 值类型
        /// </summary>
        public int ValueType { get; set; }

        /// <summary>
        /// 枚举编码
        /// </summary>
        public string EnumCode { get; set; }

        /// <summary>
        /// 设定上限
        /// </summary>
        public decimal UpperLimit { get; set; }

        /// <summary>
        /// 设定下限
        /// </summary>
        public decimal LowerLimit { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 标识
        /// </summary>
        public string Identifying { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
