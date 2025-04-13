using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    
    
    protected override void StartGame()
    {
        base.StartGame();
        
        // spawn obstacles
    }

    protected override void OnMove(Vector2 dir)
    {
        base.OnMove(dir);
        
        Debug.Log(dir);
    }
}
