using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MolaaApp.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace MolaaApp.Models
{
    public class IdentityContext:IdentityDbContext<AppUser,AppRole,string>
    {
        public IdentityContext(DbContextOptions<IdentityContext> options):base(options){
            
        }

        public DbSet<Post> Posts => Set<Post>();
        public DbSet<Comment> Comments => Set<Comment>();
        public DbSet<Tag> Tags => Set<Tag>();
        public DbSet<Like> likes => Set<Like>();
        public DbSet<Meeting> meetings => Set<Meeting>();
        public DbSet<UserMeeting> userMeetings => Set<UserMeeting>();
    }
}