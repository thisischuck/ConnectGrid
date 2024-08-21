using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWithMouse : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
                Vector3 pos = new Vector3(0, 0, -10);
#if (!UNITY_EDITOR && UNITY_ANDROID)
        if(Input.touches.Length > 0)
        {
                pos = Camera.main.ScreenToWorldPoint(Input.touches[0].position);
        }
#elif UNITY_EDITOR
                pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
#endif

                pos.z = 0;
                transform.position = pos;
        }
}
