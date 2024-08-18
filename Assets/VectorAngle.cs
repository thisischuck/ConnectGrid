using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[ExecuteAlways]
public class VectorAngle : MonoBehaviour
{
    public Vector2 a;
    public Vector2 b;

    public Vector2 diff;

    public float Angle;

    // Update is called once per frame
    void Update()
    {
        diff = b - a;
        Angle = Vector2.SignedAngle(diff, Vector2.right);
    }
}
