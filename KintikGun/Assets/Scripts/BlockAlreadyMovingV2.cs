﻿using System.Collections;
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
	Collider myCol = gameObject.AddComponent<BoxCollider> ();
	_BoundsUp = myCol.bounds.max.y;
	_BoundsDown = myCol.bounds.min.y;
	for (int i = 0; i < myMat.Length; i++) {
		myMat[i].SetFloat ("_BoundsUp", _BoundsUp);
		myMat[i].SetFloat ("_BoundsDown", _BoundsDown);
	}

	Destroy (myCol);


	if (energie < 0f) {
			energie = 0f;
		}
		float energieNew = energie / 20f;
		energieNew= Mathf.Log10 (energieNew)*3f;
		if (energieNew < 0) {
			energieNew = 0;
		}
		for (int i = 0; i < myMat.Length; i++) {			
			myMat[i].SetFloat("_Size1", energieNew);
			myMat[i].SetFloat("_Size2", energieNew*70f/100f);
		}	
		Vector3 velocity = direction * Time.deltaTime * energie/10;
		rb.velocity = velocity;
}


/*void Update(){
		if (energie < 0f) {
			energie = 0f;
		}
		float energieNew = energie / 20f;
		energieNew= Mathf.Log10 (energieNew)*3f;
		if (energieNew < 0) {
			energieNew = 0;
		}
		for (int i = 0; i < myMat.Length; i++) {			
			myMat[i].SetFloat("_Size1", energieNew);
			myMat[i].SetFloat("_Size2", energieNew*70f/100f);
		}	
		Vector3 velocity = direction * Time.deltaTime * energie;
		rb.velocity = velocity;
}*/



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
	}

	void OnCollisionExit(Collision col){
		if (col.gameObject.tag == "destructible"){
				col.gameObject.GetComponent<Rigidbody> ().mass = 100000;
		}	
	}

}