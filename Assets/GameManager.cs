using System.Collections;
using System.Collections.Generic;
using ES3Types;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = (GameManager)FindObjectOfType(typeof(GameManager));

                if (_instance == null)
                {
                    GameObject singleton = new GameObject();
                    _instance = singleton.AddComponent<GameManager>();
                    singleton.name = "(singleton) " + typeof(GameManager).ToString();

                    DontDestroyOnLoad(singleton);
                }
            }
            return _instance;
        }
    }

    static GameManager _instance = null;

    public static UnityAction<int> LevelLoaded;
    public static UnityAction<int> LevelFinished;
    public LinkedGrid grid;
    public int CurrentLevel;
    public int MaxLevel;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Awake()
    {
        CurrentLevel = 0;
        if (ES3.KeyExists("MaxLevel"))
        {
            MaxLevel = ES3.Load<int>("MaxLevel");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckFinish())
        {
            grid.elements.Clear();
            CurrentLevel++;
            if (CurrentLevel > MaxLevel)
                MaxLevel = CurrentLevel;
            LevelFinished.Invoke(CurrentLevel - 1);
        }
    }

    bool CheckFinish()
    {
        if (grid.elements.Count == 0)
            return false;

        foreach (var element in grid.elements)
        {
            if (!element.Activated) return false;
        }
        return true;
    }

    [ContextMenu("LoadLevel")]
    public void LoadLevel(int level)
    {
        var l = Resources.Load("Levels/Level_" + level);
        CurrentLevel = level;
        Instantiate(l, grid.transform);
        LevelLoaded.Invoke(level);
    }

    void OnDisable()
    {
        ES3.Save("MaxLevel", MaxLevel);
    }
}
