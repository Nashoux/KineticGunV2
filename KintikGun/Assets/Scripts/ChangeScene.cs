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


}
