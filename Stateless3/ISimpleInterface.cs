using Microsoft.ServiceFabric.Services.Remoting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Stateless3
{
    public interface ISimpleInterface : IService
    {
        public Task<string> HelloHiNamaste(string message);
    }
}
