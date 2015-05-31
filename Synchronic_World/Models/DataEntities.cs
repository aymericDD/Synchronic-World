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

        public DbSet<Event> EventTable { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>().HasMany(p => p.friends).WithMany().Map(m => 
            { 
                m.MapLeftKey("FriendsId");
                m.MapRightKey("friendIdtwo"); 
                m.ToTable("FriendLiaison"); 
            });

            modelBuilder.Entity<User>()
                .HasMany(s => s.ParticpationEvents)
                .WithMany(c => c.Participants)
                .Map(t =>
                {
                    t.MapLeftKey("UserId")
                    .MapRightKey("EventId")
                    .ToTable("ParticipationEvent");
                });


            base.OnModelCreating(modelBuilder);
        }

        public System.Data.Entity.DbSet<Synchronic_World.Models.ContributionEvent> ContributionEvents { get; set; }
    }
}