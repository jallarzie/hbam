using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {
    private static SoundManager _instance;

    public static SoundManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SoundManager>();

                if (_instance == null)
                {
                    GameObject obj = new GameObject("SoundManager");
                    _instance = obj.AddComponent<SoundManager>();
                }
            }

            return _instance;
        }
    }


    [SerializeField]
    private AudioSource _bgmSource;

    [SerializeField]
    private AudioSource _sfxSource;

    private bool soundOn;

    private void Awake () {
        DontDestroyOnLoad(gameObject);
        soundOn = PlayerPrefs.GetInt("sound", 1) == 1;
        _bgmSource.mute = !soundOn;
    }

    private void Start()
    {
        _bgmSource.Play();
    }

    public void PlaySound(AudioClip sound, float volume = 1.0f)
    {
        if (soundOn)
        {
            _sfxSource.PlayOneShot(sound, volume);
        }
    }

    public void SetSound(bool value)
    {
        soundOn = value;

        _bgmSource.mute = !soundOn;

        PlayerPrefs.SetInt("sound", soundOn ? 1 : 0);
        PlayerPrefs.Save();
    }
}
