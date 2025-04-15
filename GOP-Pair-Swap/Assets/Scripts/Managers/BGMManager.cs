using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance { get; private set; }
    [SerializeField] private AudioClip[] bgmTracks;
    private AudioSource audioSource;
    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = bgmTracks[Random.Range(0,bgmTracks.Length)];
        audioSource.Play();
        RaiseVolume();
    }

    public void lowerVolume()
    {
        audioSource.volume = .5f;
    }
    public void RaiseVolume()
    {
        audioSource.volume = 1f;
    }
}
