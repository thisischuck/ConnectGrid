using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
[RequireComponent(typeof(Button))]
public class GridElement : MonoBehaviour
{
    private LinkedGrid grid;
    private Button _button;
    public Vector2 gridPosition;
    public List<int> connections;
    protected bool _activated;
    float scale;

    public float rotation;

    public List<GridElement> neighbors;

    public virtual bool Activated
    {
        get { return _activated; }
        set
        {
            if (_activated == value)
                return;
            _activated = value;
            if (_activated && grid)
                Emit();
        }
    }

    protected float LineScale = 0.15f;
    protected int angleThreshold = 30;

    public float GetCurrentConnection(int connection)
    {
        float actualConnection = rotation - connection;
        if (actualConnection < 0)
            actualConnection = 360 + actualConnection;

        return actualConnection;
    }

    public virtual void Start()
    {
        rotation = 0;
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClick);
        grid = transform.parent.GetComponent<LinkedGrid>();
        if (grid)
        {
            grid.AdToGrid(this);
            neighbors = grid.GetNeighborElements(gridPosition);
            scale = grid.Scale;
        }
    }

    public virtual void OnClick()
    {
        _activated = false;
        transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + 60);
        rotation += 60;
        if (rotation == 360)
            rotation = 0;
        else if (rotation > 360)
            rotation = 60;
        Emit();
    }

    void OnEnable()
    {
        if (grid)
        {
            grid.AdToGrid(this);
            neighbors = grid.GetNeighborElements(gridPosition);
            scale = grid.Scale;
        }
    }

    void OnDisable()
    {
        grid.RemoveFromGrid(this);
    }

    void OnMouseDrag()
    {
        Debug.Log("Drag");
        this.transform.position = Input.mousePosition;
    }

    public virtual void Emit()
    {
        if (!grid)
            return;
        foreach (var connection in connections)
        {
            foreach (var neighbor in neighbors)
            {
                //Como e que eu verifico se estou virado para um neighbor. 
                Vector2 diff = neighbor.transform.position - transform.position;
                float angle = Vector2.SignedAngle(diff, Vector2.right);
                if (angle < 0)
                    angle = 360 + angle;
                float c = GetCurrentConnection(connection);
                if (InThreshold(angle, c + angleThreshold, c - angleThreshold))
                {
                    if (neighbor.Activated != Activated)
                        neighbor.Receive(this);
                    continue;
                }
            }
        }
    }

    public bool InThreshold(float value, float max, float min)
    {
        if (value < max && value > min)
            return true;
        return false;
    }

    public virtual void Receive(GridElement emiter)
    {
        foreach (var connection in connections)
        {
            Vector2 diff = emiter.transform.position - transform.position;
            float angle = Vector2.SignedAngle(diff, Vector2.right);
            if (angle < 0)
                angle = 360 + angle;
            float c = GetCurrentConnection(connection);
            if (InThreshold(angle, c + angleThreshold, c - angleThreshold))
            {
                Activated = emiter.Activated;
            }
        }
    }

    #region Unity Editor

    void OnDrawGizmos()
    {
        Color c = Color.white;

        if (_activated)
            c = Color.cyan;

        if (connections.Count <= 0)
            return;
        foreach (var connection in connections)
        {
            float tempConnection = connection - transform.rotation.eulerAngles.z;

            Vector3 destination = transform.position + new Vector3(Mathf.Cos(Mathf.Deg2Rad * tempConnection), Mathf.Sin(Mathf.Deg2Rad * tempConnection), 0) * scale * 1.5f;
            Vector3 upLine = transform.position + new Vector3(Mathf.Cos(Mathf.Deg2Rad * tempConnection + LineScale), Mathf.Sin(Mathf.Deg2Rad * tempConnection + LineScale), 0) * scale;
            Vector3 downLine = transform.position + new Vector3(Mathf.Cos(Mathf.Deg2Rad * tempConnection - LineScale), Mathf.Sin(Mathf.Deg2Rad * tempConnection - LineScale), 0) * scale;

            Vector3[] vertices = {
                transform.position,
                downLine,
                destination,
                upLine
            };

            Handles.DrawSolidRectangleWithOutline(vertices, c, Color.black);
        }
        _button.image.color = c;
    }

    #endregion
}