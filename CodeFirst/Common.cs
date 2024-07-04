using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirst
{
    public class Common
    {
        private static IFreeSql _fsql;
        public static IFreeSql oSql
        {
            get
            {
                if (_fsql == null)
                {
                    FreeSql.DataType dataType = (FreeSql.DataType)Enum.Parse(typeof(FreeSql.DataType),Configuration.GetConfiguration("DbType") , true);
                    string sqlConn = Configuration.GetConfiguration("MasterConnection");
                    _fsql = new FreeSql.FreeSqlBuilder()
                            .UseConnectionString(dataType, sqlConn)
                            .UseAutoSyncStructure(true) //自动同步结构到数据库
                                                        //.UseMonitorCommand(cmd => System.Diagnostics.Trace.WriteLine(cmd.CommandText)) //监听SQL命令对象，在执行后
                            .Build()
                            .SetDbContextOptions(opt => opt.EnableCascadeSave = false); //不启用关联更新
                    _fsql.Aop.ConfigEntityProperty += (s, e) => {
                        if (e.Property.PropertyType.IsEnum)
                            e.ModifyResult.MapType = typeof(int);
                    };
                }
                return _fsql;
            }
            set { _fsql = value; }
        }
    }
}
