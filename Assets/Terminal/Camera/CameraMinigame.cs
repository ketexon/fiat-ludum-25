using UnityEngine;

public class CameraMinigame : MinigameBase
{
	[SerializeField] Rover rover;

	protected override void OnMove(Vector2 dir)
	{
		base.OnMove(dir);

		rover.MoveDir = dir;
	}

	protected override void StartGame()
    {
        base.StartGame();
        // enable on start
    }


	protected override void EndGame()
	{
		rover.MoveDir = Vector2.zero;
		base.EndGame();
	}
}