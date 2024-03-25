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
                meetings = _meetingRepository.meetings.Include(m => m.UserMeetings).ToList(),
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

        [HttpPost]
        public JsonResult JoinMeeting(int meetingId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var existingUserMeeting = _userMeetingRepository.userMeetings.FirstOrDefault(um => um.UserId == userId && um.MeetingId == meetingId);

            if (existingUserMeeting != null)
            {
                // Kullanıcı zaten bu toplantıya katılmışsa, katılımını iptal eder
                _userMeetingRepository.RemoveUserMeeting(existingUserMeeting);
                return Json(new { joined = false });
            }
            else
            {
                // Kullanıcı bu toplantıya daha önce katılmamışsa, katılımını ekler
                _userMeetingRepository.AddUserMeeting(new UserMeeting { UserId = userId ?? "", MeetingId = meetingId });
                return Json(new { joined = true });
            }
        }

        [HttpGet]
        public JsonResult GetParticipantCount(int meetingId)
        {
            var participantCount = _userMeetingRepository.userMeetings.Count(um => um.MeetingId == meetingId);
            return Json(new { participantCount = participantCount });
        }



        [HttpGet]
        public JsonResult CheckUserAttendance(int meetingId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var existingUserMeeting = _userMeetingRepository.userMeetings.FirstOrDefault(um => um.UserId == userId && um.MeetingId == meetingId);
        
            return Json(new { attended = existingUserMeeting != null });
        }

        public async Task<IActionResult> List(){
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var role = User.FindFirstValue(ClaimTypes.Role);

        var meetings = await _meetingRepository.meetings.Include(m => m.User).ToListAsync(); // Burada Include ve ToListAsync metodlarını ekledik

        if (string.IsNullOrEmpty(role))
        {
            meetings = meetings.Where(i => i.UserId == userId).ToList(); // Burada Where metodunu çağırdık
        }
        return View(meetings);
        }

        [Authorize]
        public IActionResult Edit(int? id){
            if (id == null)
            {
                return NotFound();
            }

            var meetings = _meetingRepository.meetings.FirstOrDefault(i => i.MeetingId == id);

            if (meetings == null)
            {
                return NotFound();
            }

            return View(new MeetingCreateViewModel{
                MeetingId = meetings.MeetingId,
                Title = meetings.Title,
                Description = meetings.Description,
                StartTime = meetings.StartTime,
                StartTimeHour = meetings.StartTimeHour
                
            });
        }

        [Authorize]
        [HttpPost]
        public IActionResult Edit(MeetingCreateViewModel model){
            if (ModelState.IsValid)
            {
                var entityToUpdate = new Meeting
                {
                    MeetingId = model.MeetingId,
                    Title = model.Title,
                    Description = model.Description,
                    StartTime = model.StartTime,
                    StartTimeHour = model.StartTimeHour
                };

                _meetingRepository.EditMeeting(entityToUpdate);
                return RedirectToAction("List");
            }

            return View(model);
        }

        [Authorize] // kullanıcı giriş yapmadan post silme yapmasını engellemek için bunu ekledim
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var meetings = _meetingRepository.meetings.FirstOrDefault(p => p.MeetingId == id);
            if (meetings == null)
            {
                return NotFound();
            }
        
            _meetingRepository.DeleteMeeting(meetings);
        
            return RedirectToAction("List");
        }

    }
}
