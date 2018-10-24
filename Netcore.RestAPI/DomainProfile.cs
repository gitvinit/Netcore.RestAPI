using AutoMapper;
using Netcore.RestAPI.Models;
using Netcore.RestAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Netcore.RestAPI
{
    public class DomainProfile : Profile
    {
        public DomainProfile()
        {
            CreateMap<Employee, EmployeeViewModel>();
        }
    }
}
