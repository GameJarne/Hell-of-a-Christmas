using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;
    float musicLength;
    [HideInInspector] public AudioSource audioSource;
    bool isPlaying = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        musicLength = audioSource.clip.length;

        isPlaying = false;
        StartCoroutine(MusicLoop());
    }

    IEnumerator MusicLoop()
    {
        if (isPlaying == false)
        {
            audioSource.Play();
            isPlaying = true;
        }

        yield return new WaitForSecondsRealtime(musicLength);

        isPlaying = false;

        int waitTime = Random.Range(7, 21);
        yield return new WaitForSecondsRealtime(waitTime);

        StartCoroutine(MusicLoop());
    }
}
