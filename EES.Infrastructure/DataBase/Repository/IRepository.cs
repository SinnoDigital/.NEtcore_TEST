using EES.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Infrastructure.DataBase.Repository
{
    /// <summary>
    /// 业务仓储
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T> : IRepositoryBase<T> where T : EntityBase
    {

    }
}
