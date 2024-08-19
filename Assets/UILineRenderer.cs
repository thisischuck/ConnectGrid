using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class UILineRenderer : Graphic
{
    public float thickness;
    public List<Vector2> positions;

    /// <summary>
    /// Called when the script is loaded or a value is changed in the
    /// inspector (Called in the editor only).
    /// </summary>
    protected override void OnValidate()
    {
        SetVerticesDirty();
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
        Debug.Log("Test");
        foreach (var position in positions)
        {
            DrawVertexFromPoint(position, vh);
        }

        for (int i = 0; i < positions.Count - 1; i++)
        {
            int index = i * 2;
            vh.AddTriangle(index, index + 1, index + 3);
            vh.AddTriangle(index + 3, index + 2, index);
        }

    }

    void DrawVertexFromPoint(Vector2 point, VertexHelper vh)
    {
        UIVertex vertex = UIVertex.simpleVert;
        vertex.color = Color.black;

        vertex.position = new Vector3(-thickness / 2, 0);
        vertex.position += transform.position;
        vertex.position += new Vector3(point.x, point.y);
        vh.AddVert(vertex);

        vertex.position = new Vector3(thickness / 2, 0);
        vertex.position += transform.position;
        vertex.position += new Vector3(point.x, point.y);
        vh.AddVert(vertex);
    }
}
