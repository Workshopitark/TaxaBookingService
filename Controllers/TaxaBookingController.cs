using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TaxaBookingService;
using System.Xml.Linq;
using System.Text;
using RabbitMQ.Client;
using System.Threading.Channels;

namespace TaxaBookingService;




[ApiController]
[Route("[controller]")]
public class TaxaBookingController : ControllerBase
{

    // constructor
    public TaxaBookingController(ILogger<TaxaBookingController> logger, IConfiguration configuration)
    {
        _logger = logger;

        var factory = new ConnectionFactory { HostName = "localhost" };
        using var connection = factory.CreateConnection();
        _channel = connection.CreateModel();

    }


    private static List<TaxaBooking> _bookings = new List<TaxaBooking>
    {
        
    };

    private readonly ILogger<TaxaBookingController> _logger;
    private readonly IModel _channel;


    //get metoder
    [HttpGet("{BookingId}", Name = "GetBookingById")]
    public TaxaBooking Get(int BookingId)
    {
        
    }



    // post metode
    [HttpPost("upload"), DisableRequestSizeLimit]
    public async Task<IActionResult> Post([FromBody] TaxaBooking taxaBooking)
    {
        _logger.LogInformation("funktion ramt");

        using var channel = connection.CreateModel();

        //

        channel.QueueDeclare(queue: "hello",
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        const string message = "Hello World daniel!";
        var body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish(exchange: string.Empty,
                             routingKey: "hello",
                             basicProperties: null,
                             body: body);

    }



}