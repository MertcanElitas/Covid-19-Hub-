using Covid.Api.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Covid.Api.Models
{
    public class CovidService
    {
        private readonly CovidDbContext _dbContext;
        private readonly IHubContext<CovidHub> _hub;

        public CovidService(CovidDbContext dbContext, IHubContext<CovidHub> hub)
        {
            _dbContext = dbContext;
            _hub = hub;
        }

        public IQueryable<Covid> GetList()
        {
            var data = _dbContext.Covids.AsQueryable();

            return data;
        }

        public async Task SaveCovid(Covid covid)
        {
            await _dbContext.Covids.AddAsync(covid);
            await _dbContext.SaveChangesAsync();
            await _hub.Clients.All.SendAsync("ReceiveCovidList",GetCovidList() );
        }

        public List<CovidChart> GetCovidList()
        {
            var covidCharts = new List<CovidChart>();

            using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "Select tarih,[1],[2],[3],[4],[5] From (select [City],[Count],Cast([CovidDate] as date) as tarih From Covids) as covidT " +
                    "PIVOT (Sum(Count) For City In([1],[2],[3],[4],[5]))as ptable order by tarih asc";

                command.CommandType = System.Data.CommandType.Text;

                _dbContext.Database.OpenConnection();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CovidChart covidChart = new CovidChart();

                        covidChart.CovidDate = reader.GetDateTime(0).ToShortDateString();

                        Enumerable.Range(1, 5).ToList().ForEach(x =>
                        {
                            if (System.DBNull.Value.Equals(reader[x]))
                                covidChart.Counts.Add(0);
                            else
                                covidChart.Counts.Add(reader.GetInt32(x));
                        });

                        covidCharts.Add(covidChart);
                    }
                }
            }

            return covidCharts;
        }
    }
}
