using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {

	[FMODUnity.EventRef]
	public string jingle;

	FMOD.Studio.EventInstance jingle_Event;

	[FMODUnity.EventRef]
	public string jingle2;

	FMOD.Studio.EventInstance jingle2_Event;

	bool pushed = false;
	[SerializeField] Image myImage;
	[SerializeField] float timerChangeScene;
	[SerializeField] float timer;
	[SerializeField] string sceneName;

	void Start(){
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
		yield return null;
	}


}
