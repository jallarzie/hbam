using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BoardController : MonoBehaviour {

    private static BoardController _instance;

    public static BoardController instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<BoardController>();

                if (_instance == null)
                {
                    GameObject obj = new GameObject("BoardController");
                    _instance = obj.AddComponent<BoardController>();
                }
            }

            return _instance;
        }
    }

    [SerializeField]
    private GameObject _helix;
    [SerializeField]
    private GameObject _endPopup;
    [SerializeField]
    private Text _endText;
    [SerializeField]
    private GameObject _pausePopup;
    [SerializeField]
    private GameObject _lineController;
    [SerializeField]
    private float _fillTime = 1.5f;
    [SerializeField]
    private Text _matchPercentDisplay;
    [SerializeField]
    private AudioClip[] _matchSounds;
    [SerializeField]
    private AudioClip _endSound;
    [SerializeField]
    private Toggle _soundToggle;

    private const float _endSoundVolume = 0.3f;

    private List<Nucleo> _nucleos = new List<Nucleo>();
    private int currentFillIndex;

    private int maxMatches;
    private int maxFills;
    private int currentMatches;

    private void Awake()
    {
        currentMatches = 0;
        currentFillIndex = 1;
        maxFills = _helix.transform.childCount - 2;
        maxMatches = maxFills * 2;
        _matchPercentDisplay.text = string.Format("{0}%", Mathf.Round((float)currentMatches / (float)maxMatches * 100f));
        _soundToggle.isOn = PlayerPrefs.GetInt("sound", 1) == 1;
    }

    public void Update()
    {
        if (Application.platform == RuntimePlatform.WindowsPlayer && Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public int nucleoCount
    {
        get { 
            if (_nucleos == null)
            {
                return 0;
            }

            return _nucleos.Count; 
        }
    }

    public Nucleo GetNucleo(int index)
    {
        return _nucleos[index];
    }

    public void Register(Nucleo nucleo)
    {
        _nucleos.Add(nucleo);
    }

    public bool ProcessMatch(List<Nucleo> matchedNucleos)
    {
        int matchCount = matchedNucleos.Count;

        if (matchCount == 0 || matchCount > 4)
        {
            return false;
        }

        Nucleo matchedAdenine = null;
        Nucleo matchedGuanine = null;

        Nucleo matchedCytosine = null;
        Nucleo matchedThymine = null;

        for (int i = 0; i < matchCount - 1; i++)
        {
            Nucleo nucleo1 = matchedNucleos[i];
            for (int j = i + 1; j < matchCount; j++)
            {
                Nucleo nucleo2 = matchedNucleos[j];

                if (nucleo1.nucleoType == NucleoType.Cytosine)
                {
                    if (matchedCytosine == null && nucleo2.nucleoType == NucleoType.Guanine)
                    {
                        matchedCytosine = nucleo1;
                        matchedGuanine = nucleo2;
                        break;
                    }
                }
                else if (nucleo1.nucleoType == NucleoType.Guanine)
                {
                    if (matchedGuanine == null && nucleo2.nucleoType == NucleoType.Cytosine)
                    {
                        matchedGuanine = nucleo1;
                        matchedCytosine = nucleo2;
                        break;
                    }
                }
                else if (nucleo1.nucleoType == NucleoType.Adenine)
                {
                    if (matchedAdenine == null && nucleo2.nucleoType == NucleoType.Thymine)
                    {
                        matchedAdenine = nucleo1;
                        matchedThymine = nucleo2;
                        break;
                    }
                }
                else if (nucleo1.nucleoType == NucleoType.Thymine)
                {
                    if (matchedThymine == null && nucleo2.nucleoType == NucleoType.Adenine)
                    {
                        matchedThymine = nucleo1;
                        matchedAdenine = nucleo2;
                        break;
                    }
                }

                if (matchedAdenine != null && matchedCytosine != null && matchedGuanine != null && matchedThymine != null)
                {
                    break;
                }
            }

        }

        bool matchedCG = matchedCytosine != null && matchedGuanine != null;
        bool matchedAT = matchedAdenine != null && matchedThymine != null;

        if (matchCount < 5)
        {
            HelixSegmentView previousSegment = _helix.transform.GetChild(currentFillIndex).GetComponent<HelixSegmentView>();
            currentFillIndex++;
            HelixSegmentView currentSegment = _helix.transform.GetChild(currentFillIndex).GetComponent<HelixSegmentView>();
            StartCoroutine(DoFillHelix(currentSegment, matchedCG, matchedAT));

            if (matchCount == 4)
            {
                if (matchedCG && matchedAT)
                {
                    matchedCytosine.Match();
                    matchedGuanine.Match();
                    matchedAdenine.Match();
                    matchedThymine.Match();
                    currentMatches += 2;
                    _matchPercentDisplay.text = string.Format("{0}%", Mathf.Round((float)currentMatches / (float)maxMatches * 100f));
                    SoundManager.instance.PlaySound(_matchSounds[Random.Range(0, _matchSounds.Length)]);
                    previousSegment.cg.CrossFadeAlpha(1f, _fillTime, false);
                    previousSegment.at.CrossFadeAlpha(1f, _fillTime, false);
                    return true;
                }
            }
            else if (matchedCG)
            {
                matchedCytosine.Match();
                matchedGuanine.Match();
                currentMatches++;
                _matchPercentDisplay.text = string.Format("{0}%", Mathf.Round((float)currentMatches / (float)maxMatches * 100f));
                SoundManager.instance.PlaySound(_matchSounds[Random.Range(0, _matchSounds.Length)]);
                previousSegment.cg.CrossFadeAlpha(1f, _fillTime, false);
                return true;
            }

            else if (matchedAT)
            {
                matchedAdenine.Match();
                matchedThymine.Match();
                currentMatches++;
                _matchPercentDisplay.text = string.Format("{0}%", Mathf.Round((float)currentMatches / (float)maxMatches * 100f));
                SoundManager.instance.PlaySound(_matchSounds[Random.Range(0, _matchSounds.Length)]);
                previousSegment.at.CrossFadeAlpha(1f, _fillTime, false);
                return true;
            }
        }

        return false;
    }

    private IEnumerator DoFillHelix(HelixSegmentView helixSegment, bool matchedCG, bool matchedAT)
    {
        helixSegment.background.fillAmount = 0f;

        while (helixSegment.background.fillAmount < 1f)
        {
            helixSegment.background.fillAmount += (Time.deltaTime / _fillTime);
            yield return null;
        }

        helixSegment.background.fillAmount = 1f;

        if (currentFillIndex > maxFills)
        {
            for (int i = 0; i < _nucleos.Count; i++)
            {
                _nucleos[i].gameObject.SetActive(false);
                _lineController.SetActive(false);
                float result = Mathf.Round((float)currentMatches / (float)maxMatches * 100f);
                if (result > 90f)
                {
                    _endText.text = "YOU MADE A BABY!";
                    SoundManager.instance.PlaySound(_endSound, _endSoundVolume);
                }
                else if (result > 60f)
                {
                    _endText.text = "EXCELLENT!";
                    SoundManager.instance.PlaySound(_endSound, _endSoundVolume);
                }
                else if (result > 40f)
                {
                    _endText.text = "GOOD";
                }
                else
                {
                    _endText.text = "...";
                }
                _endPopup.SetActive(true);
            }
        }
    }

    public void OnRetryTap()
    {
        SceneManager.LoadScene("scene_main", LoadSceneMode.Single);
    }

    public void OnSettingsTap()
    {
        SceneManager.LoadScene("scene_settings", LoadSceneMode.Single);
		_lineController.SetActive (true);
    }

    public void OnPause(bool pause)
    {
        Time.timeScale = pause ? 0.0f : 1.0f;
        _pausePopup.SetActive(pause);
		_lineController.SetActive (false);
    }

    public void OnSoundChange(bool value)
    {
        SoundManager.instance.SetSound(value);
    }
}
