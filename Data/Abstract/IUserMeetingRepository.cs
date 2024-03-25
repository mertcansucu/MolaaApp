using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MolaaApp.Entity;

namespace MolaaApp.Data.Abstract
{
    public interface IUserMeetingRepository
    {
        IQueryable<UserMeeting> userMeetings { get; }

        void AddUserMeeting(UserMeeting userMeeting);
        void RemoveUserMeeting(UserMeeting userMeeting);
    }
}