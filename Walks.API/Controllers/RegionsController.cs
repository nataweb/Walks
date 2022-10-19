using Microsoft.AspNetCore.Mvc;
using Walks.API.Models;

namespace Walks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegionsController : Controller
    {
        [HttpGet]
        public IActionResult GetAllRegions()
        {
           var regions = new List<Region>()
           {
                new Region
                {
                    Id = Guid.NewGuid(),
                    Name = "Wellington",
                    Code = "WLG",
                    Area = 227755,
                    Lat = -1.8822,
                    Long = 299.88,
                    Population = 500000
                },
                new Region
                {
                    Id = Guid.NewGuid(),
                    Name = "Auckland",
                    Code = "AUC",
                    Area = 227755,
                    Lat = -1.8822,
                    Long = 299.88,
                    Population = 500000

                }
           };  
           
            return Ok(regions);   
        }

    }
}
