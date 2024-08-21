using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MainMenuManager : MonoBehaviour
{
    public static UnityAction ShowMenu;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.LevelLoaded += OnLevelLoaded;
        ShowMenu += OnLevelFinished;
    }

    // Update is called once per frame
    void OnLevelLoaded(int i)
    {
        this.gameObject.SetActive(false);
    }

    void OnLevelFinished()
    {
        GameManager.Instance.DestroyCurrentLevel();
        this.gameObject.SetActive(true);
    }
}
