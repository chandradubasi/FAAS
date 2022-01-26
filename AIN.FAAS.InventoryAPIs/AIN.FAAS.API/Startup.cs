using AIN.FAAS.Repository.IRepository;
using AIN.FAAS.Repository.Models;
using AIN.FAAS.Repository.Repository;
using AIN.FAAS.Services.IServices;
using AIN.FAAS.Services.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

[assembly: FunctionsStartup(typeof(AIN.FAAS.API.Startup))]
namespace AIN.FAAS.API
{
    class Startup : FunctionsStartup
    {     
        public override void Configure(IFunctionsHostBuilder builder)
        {
            string SqlConnection = Environment.GetEnvironmentVariable("SqlConnectionString");
            builder.Services.AddDbContext<AINDatabaseContext>(options => options.UseSqlServer(SqlConnection));
            builder.Services.AddTransient<IInventoryAPIServices, InventoryAPIServices>();
            builder.Services.AddTransient<IInventoryAPIRepository, InventoryAPIRepository>();
            // test comment for pull request
        }
    }
}
