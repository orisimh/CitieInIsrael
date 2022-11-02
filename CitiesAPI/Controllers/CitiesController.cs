using CitiesAPI.Models;
using CitiesAPI.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CitiesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        ICitiesService _CitisService = null;


        public CitiesController(ICitiesService CitisService )
        {
            _CitisService = CitisService;
        }
        // GET: api/<ValuesController>
        [EnableCors("ApiCorsPolicy")]
        [HttpGet]
        public ActionResult<IEnumerable<City>> Get()
        {


            IEnumerable<City> cities = null;

            try
            {
                 cities = _CitisService.GetAllCities();

            }
            catch (Exception e)
            {
                if (e.HResult == -1)
                {
                    return NotFound(e.InnerException);
                }

                return StatusCode(500, e.InnerException);

            }


            return Ok(cities);

        }

        // GET api/<ValuesController>/5
        [EnableCors("ApiCorsPolicy")]
        [HttpGet("{CityId}")]
        public ActionResult<IEnumerable<CityDetails>> GetCityDetails(string CityId)
        {
            IEnumerable<CityDetails> CityDetails = null;

            try
            {
               CityDetails = _CitisService.GetCityDetails(CityId);

            }
            catch(Exception e)
            {
                if(e.HResult== -1)
                {
                    return NotFound(e.InnerException);
                }

                return StatusCode(500, e.InnerException);

            }
            return Ok(CityDetails);
        }


        // GET api/<ValuesController>/5
        [EnableCors("ApiCorsPolicy")]
        [HttpGet ("street/{street}")]
        public ActionResult<IEnumerable<City>> GetCitiesByStreet(string street)
        {
            IEnumerable<string> cities = null;

            try
            {
                cities = _CitisService.GetCitiesByStreet(street);

            }
            catch (Exception e)
            {
            

                return StatusCode(500, e.InnerException);

            }
            return Ok(cities);
        }



    }
}
