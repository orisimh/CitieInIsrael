using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CitiesAPI.Models
{
    public class CityDetails
    {
        public string Id { get; set; }

        public string HebName { get; set; }


        public string RegionId { get; set; }
        public string RegionName { get; set; }

        public string SubRegionId { get; set; }
        public string SubRegionName { get; set; }

        public string RegionalCouncilId { get; set; }
        public string RegionalCouncilIdName { get; set; }
        public string StreetCount { get; set; }

    }
}
