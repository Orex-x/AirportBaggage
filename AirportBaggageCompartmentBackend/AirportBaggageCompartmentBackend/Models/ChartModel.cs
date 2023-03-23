namespace AirportBaggageCompartmentBackend.Models;

public class ChartModel
{
    public Dictionary<string, int> BarData { get; set; }
    
    public Dictionary<string, int> LineData { get; set; }
}