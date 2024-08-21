using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelLoadListener : MonoBehaviour
{
    TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        GameManager.LevelLoaded += OnLevelLoaded;
    }

    void OnLevelLoaded(int i)
    {
        if (text)
            text.text = "LVL_" + i;
    }
}
