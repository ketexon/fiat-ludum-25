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
    
    public Rect GetCellRect(Vector2 cell)
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
        t.ForceUpdateRectTransforms();
    }
    
    public void MoveTo(RectTransform t, Vector2 cell, bool negateY = false)
    {
        if (negateY) cell.y *= -1;
        t.anchoredPosition = GetCellRect(cell).position;
        t.ForceUpdateRectTransforms();
    }

    public Vector2Int GetPoint(RectTransform obj)
    {
        return RoundToPoint(obj.anchoredPosition);
    }

    public Vector2Int RoundToPoint(Vector2 point)
    {
        return Vector2Int.RoundToInt(new Vector2(point.x / CellWidth, point.y / CellHeight));
    }

    public bool IsValidPoint(Vector2Int point, bool negateY = false)
    {
        if (negateY) point.y *= -1;
        return point.x >= 0 && point.x <= nCols && point.y >= 0 && point.y <= nRows;
    }
}
