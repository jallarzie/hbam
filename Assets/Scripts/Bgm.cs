using UnityEngine;
using System.Collections;

public class Bgm : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		GetComponent<AudioSource> ().Play();
	}

}
