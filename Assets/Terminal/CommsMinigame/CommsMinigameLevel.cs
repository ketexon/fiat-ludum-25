using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CommsMinigameLevel : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField, FormerlySerializedAs("startDir")]
    private Vector2Int StartDir;
    [SerializeField] private Button startButton;
    [SerializeField] private MinigameGrid grid;
    [SerializeField] private Image rover;
    [SerializeField] private GameObject signalPrefab;
    [SerializeField] private List<Image> packets;
    [SerializeField] private float signalSpeed = 4;
    [SerializeField] private float signalBlinkDuration = 1f;
    [SerializeField] private float uncollectedOpacity = 0.25f;
    
    [System.NonSerialized] public CommsMinigame Comms;

    private List<CommsMinigameButton> buttons;
    
    public Dictionary<Vector2Int, CommsMinigameButton> ButtonCoords = new();
    public Dictionary<Vector2Int, Image> PacketCoords = new();
    
    [System.NonSerialized] Vector2Int EndPoint;

    private GameObject signalGO = null;
    private Coroutine signalCoroutine = null;

    private bool init = false;

    private void Awake()
    {
        canvas.enabled = false;
        canvasGroup.interactable = false;
    }

    public void StartLevel()
    {
        canvas.enabled = true;
        canvasGroup.interactable = true;
        
        ResetGame();

        if (init) return;
        init = true;
        buttons = new(GetComponentsInChildren<CommsMinigameButton>());

        foreach (var button in buttons)
        {
            ButtonCoords.Add(
                grid.GetPoint(button.transform as RectTransform),
                button
            );
        }
        
        foreach (var packet in packets)
        {
            Vector2Int point = grid.GetPoint(packet.transform as RectTransform);
            PacketCoords.Add(point, packet);
        }

        EndPoint = grid.GetPoint(rover.rectTransform);
        
        startButton.onClick.AddListener(SendSignal);
    }
    
    void ResetGame()
    {
        startButton.Select();
        EventSystem.current.SetSelectedGameObject(startButton.gameObject);
        
        foreach(var packetImage in packets)
        {
            packetImage.color = new Color(
                1,1,1,
                uncollectedOpacity
            );
        }
        
        rover.color = new Color(1,1,1,uncollectedOpacity);
    }

    public void StopLevel()
    {
        if (signalCoroutine != null)
        {
            StopCoroutine(signalCoroutine);
            signalCoroutine = null;
        }
        EventSystem.current.SetSelectedGameObject(null);
        canvas.enabled = false;
        canvasGroup.interactable = false;
    }

    void SendSignal()
    {
        // calculate path
        List<Vector2Int> path = new();
        Vector2Int cur = grid.GetPoint(startButton.transform as RectTransform);
        path.Add(cur);
        Vector2Int dir = StartDir;
        bool pathSucceeds = true;
        HashSet<Vector2Int> collectedPackets = new();

        while (true)
        {
            cur = cur + dir;
            path.Add(cur);
            
            if (!grid.IsValidPoint(cur, negateY: true))
            {
                pathSucceeds = false;
                break;
            }

            if (cur == EndPoint)
            {
                break;
            }
            if (ButtonCoords.TryGetValue(cur, out var button))
            {
                // note that dir is negative in the grid
                // right will rotate 90 degrees counter-clockwise
                if (button.State == CommsMinigameButton.ButtonState.Right)
                {
                    dir = new Vector2Int(dir.y, dir.x);
                }
                // left will rotate 90 degrees clockwise
                else if (button.State == CommsMinigameButton.ButtonState.Left)
                {
                    dir = new Vector2Int(-dir.y, -dir.x);
                }
            }
            if (PacketCoords.TryGetValue(cur, out var packet))
            {
                collectedPackets.Add(cur);
            }
        }
        
        pathSucceeds &= collectedPackets.Count == packets.Count;

        Image signalImage;
        IEnumerator SignalCoro()
        {
            float distance = 0;
            float lastBlinkTime = Time.time;
            int lastPosIndex = 0;
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
                
                // blink signal if we just hit
                // a button or a packet
                if (curPosIndex != lastPosIndex)
                {
                    lastPosIndex = curPosIndex;
                    bool blink = false;
                    if (ButtonCoords.TryGetValue(curPos, out var button))
                    {
                        if (button.State != CommsMinigameButton.ButtonState.Inactive)
                        {
                            blink = true;
                        }
                    }
                    if (PacketCoords.TryGetValue(curPos, out var packet))
                    {
                        packet.color = Color.white;
                        blink = true;
                    }

                    if (blink)
                    {
                        lastBlinkTime = Time.time;
                        AudioManager.Instance.Play("Beep");
                    }
                }
                
                Vector2Int dir = nextPos - curPos;
                Vector2 fractionalPos = curPos + (Vector2) dir * t;
                float angle = Vector2.SignedAngle(Vector2.right, dir);
                grid.MoveTo(signalGO.transform as RectTransform, fractionalPos, negateY: true);
                signalGO.transform.rotation = Quaternion.Euler(0, 0, angle);
                signalImage.color = new Color(
                    1,
                    1,
                    1,
                    Mathf.Lerp(1, 0, (Time.time - lastBlinkTime) / signalBlinkDuration)
                );
                
                distance += signalSpeed * Time.deltaTime;
                
                yield return null;
            }

            signalCoroutine = null;
            Destroy(signalGO);
            signalGO = null;
            if (pathSucceeds)
            {
                OnSignalReachedRover();
            }
            else
            {
                OnSignalLost();
            }
        }
        EventSystem.current.SetSelectedGameObject(null);
        
        signalGO = Instantiate(signalPrefab, transform);
        signalImage = signalGO.GetComponent<Image>();
        signalCoroutine = StartCoroutine(SignalCoro());
    }

    void OnSignalLost()
    {
        ResetGame();
        Comms.ShowStatus("LOST SIGNAL");
        AudioManager.Instance.Play("Error");
    }

    void OnSignalReachedRover()
    {
        rover.color = Color.white;
        Comms.FixComms();
        Comms.ShowStatus("CONNECTED", () =>
        {
            Comms.CompleteLevel();
        });
        AudioManager.Instance.Play("Victory");
    }
}