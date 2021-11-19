using App.Repository.ApiClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Repository
{
    public class ConnectionTestRepository : IConnectionTestRepository
    {
        private readonly IWebApiExecuter _webApiExecuter;
        public ConnectionTestRepository(IWebApiExecuter webApiExecuter)
        {
            _webApiExecuter = webApiExecuter;
        }

        public async Task<dynamic> GetAsync()
        {
            return await _webApiExecuter.InvokeGet<dynamic>($"api/connectiontest");
        }
    }
}
