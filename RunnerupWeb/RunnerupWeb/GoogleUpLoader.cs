using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Web.Hosting;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Fusiontables.v1;
using Google.Apis.Services;
using RunnerupWeb.Models;

namespace RunnerupWeb
{
    public class GoogleUpLoader : IGoogleUploader, IRegisteredObject
    {
        Queue<RunningEvent> _queue;
        private bool _shuttingDown;
        private string _appDomainAppPath;
        private Timer _timer;

        public GoogleUpLoader()
        {
            _queue = new Queue<RunningEvent>();
            HostingEnvironment.RegisterObject(this);
            _timer = new Timer(state => SaveToGoogle(), null, new TimeSpan(0, 0, 0, 50), new TimeSpan(0, 0, 0, 50));
        }

        
        public void QueueEvent(RunningEvent runningEvent)
        {
            lock (_queue)
            {
                _queue.Enqueue(runningEvent);
            }
        }
        public void SaveToGoogle()
        {
            lock (_queue)
                if(!_queue.Any())
                    return;

            var stopwatch = Stopwatch.StartNew();
            var certificate = new X509Certificate2(Path.Combine(_appDomainAppPath, "b6cfce72e7948d656012a831b9f6106d89c7f26e-privatekey.p12"), "notasecret", X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable);

            var credential = new ServiceAccountCredential(
               new ServiceAccountCredential.Initializer("1040291453308-itvvu2ln6p6ek564nem7435deriv5jm7@developer.gserviceaccount.com")
               {
                   Scopes = new[] { FusiontablesService.Scope.Fusiontables }
               }.FromCertificate(certificate));

            // Create the service.
            var fusiontablesService = new FusiontablesService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "nebmo-rulive", //not sure if the name here matters or not
            });
            var sb = new StringBuilder();
            lock (_queue)
            {
                while (_queue.Any())
                {
                    sb.Append(CreateInsertQuery(_queue.Dequeue()));
                }
                
            }
            var response = fusiontablesService.Query.Sql(sb.ToString()).Execute();
            stopwatch.Stop();
            Console.WriteLine(stopwatch.Elapsed);
        }

        private string CreateInsertQuery(RunningEvent runningEvent)
        {
            var values = string.Format("'{0}',{1},{2},{3},'{4}','{5}','{6}','{7}','{8}','{9}'",
                runningEvent.UserName,
                runningEvent.Long.ToString(CultureInfo.InvariantCulture),
                runningEvent.Lat.ToString(CultureInfo.InvariantCulture),
                "'2014-01-01'",
                runningEvent.Date,
                (int) runningEvent.RunningEventType,
                runningEvent.TotalDistance,
                runningEvent.TotalTime,
                runningEvent.Pace,
                runningEvent.HR
                );
            var query =
                @"INSERT INTO 1qQhbN8HTdmClaKsylD-PKtaI_FZbn9qYU3QXrUFk ('UserName','Longitude','Latitude','Date','Time','RunningEventType','TotalDistance','TotalTime','Pace','HR')
                VALUES (" + values + ");";
            return query;
        }

        public IEnumerable<RunningEvent> GetEvents()
        {
            var stopwatch = Stopwatch.StartNew();
            var certificate = new X509Certificate2(Path.Combine(_appDomainAppPath, "b6cfce72e7948d656012a831b9f6106d89c7f26e-privatekey.p12"), "notasecret", X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable);

            var credential = new ServiceAccountCredential(
               new ServiceAccountCredential.Initializer("1040291453308-itvvu2ln6p6ek564nem7435deriv5jm7@developer.gserviceaccount.com")
               {
                   Scopes = new[] { FusiontablesService.Scope.Fusiontables }
               }.FromCertificate(certificate));

            // Create the service.
            var fusiontablesService = new FusiontablesService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "nebmo-rulive", //not sure if the name here matters or not
            });

            var query = "select UserName,Longitude,Latitude,Date,Time,RunningEventType,TotalDistance,TotalTime,Pace,HR from 1qQhbN8HTdmClaKsylD-PKtaI_FZbn9qYU3QXrUFk order by UserName";
            var response = fusiontablesService.Query.Sql(query).Execute();
            foreach (var row in response.Rows)
            {
                yield return new RunningEvent()
                {
                    UserName = row[0].ToString(),
                    Long = double.Parse(row[1].ToString()),
                    Lat = double.Parse(row[2].ToString()),
                    Date = DateTime.Parse(row[4].ToString()),
                    RunningEventType = (RunningEventType) int.Parse(row[5].ToString()),
                    TotalDistance = row[6].ToString(),
                    TotalTime = row[7].ToString(),
                    Pace = row[8].ToString()
                };
            }
            stopwatch.Stop();
            Console.WriteLine(stopwatch.Elapsed);
        }

        public void RemoveOldEvents(string userName)
        {
            
        }

        public string CertificatePath
        {
            get { return _appDomainAppPath; }
            set { _appDomainAppPath = value; }
        }

        public void Stop(bool immediate)
        {
            lock (_queue)
            {
                _shuttingDown = true;
            }
            HostingEnvironment.UnregisterObject(this); 
        }
    }


}