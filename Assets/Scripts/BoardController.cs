using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

    private List<Nucleo> _nucleos = new List<Nucleo>();

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
}
