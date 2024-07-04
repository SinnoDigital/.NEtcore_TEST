// See https://aka.ms/new-console-template for more information

using CodeFirst;
using LinCms.Core.Entities;
try
{
    Console.WriteLine("开始同步");
    Common.oSql.Select<UmsAdministrativeArea>().ToOne();
    Common.oSql.Select<UmsData>().ToOne();
    Common.oSql.Select<UmsDepartment>().ToOne();
    Common.oSql.Select<UmsFunction>().ToOne();
    Common.oSql.Select<UmsMenu>().ToOne();
    Common.oSql.Select<UmsRole>().ToOne();
    Common.oSql.Select<UmsRoleDatas>().ToOne();
    Common.oSql.Select<UmsRoleFunctions>().ToOne();
    Common.oSql.Select<UmsRoleMenus>().ToOne();
    Common.oSql.Select<UmsSystemParam>().ToOne();
    Common.oSql.Select<UmsTeam>().ToOne();
    Common.oSql.Select<UmsTeamMembers>().ToOne();
    Common.oSql.Select<UmsUser>().ToOne();
    Common.oSql.Select<UmsUserParam>().ToOne();
    Common.oSql.Select<UmsUserRoles>().ToOne();
    //Wms模块初始化数据
    Console.WriteLine("Wms模块正在初始化数据...");
    WmsInitialize();
    Console.WriteLine("执行完毕");
    Console.ReadKey();
}
catch (Exception ex)
{
    Console.WriteLine("执行报错:" + ex.Message);
    Console.ReadKey();
}


void WmsInitialize()
{
    #region 权限初始化
    //取消盘点审核接口权限 20240609，李堃新增
    //获取仓储模块
    var selectWmsStorage = Common.oSql.Select<UmsFunction>().Where(a => a.Name == "仓储" && a.Type == 1).ToOne();
    //获取执行回传模块
    var selectWmsStockExecute = Common.oSql.Select<UmsFunction>().Where(a => a.Name == "执行回传" && a.Type == 1 && a.ParentId == selectWmsStorage.Id).ToOne();
    if(selectWmsStockExecute == null)
    {
        Common.oSql.Insert<UmsFunction>(new UmsFunction()
        {
            Name = "执行回传",
            Type = 1,
            ParentId = selectWmsStorage.Id,
            Description = "执行回传",
            Identifier = "",
            CreateUserId = 1,
            CreateUserName = "admin",
            CreateTime = DateTime.Now,
            UpdateUserId = 0,
            UpdateUserName = "",
            UpdateTime = Convert.ToDateTime("1900-01-01")
        }).ExecuteAffrows();
    }

    //获取修复回传状态权限
    //获取执行回传模块
    var selectWmsStockExecuteAgain = Common.oSql.Select<UmsFunction>().Where(a => a.Name == "执行回传" && a.Type == 1 && a.ParentId == selectWmsStorage.Id).ToOne();
    var updateWmsStockExecute = Common.oSql.Select<UmsFunction>().Where(a => a.Identifier == "wms_modify_return_result_state_toNoNeed" && a.Type == 2 && a.ParentId == selectWmsStockExecuteAgain.Id).ToOne();
    if (updateWmsStockExecute == null)
    {
        Common.oSql.Insert<UmsFunction>(new UmsFunction()
        {
            Name = "修改回传状态为无需回传",
            Type = 2,
            ParentId = selectWmsStockExecuteAgain.Id,
            Description = "修改回传状态为无需回传",
            Identifier = "wms_modify_return_result_state_toNoNeed",
            CreateUserId = 1,
            CreateUserName = "admin",
            CreateTime = DateTime.Now,
            UpdateUserId = 0,
            UpdateUserName = "",
            UpdateTime = Convert.ToDateTime("1900-01-01")
        }).ExecuteAffrows();
    }
    #endregion

    #region 系统参数初始化

    #endregion

    #region 自定义类初始化

    #endregion

    #region 菜单初始化

    #endregion
}
