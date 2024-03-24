using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MolaaApp.Models;

namespace MolaaApp.Entity
{
    public class UserMeeting
    {
        public int UserMeetingId { get; set; }
        public string UserId { get; set; } = null!;// UserId'yi string olarak tanımlıyoruz çünkü IdentityUser'ın anahtar tipi string
        public AppUser User { get; set; } = null!;
        public int MeetingId { get; set; }
        public Meeting Meeting { get; set; } = null!;

    }
}