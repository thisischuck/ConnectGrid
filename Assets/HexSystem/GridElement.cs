using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class GridElement : MonoBehaviour
{
    protected UILineRenderer lineRenderer;
    public LinkedGrid grid;
    private Button _button;
    public Vector2 gridPosition = new Vector2(-1, -1);
    public List<int> connections;
    protected bool _activated;
    float scale = 25f;

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
            lineRenderer.color = Activated ? Color.cyan : Color.white;
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
        lineRenderer = GetComponentInChildren<UILineRenderer>();
        rotation = 0;
        if (!_button)
            _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClick);

        grid = FindAnyObjectByType<LinkedGrid>();

        if (grid)
        {
            grid.AdToGrid(this);
            neighbors = grid.GetNeighborElements(this);
            scale = grid.Scale;
        }

        if (lineRenderer)
        {
            lineRenderer.points.Clear();
            foreach (var connection in connections)
            {
                float x = Mathf.Cos(Mathf.Deg2Rad * connection);
                float y = Mathf.Sin(Mathf.Deg2Rad * connection);

                lineRenderer.AddPoint(new Vector2(x, y) * 0.83f * grid.Scale);
            }
        }
    }

    public void DisableInput()
    {
        if (!_button)
            _button = GetComponent<Button>();
        _button.interactable = false;
    }

    public void EnableInput()
    {
        if (!_button)
            _button = GetComponent<Button>();
        _button.interactable = true;
    }

    public virtual void OnClick()
    {
        AudioManager.Instance.PlayPopIt();
        Activated = false;
        transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + 60);
        rotation += 60;
        if (rotation == 360)
            rotation = 0;
        else if (rotation > 360)
            rotation = 60;
        Emit();
    }

    protected virtual void OnEnable()
    {
        if (grid)
        {
            grid.AdToGrid(this);
            neighbors = grid.GetNeighborElements(this);
            scale = grid.Scale;
        }
    }

    void OnDisable()
    {
        if (grid)
            grid.RemoveFromGrid(this);
    }

    void OnValidate()
    {
        lineRenderer = GetComponentInChildren<UILineRenderer>();
        if (lineRenderer)
        {
            foreach (var connection in connections)
            {
                float x = Mathf.Cos(Mathf.Deg2Rad * connection);
                float y = Mathf.Sin(Mathf.Deg2Rad * connection);

                lineRenderer.AddPoint(new Vector2(x, y) * scale);
            }
        }
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

#if UNITY_EDITOR

    void OnDrawGizmos()
    {
        Color c = Color.white;

        if (_activated)
            c = Color.cyan;

        if (connections == null)
            return;
        if (connections.Count <= 0)
            return;
        foreach (var connection in connections)
        {
            float tempConnection = connection - transform.rotation.eulerAngles.z;

            Vector3 destination = transform.position + new Vector3(Mathf.Cos(Mathf.Deg2Rad * tempConnection), Mathf.Sin(Mathf.Deg2Rad * tempConnection), 0) * scale / 2 * 1.5f;
            Vector3 upLine = transform.position + new Vector3(Mathf.Cos(Mathf.Deg2Rad * tempConnection + LineScale), Mathf.Sin(Mathf.Deg2Rad * tempConnection + LineScale), 0) * scale / 2;
            Vector3 downLine = transform.position + new Vector3(Mathf.Cos(Mathf.Deg2Rad * tempConnection - LineScale), Mathf.Sin(Mathf.Deg2Rad * tempConnection - LineScale), 0) * scale / 2;

            Vector3[] vertices = {
                transform.position,
                downLine,
                destination,
                upLine
            };

            Handles.DrawSolidRectangleWithOutline(vertices, c, Color.black);
        }
    }

#endif

    #endregion
}