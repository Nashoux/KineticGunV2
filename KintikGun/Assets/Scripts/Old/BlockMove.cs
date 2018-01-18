using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BlockMove : MonoBehaviour {

//	enum mouvementType {noMouv, UpAndDown, Up}
//	[SerializeField] mouvementType myNewMouv = mouvementType.noMouv;
//	List<mouvementType> allMyMouv = new List<mouvementType>();
//	Dictionary<string, int> speeds = new Dictionary<int, int>();


	[SerializeField] float speedOfReturn = 5;

	public struct Force{
		
		public Vector3 direction;
		public Vector3 orientation;
		public float speed;

		public Force (Vector3 _direction, Vector3 _orientation, float _speed){
			direction = _direction;
			orientation = _direction;
			speed = _speed;
		}

	}

	List<Vector3> AllThePlace = new List<Vector3> ();

	Vector3 positionStart;

	[SerializeField] float timerReturn = 0;
	[SerializeField] float timerReturnBase = 5;

	void Start(){
		positionStart = transform.position;
	}


	public Force myForce = new Force (new Vector3 (0, 0, 0), new Vector3 (0, 0, 0), 0);
		

	void Update () {

		if (timerReturn > timerReturnBase) {
			myForce = new Force (new Vector3 (0, 0, 0), new Vector3 (0, 0, 0), 0);
			ReturnToStart ();
		} else {
			timerReturn += Time.deltaTime;
		}

		transform.position += myForce.direction * myForce.speed * Time.deltaTime;

		if(Input.GetKeyDown(KeyCode.R)){
			transform.position = positionStart;
			myForce = new Force (new Vector3 (0, 0, 0), new Vector3 (0, 0, 0), 0);
		}

//		switch (myNewMouv) {
//
//		case mouvementType.noMouv:
//
//			break;
//
//		case mouvementType.UpAndDown:
//
//			StartCoroutine ("MouvToUpAndDown");
//
//			myNewMouv = mouvementType.noMouv;
//
//			break;
//		}
			
	}

	void ReturnToStart(){
		if (AllThePlace.Count > 0) {

			Debug.Log ("a");
			
			transform.position = Vector3.MoveTowards (transform.position, AllThePlace [AllThePlace.Count - 1], speedOfReturn*Time.deltaTime);

			if (Vector3.Distance (transform.position, AllThePlace [AllThePlace.Count - 1]) < 0.2f) {
				AllThePlace.RemoveAt (AllThePlace.Count - 1);
			}
		}

	}

	void OnCollisionEnter(){
		timerReturn = timerReturnBase + 1;
	}

	public void changeMyForce ( Force _force ){

		timerReturn = 0;

		AllThePlace.Add (transform.position);


		myForce.direction = Vector3.Normalize (_force.direction);

		myForce.speed = _force.speed;

		//transform.rotation = Quaternion.identity;

		//transform.Rotate (_force.orientation);

	}







//		 IEnumerator MouvToUpAndDown(){
//
//		for (float i = 0; i < 1; i += Time.deltaTime) {
//			transform.position = new Vector3(transform.position.x ,transform.position.y + Time.deltaTime,transform.position.z);
//			yield return new WaitForEndOfFrame ();
//			}
//
//		for (float i = 0; i < 2; i += Time.deltaTime) {
//			transform.position = new Vector3(transform.position.x,transform.position.y - Time.deltaTime,transform.position.z);
//			yield return new  WaitForEndOfFrame ();
//		}
//
//		for (float i = 0; i < 1; i += Time.deltaTime) {
//			transform.position = new Vector3(transform.position.x + Time.deltaTime,transform.position.y,transform.position.z);
//			yield return new  WaitForEndOfFrame ();
//		}
//
//		StartCoroutine ("MouvToUpAndDown");
//
//		}

		
}
