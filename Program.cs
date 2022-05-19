using MetroPathFinded;

IEnumerable<Station> stations;
IEnumerable<Tuple<Station, Station, int>> routes;
var provider = new SQLiteMetroDataProvider("MetroRoutes.sqlite");
provider.Open();
while (true)
{
    try
    {
        Console.WriteLine("Введите город: ");
        var city = Console.ReadLine();
        Console.WriteLine(city);
        stations = provider.GetStations(city);
        routes = provider.GetRoutes(city);
        break;
    }
    catch (ArgumentException e)
    {
        Console.WriteLine(e.Message);
    }
    catch (Exception ex)
    {
        Console.WriteLine("Ошибка ввода!" + ex.Message);
    }
}
provider.Cloes();


var graph = new int[stations.Count(), stations.Count()];

for (int i = 0; i < stations.Count(); ++i)
    for (int j = 0; j < stations.Count(); ++j)
        graph[i, j] = routes.FirstOrDefault((s) =>
        {
            var (firstSt, secondSt, time) = s;
            var finded = firstSt.Name == stations.ElementAt(i).Name && secondSt.Name == stations.ElementAt(j).Name;
            return finded;
        }, null)?.Item3 ?? 0;

foreach (var i in Enumerable.Range(0, stations.Count()))
    Console.WriteLine($"{i}\t{stations.ElementAt(i).Name}");

while (true)
{
    int id1, id2 = 0;
    List<(int index, int weight)> path;
    try
    {
        Console.WriteLine($"Enter station id (max:{stations.Count() - 1})");
        id1 = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine($"First station is {stations.ElementAt(id1)}");
        id2 = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine($"Second station is {stations.ElementAt(id2)}");
        path = Dijkstra.GetShortestPath(graph, id1, id2);
    }
    catch (Exception ex)
    {
        Console.WriteLine("Wrong input");
        continue;
    }


    if (path.Count() == 0)
    {
        Console.WriteLine("EMPTY");
        continue;
    }

    var fullWeight = 0;
    foreach (var v in path)
    {
        Console.WriteLine($"{stations.ElementAt(v.index).Name}: {v.weight} сек.");
        fullWeight += v.weight;
    }
    Console.WriteLine($"Путь займет:{fullWeight} сек. ({fullWeight / 60} мин.)");
}