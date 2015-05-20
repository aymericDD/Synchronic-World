using Synchronic_World.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Synchronic_World
{
    public class DataEntities:DbContext
    {
        public DbSet<User> UserTable { get; set; }

        public DbSet<RoleUser> RoleUserTable { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasMany(s => s.friends).WithMany().Map(mc =>
            {
                mc.ToTable("Friend");
                mc.MapLeftKey("SenderId");
                mc.MapRightKey("ReceiverId");
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}