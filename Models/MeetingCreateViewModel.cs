using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MolaaApp.Models
{
    public class MeetingCreateViewModel
    {
        /*
        public int MeetingId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime StartTime { get; set; }
        public string UserId { get; set; } = null!; // OrganizerId'yi string olarak tanımlıyoruz çünkü IdentityUser'ın anahtar tipi string
        public AppUser User { get; set; } = null!;
        public List<UserMeeting> UserMeetings { get; set; } = new List<UserMeeting>();
        */

        public int MeetingId { get; set; }

        [Required]//zorunlu alan
        [Display(Name = "Başlık")]
        public string? Title { get; set; }

        [Required]
        [Display(Name = "Açıklama")]
        public string? Description { get; set; }
    }
}