using System;
using UnityEngine;

public class MinigameGrid : MonoBehaviour
{
    RectTransform RectTransform => (RectTransform)transform;

    public Transform Container;
    
    [SerializeField] private int nRows = 6;
    [SerializeField] private int nCols = 7;

    private float CellWidth => RectTransform.rect.width / nCols;
    private float CellHeight => RectTransform.rect.height / nRows;

    public Rect GetCellRect(Vector2Int cell)
    {
        return new Rect(
            cell.x * CellWidth,
            -cell.y * CellHeight,
            CellWidth,
            CellHeight
        );
    }

    public void MoveTo(RectTransform t, Vector2Int cell)
    {
        t.anchoredPosition = GetCellRect(cell).position;
        Debug.Log(GetCellRect(cell).position);
    }
}
