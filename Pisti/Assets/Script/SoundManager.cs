using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public List<AudioSource> sound = new List<AudioSource>();
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        
    }

   public void SoundPlay(int soundIndex)
    {
        sound[soundIndex].Play();
    }
    public void SoundStop(int soundIndex)
    {
        sound[soundIndex].Stop();
    }
}
