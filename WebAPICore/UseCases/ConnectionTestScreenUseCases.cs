using App.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCases
{
    public class ConnectionTestScreenUseCases : IConnectionTestScreenUseCases
    {
        private readonly IConnectionTestRepository _connectionTestRepository;

        public ConnectionTestScreenUseCases(IConnectionTestRepository connectionTestRepository)
        {
            _connectionTestRepository = connectionTestRepository;
        }

        public async Task<dynamic> TestConnection()
        {
            return await _connectionTestRepository.GetAsync();
        }
    }
}
