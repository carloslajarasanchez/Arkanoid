using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]private AudioSource sfxAudioSource, musicAudioSource;// Audio sources del juego
    private bool isMusicPlaying;// Controla si la musica esta sonando

    public static AudioManager Instance { get; private set; }

    // Patron de Singelton
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.M)) ToggleMusic();// Mutea la musica
    }
    // Metodo para ejecutar un sonidoSFX
    public void PlaySound(AudioClip clip)
    {
        sfxAudioSource.PlayOneShot(clip);
    }
    // Metodo para ejecutar un musica
    public void PlayMusic(AudioClip clip)
    {
        musicAudioSource.clip = clip;
        musicAudioSource.Play();
    }
    // Metodo para saber si la musica esta sonando
    public bool GetIsMusicPlaying()
    {
        return isMusicPlaying;
    }
    // Metodo para mutear la musica
    private void ToggleMusic()
    {
        musicAudioSource.mute = !musicAudioSource.mute;
    }
}
