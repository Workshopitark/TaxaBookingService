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
            Id = 1,
            Name = "Internationale vognmænd A/S",
            Address1 = "Nydamsvej 8",
            Address2 = null,
            PostalCode = 8362,
            City = "H�rning",
            TaxNumber = "DK-75627732",
            ContactName = "Dennis Jørgensen"
        }

    };
}