using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerMusique : MonoBehaviour {

	[FMODUnity.EventRef]
	public string music;

	FMOD.Studio.EventInstance music_Event;

	bool alreadyDiscovered = false;

	// Use this for initialization
	void Start () {
		music_Event = FMODUnity.RuntimeManager.CreateInstance (music);

	}
	
	// Update is called once per frame
	void Update () {
		
	}



	void OnTriggerEnter(Collider col){
		if (col.tag == "Player" && !alreadyDiscovered) {
			music_Event.start ();
			alreadyDiscovered = true;
		}

	}
}
