using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalR.Api.Hubs;

namespace SignalR.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly IHubContext<MyHub> _hubContext;

        public NotificationController(IHubContext<MyHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpPost("SendTeamCount")]
        public async Task<IActionResult> Index(int teamCount)
        {
            MyHub.MaxTeamCount = teamCount;

            await _hubContext.Clients.All.SendAsync("SendTeamCount", $"Selam Arkadaşlar Takım Şuan {teamCount} kişidir.");

            return Ok();
        }
    }
}