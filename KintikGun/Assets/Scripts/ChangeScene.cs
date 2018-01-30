using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {



	bool pushed = false;
	[SerializeField] Image myImage;
	[SerializeField] float timerChangeScene;
	[SerializeField] float timer;
	[SerializeField] string sceneName;

	[FMODUnity.EventRef]
	public string jingle;

	FMOD.Studio.EventInstance jingle_Event;

	[FMODUnity.EventRef]
	public string jingle2;

	FMOD.Studio.EventInstance jingle2_Event;

	[FMODUnity.EventRef]
	public string music;

	FMOD.Studio.EventInstance music_Event;

	void Start(){

		Cursor.visible = false;
		jingle_Event = FMODUnity.RuntimeManager.CreateInstance (jingle);
		jingle2_Event = FMODUnity.RuntimeManager.CreateInstance (jingle2);
		music_Event = FMODUnity.RuntimeManager.CreateInstance (music);
		//jingle_Event.start ();
		StartCoroutine ("changeSceneBegin");

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.anyKey && !pushed) {
			pushed = true;
			StartCoroutine ("changeScene");
		}
	}

	IEnumerator changeScene(){
		music_Event.stop (FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		jingle2_Event.start ();
		for (int i = 0; i < timer; i++) {
			myImage.color = new Color(myImage.color.r,myImage.color.g,myImage.color.b, myImage.color.a+ 1 / timer);
			yield return new WaitForEndOfFrame ();
		}
		yield return new WaitForSeconds(timerChangeScene);
		SceneManager.LoadSceneAsync (sceneName, LoadSceneMode.Single);
		yield return null;
	}

	IEnumerator changeSceneBegin(){
		for (int i = 0; i < timer/2; i++) {
			myImage.color = new Color(myImage.color.r,myImage.color.g,myImage.color.b, myImage.color.a- 1 / (timer/2));
			yield return new WaitForEndOfFrame ();
		}
		music_Event.start ();
		yield return null;
	}


}
