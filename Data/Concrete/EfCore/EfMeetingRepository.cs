using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MolaaApp.Data.Abstract;
using MolaaApp.Entity;
using MolaaApp.Models;

namespace MolaaApp.Data.Concrete
{
    public class EfMeetingRepository : IMeetingRepository
    {
        private IdentityContext _context;
        public EfMeetingRepository(IdentityContext context)
        {
            _context = context;
        }

        public IQueryable<Meeting> meetings => _context.meetings;

        public void CreateMeeting(Meeting meeting)
        {
            _context.meetings.Add(meeting);
            _context.SaveChanges();
        }

        public void EditMeeting(Meeting meeting){
            var entity = _context.meetings.FirstOrDefault(i => i.MeetingId == meeting.MeetingId);

            if (entity != null)
            {
                entity.Title = meeting.Title;
                entity.Description = meeting.Description;
                entity.StartTime = meeting.StartTime;
                entity.StartTimeHour = meeting.StartTimeHour;

                _context.SaveChanges();
            }
        }
    }
}