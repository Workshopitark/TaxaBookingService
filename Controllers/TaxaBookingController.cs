using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TaxaBookingService;
using System.Xml.Linq;

namespace TaxaBookingService;

[ApiController]
[Route("[controller]")]
public class TaxaBookingController : ControllerBase
{



    private readonly ILogger<TaxaBookingController> _logger;

    public TaxaBookingController(ILogger<TaxaBookingController> logger, IConfiguration configuration)
    {
        _logger = logger;
        // _imagePath = configuration["ImagePath"] ?? String.Empty;
    }


    [HttpPost("upload"), DisableRequestSizeLimit]
    public IActionResult UploadBooking()
    {
        _logger.LogInformation("funktion ramt");


    }



    private static List<TaxaBooking> _customers = new List<TaxaBooking>() {

        new TaxaBooking()
        {
            Kundenavn = "Peter Petersen",
            Starttidspunkt = DateTime.Now,
            Startsted = "Nydamsvej 8",
            Endested = "Nydamsvej 9",
        }

    };
}