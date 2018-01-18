using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CineticGun : MonoBehaviour {

	[SerializeField] private FirstPersonController fpc;

	BlockMove.Force myForce;

	bool stocked = false;
	LayerMask myMask;

	Quaternion myVectorRot;
	Rigidbody rb;
	public bool isLock = false;

	[SerializeField] GameObject directionVectorSign;

	void Start () {
		//myForces = new BlockMove.Force (new Vector3(1,0,0), new Vector3( transform.rotation.x, transform.rotation.y, transform.rotation.z) , 0.5f);
		myMask = 5;
		rb = GetComponent<Rigidbody> ();

		//myMask = ~myMask;
	}
	
	void Update ()	{

		myVectorRot = transform.rotation;


		//Prendre une force

		float triger2 = Input.GetAxis ("trigger2");

		if (triger2 > 0.2f || Input.GetMouseButtonDown (1)) {
			RaycastHit hit; 
			if (Physics.Raycast (transform.position, Camera.main.transform.TransformDirection (Vector3.forward), out hit, Mathf.Infinity, myMask) && (hit.collider.GetComponent<BlockMove> () || hit.collider.GetComponent<BlockAlreadyMoving> ())) {
			
				if (hit.collider.GetComponent<BlockMove> ()) {
					myForce = hit.collider.GetComponent<BlockMove> ().myForce;
				} else {
					myForce = hit.collider.GetComponent<BlockAlreadyMoving> ().myForce;
				}			
			}
		}

//		if(Input.GetKeyDown(KeyCode.Joystick1Button4)){
//
//			RaycastHit hit; 
//			if (Physics.Raycast (transform.position, Camera.main.transform.TransformDirection(Vector3.forward),  out hit, Mathf.Infinity, myMask) && (hit.collider.GetComponent<BlockMove>() || hit.collider.GetComponent<BlockAlreadyMoving>())) {
//
//				float result = Mathf.InverseLerp (0.9f, 9000, hit.distance);
//				result = Mathf.Lerp (0.90f, 10, result);
//			
//				myForces.Add(hit.collider.GetComponent<BlockMove> ().myForce);
//
//				BlockMove.Force intermediaryForce = myForces [myForces.Count - 1];
//				intermediaryForce.speed /= result;
//				myForces [myForces.Count - 1] = intermediaryForce;
//			}
//
//		}


		//donner une force

		float triger1 = Input.GetAxis ("trigger1");

		if ((triger1 > 0.2f || Input.GetMouseButtonDown (0)) && !stocked) {
			stocked = true;
			RaycastHit hit; 
			if (Physics.Raycast (transform.position, Camera.main.transform.TransformDirection (Vector3.forward), out hit, Mathf.Infinity, myMask) && ( hit.collider.GetComponent<BlockMove> () ||  hit.collider.GetComponent<BlockAlreadyMoving>()  )) {
				if (hit.collider.GetComponent<BlockMove> ()) {
					hit.collider.GetComponent<BlockMove> ().changeMyForce (myForce);
				} else {
					hit.collider.GetComponent<BlockAlreadyMoving> ().changeMyForce (myForce);
				}
			}
		} else if (triger1 < 0.2f) {
			stocked = false;
		}



		// Système de lock

		if (Input.GetMouseButtonDown (2) || Input.GetKeyDown(KeyCode.JoystickButton5) ) {
			if (transform.parent == null) {
				RaycastHit hit; 
				if (Physics.Raycast (transform.position, Camera.main.transform.TransformDirection (Vector3.forward), out hit, Mathf.Infinity, myMask) && (hit.collider.GetComponent<BlockMove> () || hit.collider.GetComponent<BlockAlreadyMoving> ())) {
					//rb.useGravity = false;
					isLock = true;
					Quaternion rotation = Quaternion.Euler( transform.rotation.eulerAngles);

					transform.SetParent(hit.transform,true);

					rotation = Quaternion.Euler( rotation.eulerAngles -transform.parent.transform.rotation.eulerAngles);

					Debug.Log (rotation.eulerAngles);

					transform.rotation = rotation;
					fpc.m_MouseLook.m_CharacterTargetRot = Quaternion.identity;
					fpc.m_MouseLook.m_CharacterTargetRot = rotation;
					transform.position += new Vector3 (0, 0.001f, 0);
				}

			} else {
				
				Quaternion rotation = Quaternion.Euler( transform.rotation.eulerAngles);
				transform.SetParent( null, true );
				isLock = false;
				fpc.m_MouseLook.m_CharacterTargetRot = rotation;

				RaycastHit hit; 
				if (Physics.Raycast (transform.position, Camera.main.transform.TransformDirection (Vector3.forward), out hit, Mathf.Infinity, myMask) && (hit.collider.GetComponent<BlockMove> () || hit.collider.GetComponent<BlockAlreadyMoving> ())) {
					transform.SetParent(hit.transform,true);
					rotation = Quaternion.Euler( rotation.eulerAngles - transform.parent.transform.rotation.eulerAngles);
					isLock = true;
					fpc.m_MouseLook.m_CharacterTargetRot = rotation;
					transform.position += new Vector3 (0, 0.001f, 0);
				}
			}
		}

		LookRotationSign ();

	}

	void OnCollisionEnter(){
		delock ();
	}


	void delock(){
		Quaternion rotation = Quaternion.Euler( transform.rotation.eulerAngles);
		transform.SetParent( null, true );
		isLock = false;
		fpc.m_MouseLook.m_CharacterTargetRot = rotation;
	}



	public void LookRotationSign(){
		

		directionVectorSign.transform.rotation =   Quaternion.Euler ( new Vector3 ((-myForce.direction.y )*180,  (-myForce.direction.z )*180, (myForce.direction.x )*180 ));

	}
	


}
