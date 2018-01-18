﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CineticGunV2 : MonoBehaviour {

	#region sons
	[FMODUnity.EventRef]
	public string Gun_Absorbe_Energie;

	FMOD.Studio.EventInstance Gun_Absorbe_Energie_Event;

	[FMODUnity.EventRef]
	public string Gun_Don_Energie;

	FMOD.Studio.EventInstance Gun_Don_Energie_Event;

	[FMODUnity.EventRef]
	public string Gun_Absorbe_Direction;

	FMOD.Studio.EventInstance Gun_Absorbe_Direction_Event;

	[FMODUnity.EventRef]
	public string Gun_Don_Direction;

	FMOD.Studio.EventInstance Gun_Don_Direction_Event;

	#endregion

	#region trucs
	[SerializeField] GameObject ParticulesAspiration;
	[SerializeField] GameObject ParticulesDirection;

	[SerializeField] GameObject myGameObbject;

	[SerializeField] private FirstPersonController fpc;

	float myEnergie = 0;
	[SerializeField] float myEnergieMax = 200;
	bool stocked = false;
	LayerMask myMask;

	bool energiseGift = false;
	float energiseGiftTimer = 0.8f;
	bool energiseTake = false;
	float energiseTakeTimer = 0.8f;

	public BlockAlreadyMovingV2 blockLock;
	public GameObject[] myDirectionGo = new GameObject[2];

	[SerializeField] GameObject directionVectorSign;

	GameObject lastParticuleAspiration;
	GameObject lastPlateformSeen = null;
	GameObject lastParticuleDirection;

	#endregion 

	[SerializeField] Image[] viseurObjects;
	[SerializeField] float[] viseurBegin;
	[SerializeField] float[] viseurEnd;
	float viseurMax = 0;


	float lastInputTrigger = 0;

	void Start () {

	

		//myForces = new BlockMove.Force (new Vector3(1,0,0), new Vector3( transform.rotation.x, transform.rotation.y, transform.rotation.z) , 0.5f);
		myMask = 5;
		Gun_Absorbe_Energie_Event = FMODUnity.RuntimeManager.CreateInstance (Gun_Absorbe_Energie);
		Gun_Don_Energie_Event = FMODUnity.RuntimeManager.CreateInstance (Gun_Don_Energie);
		Gun_Absorbe_Direction_Event = FMODUnity.RuntimeManager.CreateInstance (Gun_Absorbe_Direction);
		Gun_Don_Direction_Event = FMODUnity.RuntimeManager.CreateInstance (Gun_Don_Direction);

		//myMask = ~myMask;
	}
	
	void Update ()	{

		viseurObjects [0].fillAmount = viseurBegin[0] + myEnergie / myEnergieMax*4;
		viseurObjects [1].fillAmount = viseurBegin [1] + myEnergie / myEnergieMax*4 - 1;
		viseurObjects [2].fillAmount = viseurBegin [2] + myEnergie / myEnergieMax*4 - 2;
		viseurObjects [3].fillAmount = viseurBegin [3] + myEnergie / myEnergieMax*4 - 3;


		#region direction
		//Prendre une force
		RaycastHit hita; 
		if (Physics.Raycast (transform.position, Camera.main.transform.TransformDirection (Vector3.forward), out hita, Mathf.Infinity, myMask) && hita.collider.GetComponent<BlockAlreadyMovingV2> () && lastPlateformSeen != hita.collider.gameObject) { 
			if(lastParticuleDirection!= null){
				lastParticuleDirection.GetComponent<ParticleSystem>().Stop();
				Destroy(lastParticuleDirection,6);
			}
			lastPlateformSeen = hita.collider.gameObject;
			lastParticuleDirection = Instantiate<GameObject>(ParticulesDirection);
			lastParticuleDirection.transform.position = lastPlateformSeen.transform.position;
			lastParticuleDirection.transform.LookAt (lastParticuleDirection.transform.position+hita.collider.GetComponent<BlockAlreadyMovingV2>().direction);
			lastParticuleDirection.transform.parent = lastPlateformSeen.transform;
		}else if ( !Physics.Raycast (transform.position, Camera.main.transform.TransformDirection (Vector3.forward), out hita, Mathf.Infinity, myMask)){
			lastPlateformSeen = null;
			if(lastParticuleDirection != null){
				lastParticuleDirection.GetComponent<ParticleSystem>().Stop();
				Destroy(lastParticuleDirection,6);
		}
		}else{
				lastPlateformSeen = hita.collider.gameObject;
			}




		if (Input.GetKey (KeyCode.Joystick1Button5) || Input.GetMouseButtonDown (1)) {
			RaycastHit hit; 
			if (Physics.Raycast (transform.position, Camera.main.transform.TransformDirection (Vector3.forward), out hit, Mathf.Infinity, myMask) && hit.collider.GetComponent<BlockAlreadyMovingV2> ()) {

				Gun_Absorbe_Direction_Event.start();

				if (myDirectionGo [0] != null) {
					Destroy (myDirectionGo [0].gameObject);
					Destroy (myDirectionGo [1].gameObject);
				}

				myDirectionGo[0] = (GameObject)Instantiate(myGameObbject);
				myDirectionGo [0].transform.position = new Vector3 (hit.transform.position.x, hit.transform.position.y, hit.transform.position.z);
				myDirectionGo[1] = (GameObject)Instantiate(myGameObbject);
				myDirectionGo [1].transform.position = new Vector3 (hit.transform.position.x + hit.transform.GetComponent<BlockAlreadyMovingV2> ().direction.x, hit.transform.position.y + hit.transform.GetComponent<BlockAlreadyMovingV2> ().direction.y , hit.transform.position.z + hit.transform.GetComponent<BlockAlreadyMovingV2> ().direction.z);

				myDirectionGo [0].transform.parent = transform.GetChild (0).transform;
				myDirectionGo [1].transform.parent = transform.GetChild (0).transform;
			}
		}



		//donner une force	
		float triger2 = Input.GetAxis ("trigger2");

		if (( triger2 > 0.2f  || Input.GetMouseButtonDown (0)) && !stocked) {
			stocked = true;
			RaycastHit hit;
			if (Physics.Raycast (transform.position, Camera.main.transform.TransformDirection (Vector3.forward), out hit, Mathf.Infinity, myMask) &&  hit.collider.GetComponent<BlockAlreadyMovingV2>()  ) {

				Gun_Don_Direction_Event.start();
				hit.collider.GetComponent<BlockAlreadyMovingV2> ().direction = Vector3.Normalize( new Vector3 ( myDirectionGo [1].transform.position.x - myDirectionGo [0].transform.position.x,  myDirectionGo [1].transform.position.y - myDirectionGo [0].transform.position.y , myDirectionGo [1].transform.position.z - myDirectionGo [0].transform.position.z));
				lastParticuleDirection.transform.LookAt(lastParticuleDirection.transform.position+hit.collider.GetComponent<BlockAlreadyMovingV2>().direction);	
			}
		} else {
			stocked = false;
		}
		#endregion

		#region Energise
		//take
		bool isTackingEnergie = false;
		if ( Input.GetKey (KeyCode.Joystick1Button4)|| Input.GetKey (KeyCode.A) ) {
			RaycastHit energiseHit;
			if (Physics.Raycast (transform.position, Camera.main.transform.TransformDirection (Vector3.forward), out energiseHit, Mathf.Infinity, myMask) && energiseHit.collider.GetComponent<BlockAlreadyMovingV2> () && myEnergie < myEnergieMax) {

				if(energiseHit.collider.GetComponent<BlockAlreadyMovingV2> ().energie>0){

					if (energiseTake && ( Input.GetKeyDown (KeyCode.Joystick1Button4) || Input.GetKeyDown (KeyCode.A) )) {
						myEnergie += energiseHit.collider.GetComponent<BlockAlreadyMovingV2> ().energie;
						energiseHit.collider.GetComponent<BlockAlreadyMovingV2> ().energie = 0;
						lastParticuleAspiration.GetComponent<ParticleSystem>().Emit((int)myEnergie/3);
						isTackingEnergie = false;
					} else {
						if(Input.GetKeyDown (KeyCode.Joystick1Button4)|| Input.GetKeyDown (KeyCode.A) ){
							
							lastParticuleAspiration = Instantiate<GameObject>(ParticulesAspiration);
							lastParticuleAspiration.GetComponent<particleAttractorLinear>().target = this.transform;
							lastParticuleAspiration.transform.parent = energiseHit.collider.transform;
							lastParticuleAspiration.transform.position = energiseHit.collider.transform.position;

							energiseTake = true;
							energiseTakeTimer = 0.8f;
						}
						isTackingEnergie = true;
						energiseHit.collider.GetComponent<BlockAlreadyMovingV2> ().energie -= 3;
						myEnergie += 3;
					}
				}
			}
		}
		if(energiseTakeTimer > 0){
			energiseTakeTimer -=Time.deltaTime;
		}else{
			energiseTake = false;
		}
		if(isTackingEnergie == false && lastParticuleAspiration != null){
			lastParticuleAspiration.GetComponent<ParticleSystem>().Stop();
			Destroy(lastParticuleAspiration.gameObject,10);

		}
			

		//give
		float triger1 = Input.GetAxis ("trigger1");

		if ( triger1 > 0.2f  || Input.GetKey (KeyCode.E)) {
			RaycastHit hit;
			if (Physics.Raycast (transform.position, Camera.main.transform.TransformDirection (Vector3.forward), out hit, Mathf.Infinity, myMask) && hit.collider.GetComponent<BlockAlreadyMovingV2> ()) {
				if(myEnergie>=3){

					Debug.Log(triger1);
					Debug.Log(lastInputTrigger);
					if (energiseGift &&  (lastInputTrigger <= 0.09f  || Input.GetKeyDown (KeyCode.E))) {
						
					hit.collider.GetComponent<BlockAlreadyMovingV2> ().energie += myEnergie;
					myEnergie = 0;
					} else {
						if (lastInputTrigger <= 0.09f  || Input.GetKeyDown (KeyCode.E)){
							energiseGift = true;
							energiseGiftTimer = 0.8f;
						}
						hit.collider.GetComponent<BlockAlreadyMovingV2> ().energie += 3;
						myEnergie -= 3;
					}
				}
			}
		}
		if(energiseGiftTimer > 0){
			energiseGiftTimer -=Time.deltaTime;
		}else{
			energiseGift = false;
		}
		lastInputTrigger = triger1;



		#endregion

		#region Lock
		// Système de lock

		if (Input.GetMouseButtonDown (2) || Input.GetKeyDown(KeyCode.JoystickButton9) ) {
			if (blockLock == null) {
				RaycastHit hit; 
				if (Physics.Raycast (transform.position, Camera.main.transform.TransformDirection (Vector3.forward), out hit, Mathf.Infinity, myMask) && hit.collider.GetComponent<BlockAlreadyMovingV2> ()) {
					//rb.useGravity = false;
					blockLock = hit.collider.GetComponent<BlockAlreadyMovingV2> ();
				}

			} else {
				
				blockLock = null;

				RaycastHit hit; 
				if (Physics.Raycast (transform.position, Camera.main.transform.TransformDirection (Vector3.forward), out hit, Mathf.Infinity, myMask) &&  hit.collider.GetComponent<BlockAlreadyMovingV2> ()) {
					blockLock = hit.collider.GetComponent<BlockAlreadyMovingV2> ();
				}
			}
		}
		#endregion

		LookRotationSign ();

	}

	void OnCollisionEnter(){
		delock ();
	}


	void delock(){
		blockLock = null;
	}



	public void LookRotationSign(){
		
		
	//	directionVectorSign.transform.rotation =   Quaternion.Euler ( new Vector3 ((-myForce.direction.y )*180,  (-myForce.direction.z )*180, (myForce.direction.x )*180 ));

	}
	


}