using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance { get; private set; }
    private AudioSource audioSource;
    [SerializeField] private AudioClip playerMove, waterDeath, explosionDeath;
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
    }
    
    public void PlayerMove()
    {
        audioSource.PlayOneShot(playerMove);
    }
    public void WaterDeath()
    {
        audioSource.PlayOneShot(waterDeath);
    }
    public void VehicleHit()
    {
        audioSource.PlayOneShot(explosionDeath);
    }
}
