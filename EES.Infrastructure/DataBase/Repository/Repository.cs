using EES.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
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
    public class Repository<T> : RepositoryBase<T>, IRepository<T> where T : EntityBase
    {
        public Repository(MasterDbContext context) : base(context)
        {

        }
    }
}
