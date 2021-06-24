using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Covid.Api.Models
{
    public enum CityEnum
    {
        Null = 0,
        Istanbul = 1,
        Ankara = 2,
        Sivas = 3,
        Konya = 4,
        Balikesir = 5,

    }
    public class Covid
    {
        public int Id { get; set; }
        public int Count { get; set; }
        public CityEnum City { get; set; }
        public DateTime CovidDate { get; set; }

    }
}
