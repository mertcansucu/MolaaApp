using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MolaaApp.Data.Abstract;
using MolaaApp.Entity;
using MolaaApp.Models;

namespace MolaaApp.Data.Concrete
{
    public class EfUserMeetingRepository : IUserMeetingRepository
    {
        private IdentityContext _context;

        public EfUserMeetingRepository(IdentityContext context){
            _context = context;
        }

        public IQueryable<UserMeeting> userMeetings => _context.userMeetings;

        public void AddUserMeeting(UserMeeting userMeeting)
        {
            _context.userMeetings.Add(userMeeting);
            _context.SaveChanges();
        }
    
        public void RemoveUserMeeting(UserMeeting userMeeting)
        {
            _context.userMeetings.Remove(userMeeting);
            _context.SaveChanges();
        }
    }
}