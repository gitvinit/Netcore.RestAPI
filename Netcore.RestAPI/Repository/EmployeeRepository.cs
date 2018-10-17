using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using Netcore.RestAPI.DatabaseContext;
using Netcore.RestAPI.Models;

namespace Netcore.RestAPI.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly IEmployeeContext _context;
        public EmployeeRepository(IEmployeeContext context)
        {
            _context = context;
        }

        public async Task Create(Employee employee)
        {
            await _context.Employees.InsertOneAsync(employee);
        }

        public async Task<bool> Delete(string name)
        {
            FilterDefinition<Employee> filter = Builders<Employee>.Filter.Eq(m => m.Name, name);
            DeleteResult deleteResult = await _context
                                                .Employees
                                                .DeleteOneAsync(filter);
            return deleteResult.IsAcknowledged
                && deleteResult.DeletedCount > 0;
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            return await _context.Employees.Find(_ => true).ToListAsync();
        }

        public Task<Employee> GetEmployee(string name)
        {
            FilterDefinition<Employee> filter = Builders<Employee>.Filter.Eq(m => m.Name, name);
            return _context
                    .Employees
                    .Find(filter)
                    .FirstOrDefaultAsync();
        }

        public async Task<bool> Update(Employee employee)
        {
            ReplaceOneResult updateResult =
            await _context
                    .Employees
                    .ReplaceOneAsync(
                        filter: g => g.Id == employee.Id,
                        replacement: employee);
            return updateResult.IsAcknowledged
                    && updateResult.ModifiedCount > 0;
        }
    }
}
