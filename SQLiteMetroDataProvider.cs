using Microsoft.Data.Sqlite;

namespace MetroPathFinded;

public class SQLiteMetroDataProvider : IMetroDataProvider
{
    SqliteConnection _connection;
    public SQLiteMetroDataProvider(string path)
    {
        _connection = new SqliteConnection($"Data Source={path};Mode=ReadOnly;");
        
    }
    public void Open() 
    {
        _connection.Open();
    }
    public void Cloes() 
    {
        _connection.Close();
    }
    public IEnumerable<Line> GetLines()
    {
        string sqlQuery = "SELECT line_name, color, c.city_name FROM lines "
                        + "LEFT JOIN cities c on lines.city_id_FK = c.id";
        SqliteCommand cmd = _connection.CreateCommand();
        cmd.CommandText = sqlQuery;
        var lines = new List<Line>();
        using (SqliteDataReader reader = cmd.ExecuteReader())
        {
            if (reader.HasRows) // если есть данные
            {
                while (reader.Read())   // построчно считываем данные
                {
                    var line = new Line(
                        color: reader.GetInt32(1),
                        name: reader.GetString(0),
                        city: reader.GetString(2)
                        );
                    lines.Add(line);
                }
            }
        }
        return lines;
    }

    public IEnumerable<Tuple<Station, Station, int>> GetRoutes()
    {
        string sqlQuery = "SELECT s1.name,s1.latitude,s1.longitude,s2.name,s2.latitude,s2.longitude,n.approximate_time FROM neighboring n "
                        + "LEFT JOIN stations s1 on s1.id = n.first_station_id_FK "
                        + "LEFT JOIN stations s2 on s2.id = n.second_station_id_FK";
        SqliteCommand cmd = _connection.CreateCommand();
        cmd.CommandText = sqlQuery;
        var routes = new List<Tuple<Station, Station, int>>();
        using (SqliteDataReader reader = cmd.ExecuteReader())
        {
            if (reader.HasRows) // если есть данные
            {
                while (reader.Read())   // построчно считываем данные
                {
                    var startStation = new Station(
                        name: reader.GetString(0),
                        latitude: reader.GetDouble(1),
                        longitude: reader.GetDouble(2)
                        );
                    var endStation = new Station(
                        name: reader.GetString(3),
                        latitude: reader.GetDouble(4),
                        longitude: reader.GetDouble(5)
                        );
                    var time = reader.GetInt32(6);

                    routes.Add(new Tuple<Station, Station, int>(startStation, endStation, time));
                }
            }
        }
        return routes;
    }

    public IEnumerable<Station> GetStations()
    {
        string sqlQuery = "SELECT s.name, s.latitude, s.longitude, l.line_name, l.color, c.city_name FROM ( stations s "
                        + "LEFT JOIN lines l on l.id = s.line_id_FK )"
                        + "LEFT JOIN cities c on c.id = l.city_id_FK";
        SqliteCommand cmd = _connection.CreateCommand();
        cmd.CommandText = sqlQuery;
        var stations = new List<Station>();
        using (SqliteDataReader reader = cmd.ExecuteReader())
        {
            if (reader.HasRows) // если есть данные
            {
                while (reader.Read())   // построчно считываем данные
                {
                    var station = new Station(
                        name: reader.GetString(0),
                        latitude: reader.GetDouble(1),
                        longitude: reader.GetDouble(2),
                        line: new Line(
                            name: reader.GetString(3),
                            color: reader.GetInt32(4),
                            city: reader.GetString(5)
                            )
                        );
                    stations.Add(station);
                }
            }
        }
        return stations;
    }
}
