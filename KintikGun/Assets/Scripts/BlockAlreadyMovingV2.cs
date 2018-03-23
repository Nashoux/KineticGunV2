using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BlockAlreadyMovingV2 : MonoBehaviour {



public float maxEnergie = 200;

public float energie = 0;
public Vector3 direction = new Vector3(0,0,0);

public Rigidbody rb;

public Material[] myMat;

public 	float _BoundsUp;
public	float _BoundsDown;

void Start(){

	direction = Vector3.Normalize(direction);
	rb = GetComponent<Rigidbody>();
	myMat = GetComponent<MeshRenderer> ().materials;

	ApplyTheVelocity();
}

public void ApplyTheVelocity(){
	if (energie < 0f) {
			energie = 0f;
		}
		float energieNew = energie / (maxEnergie*8);
		//energieNew= Mathf.Log10 (energieNew)*3f;
		if (energieNew < 0) {
			energieNew = 0;
		}
		for (int i = 0; i < myMat.Length; i++) {			
			myMat[i].SetFloat("_MKGlowTexStrength", energieNew);
		}	
		Vector3 newVelocity = direction * Time.deltaTime * energie;
		rb.velocity = newVelocity;
}

	void OnCollisionEnter(Collision col){
		
		if(gameObject.tag != "destructible"){
			if (col.gameObject.GetComponent<BlockAlreadyMovingV2> ()) {
				if (energie > maxEnergie / 2) {

				}
			} 
			if (!col.gameObject.GetComponent<CineticGunV2> () && col.gameObject.tag != "destructible") {
				direction = col.contacts [0].normal.normalized;
				Vector3 velocity = direction * Time.deltaTime * energie;
				rb.velocity = velocity;
			}
			if (col.gameObject.tag == "destructible"){
				if (energie > maxEnergie / 2f) {
					col.gameObject.GetComponent<Rigidbody> ().mass = 10f;
				} else {
					direction = col.contacts [0].normal.normalized;
				}
			}
		}
		ApplyTheVelocity();
	}

	void OnCollisionExit(Collision col){
		if (col.gameObject.tag == "destructible"){
				col.gameObject.GetComponent<Rigidbody> ().mass = 100000;
		}	
	}

}