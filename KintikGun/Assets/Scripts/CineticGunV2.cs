using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CineticGunV2 : MonoBehaviour {

	#region sons

	[FMODUnity.EventRef]
	public string Gun_Max_Energie;

	FMOD.Studio.EventInstance Gun_Max_Energie_Event;

	[FMODUnity.EventRef]
	public string Gun_Min_Energie;

	FMOD.Studio.EventInstance Gun_Min_Energie_Event;


	[FMODUnity.EventRef]
	public string Gun_Lock;

	FMOD.Studio.EventInstance Gun_Lock_Event;

	[FMODUnity.EventRef]
	public string Gun_Unlock;

	FMOD.Studio.EventInstance Gun_Unlock_Event;

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

	GameObject lastParticuleAspiration;
	GameObject lastParticuleAspirationGive;

	GameObject lastPlateformSeen = null;
	GameObject lastParticuleDirection;

	#endregion 

	[SerializeField] Image[] viseurObjects;
	[SerializeField] float[] viseurFill;
	float viseurMax = 0;

	float lastInputTrigger = 0;

	[SerializeField] float castSize = 0.5f;

	[SerializeField] GameObject TurnUi;


	void Start () {
	

		//myForces = new BlockMove.Force (new Vector3(1,0,0), new Vector3( transform.rotation.x, transform.rotation.y, transform.rotation.z) , 0.5f);
		myMask = 5;
		Gun_Absorbe_Energie_Event = FMODUnity.RuntimeManager.CreateInstance (Gun_Absorbe_Energie);
		Gun_Don_Energie_Event = FMODUnity.RuntimeManager.CreateInstance (Gun_Don_Energie);
		Gun_Absorbe_Direction_Event = FMODUnity.RuntimeManager.CreateInstance (Gun_Absorbe_Direction);
		Gun_Don_Direction_Event = FMODUnity.RuntimeManager.CreateInstance (Gun_Don_Direction);
		Gun_Lock_Event = FMODUnity.RuntimeManager.CreateInstance (Gun_Lock);
		Gun_Unlock_Event = FMODUnity.RuntimeManager.CreateInstance (Gun_Unlock);
		Gun_Max_Energie_Event = FMODUnity.RuntimeManager.CreateInstance (Gun_Max_Energie);
		Gun_Min_Energie_Event = FMODUnity.RuntimeManager.CreateInstance (Gun_Min_Energie);

		myDirectionGo[0] = (GameObject)Instantiate(myGameObbject);
		myDirectionGo [0].transform.position = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
		myDirectionGo[1] = (GameObject)Instantiate(myGameObbject);
		myDirectionGo [1].transform.position = new Vector3 (transform.position.x, transform.position.y, transform.position.z+1);
		myDirectionGo [0].transform.parent = transform.GetChild (0).transform;
		myDirectionGo [1].transform.parent = transform.GetChild (0).transform;


	}
	
	void Update ()	{

		viseurObjects [0].fillAmount = viseurFill[0] + myEnergie / myEnergieMax*4;
		viseurObjects [1].fillAmount = viseurFill [1] + myEnergie / myEnergieMax*4 - 1;
		viseurObjects [2].fillAmount = viseurFill [2] + myEnergie / myEnergieMax*4 - 2;
		viseurObjects [3].fillAmount = viseurFill [3] + myEnergie / myEnergieMax*4 - 3;				



		#region direction

		//donner une force	
		float triger2 = Input.GetAxis ("trigger2");

		if (( triger2 > 0.2f  || Input.GetMouseButtonDown (0)) && !stocked) {
			stocked = true;
			RaycastHit hit;
			if (Physics.Raycast (transform.position, Camera.main.transform.TransformDirection (Vector3.forward), out hit, Mathf.Infinity, myMask) &&  hit.collider.GetComponent<BlockAlreadyMovingV2>()  ) {
				BlockAlreadyMovingV2 myBlock = hit.collider.GetComponent<BlockAlreadyMovingV2>();
				Gun_Don_Direction_Event.start();
				myBlock.direction = Vector3.Normalize( new Vector3 ( myDirectionGo [1].transform.position.x - myDirectionGo [0].transform.position.x,  myDirectionGo [1].transform.position.y - myDirectionGo [0].transform.position.y , myDirectionGo [1].transform.position.z - myDirectionGo [0].transform.position.z));
				lastParticuleDirection.transform.LookAt(lastParticuleDirection.transform.position+myBlock.direction);	
				Vector3 velocity = myBlock.direction * Time.deltaTime * myBlock.energie;
				myBlock.rb.velocity = velocity;
				myBlock.rb.angularVelocity = new Vector3(0,0,0);
			}
		} else {
			stocked = false;
		}

		//inverse
		if(Input.GetKeyDown(KeyCode.Joystick1Button5) || Input.GetMouseButtonDown(1)){
			RaycastHit hit;
			if (Physics.Raycast (transform.position, Camera.main.transform.TransformDirection (Vector3.forward), out hit, Mathf.Infinity, myMask) &&  hit.collider.GetComponent<BlockAlreadyMovingV2>()  ) {
				BlockAlreadyMovingV2 myBlock = hit.collider.GetComponent<BlockAlreadyMovingV2>();
				Gun_Don_Direction_Event.start();
				myBlock.direction = Vector3.Normalize( new Vector3 ( myDirectionGo [0].transform.position.x-myDirectionGo [1].transform.position.x,  myDirectionGo [0].transform.position.y - myDirectionGo [1].transform.position.y,myDirectionGo [0].transform.position.z -  myDirectionGo [1].transform.position.z));
				lastParticuleDirection.transform.LookAt(lastParticuleDirection.transform.position+myBlock.direction);	
				Vector3 velocity = myBlock.direction * Time.deltaTime * myBlock.energie;
				myBlock.rb.velocity = velocity;
				myBlock.rb.angularVelocity = new Vector3(0,0,0);
			}
		}
		#endregion

		#region Energise
		//take
		bool isTackingEnergie = false;
		if ( Input.GetKey (KeyCode.Joystick1Button4)|| Input.GetKey (KeyCode.A) ) {
				RaycastHit energiseHit;
				if (Physics.SphereCast (transform.position, castSize, Camera.main.transform.TransformDirection (Vector3.forward), out energiseHit, Mathf.Infinity, myMask) && energiseHit.collider.GetComponent<BlockAlreadyMovingV2> ()  ) {
					BlockAlreadyMovingV2 myBlock = energiseHit.collider.GetComponent<BlockAlreadyMovingV2> ();
					if(myEnergie < myEnergieMax){
						if(myBlock.energie>0){							
							if (energiseTake && ( Input.GetKeyDown (KeyCode.Joystick1Button4) || Input.GetKeyDown (KeyCode.A) )) {
								float myAddedEnergie = myEnergieMax - myEnergie;
								if(myAddedEnergie <= myBlock.energie){
									myEnergie += myAddedEnergie;
									myBlock.energie -= myAddedEnergie;
								}else{
									myEnergie += myBlock.energie;
									myBlock.energie = 0;
								}

								lastParticuleAspiration.GetComponent<ParticleSystem>().Emit((int)myEnergie/3);
								lastParticuleAspiration.GetComponent<ParticleSystem>().Stop();
								Destroy(lastParticuleAspiration.gameObject,2f);
								isTackingEnergie = false;
							} else {
								if(Input.GetKeyDown (KeyCode.Joystick1Button4)|| Input.GetKeyDown (KeyCode.A) || lastPlateformSeen != energiseHit.collider.gameObject){
									Gun_Absorbe_Energie_Event.start();
									lastParticuleAspiration = Instantiate<GameObject>(ParticulesAspiration);
									lastParticuleAspiration.GetComponent<particleAttractorLinear>().target = this.transform;
									lastParticuleAspiration.transform.parent = energiseHit.collider.transform;
									lastParticuleAspiration.transform.position = energiseHit.collider.transform.position;
									Destroy(lastParticuleAspiration.gameObject,8);
									energiseTake = true;
									energiseTakeTimer = 0.8f;
								}
								isTackingEnergie = true;
								myBlock.energie -= 3;
								myEnergie += 3;
							}

							if (myBlock.energie < 0f) {
								myBlock.energie = 0f;
							}
							float energieNew = myBlock.energie / 20f;
							energieNew= Mathf.Log10 (energieNew)*3f;
							if (energieNew < 0) {
								energieNew = 0;
							}
							for (int i = 0; i < myBlock.myMat.Length; i++) {			
								myBlock.myMat[i].SetFloat("_Size1", energieNew);
								myBlock.myMat[i].SetFloat("_Size2", energieNew*70f/100f);
							}	
							Vector3 velocity = myBlock.direction * Time.deltaTime * myBlock.energie;
							myBlock.rb.velocity = velocity;				
						}
					}else{
						if(Input.GetKeyDown (KeyCode.Joystick1Button4) || Input.GetKeyDown (KeyCode.A)){
							Gun_Min_Energie_Event.start();
						}
					}
			}else if (energiseHit.collider != null && energiseHit.transform.tag == "destructible"){
					if(myEnergie < myEnergieMax){
						if(energiseHit.rigidbody.velocity.magnitude >0){

							if (energiseTake && ( Input.GetKeyDown (KeyCode.Joystick1Button4) || Input.GetKeyDown (KeyCode.A) )) {
								float myAddedEnergie = myEnergieMax - myEnergie;
								if(myAddedEnergie <= energiseHit.rigidbody.velocity.magnitude){
									myEnergie += myAddedEnergie;
									energiseHit.rigidbody.velocity = new Vector3(0,0,0);
								}else{
									myEnergie += energiseHit.rigidbody.velocity.magnitude;
									Debug.Log(energiseHit.rigidbody.velocity.magnitude);
									energiseHit.rigidbody.velocity = new Vector3(0,0,0);
								}
								energiseHit.rigidbody.angularVelocity = new Vector3(0,0,0);
								lastParticuleAspiration.GetComponent<ParticleSystem>().Emit((int)myEnergie/3);
								lastParticuleAspiration.GetComponent<ParticleSystem>().Stop();
								Destroy(lastParticuleAspiration.gameObject,2f);
								isTackingEnergie = false;
							} else {
								if(Input.GetKeyDown (KeyCode.Joystick1Button4)|| Input.GetKeyDown (KeyCode.A) || lastPlateformSeen != energiseHit.collider.gameObject){
									Gun_Absorbe_Energie_Event.start();
									lastParticuleAspiration = Instantiate<GameObject>(ParticulesAspiration);
									lastParticuleAspiration.GetComponent<particleAttractorLinear>().target = this.transform;
									lastParticuleAspiration.transform.parent = energiseHit.collider.transform;
									lastParticuleAspiration.transform.position = energiseHit.collider.transform.position;
									Destroy(lastParticuleAspiration.gameObject,8);
									energiseTake = true;
									energiseTakeTimer = 0.8f;
								}
								isTackingEnergie = true;
								energiseHit.rigidbody.velocity -= new Vector3(1,1,1) ;
								energiseHit.rigidbody.angularVelocity -= new Vector3(1,1,1);
								if(energiseHit.rigidbody.velocity.x < 0){
									energiseHit.rigidbody.velocity = new Vector3 (0, energiseHit.rigidbody.velocity.y, energiseHit.rigidbody.velocity.z);
								}if(energiseHit.rigidbody.velocity.y < 0){
									energiseHit.rigidbody.velocity = new Vector3 (energiseHit.rigidbody.velocity.x, 0, energiseHit.rigidbody.velocity.z);
								}if(energiseHit.rigidbody.velocity.z < 0){
									energiseHit.rigidbody.velocity = new Vector3 (energiseHit.rigidbody.velocity.z, energiseHit.rigidbody.velocity.y, 0);
								}if(energiseHit.rigidbody.angularVelocity.x < 0){
									energiseHit.rigidbody.angularVelocity = new Vector3 (0, energiseHit.rigidbody.angularVelocity.y, energiseHit.rigidbody.angularVelocity.z);
								}if(energiseHit.rigidbody.angularVelocity.y < 0){
									energiseHit.rigidbody.angularVelocity = new Vector3 (energiseHit.rigidbody.angularVelocity.x, 0, energiseHit.rigidbody.angularVelocity.z);
								}if(energiseHit.rigidbody.angularVelocity.z < 0){
									energiseHit.rigidbody.angularVelocity = new Vector3 (energiseHit.rigidbody.angularVelocity.z, energiseHit.rigidbody.angularVelocity.y, 0);
								}
								myEnergie += 3;
							}				
						}
					}else{
						if(Input.GetKeyDown (KeyCode.Joystick1Button4) || Input.GetKeyDown (KeyCode.A)){
							Gun_Min_Energie_Event.start();
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
		bool isTackingEnergieGive = false;
		if ( triger1 > 0.2f  || Input.GetKey (KeyCode.E)) {
			RaycastHit hit;
			if (Physics.SphereCast (transform.position,castSize, Camera.main.transform.TransformDirection (Vector3.forward), out hit, Mathf.Infinity, myMask) && hit.collider.GetComponent<BlockAlreadyMovingV2> ()) {
				if(myEnergie>=3){
					BlockAlreadyMovingV2 myBlock = hit.collider.GetComponent<BlockAlreadyMovingV2> ();
					Debug.Log(triger1);
					Debug.Log(lastInputTrigger);
					if (energiseGift &&  (lastInputTrigger <= 0.09f  || Input.GetKeyDown (KeyCode.E))) {
						lastParticuleAspirationGive.GetComponent<ParticleSystem>().Emit((int)myEnergie/1);
						lastParticuleAspirationGive.GetComponent<ParticleSystem>().Stop();
						Destroy(lastParticuleAspirationGive.gameObject,2.5f);
						isTackingEnergieGive = false;
					myBlock.energie += myEnergie;
					myEnergie = 0;
					} else {
						if (lastInputTrigger <= 0.09f  || Input.GetKeyDown (KeyCode.E) || lastPlateformSeen != hit.collider.gameObject){
							Gun_Don_Energie_Event.start();
							lastParticuleAspirationGive = Instantiate<GameObject>(ParticulesAspiration);
							lastParticuleAspirationGive.GetComponent<particleAttractorLinear>().target = hit.collider.transform;
							lastParticuleAspirationGive.GetComponent<particleAttractorLinear>().speed = 5;
							lastParticuleAspirationGive.transform.parent = this.transform;
							lastParticuleAspirationGive.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y -3f, this.transform.position.z);
							Destroy(lastParticuleAspirationGive.gameObject,5);
							energiseGift = true;
							energiseGiftTimer = 0.8f;
						}
						myBlock.energie += 3;
						myEnergie -= 3;
						lastParticuleAspirationGive.GetComponent<ParticleSystem>().Emit((int)myEnergie/3);
					}

					if (myBlock.energie < 0f) {
								myBlock.energie = 0f;
							}
							float energieNew = myBlock.energie / 20f;
							energieNew= Mathf.Log10 (energieNew)*3f;
							if (energieNew < 0) {
								energieNew = 0;
							}
							for (int i = 0; i < myBlock.myMat.Length; i++) {			
								myBlock.myMat[i].SetFloat("_Size1", energieNew);
								myBlock.myMat[i].SetFloat("_Size2", energieNew*70f/100f);
							}	
							Vector3 velocity = myBlock.direction * Time.deltaTime * myBlock.energie;
							myBlock.rb.velocity = velocity;	
				}else{
					if(lastInputTrigger <= 0.09f  || Input.GetKeyDown (KeyCode.E)){
						Gun_Min_Energie_Event.start();
					}
				}
			}
		}
		if(energiseGiftTimer > 0){
			energiseGiftTimer -=Time.deltaTime;
		}else{
			energiseGift = false;
		}
		if(isTackingEnergieGive == false && lastParticuleAspirationGive != null){
			lastParticuleAspirationGive.GetComponent<ParticleSystem>().Stop();
			Destroy(lastParticuleAspiration.gameObject,10);

		}
		lastInputTrigger = triger1;


		#endregion


		RaycastHit hita; 
		if (Physics.SphereCast (transform.position,castSize,Camera.main.transform.TransformDirection (Vector3.forward), out hita, Mathf.Infinity, myMask) && hita.collider.GetComponent<BlockAlreadyMovingV2> () && lastPlateformSeen != hita.collider.gameObject && hita.collider.GetComponent<BlockAlreadyMovingV2> ().direction != new Vector3(0,0,0)) { 
			if(lastParticuleDirection!= null){
				lastParticuleDirection.GetComponent<ParticleSystem>().Stop();
				Destroy(lastParticuleDirection,6);
			}
			lastPlateformSeen = hita.collider.gameObject;
			lastParticuleDirection = Instantiate<GameObject>(ParticulesDirection);
			lastParticuleDirection.transform.position = lastPlateformSeen.transform.position;
			lastParticuleDirection.transform.LookAt (lastParticuleDirection.transform.position+hita.collider.GetComponent<BlockAlreadyMovingV2>().direction);
			lastParticuleDirection.transform.parent = lastPlateformSeen.transform;
		}else if ( !Physics.SphereCast (transform.position,castSize, Camera.main.transform.TransformDirection (Vector3.forward), out hita, Mathf.Infinity, myMask )){
			lastPlateformSeen = null;
			if(lastParticuleDirection != null){
				lastParticuleDirection.GetComponent<ParticleSystem>().Stop();
				Destroy(lastParticuleDirection,6);
			}
		}else{
			lastPlateformSeen = hita.collider.gameObject;
		}


		#region Lock
		// Système de lock

		if (Input.GetMouseButtonDown (2) || Input.GetKeyDown(KeyCode.JoystickButton9) ) {
			if (blockLock == null) {
				Gun_Lock_Event.start();
				RaycastHit hit; 
				if (Physics.SphereCast (transform.position,castSize, Camera.main.transform.TransformDirection (Vector3.forward), out hit, Mathf.Infinity, myMask) && hit.collider.GetComponent<BlockAlreadyMovingV2> ()) {
					//rb.useGravity = false;
					blockLock = hit.collider.GetComponent<BlockAlreadyMovingV2> ();
					StartCoroutine("turnLeftUi");
				}

			} else {
				Gun_Unlock_Event.start();
				blockLock = null;
				StartCoroutine("turnRightUi");

//				RaycastHit hit; 
//				if (Physics.Raycast (transform.position, Camera.main.transform.TransformDirection (Vector3.forward), out hit, Mathf.Infinity, myMask) &&  hit.collider.GetComponent<BlockAlreadyMovingV2> ()) {
//					blockLock = hit.collider.GetComponent<BlockAlreadyMovingV2> ();
//				}
			}
		}
		#endregion


	}

	void OnCollisionEnter(){
		delock ();
	}




	void delock(){
		if (blockLock != null) {
			Gun_Unlock_Event.start();
			StartCoroutine("turnRightUi");
			blockLock = null;
		}
	}



	IEnumerator turnLeftUi(){
		for (int i = 0; i < 8; i++) {
			TurnUi.transform.Rotate (0, 0, 10 + i*2);
			yield return new WaitForEndOfFrame ();
		}
		yield return null;
	}

	IEnumerator turnRightUi(){
		for (int i = 0; i < 8; i++) {
			TurnUi.transform.Rotate (0, 0, -10 - i*2);
			yield return new WaitForEndOfFrame ();
		}
		yield return null;
	}


}
