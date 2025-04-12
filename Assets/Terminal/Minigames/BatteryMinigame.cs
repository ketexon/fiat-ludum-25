using UnityEngine;
using UnityEngine.InputSystem;

public class BatteryMinigame : MinigameBase
{
    protected override void StartGame()
    {
        base.StartGame();
    }

    protected override void OnMove(Vector2 dir)
    {
        base.OnMove(dir);
        
        Debug.Log(dir);
    }
}
