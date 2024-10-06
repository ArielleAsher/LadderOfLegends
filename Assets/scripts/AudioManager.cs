using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;

    public bool isSoundOn = true;

    private AudioSource bgmSource;
    public AudioClip bgmMainMenu;
    public AudioClip bgmGameplay;

    public bool isMusicOn = true;
    public float musicVolume = 0.1f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            if (bgmSource == null)
            {
                bgmSource = gameObject.AddComponent<AudioSource>();
                bgmSource.loop = true;
            }
            AudioSource[] audioSources = GetComponents<AudioSource>();
            if (audioSources.Length >= 1)
            {
                bgmSource = audioSources[0];
            }
            else
            {
                Debug.LogError("Not enough AudioSources attached to AudioManager.");
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayBGM(bgmMainMenu);
    }

    public void PlayBGM(AudioClip bgm)
    {
        if (!isMusicOn || bgm == null) return;

        if (bgmSource.isPlaying)
        {
            bgmSource.Stop();
        }

        bgmSource.clip = bgm;
        bgmSource.volume = musicVolume;
        bgmSource.Play();
    }

    public void StopBGM()
    {
        if (bgmSource.isPlaying)
        {
            bgmSource.Stop();
        }
    }

    public void ToggleMusic()
    {
        isMusicOn = !isMusicOn;

        if (!isMusicOn)
        {
            StopBGM();
        }
        else
        {
            PlayBGM(bgmSource.clip);
        }
    }

    public bool IsMusicOn()
    {
        return isMusicOn;
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        if (bgmSource.isPlaying)
        {
            bgmSource.volume = musicVolume;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleMusic();
        }
    }

    public void toggleMusic()
    {
        isSoundOn = !isSoundOn;
    }

}