using Core.Models;
using DataStore.EF.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Filters.V2;

namespace WebAPI.Controllers.V2
{
    [ApiVersion("2.0")]
    [ApiController]
    [Route("api/offers")]

    //demonstrate purpose
    //don't forget to override Route becasue of versioning, otherwise route name will be OffersV2
    public class OfferV2Controller : ControllerBase
    {
        private readonly AppDbContext _db;
        public OfferV2Controller(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _db.Offers.ToListAsync());
        }
    }
}
