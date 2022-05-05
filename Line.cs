public record Line
{
    int _color;
    string _name;
    string _city;

    public Line(int color, string name, string city)
    {
        _color = color;
        _name = name;
        _city = city;
    }

    public int Color { get => _color; }
    public string Name { get => _name; }
    public string City { get => _city; }
}
