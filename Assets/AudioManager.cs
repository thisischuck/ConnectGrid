using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = (AudioManager)FindObjectOfType(typeof(AudioManager));

                if (_instance == null)
                {
                    GameObject singleton = new GameObject();
                    _instance = singleton.AddComponent<AudioManager>();
                    singleton.name = "(singleton) " + typeof(AudioManager).ToString();

                    DontDestroyOnLoad(singleton);
                }
            }
            return _instance;
        }
    }

    static AudioManager _instance = null;

    public AudioClip popItClip;
    public AudioClip victoryClip;

    private List<AudioSource> sources;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        sources = new List<AudioSource>();
        foreach (Transform t in transform)
        {
            AudioSource source = t.GetComponent<AudioSource>();
            if (!sources.Contains(source))
                sources.Add(source);
        }
    }


    public void PlayPopIt()
    {
        PlayAudio(popItClip);
    }

    public void PlayVictory()
    {
        PlayAudio(victoryClip, 1);
    }

    void PlayAudio(AudioClip clip, int priority = 0)
    {
        foreach (AudioSource source in sources)
        {
            if (!source.isPlaying)
            {
                source.PlayOneShot(clip);
                return;
            }
        }

        if (priority > 0)
        {
            foreach (AudioSource source in sources)
            {
                source.PlayOneShot(clip);
                return;
            }
        }
    }
}
