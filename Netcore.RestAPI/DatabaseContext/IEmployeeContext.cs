using MongoDB.Driver;
using Netcore.RestAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Netcore.RestAPI.DatabaseContext
{
    public interface IEmployeeContext
    {
        IMongoCollection<Employee> Employees { get; }  
    }
}
