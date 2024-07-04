using EES.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Infrastructure.Service
{
    public interface ILogDispatchProvider
    {
        Task PublishAsync(LogEntityBase message);

        void Publish(LogEntityBase message);
    }
}
