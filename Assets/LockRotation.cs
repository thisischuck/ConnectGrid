using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockRotation : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        this.transform.rotation = Quaternion.identity;
    }
}
