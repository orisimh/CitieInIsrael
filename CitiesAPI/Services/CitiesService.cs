using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using CitiesAPI.Models;
using System.Text.RegularExpressions;

namespace CitiesAPI.Services
{
    public class CitiesService : ICitiesService
    {
            string numofrecords= string.Empty;
            string pattern = @" \((.*?)\)";


        public IEnumerable<City> GetAllCities() 
            {

            string apiUrl = "https://data.gov.il/api/3/action/datastore_search?resource_id=5c78e9fa-c2e2-4771-93ff-7f400a12f7ba&limit=2000";

            var json = string.Empty;
            using (HttpClient client = new HttpClient())
            {

                var result = client.GetAsync(apiUrl).Result;
                json = result.Content.ReadAsStringAsync().Result;

                JObject rss = JObject.Parse(json);


                var result1 = rss["result"];
                var records_new = JArray.Parse(result1["records"].ToString());

                pattern = @" \((.*?)\)";

                var AllCities = records_new.Select(i =>
                new City
                {
                    Id = i["סמל_ישוב"].ToString(),
                    HebName = Regex.Replace(i["שם_ישוב"].ToString(), pattern, string.Empty),
                    EngName = i["שם_ישוב_לועזי"].ToString()
                })
                .Distinct()
                .Where(c => c.HebName.Trim() != "לא רשום")
                .ToList();


                if (AllCities.Count == 0)
                {
                    Exception e = new Exception("No Found Cities");
                    e.HResult = -1;
                    throw e;
                }

                return AllCities;

                //var grouped = GetCountByCity();
                //var query =
                //from cityg in grouped
                //join city in records_new
                //on cityg.Metric equals city["סמל_ישוב"].ToString()

                //select new City
                //{
                //    Id = city["סמל_ישוב"].ToString(),
                //    HebName = Regex.Replace(city["שם_ישוב"].ToString(), pattern, string.Empty),
                //    EngName = city["שם_ישוב_לועזי"].ToString(),
                //    StreetCount = cityg.Count.ToString()
                //};

                //AllCities = query.ToList();
                //return AllCities;

            }


        }

   



        public IEnumerable<String> GetCitiesByStreet(string StreetName)
        {


            string fullURL = $"https://data.gov.il/api/3/action/datastore_search?limit=10000&resource_id=9ad3862c-8391-4b2f-84a4-2d4c68625f4b&q={{\"שם_רחוב\":\"{StreetName}\"}}";

            fullURL = Uri.EscapeUriString(fullURL);


            var json = string.Empty;
            using (HttpClient client = new HttpClient())
            {

                var result = client.GetAsync(fullURL).Result;
                json = result.Content.ReadAsStringAsync().Result;

                JObject rss = JObject.Parse(json);

                var result1 = rss["result"];
                var records_new = JArray.Parse(result1["records"].ToString());

                var CitiesIds = from c in records_new.Select(i => i["סמל_ישוב"]).Values<string>()
                          .Where(c => c.Trim() != "לא רשום")
                          .Distinct()
                          select new { City = c.Trim() }.City;


              

                return CitiesIds;

            }
        }




        public IEnumerable<CityDetails>GetCityDetails(string id)
        {
            var json = string.Empty;
            string apiUrl = $"https://data.gov.il/api/3/action/datastore_search?resource_id=5c78e9fa-c2e2-4771-93ff-7f400a12f7ba&limit=2000&q={{\"סמל_ישוב\":\"{id}\"}}";

            using (HttpClient client = new HttpClient())
            {

                var result = client.GetAsync(apiUrl).Result;
                json = result.Content.ReadAsStringAsync().Result;
                JObject rss = JObject.Parse(json);


                var result1 = rss["result"];
                var records_new = JArray.Parse(result1["records"].ToString());

                var CityDetail = records_new.Select(i =>
                new CityDetails
                {
                    Id = i["סמל_ישוב"].ToString(),
                    HebName = i["שם_ישוב"].ToString(),
                    RegionId = i["סמל_נפה"].ToString(),
                    RegionName = i["שם_נפה"].ToString(),
                    SubRegionId = i["סמל_לשכת_מנא"].ToString(),
                    SubRegionName = i["לשכה"].ToString(),
                    RegionalCouncilId = i["סמל_מועצה_איזורית"].ToString(),
                    RegionalCouncilIdName = i["שם_מועצה"].ToString(),
                    StreetCount = GetCount(id),
                });

                if (!CityDetail.Any<CityDetails>())
                {

                    Exception e = new Exception("No Found Cities");
                    e.HResult = -1;
                    throw e;
                }

                return CityDetail;

            }
        }


        #region Private Methods
        private string GetCount(string id)
        {

            string fullURL = $"https://data.gov.il/api/3/action/datastore_search?limit=10000&resource_id=9ad3862c-8391-4b2f-84a4-2d4c68625f4b&q={{\"סמל_ישוב\":\"{id}\"}}";

            using (HttpClient client = new HttpClient())
            {

                fullURL = Uri.EscapeUriString(fullURL);
                var result = client.GetAsync(fullURL).Result;
                var json = result.Content.ReadAsStringAsync().Result;


                JObject rss = JObject.Parse(json);
                numofrecords = rss["result"]["total"].ToString();

                if (numofrecords == "0")
                {
                    Exception e = new Exception("No Found Streets");
                    e.HResult = -1;
                    throw e;
                }

            }

            return numofrecords;

        }

        private List<dynamic> GetCountByCity()
        {

            string fullURL = $"https://data.gov.il/api/3/action/datastore_search?limit=60000&resource_id=9ad3862c-8391-4b2f-84a4-2d4c68625f4b";

            using (HttpClient client = new HttpClient())
            {

                fullURL = Uri.EscapeUriString(fullURL);
                var result = client.GetAsync(fullURL).Result;
                var json = result.Content.ReadAsStringAsync().Result;


                JObject rss = JObject.Parse(json);
                var records = rss["result"]["records"];

                var lines = records.Select(i =>
                new City
                {
                    Id = i["סמל_ישוב"].ToString(),
                }).

                    GroupBy(info => info.Id)
                        .Select(group => new {
                            Metric = group.Key,
                            Count = group.Count()
                        })
                        .OrderByDescending(x => x.Count)

                        ;

                var groupedList = lines.ToList<dynamic>();
                return groupedList;



            }



        }
        #endregion

    }
}
