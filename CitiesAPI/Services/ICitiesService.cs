using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CitiesAPI.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CitiesAPI.Services
{
    public interface ICitiesService
    {

         public  IEnumerable<City> GetAllCities();

        //public string GetCount(string id);
        public IEnumerable<CityDetails> GetCityDetails(string id);



         public IEnumerable<String> GetCitiesByStreet(string name);

    }
}
