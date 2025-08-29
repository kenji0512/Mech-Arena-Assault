using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public static Pathfinding Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public List<Vector2Int> FindPath(Vector2Int start, Vector2Int goal)
    {
        PriorityQueue<Vector2Int> open = new();
        HashSet<Vector2Int> closed = new();
        Dictionary<Vector2Int, Vector2Int> cameFrom = new();
        Dictionary<Vector2Int, int> costSoFar = new();

        open.Enqueue(start, 0);
        cameFrom[start] = start;
        costSoFar[start] = 0;

        int[] dx = { 1, -1, 0, 0 };
        int[] dy = { 0, 0, 1, -1 };

        while (open.Count > 0)
        {
            var current = open.Dequeue();

            if (current == goal)
                break;

            for (int i = 0; i < 4; i++)
            {
                Vector2Int next = current + new Vector2Int(dx[i], dy[i]);

                if (!MapManager.Instance.HasTile(next)) continue;
                if (MapManager.Instance.GetTileAt(next).IsBlocked) continue;

                int newCost = costSoFar[current] + 1;

                if (!costSoFar.ContainsKey(next) || newCost < costSoFar[next])
                {
                    costSoFar[next] = newCost;
                    int priority = newCost + Heuristic(next, goal);
                    open.Enqueue(next, priority);
                    cameFrom[next] = current;
                }
            }
        }

        return ReconstructPath(start, goal, cameFrom);
    }

    private List<Vector2Int> ReconstructPath(Vector2Int start, Vector2Int goal, Dictionary<Vector2Int, Vector2Int> cameFrom)
    {
        List<Vector2Int> path = new();
        Vector2Int current = goal;

        if (!cameFrom.ContainsKey(goal))
            return path;

        while (current != start)
        {
            path.Add(current);
            current = cameFrom[current];
        }

        path.Reverse();
        return path;
    }

    private int Heuristic(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }
}
