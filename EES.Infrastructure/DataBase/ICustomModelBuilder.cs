using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Infrastructure.DataBase
{
    /// <summary>
    /// 模块的模型构建
    /// </summary>
    public interface ICustomModelBuilder
    {
        void Build(ModelBuilder modelBuilder);
    }
}
