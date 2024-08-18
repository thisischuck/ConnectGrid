using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollision : MonoBehaviour
{
    GridElement Element;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        Element = GetComponentInParent<GridElement>();
    }

    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    void OnDisable()
    {
        Destroy(this.gameObject);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        GridElement ge = other.GetComponent<GridElement>();
    }
}
