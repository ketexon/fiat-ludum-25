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

    public override void Execute(string[] args)
    {
        if (Rover.Status.Comms)
        {
            Terminal.Println("Comms are operational.");
            return;
        }
        base.Execute(args);
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
        
        StartCoroutine(WaitCoro());
    }

    public void FixComms()
    {
        numSolved++;
        Rover.Status.Comms = true;
        Rover.repairsNeeded = false;
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
