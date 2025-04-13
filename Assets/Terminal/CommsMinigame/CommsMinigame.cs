using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class CommsMinigame : MinigameBase
{
    [SerializeField] CommsMinigameLevel minigameLevel;
    [SerializeField] MinigameGrid minigameGrid;

    protected override void StartGame()
    {
        base.StartGame();
        
        // wait one frame to start the minigame to prevent input from being passed
        // into game
        IEnumerator WaitCoro()
        {
            yield return null;
            // Start the minigame
            minigameLevel.StartLevel();
        }
        
        StartCoroutine(WaitCoro());
    }

    private void Update()
    {
    }
}
