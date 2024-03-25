using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MolaaApp.Entity;

namespace MolaaApp.Models
{
    public class MeetingsViewModel
    {
        public List<Meeting> meetings {get; set;} = new();
        public List<AppUser> Users { get; set; } = new();
        public List<UserMeeting> userMeetings { get; set; } = new();
    }
}