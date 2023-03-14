using TaxaBookingService;

namespace TaxaBookingService;

public class CSVService
{

    public List<TaxaBooking> Read(string path)
    {
        List<TaxaBooking> taxaBookings = new List<TaxaBooking>();

        var lines = File.ReadAllLines(path);

        foreach (var line in lines)
        {
            var values = line.Split(",");

            var taxaBooking = new TaxaBooking(values[0], DateTime.Parse(values[1]), values[2], values[3]);
            taxaBookings.Add(taxaBooking);
        }

        return taxaBookings;
    }
    


}