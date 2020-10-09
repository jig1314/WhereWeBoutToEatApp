using AppModels;
using AppServer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppServer.DatabaseContexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {

        }

        public DbSet<Recipe> Recipes { get; set; }

        public DbSet<AppSetting> AppSettings { get; set; }

    }
}
