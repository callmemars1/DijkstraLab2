namespace MetroPathFinded;


using Path = List<(int index, int weight)>;

public class Dijkstra
{
    /// <summary>
    /// Метод находит кратчайший путь из sourceIndex в destinationIndex
    /// Матрица должна быть в виде
    /// <code>
    ///     a   b   c   d
    /// a   0   1   3   7
    /// b   1   0   5   0
    /// c   3   5   0   4
    /// d   7   0   4   0
    /// </code>
    /// Где matrix[a,b] - расстояние между точками.  
    /// Если пути между точками не существует, в матрице должен быть проставлен 0
    /// </summary>
    /// <param name="graph">Матрица с весами</param>
    /// <param name="sourceIndex">Точка, в которой начинается путь</param>
    /// <param name="destinationIndex">Точка, в которой заканчивается путь</param>
    /// <returns>Набор индексов точек, по которым можно кратчайшим образом попасть из sourceIndex в destinationIndex</returns>
    public static Path GetShortestPath(int[,] graph,
                                            int sourceIndex,
                                            int destinationIndex)
    {
        // Число всех вершин (точек)
        var vertexCount = graph.GetLength(0);

        // Массив, который содержит все возможные дистанции
        // из sourceIndex в любую другую точку
        var distances = new int[vertexCount];

        // Массив, который содержит информацию, что кратчайший путь
        // до точки уже найден
        var shortestPathSet = new bool[vertexCount];

        // Массив, который хранит родительский узел
        // Чтобы легко восстанавливать пути
        var parent = new int[vertexCount];

        // init
        for (int i = 0; i < vertexCount; ++i)
        {
            distances[i] = int.MaxValue;
            shortestPathSet[i] = false;
            parent[i] = -1;
        }

        // Дистанция от начальной точки до самой себя равна нулю
        distances[sourceIndex] = 0;

        // Ищем кратчайший путь до всех точек, пока не найдем путь до destinationIndex
        for (int counter = 0; counter < vertexCount - 1; ++counter)
        {
            // Ищем ближайшую точку
            int closestVertex = FindClosestVertex(distances, shortestPathSet, vertexCount);

            // Отмечаем, что мы нашли кратчайший путь до такой точки
            shortestPathSet[closestVertex] = true;


            // Обновляем веса для точек, в которые можно попасть из найденной
            for (int vertex = 0; vertex < vertexCount; ++vertex)
                // Обновляем вес для точки, только в том случае,
                // если кратчайший путь до нее еще не найден
                if (shortestPathSet[vertex] == false                                                // Кратчайший путь не найдке
                    && graph[closestVertex, vertex] != 0                                            // Путь существует
                    && distances[closestVertex] != int.MaxValue                                     // Путь существует до ближайшей найденной точки 
                    && distances[closestVertex] + graph[closestVertex, vertex] < distances[vertex]) // Путь из стартовой точки в рассматриваемою через найденную ближайшую меньше, чем не через неё
                {
                    parent[vertex] = closestVertex;
                    distances[vertex] = distances[closestVertex] + graph[closestVertex, vertex];
                }
            // Если мы нашли путь до нужной точки, то выходим из цикла
            if (closestVertex == destinationIndex)
                break;
        }

        // Если нам не удалось найти путь, возвращаем пустой путь
        if (shortestPathSet[destinationIndex] == false)
            return new Path();

        return CreatePath(parent, graph, destinationIndex);
    }


    /// <summary>
    /// Метод ищет кратчайший путь до точки из тех, что еще не обработаны
    /// </summary>
    /// <param name="distances">Массив, который содержит все 
    /// возможные дистанции из начальной точки в любую другую точку</param>
    /// <param name="shortestPathSet">Массив, который содержит информацию, 
    /// что кратчайший путь до точки уже найден или не найден</param>
    /// <param name="vertexCount">Количество точек</param>
    /// <returns></returns>
    private static int FindClosestVertex(int[] distances, bool[] shortestPathSet, int vertexCount)
    {
        int minWeight = int.MaxValue;
        int minWeightVertexIndex = -1;

        for (int i = 0; i < vertexCount; ++i)
            if (shortestPathSet[i] == false && distances[i] <= minWeight)
            {
                minWeight = distances[i];
                minWeightVertexIndex = i;
            }

        return minWeightVertexIndex;
    }

    /// <summary>
    /// Метод собирает путь из данных, полученных при поиске пути
    /// Возвращает пустой путь, если решение не найдено
    /// </summary>
    /// <param name="graph">Матрица весов</param>
    /// <param name="parent">Массив, который содержит родительские точки</param>
    /// <returns>Путь</returns>
    private static Path CreatePath(int[] parent, int[,] graph, int destinationIndex)
    {
        // Если для пути нет родительской точки, возвращаем пустой путь
        if (parent[destinationIndex] == -1)
            return new Path();

        var solution = new Path();
        int i = destinationIndex;
        // проходим по всем родительским точкам для построения пути
        while (i != -1)
        {
            solution.Add((i, graph[i,parent[i] != -1 ? parent[i] : i]));
            i = parent[i];
        }
        solution.Reverse(); // разворачиваем, чтобы восстановить порядок
        return solution;
    }
}
