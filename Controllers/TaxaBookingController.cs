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
    private readonly string _pathCSV;



    // constructor
    public TaxaBookingController(ILogger<TaxaBookingController> logger, IConfiguration configuration)
    {
        _logger = logger;


        _MQHostName = configuration["MQHostName"] ?? "rabbitmq";
        _pathCSV = configuration["pathCSV"] ?? string.Empty;

        _logger.LogInformation(_pathCSV + _MQHostName);

        var factory = new ConnectionFactory { HostName = _MQHostName };
        var connection = factory.CreateConnection();
        _channel = connection.CreateModel();


    }





    // get på csv fil



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


    [HttpGet("getplan"), DisableRequestSizeLimit]
    public async Task<IActionResult> GetPlan()
    {
        _logger.LogInformation("funktion ramt, build test");
        // kommentar til test
        CSVService cSVService = new CSVService();

        List<TaxaBooking> taxaBookings = cSVService.Read(_pathCSV);

        return Ok(taxaBookings);
    }

    [HttpGet("version")]
    public IEnumerable<string> Get()
    {
        _logger.LogInformation("rammer version metode");

        var properties = new List<string>();
        var assembly = typeof(Program).Assembly;
        foreach (var attribute in assembly.GetCustomAttributesData())
        {
            properties.Add($"{attribute.AttributeType.Name} - {attribute.ToString()}");
        }
        return properties;

        _logger.LogInformation("Slut version funk");
    }


}