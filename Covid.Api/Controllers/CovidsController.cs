using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Covid.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Covid.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CovidsController : ControllerBase
    {
        private readonly CovidService _covidService;

        public CovidsController(CovidService covidService)
        {
            _covidService = covidService;
        }

        [HttpPost("SaveCovid")]
        public async Task<IActionResult> SaveCovid(Covid.Api.Models.Covid covid)
        {
            await _covidService.SaveCovid(covid);

            //IQueryable<Covid.Api.Models.Covid> covidList = _covidService.GetList();

            return Ok(_covidService.GetCovidList());
        }

        [HttpGet("InitiliazeCovid")]
        public IActionResult InitiliazeCovid()
        {
            Random rnd = new Random();

            Enumerable.Range(1, 10).ToList().ForEach(x =>
            {
                foreach (CityEnum item in Enum.GetValues(typeof(CityEnum)))
                {
                    if (item == CityEnum.Null)
                        continue;

                    var newCovid = new Covid.Api.Models.Covid
                    {
                        City = item,
                        Count = rnd.Next(100, 1000),
                        CovidDate = DateTime.Now.AddDays(x)
                    };

                    _covidService.SaveCovid(newCovid).Wait();

                    Thread.Sleep(1000);

                }
            });

            return Ok("Random Datalar Kaydedildi");
        }
    }
}