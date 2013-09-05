using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace RunnerupWeb.Hubs
{
    // This hub has no inbound APIs, since all inbound communication is done
    // via the HTTP API. It's here for clients which want to get continuous
    // notification of changes to the ToDo database.
    [HubName("RunnerUp")]
    public class RunnerUpHub : Hub{}
}
