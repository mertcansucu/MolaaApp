using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MolaaApp.Entity;

namespace MolaaApp.Data.Abstract
{
    public interface IMeetingRepository
    {
        IQueryable<Meeting> meetings {get; }

        void MeetingPost(Meeting meeting); 
    }
}