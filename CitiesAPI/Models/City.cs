using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CitiesAPI.Models
{
    //[Serializable]
    public class City
    {

        public string Id { get; set; }
        public string HebName { get; set; }
        public string EngName { get; set; }

        public string StreetCount { get; set; }

        

    }
}
