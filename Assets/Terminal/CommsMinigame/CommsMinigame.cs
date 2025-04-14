using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CommsMinigame : MinigameBase
{
    [SerializeField] List<CommsMinigameLevel> levels;
    [SerializeField] public Rover Rover;
    [SerializeField] private TMPro.TMP_Text statusText;
    [SerializeField] private float statusTextDuration = 2f;
    int numSolved = 0;

    private CommsMinigameLevel curLevel;

    private Coroutine statusCoroutine = null;

    protected override void OnExecute(string[] args)
    {
        if (args.Length != 1)
        {
            base.OnExecute(args);
            return;
        }
        if (Rover.Status.Comms)
        {
            Terminal.Println("Comms are operational.");
            return;
        }
    }

    protected override void OnReset()
    {
        base.OnReset();
        
        curLevel.ResetLevel();
    }

    protected override void StartGame()
    {
        base.StartGame();

        statusText.text = "";
        
        curLevel = levels[numSolved];
        curLevel.Comms = this;
        // wait one frame to start the minigame to prevent input from being passed
        // into game
        IEnumerator WaitCoro()
        {
            yield return null;
            // Start the minigame
            curLevel.StartLevel();
        }

        AudioManager.Instance.Dim("Alert", true);
        StartCoroutine(WaitCoro());
    }

    public void FixComms()
    {
        numSolved++;
        Rover.Status.Comms = true;
    }
    
    public void CompleteLevel()
    {
        EndGame();
        Terminal.Println("Comms are operational.");
    }

    protected override void EndGame()
    {
        if (statusCoroutine != null)
        {
            StopCoroutine(statusCoroutine);
            statusCoroutine = null;
        }
        curLevel.StopLevel();
        AudioManager.Instance.Dim("Alert", false);
        base.EndGame();
    }
    
    public void ShowStatus(string text, System.Action then = null)
    {
        statusText.text = text;
        IEnumerator ResetText()
        {
            yield return new WaitForSeconds(statusTextDuration);
            statusText.text = "";
            then?.Invoke();
        }
        statusCoroutine = StartCoroutine(ResetText());
    }
}
