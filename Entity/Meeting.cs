using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MolaaApp.Models;

namespace MolaaApp.Entity
{
    public class Meeting
    {
        public int MeetingId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime StartTime { get; set; }
        public string UserId { get; set; } = null!; // OrganizerId'yi string olarak tanımlıyoruz çünkü IdentityUser'ın anahtar tipi string
        public AppUser User { get; set; } = null!;
        public List<UserMeeting> UserMeetings { get; set; } = new List<UserMeeting>();
    }
}