using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using Netcore.RestAPI.DatabaseContext;
using Netcore.RestAPI.Models;
using Netcore.RestAPI.ViewModels;

namespace Netcore.RestAPI.Repository
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeContext _context;
        private readonly Settings _appSettings;
        private readonly IMapper _mapper;
        public EmployeeService(IEmployeeContext context, IOptions<Settings> options, IMapper mapper)
        {
            _context = context;
            _appSettings = options.Value;
            _mapper = mapper;
        }

        public async Task Create(Employee employee)
        {
            await _context.Employees.InsertOneAsync(employee);
        }

        public async Task<bool> Delete(string username)
        {
            FilterDefinition<Employee> filter = Builders<Employee>.Filter.Eq(m => m.Username, username);
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

        public Task<Employee> GetEmployee(string username)
        {
            FilterDefinition<Employee> filter = Builders<Employee>.Filter.Eq(m => m.Username, username);
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

        public async Task<EmployeeViewModel> Authenticate(string username, string password)
        {
            FilterDefinition<Employee> filter = Builders<Employee>.
                Filter.Eq(m => m.Username, username) & Builders<Employee>.
                Filter.Eq(m => m.Password, password);
            var employee = await _context
                    .Employees
                    .Find(filter)
                    .FirstOrDefaultAsync();

            // return null if employee is not found
            if (employee == null)
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, employee.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            employee.Token = tokenHandler.WriteToken(token);

            // remove password before returning
            employee.Password = null;

            return _mapper.Map<EmployeeViewModel>(employee);
        }
    }
}
