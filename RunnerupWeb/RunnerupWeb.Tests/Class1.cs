using System;
using System.Linq;
using NUnit.Framework;
using RunnerupWeb.Models;

namespace RunnerupWeb.Tests
{
    [TestFixture]
    public class Class1
    {
        [Test]
        public void test()
        {
            var @event = new RunningEvent()
            {
                Date = DateTime.Now,
                HR = 0,
                Lat = 59.4212238,
                Long = 17.8588099,
                Pace = "4.5m/s",
                RunningEventType = RunningEventType.GPS,
                TotalDistance = "12",
                TotalTime = "13",
                UserName = "Weide"

            };

            GoogleUpLoader.SaveToGoogle(@event);

        }

        [Test]
        public void test2()
        {
            var result = GoogleUpLoader.GetEvents().ToList();

        }
    }

}
