using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DMS_thesis.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DMS_thesis.Context
{
    public class ApplicationDataContext : IdentityDbContext<AppUser>
    {
        //public ApplicationDataContext()
        //    : base("DefaultConnection")
        //{ }

        //public System.Data.Entity.DbSet<AppUser> AppUsers { get; set; }
    }
}