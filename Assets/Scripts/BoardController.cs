using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

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
    private float _fillTime = 1.5f;

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
            currentFillIndex++;
            StartCoroutine(DoFillHelix(_helix.transform.GetChild(currentFillIndex).GetComponent<Image>()));

            if (matchCount == 4)
            {
                if (matchedCG && matchedAT)
                {
                    matchedCytosine.Match();
                    matchedGuanine.Match();
                    matchedAdenine.Match();
                    matchedThymine.Match();
                    currentMatches++;
                    return true;
                }
            }
            else if (matchedCG)
            {
                matchedCytosine.Match();
                matchedGuanine.Match();
                currentMatches++;
                return true;
            }

            else if (matchedAT)
            {
                matchedAdenine.Match();
                matchedThymine.Match();
                currentMatches++;
                return true;
            }
        }

        return false;
    }

    private IEnumerator DoFillHelix(Image helixSegment)
    {
        helixSegment.fillAmount = 0f;

        while (helixSegment.fillAmount < 1f)
        {
            helixSegment.fillAmount += (Time.deltaTime / _fillTime);
            yield return null;
        }

        helixSegment.fillAmount = 1f;

        if (currentFillIndex > maxFills)
        {
            Time.timeScale = 0f;
            Debug.Log(((float)currentMatches / (float)maxMatches * 100f) + "%");
        }
    }


}
