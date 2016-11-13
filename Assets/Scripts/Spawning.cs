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

	// Use this for initialization
	void Start () {
		
		GameObject temp;

		for (int i = 0; i < spawnPoints.Length; i++) {
			if (i < 4) {
				temp = Instantiate (getPrefab ((NucleoType)i));
			} else {
				temp = Instantiate (getPrefab ((NucleoType)Random.Range (0, 4)));
			}
			temp.transform.position = spawnPoints [i].transform.position;
			BoardController.instance.Register (temp.GetComponent<Nucleo>());
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
