using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RunnerupWeb.Models;

namespace RunnerupWeb
{
    public interface IGoogleUploader
    {
        void QueueEvent(RunningEvent runningEvent);
        IEnumerable<RunningEvent> GetEvents();
        void RemoveOldEvents(string userName);
        string CertificatePath { get; set; }
    }
}