using UnityEngine;
using System.Collections;

public class Bgm : MonoBehaviour {

	public AudioClip bgm;

	// Use this for initialization
	void Awake () {
		GetComponent<AudioSource> ().PlayOneShot (bgm);
	}

}
