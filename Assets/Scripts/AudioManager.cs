using UnityEngine;
using Zenject;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioSource audio;
    public AudioListener audioListener;
    public static bool on = false;
    private static bool audioListenerExists = false;
    public void PlaySound()
    {
        audio.Play();
        audio.loop = true;
    }
    void Awake()
    {
        audio = FindObjectOfType<AudioSource>();
        if (!audioListenerExists)
        {
            audioListenerExists = true;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(audioListener.gameObject);
            Destroy(gameObject);
        }
        if (!audio.isPlaying)
        {
            AudioClip newMusic = Resources.Load<AudioClip>($"Music\\background");
            audio.clip = newMusic;
            audio.Play();
        }
        if (PlayerPrefs.GetInt("sound") == 0 && audio.volume == 0)
        {
            audio.volume = 1;
            PlaySound();
            on = true;
        }
        try
        {
            DontDestroyOnLoad(audio.gameObject);
        }
        catch
        {
        }
    }
}
