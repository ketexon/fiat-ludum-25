using System;
using UnityEngine;

public class MinigameGrid : MonoBehaviour
{
    RectTransform RectTransform => (RectTransform)transform;
    
    [SerializeField] private int nRows = 6;
    [SerializeField] private int nCols = 7;

    private float CellWidth => RectTransform.rect.width / nCols;
    private float CellHeight => RectTransform.rect.height / nRows;

    public Rect GetCellRect(Vector2Int cell)
    {
        return new Rect(
            RectTransform.rect.x + (cell.x * CellWidth),
            RectTransform.rect.y + (cell.y * CellHeight),
            CellWidth,
            CellHeight
        );
    }
}
