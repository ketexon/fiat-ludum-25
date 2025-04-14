using UnityEngine;

public class CameraMinigame : MinigameBase
{
	[SerializeField] Rover rover;

	protected override void OnMove(Vector2 dir)
	{
		base.OnMove(dir);

		rover.MoveDir = dir;
	}

	public override void Execute(string[] args)
	{
		if (!rover.Status.Comms)
		{
			Terminal.Println("Could not connect to rover.");
			return;
		}
		
		base.Execute(args);
	}

	protected override void StartGame()
    {
        base.StartGame();
		AudioManager.Instance.Play("CRTBuzz");
		AudioManager.Instance.Mute("KeyClick", true);

		rover.InCamera = true;
		rover.Status.ChangedEvent.AddListener(OnStatusChanged);
		// enable on start
    }

	void OnStatusChanged()
	{
		if (!rover.Status.Comms)
		{
			Terminal.Println("Lost connection to rover.");
			EndGame();
		}
	}


	protected override void EndGame()
	{
		AudioManager.Instance.Stop("CRTBuzz");
		AudioManager.Instance.Mute("KeyClick", false);
		rover.MoveDir = Vector2.zero;
		rover.InCamera = false;
		rover.Status.ChangedEvent.RemoveListener(OnStatusChanged);
		base.EndGame();
	}
}