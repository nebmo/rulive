using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RunnerupWeb.Models
{
    public enum RunningEventType
    {
        Started,
        GPS,
        Paused,
    }
    public class RunningEvent
    {
        public string UserName { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }
        public RunningEventType RunningEventType { get; set; }
        public string TotalDistance { get; set; }
        public string TotalTime { get; set; }
        public string Pace { get; set; }
        public DateTime Date { get; set; }
    }
}