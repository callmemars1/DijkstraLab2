public record Station
{
    string _name;
    double _latitude;
    double _longitude;
    Line? _line;

    public Station(string name, double latitude, double longitude, Line? line = null)
    {
        _name = name;
        _latitude = latitude;
        _longitude = longitude;
        _line = line;
    }

    public string Name { get => _name; }
    public double Latitude { get => _latitude; }
    public double Longitude { get => _longitude; }
    internal Line? Line { get => _line; init => _line = value; }
}