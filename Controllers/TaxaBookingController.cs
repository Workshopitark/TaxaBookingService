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
using System.Text.Json;

namespace TaxaBookingService;




[ApiController]
[Route("[controller]")]
public class TaxaBookingController : ControllerBase
{

    private readonly ILogger<TaxaBookingController> _logger;
    private readonly IModel _channel;
    private readonly string _MQHostName;


    // constructor
    public TaxaBookingController(ILogger<TaxaBookingController> logger, IConfiguration configuration)
    {
        _logger = logger;


        _MQHostName = configuration["MQHostName"] ?? "rabbitmq";

        var factory = new ConnectionFactory { HostName = _MQHostName };
        using var connection = factory.CreateConnection();
        _channel = connection.CreateModel();

    }


    private List<TaxaBooking> _bookings = new List<TaxaBooking>
    {

        // hard code data

    };





    // post metode
    [HttpPost("upload"), DisableRequestSizeLimit]
    public async Task<IActionResult> Post([FromBody] TaxaBooking taxaBooking)
    {
        _logger.LogInformation("funktion ramt");

        var newTaxaBooking = new TaxaBooking
        {
            Kundenavn = taxaBooking.Kundenavn,
            Starttidspunkt = taxaBooking.Starttidspunkt,
            Startsted = taxaBooking.Startsted,
            Endested = taxaBooking.Endested
        };


        //

        _channel.QueueDeclare(queue: "planqueue",
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);


        var message = JsonSerializer.SerializeToUtf8Bytes(newTaxaBooking);

        _channel.BasicPublish(exchange: string.Empty,
                             routingKey: "planqueue",
                             basicProperties: null,
                             body: message);

        Console.WriteLine("status ok");

        return Ok(taxaBooking);
    }



}