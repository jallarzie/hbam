using UnityEngine;
using System.Collections;

public class Spawning : MonoBehaviour {

	[SerializeField]
	private GameObject[] spawnPoints;
	[SerializeField]
	private GameObject adeninePrefab;
	[SerializeField]
	private GameObject thyminePrefab;
	[SerializeField]
	private GameObject cytosinePrefab;
	[SerializeField]
	private GameObject guaninePrefab;

    [SerializeField]
    private float[] speedMultipliers;

	// Use this for initialization
	void Start () {
		
        int nbSpawns = PlayerPrefs.GetInt("amount", 7);
        int speed = PlayerPrefs.GetInt("speed", 2);

		GameObject temp;

        for (int i = 0; i < nbSpawns; i++) {
			if (i < 8) {
                temp = Instantiate (getPrefab ((NucleoType)(i % 4)));
			} else {
				temp = Instantiate (getPrefab ((NucleoType)Random.Range (0, 4)));
			}
            temp.transform.position = spawnPoints [Random.Range(0, spawnPoints.Length)].transform.position;
            Nucleo nucleo = temp.GetComponent<Nucleo>();
            nucleo.speedMultiplier = speedMultipliers[speed];
			BoardController.instance.Register (nucleo);
		}
	}
	
	GameObject getPrefab(NucleoType nucleoType){
		switch (nucleoType) {
		case NucleoType.Adenine:
			return adeninePrefab;
		case NucleoType.Cytosine:
			return cytosinePrefab;
		case NucleoType.Guanine:
			return guaninePrefab;
		case NucleoType.Thymine:
			return thyminePrefab;
		default:
			return null;
		}
	}

	// Update is called once per frame
	void Update () {
		
	}
}
