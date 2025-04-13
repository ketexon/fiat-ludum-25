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
		AudioManager.Instance.Play("CRTBuzz");
        // enable on start
    }


	protected override void EndGame()
	{
		AudioManager.Instance.Stop("CRTBuzz");
		rover.MoveDir = Vector2.zero;
		base.EndGame();
	}
}