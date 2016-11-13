using UnityEngine;
using System.Collections;

public class Bgm : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        DontDestroyOnLoad(gameObject);
		GetComponent<AudioSource> ().Play();
	}

}
