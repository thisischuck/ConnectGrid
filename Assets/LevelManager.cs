using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public int Level;
    public GameObject LevelEnd;
    // Start is called before the first frame update
    void OnEnable()
    {
        GameManager.LevelFinished += OnLevelFinished;
    }

    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    void OnDisable()
    {
        GameManager.LevelFinished -= OnLevelFinished;
    }

    // Update is called once per frame
    void OnLevelFinished(int l)
    {
        if (Level == l)
        {
            AudioManager.Instance.PlayVictory();
            LevelEnd.SetActive(true);
        }
    }
}
