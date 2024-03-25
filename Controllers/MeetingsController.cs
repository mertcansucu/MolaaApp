using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MolaaApp.Data.Abstract;
using MolaaApp.Entity;
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


        public IActionResult Index(){
            var model = new MeetingsViewModel{
                meetings = _meetingRepository.meetings.ToList(),
                Users = _meetingRepository.meetings.Select(m => m.User).ToList()
            };
            return View(model);
        }

        

        [Authorize] 
        public IActionResult Create(){
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(MeetingCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                _meetingRepository.CreateMeeting(
                    new Meeting
                    {
                        Title = model.Title,
                        Description = model.Description,
                        StartTime = model.StartTime,
                        StartTimeHour = model.StartTimeHour,
                        UserId = userId ?? ""
                    }
                );
                return RedirectToAction("Index"); // Değişebilir, yönlendirme işlemi
            }
            return View(model);
        }
    }
}