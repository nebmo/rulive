using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Caching;
using System.Web.Http;
using RunnerupWeb.Controllers;
using RunnerupWeb.Hubs;
using RunnerupWeb.Models;

namespace RunnerupWeb.Controllers
{
    public class ResourceController : ApiControllerWithHub<RunnerUpHub>
    {
        Random random = new Random();
        private Dictionary<string, Resource> _resources;// = new Dictionary<string, Resource>();

        public IEnumerable<Resource> Get()
        {
            _resources = MemoryCache.Default["resource"] == null ? new Dictionary<string, Resource>() : (Dictionary<string, Resource>)MemoryCache.Default["resource"];
            foreach (var resource in _resources)
            {
                yield return resource.Value;
            }
            if (_resources.Count == 0)
            {

                //for (int i = 0; i < 3; i++)
                //{
                //    var resource = new Resource() { RegistrationNumber = "ABC" + i, Positions = new List<RunningEvent>() };
                //    for (int j = 0; j < 3; j++)
                //    {
                //        var latlong = new RunningEvent()
                //            {
                //                Lat = GetRandomNumber(59 + i, 65 + i),
                //                Long = GetRandomNumber(10 + i, 15 + i),
                //                RunningEventType = RunningEventType.GPS,
                //                Pace = "5.04",
                //                TotalDistance = "23km",
                //                TotalTime = "1h45min",
                //                UserName = resource.RegistrationNumber
                //            };
                //        resource.Positions.Add(latlong);

                //    }
                //    resource.Positions[0].RunningEventType = RunningEventType.Started;
                //    resource.Positions[2].RunningEventType = RunningEventType.Paused;
                //    _resources.Add(resource.RegistrationNumber,resource);
                //    yield return resource;
                //}
            }
            yield return null;
        }


        // GET api/resource/5
        public string Get(int id)
        {
            return "value";
        }

        //POST api/resource
        public HttpResponseMessage Post(RunningEvent runningEvent)
        {
            _resources = MemoryCache.Default["resource"] == null ? new Dictionary<string, Resource>() : (Dictionary<string, Resource>)MemoryCache.Default["resource"];

            runningEvent.Date = DateTime.Now;
            //Notify the connected clients
            Hub.Clients.All.addRunningEvent(runningEvent);
            var response = Request.CreateResponse(HttpStatusCode.Created, runningEvent);
            //string link = Url.Link("apiRoute", new { controller = "todo", id = item.ID });
            //response.Headers.Location = new Uri(link);
            Resource resource = null;
            if(!_resources.ContainsKey(runningEvent.UserName))
            {
                resource = new Resource()
                    {
                        RegistrationNumber = runningEvent.UserName,
                        Positions = new List<RunningEvent>(){runningEvent}
                    };
                _resources.Add(runningEvent.UserName,resource);
            }
            else
            {
                resource = _resources[runningEvent.UserName];
                TimeSpan span1, span2;


                if (ShouldResetRoute(runningEvent, resource))
                {
                    
                     resource.Positions = new List<RunningEvent>() { runningEvent };
                    
                }
                else
                {
                    resource.Positions.Add(runningEvent);
                }
                //if (resource.Positions.Last().Date.AddMinutes(5) < DateTime.Now || resource.Positions.Last().Date < DateTime.Now && resource.Positions.Last().RunningEventType == RunningEventType.Paused) //&& resource.Positions.Last().TotalTime > runningEvent.TotalTime))
                //{
                //    resource.Positions = new List<RunningEvent>(){runningEvent};
                //}
            }
            
            
            MemoryCache.Default.Set("resource", _resources, DateTimeOffset.Now.AddDays(5));
            
            

            return response;
        }

        private bool ShouldResetRoute(RunningEvent runningEvent, Resource resource)
        {
            if (runningEvent.RunningEventType == RunningEventType.Started && string.IsNullOrEmpty(runningEvent.TotalTime))
                return true;
            TimeSpan span1, span2;
            return TimeSpan.TryParse(runningEvent.TotalTime.Replace("m", "").Replace("h", ""), out span1) 
                   &&
                   TimeSpan.TryParse(resource.Positions.Last().TotalTime.Replace("m", "").Replace("h", ""), out span2)
                   && span1 < span2;
        }

        // PUT api/resource/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/resource/5
        public void Delete(int id)
        {
        }

        public double GetRandomNumber(double minimum, double maximum)
        {
            return random.NextDouble() * (maximum - minimum) + minimum;
        }

    }



    public class Resource
    {
        public string RegistrationNumber { get; set; }
        public List<RunningEvent> Positions { get; set; }
    }

   
}
