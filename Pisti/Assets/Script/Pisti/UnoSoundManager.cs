using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnoSoundManager : MonoBehaviour
{
    public static UnoSoundManager Instance;

    public AudioSource cardDrawSound;
    public AudioSource backdroudMusic;
    public AudioSource smileSound;
    public AudioSource winnerSound;
    public AudioSource sentCardToCenter;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        
    }
    public void cardDrawSoundPlay()
    {
        cardDrawSound.Play();
    }
    public void backgroundMusicPlay()
    {
        backdroudMusic.Play();
    }
    public void smileSoundPlay()
    {
        smileSound.Play();
    }
    public void winnerSoundPlay()
    {
        winnerSound.Play();
    }
    public void SendCardToCenterPlay()
    {
        sentCardToCenter.Play();
    }
}
