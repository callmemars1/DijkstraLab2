namespace MetroPathFinded;

public interface IMetroDataProvider
{
    IEnumerable<Line> GetLines(string city);

    IEnumerable<Station> GetStations(string city);

    IEnumerable<Tuple<Station, Station, int>> GetRoutes(string city);
}
