using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;

[System.Serializable]
enum BatteryObstacleType
{
    B1x1H,
    B1x1V,
    B2x1H,
    B2x1V,
    B1x2H,
    B1x2V,
    B2x2H,
    B2x2V,
}

[System.Serializable]
struct BatteryObstacle
{
    public Vector2Int Position;
    public BatteryObstacleType Type;
}

[System.Serializable]
struct BatteryGame
{
    public Vector2Int StartPos;
    public Vector2Int EndPos;
    public List<BatteryObstacle> Obstacles;
}

public class BatteryMinigame : MinigameBase
{
    [SerializeField] private MinigameGrid grid;

    [SerializeField] private GameObject powerSymbolPrefab;
    [SerializeField] private GameObject b1x1hPrefab;
    [SerializeField] private GameObject b1x1vPrefab;
    [SerializeField] private GameObject b2x1hPrefab;
    [SerializeField] private GameObject b2x1vPrefab;
    [SerializeField] private GameObject b1x2hPrefab;
    [SerializeField] private GameObject b1x2vPrefab;
    [SerializeField] private GameObject b2x2hPrefab;
    [SerializeField] private GameObject b2x2vPrefab;
    [SerializeField] private GameObject curPosPrefab;
    [SerializeField] private GameObject hlinePrefab;
    [SerializeField] private GameObject vlinePrefab;

    [SerializeField] private BatteryGame game;

    // the first is the list of positions that need to be filled
    // the second is the list of edges
    // the third is the image
    List<(List<Vector2Int>, List<(Vector2Int, Vector2Int)>, Image)> batteryRequirements = new();
    HashSet<(Vector2Int, Vector2Int)> blockedEdges = new();

    Stack<Vector2Int> posStack = new();
    Stack<GameObject> lineStack = new();
    HashSet<Vector2Int> visited = new();
    HashSet<(Vector2Int, Vector2Int)> edges = new();

    Vector2Int curPos;
    GameObject curPosSymbol;

    protected override void StartGame()
    {
        base.StartGame();

        // spawn start and end point prefabs
        var startSymbol = Instantiate(powerSymbolPrefab, grid.transform);
        grid.MoveTo(startSymbol.transform as RectTransform, game.StartPos);

        var endSymbol = Instantiate(powerSymbolPrefab, grid.transform);
        grid.MoveTo(endSymbol.transform as RectTransform, game.EndPos);

        // spawn circle
        curPos = game.StartPos;
        curPosSymbol = Instantiate(curPosPrefab, grid.transform);
        grid.MoveTo(curPosSymbol.transform as RectTransform, curPos);

        // spawn obstacles
        foreach (var obstacle in game.Obstacles)
        {
            GameObject prefab = null;
            List<Vector2Int> requiredPositions = new();
            List<(Vector2Int, Vector2Int)> requiredEdges = new();
            int sizeX = 1;
            int sizeY = 1;
            bool v = false;
            switch (obstacle.Type)
            {
                case BatteryObstacleType.B1x1H:
                    prefab = b1x1hPrefab;
                    break;
                case BatteryObstacleType.B1x1V:
                    prefab = b1x1vPrefab;
                    v = true;
                    break;
                case BatteryObstacleType.B2x1H:
                    prefab = b2x1hPrefab;
                    sizeX = 2;
                    break;
                case BatteryObstacleType.B2x1V:
                    prefab = b2x1vPrefab;
                    v = true;
                    sizeX = 2;
                    break;
                case BatteryObstacleType.B1x2H:
                    prefab = b1x2hPrefab;
                    sizeY = 2;
                    break;
                case BatteryObstacleType.B1x2V:
                    prefab = b1x2vPrefab;
                    v = true;
                    sizeY = 2;
                    break;
                case BatteryObstacleType.B2x2H:
                    prefab = b2x2hPrefab;
                    sizeX = 2;
                    sizeY = 2;
                    break;
                case BatteryObstacleType.B2x2V:
                    prefab = b2x2vPrefab;
                    v = true;
                    sizeX = 2;
                    sizeY = 2;
                    break;
            }

            if(v){
                if(sizeX == 1){
                    requiredEdges.Add((obstacle.Position, obstacle.Position + Vector2Int.right));
                    requiredEdges.Add((obstacle.Position, obstacle.Position + Vector2Int.right + Vector2Int.down * sizeY));
                }
                else {
                    requiredPositions.Add(obstacle.Position + Vector2Int.right);
                    requiredPositions.Add(obstacle.Position + Vector2Int.right + Vector2Int.down * sizeY);
                }
            }
            else {
                if(sizeY == 1){
                    requiredEdges.Add((obstacle.Position, obstacle.Position + Vector2Int.down));
                    requiredEdges.Add((obstacle.Position, obstacle.Position + Vector2Int.right * sizeX + Vector2Int.down));
                }
                else {
                    requiredPositions.Add(obstacle.Position + Vector2Int.down);
                    requiredPositions.Add(obstacle.Position + Vector2Int.right * sizeX + Vector2Int.down);
                }
            }

            if(sizeX == 2 && sizeY == 2) {
                blockedEdges.Add((obstacle.Position + Vector2Int.right, obstacle.Position + Vector2Int.right + Vector2Int.down));
                blockedEdges.Add((obstacle.Position + Vector2Int.right + Vector2Int.down, obstacle.Position + Vector2Int.right + Vector2Int.down * 2));
                blockedEdges.Add((obstacle.Position + Vector2Int.down, obstacle.Position + Vector2Int.right + Vector2Int.down));
                blockedEdges.Add((obstacle.Position + Vector2Int.down + Vector2Int.right, obstacle.Position + Vector2Int.right * 2 + Vector2Int.down));
            }
            else if(sizeX == 2){
                blockedEdges.Add((obstacle.Position + Vector2Int.right, obstacle.Position + Vector2Int.right + Vector2Int.down));
            }
            else if (sizeY == 2){
                blockedEdges.Add((obstacle.Position + Vector2Int.down, obstacle.Position + Vector2Int.right + Vector2Int.down));
            }

            var obstacleSymbol = Instantiate(prefab, grid.transform);
            batteryRequirements.Add((requiredPositions, requiredEdges, obstacleSymbol.GetComponent<Image>()));
            grid.MoveTo(obstacleSymbol.transform as RectTransform, obstacle.Position);
        }

        visited.Add(curPos);
        UpdateBatteryRequirements();
    }

