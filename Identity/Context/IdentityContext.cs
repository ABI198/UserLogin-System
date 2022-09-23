using Identity.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Context
{
    public class IdentityContext : IdentityDbContext<AppUser, AppRole, int>
    {
        public IdentityContext(DbContextOptions<IdentityContext> opt) : base(opt)
        {

        }
    }
}
