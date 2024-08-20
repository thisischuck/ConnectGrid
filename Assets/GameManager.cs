using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public LinkedGrid grid;

    // Update is called once per frame
    void Update()
    {
        if (CheckFinish())
            Debug.Log("Finished Level");
    }

    bool CheckFinish()
    {
        foreach (var element in grid.elements)
        {
            if (!element.Activated) return false;
        }
        return true;
    }
}