    private bool IsValidMove(Vector2Int pos)
    {
        if(!grid.IsValidPoint(pos)){
            return false;
        }
        if(visited.Contains(pos)){
            // check if it is the previous pos
            if(posStack.Count > 0){
                var lastPos = posStack.Peek();
                if(lastPos == pos){
                    return true;
                }
            }
            return false;
        }
        if(blockedEdges.Contains((curPos, pos)) || blockedEdges.Contains((pos, curPos))){
            return false;
        }
        return true;
    }

    protected override void OnMove(Vector2 dir)
    {
        base.OnMove(dir);

        Debug.Log(dir);

        if(dir.x == 0 && dir.y == 0){
            return;
        }
        if(dir.x != 0 && dir.y != 0){
            return;
        }

        dir.y *= -1;

        var newPos = curPos + new Vector2Int((int)dir.x, (int)dir.y);
        var edge = (curPos, newPos);
        var edgeInverse = (newPos, curPos);
        if(!IsValidMove(newPos)){
            Debug.Log($"Invalid move {newPos}");
            return;
        }

        bool found = false;
        if(posStack.Count > 0){
            var lastPos = posStack.Peek();
            Debug.Log($"{lastPos} {newPos}");
            if(lastPos == newPos){
                posStack.Pop();
                Destroy(lineStack.Pop());
                visited.Remove(lastPos);
                edges.Remove(edge);
                edges.Remove(edgeInverse);
                found = true;
            }
        }

        if(!found){
            var line = Instantiate(hlinePrefab, grid.transform);
            grid.MoveTo(line.transform as RectTransform, curPos);
            line.transform.localRotation = Quaternion.AngleAxis(
                -Vector2.SignedAngle(Vector2.right, newPos - curPos),
                Vector3.forward
            );

            posStack.Push(curPos);
            lineStack.Push(line);
            visited.Add(curPos);
            edges.Add(edge);
            edges.Add(edgeInverse);
        }

        curPos = newPos;
        grid.MoveTo(curPosSymbol.transform as RectTransform, curPos);
        UpdateBatteryRequirements();
    }

    void UpdateBatteryRequirements(){
        foreach (var (positions, edges, image) in batteryRequirements)
        {
            bool valid = true;
            foreach (var pos in positions)
            {
                if (!visited.Contains(pos) && curPos != pos)
                {
                    valid = false;
                    break;
                }
            }

            foreach (var edge in edges)
            {
                if(!this.edges.Contains(edge)){
                    valid = false;
                    break;
                }
            }

            image.color = valid ? Color.white : new Color(1, 1, 1, 0.25f);
        }
    }
}
