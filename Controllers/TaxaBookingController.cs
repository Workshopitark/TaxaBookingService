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




    private readonly ILogger<TaxaBookingController> _logger;
    private readonly IModel _channel;


    // constructor
    public TaxaBookingController(ILogger<TaxaBookingController> logger, IConfiguration configuration)
    {
        _logger = logger;

        var factory = new ConnectionFactory { HostName = "localhost" };
        using var connection = factory.CreateConnection();
        _channel = connection.CreateModel();

    }





    // post metode
    [HttpPost("upload"), DisableRequestSizeLimit]
    public async Task<IActionResult> Post([FromBody] TaxaBooking taxaBooking)
    {
        _logger.LogInformation("funktion ramt");

        _channel.QueueDeclare(queue: "hello",
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

        const string message = "Hello World daniel!";
        var body = Encoding.UTF8.GetBytes(message);

        _channel.BasicPublish(exchange: string.Empty,
                             routingKey: "hello",
                             basicProperties: null,
                             body: body);


    }


}