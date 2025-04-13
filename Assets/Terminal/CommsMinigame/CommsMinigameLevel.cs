using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CommsMinigameLevel : MonoBehaviour
{
    [SerializeField] private Vector2Int startDir;
    [SerializeField] private Button startButton;
    [SerializeField] private MinigameGrid grid;
    [SerializeField] private RectTransform roverSymbol;
    [SerializeField] private GameObject signalPrefab;
    [SerializeField] private float signalSpeed = 0.5f;

    private List<CommsMinigameButton> buttons;
    
    Dictionary<Vector2Int, CommsMinigameButton> buttonDictionary = new();
    private Vector2Int endPoint;

    private GameObject signalGO = null;
    public void StartLevel()
    {
        SelectButton();
        
        buttons = new(GetComponentsInChildren<CommsMinigameButton>());

        foreach (var button in buttons)
        {
            buttonDictionary.Add(
                grid.GetPoint(button.transform as RectTransform),
                button
            );
        }

        endPoint = grid.GetPoint(roverSymbol);
        
        startButton.onClick.AddListener(SendSignal);
    }
    
    void SelectButton()
    {
        startButton.Select();
        EventSystem.current.SetSelectedGameObject(startButton.gameObject);
    }

    void SendSignal()
    {
        // calculate path
        List<Vector2Int> path = new();
        Vector2Int cur = grid.GetPoint(startButton.transform as RectTransform);
        path.Add(cur);
        Vector2Int dir = startDir;
        bool pathSucceeds = true;

        while (true)
        {
            cur = cur + dir;
            path.Add(cur);
            
            if (!grid.IsValidPoint(cur, negateY: true))
            {
                pathSucceeds = false;
                break;
            }

            if (cur == endPoint)
            {
                break;
            }
            if (buttonDictionary.TryGetValue(cur, out var button))
            {
                // right will rotate 90 degrees counter-clockwise
                if (button.State == CommsMinigameButton.ButtonState.Right)
                {
                    dir = new Vector2Int(-dir.y, dir.x);
                }
                // left will rotate 90 degrees clockwise
                else if (button.State == CommsMinigameButton.ButtonState.Left)
                {
                    dir = new Vector2Int(dir.y, -dir.x);
                }
            }
        }

        IEnumerator SignalCoro()
        {
            float distance = 0;
            while (true)
            {
                int curPosIndex = (int)distance;
                if (curPosIndex + 1 >= path.Count)
                {
                    break;
                }
                
                float t = distance - curPosIndex;
                
                Vector2Int curPos = path[curPosIndex];
                Vector2Int nextPos = path[curPosIndex + 1];
                Vector2Int dir = nextPos - curPos;
                Vector2 fractionalPos = curPos + (Vector2) dir * t;

                grid.MoveTo(signalGO.transform as RectTransform, fractionalPos, negateY: true);
                
                distance += signalSpeed * Time.deltaTime;
                
                yield return null;
            }

            Destroy(signalGO);
            signalGO = null;
            SelectButton();
        }
        EventSystem.current.SetSelectedGameObject(null);
        
        signalGO = Instantiate(signalPrefab, transform);
        StartCoroutine(SignalCoro());
    }
}