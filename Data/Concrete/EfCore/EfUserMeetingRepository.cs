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

        
    }
}