using Covid.Api.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Covid.Api.Hubs
{
    public class CovidHub : Hub
    {
        private readonly CovidService _covidService;

        public CovidHub(CovidService covidService)
        {
            _covidService = covidService;
        }

        public async Task GetCovidList()
        {
            var data = _covidService.GetCovidList();

            await Clients.All.SendAsync("ReceiveCovidList", data);
        }
    }
}
