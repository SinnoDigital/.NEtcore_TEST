namespace EES.Infrastructure.Enums
{
    /// <summary>
    /// 系统模块
    /// </summary>
    public enum SystemModule
    {
        /// <summary>
        /// 系统框架
        /// </summary>
        SYSTEM = 0,

        /// <summary>
        /// 用户管理
        /// </summary>
        UMS = 1,

        /// <summary>
        /// 车间设备管理
        /// </summary>
        PEM = 2,

        /// <summary>
        /// 配方管理
        /// </summary>
        PMS = 3,

        /// <summary>
        /// 工单管理
        /// </summary>
        PSM = 4,

        /// <summary>
        /// 称量
        /// </summary>
        PSM_WEIGHING = 5,

        /// <summary>
        /// 乳化
        /// </summary>
        PSM_BULK = 6,

        /// <summary>
        /// 灌包
        /// </summary>
        PSM_FP = 7,

        /// <summary>
        /// 条码管理
        /// </summary>
        BIS = 8,

        /// <summary>
        /// 质检管理
        /// </summary>
        QCS = 9,

        /// <summary>
        /// 仓储管理
        /// </summary>
        WMS = 10,

        /// <summary>
        /// 批次追溯
        /// </summary>
        BTR = 11,

        /// <summary>
        /// 审计
        /// </summary>
        ATF = 12,

        /// <summary>
        /// 绩效
        /// </summary>
        OEE = 13,

        /// <summary>
        /// 设备管理
        /// </summary>
        EAM = 14,

        /// <summary>
        /// 扩展数据接口
        /// </summary>
        EDI = 15,
    }
}
