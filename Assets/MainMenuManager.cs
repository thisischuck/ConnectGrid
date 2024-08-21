using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.LevelLoaded += OnLevelLoaded;
        GameManager.LevelFinished += OnLevelFinished;
    }

    // Update is called once per frame
    void OnLevelLoaded(int i)
    {
        this.gameObject.SetActive(false);
    }

    void OnLevelFinished(int i)
    {
        this.gameObject.SetActive(true);
    }
}
