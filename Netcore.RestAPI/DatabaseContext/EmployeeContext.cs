using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Netcore.RestAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Netcore.RestAPI.DatabaseContext
{
    public class EmployeeContext : IEmployeeContext
    {
        private readonly IMongoDatabase _db;
        public EmployeeContext(IOptions<Settings> options)
        {
            var client = new MongoClient(options.Value.ConnectionString);
            _db = client.GetDatabase(options.Value.Database);
        }
        public IMongoCollection<Employee> Employees => _db.GetCollection<Employee>("Employees");
    }
}
