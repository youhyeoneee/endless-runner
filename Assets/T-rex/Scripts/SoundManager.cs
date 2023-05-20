using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] _audioClips;
    private AudioSource _audioSource;
    
    #region instance
    public static SoundManager instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }
    #endregion

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void Jump()
    {
        _audioSource.PlayOneShot(_audioClips[0]);
    }
    
    public void Point()
    {
        _audioSource.PlayOneShot(_audioClips[1]);
    }
    
    public void Die()
    {
        _audioSource.PlayOneShot(_audioClips[2]);
    }
}
