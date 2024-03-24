using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MolaaApp.Data.Abstract;
using MolaaApp.Models;

namespace MolaaApp.Controllers
{
    public class MeetingsController : Controller
    {
        private UserManager<AppUser> _userManager;

        private IMeetingRepository _meetingRepository;

        private IUserMeetingRepository _userMeetingRepository;
        
        public MeetingsController(IMeetingRepository meetingRepository,UserManager<AppUser> userManager,IUserMeetingRepository userMeetingRepository){
            _meetingRepository = meetingRepository;
            _userManager = userManager;
            _userMeetingRepository = userMeetingRepository;
            
        }
    }
}