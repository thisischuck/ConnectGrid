using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class LinkedGrid : MonoBehaviour
{
    public RectTransform rect;
    [SerializeField] int Width;
    [SerializeField] int Height;

    [Range(10, 100)]
    public int Scale = 1;

    public List<Vector4> gridPositions;

    public List<GridElement> elements;

    readonly Vector3[] Hexagon = new Vector3[]{
        new Vector3(0.86f, 0.5f, 0),
        new Vector3(0, 1, 0),
        new Vector3(-0.86f , 0.5f, 0 ),
        new Vector3(-0.86f, -0.5f, 0),
        new Vector3(0, -1f, 0),
        new Vector3(0.86f, -0.5f, 0),
    };

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        Physics2D.queriesHitTriggers = true;
        rect = GetComponent<RectTransform>();

    }

    void OnValidate()
    {
        Vector2 parentSize = rect.rect.size;

        Height = Mathf.CeilToInt((parentSize.y - Scale) / (Scale * 2));
        Width = Mathf.CeilToInt((parentSize.x - Scale) / (Scale * 2f));
        GenerateGrid();
    }

    public Vector3[] MakeHexagon(Vector3 pos)
    {
        Vector3[] currentHexagon = new Vector3[6];

        for (int i = 0; i < Hexagon.Length; i++)
        {
            currentHexagon[i] = Hexagon[i] * Scale + pos;
        }

        return currentHexagon;
    }

    [ContextMenu("RegenerateGrid")]
    public void GenerateGrid()
    {
        gridPositions.Clear();
        for (int i = 0; i < Width; i++)
            for (int j = 0; j < Height; j++)
            {
                Vector3 pos = transform.position;
                pos += Vector3.down * (1.5f * Scale * j);
                if (j % 2 == 0)
                    pos += Vector3.right * (1.72f * Scale * i);
                else
                    pos += Vector3.right * (1.72f * Scale * (i + 0.5f));

                float enconding;

                if (j % 2 != 1)
                    enconding = i * 1000 + j;
                else enconding = (i + 1) * 1000 + j;

                gridPositions.Add(new Vector4(pos.x, pos.y, pos.z, enconding));
            }
    }

    public Vector4 GetClosestPosition(Vector3 input)
    {
        List<Vector4> tempGrid = gridPositions.Clone(null, false);
        tempGrid.Sort((x, y) =>
        {
            float xDistance = Vector3.Distance(x, input);
            float yDistance = Vector3.Distance(y, input);
            if (xDistance > yDistance)
                return 1;
            else
                return -1;
        });
        return tempGrid[0];
    }

    public void AdToGrid(GridElement element, bool force = false)
    {
        if (force || !elements.Contains(element))
        {
            elements.Add(element);
        }

        Vector4 position = GetClosestPosition(element.transform.position);

        element.gridPosition = new Vector2((int)(position.w / 1000), position.w - ((int)(position.w / 1000) * 1000));
        element.transform.position = position;
    }

    public void RemoveFromGrid(GridElement element)
    {
        elements.Remove(element);
    }

    public List<GridElement> GetNeighborElements(GridElement input)
    {
        List<GridElement> neighbors = new List<GridElement>();

        Vector2 gridPosition = input.gridPosition;
        foreach (GridElement element in elements)
        {
            if (element.gridPosition == gridPosition)
                continue;

            /* 
            
            EVEN Y (X + 1, Y - 1),(X + 1, Y + 1)
            ODD Y (X - 1, Y - 1),(X - 1, Y + 1)

            COMMON (X, Y - 1), (X - 1, Y), (X + 1, Y), (X, Y + 1)
            */

            if (gridPosition.y % 2 == 0)
            {
                if (element.gridPosition == gridPosition + new Vector2(1, -1) ||
                    element.gridPosition == gridPosition + new Vector2(1, 1))
                {

                    neighbors.Add(element);
                    continue;
                }
            }
            else
            {
                if (element.gridPosition == gridPosition + new Vector2(-1, -1) ||
                    element.gridPosition == gridPosition + new Vector2(-1, 1))
                {
                    neighbors.Add(element);
                    continue;
                }
            }

            if (element.gridPosition == gridPosition + new Vector2(0, -1) ||
                element.gridPosition == gridPosition + new Vector2(-1, 0) ||
                element.gridPosition == gridPosition + new Vector2(1, 0) ||
                element.gridPosition == gridPosition + new Vector2(0, 1))
            {
                neighbors.Add(element);
                continue;
            }
        }

        foreach (var neighbor in neighbors)
        {
            if (!neighbor.neighbors.Contains(input))
                neighbor.neighbors.Add(input);
        }

        return neighbors;
    }

    /// <summary>
    /// Callback to draw gizmos only if the object is selected.
    /// </summary>
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (var pos in gridPositions)
        {
            Gizmos.DrawLineStrip(MakeHexagon(pos), true);
            //Handles.DrawWireDisc(pos, Vector3.forward, Scale);
        }

    }
}
