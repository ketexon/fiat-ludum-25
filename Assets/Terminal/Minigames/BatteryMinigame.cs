using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

[System.Serializable]
enum BatteryObstacleType
{
    B1x1H,
    B1x1V,
    B2x1H,
    B2x1V,
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
    [SerializeField] private GameObject b2x2hPrefab;
    [SerializeField] private GameObject b2x2vPrefab;
    
    [SerializeField] private BatteryGame game;
    
    protected override void StartGame()
    {
        base.StartGame();
        
        // spawn start and end point prefabs
        var startSymbol = Instantiate(powerSymbolPrefab, grid.transform);
        grid.MoveTo(startSymbol.transform as RectTransform, game.StartPos);
        
        var endSymbol = Instantiate(powerSymbolPrefab, grid.transform);
        grid.MoveTo(endSymbol.transform as RectTransform, game.EndPos);
    }

    protected override void OnMove(Vector2 dir)
    {
        base.OnMove(dir);
        
        Debug.Log(dir);
    }
}
