namespace MetroPathFinded;

public interface IMetroDataProvider
{
    IEnumerable<Line> GetLines();

    IEnumerable<Station> GetStations();

    IEnumerable<Tuple<Station, Station, int>> GetRoutes();
}
