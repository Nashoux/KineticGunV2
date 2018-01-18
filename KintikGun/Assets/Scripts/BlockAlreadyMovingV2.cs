using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BlockAlreadyMovingV2 : MonoBehaviour {


float maxEnergie = 200;

public float energie = 0;
public Vector3 direction = new Vector3(0,0,0);

Rigidbody rb;

	Material myMat;

	float _BoundsUp;
	float _BoundsDown;

void Start(){

	direction = Vector3.Normalize(direction);
	rb = GetComponent<Rigidbody>();
	myMat = GetComponent<MeshRenderer> ().material;
	Collider myCol = gameObject.AddComponent<BoxCollider> ();
	_BoundsUp = myCol.bounds.max.y;
	_BoundsDown = myCol.bounds.min.y;

	Destroy (myCol);
}


void Update(){

		myMat.SetFloat ("_BoundsUp", _BoundsUp );
		myMat.SetFloat ("_BoundsDown", _BoundsDown);

		if (energie <= 10f) {
			energie = 0f;
		}

		float energieNew = energie / 20;
		energieNew= Mathf.Log10 (energieNew)*3;

		myMat.SetFloat("_Size1", energieNew);
		myMat.SetFloat("_Size2", energieNew*70/100);

		myMat.SetFloat ("_fillPourcent", energie / maxEnergie*2 -1);


	
		Vector3 velocity = direction * Time.deltaTime * energie;



rb.velocity = velocity;
}



	void OnCollisionEnter(Collision col){

		if (col.gameObject.GetComponent<BlockAlreadyMovingV2> ()) {
			if (energie > maxEnergie / 2) {

			}
		} 
		if (!col.gameObject.GetComponent<CineticGunV2> () && col.gameObject.tag != "destructible") {
			direction = col.contacts [0].normal.normalized;
		}
		if (col.gameObject.tag == "destructible"){
			if (energie > maxEnergie / 2) {
				col.gameObject.GetComponent<Rigidbody> ().mass = 10;
			} else {
				direction = col.contacts [0].normal.normalized;
			}
		}

	}

	void OnCollisionExit(Collision col){
		if (col.gameObject.tag == "destructible"){
				col.gameObject.GetComponent<Rigidbody> ().mass = 100000;
		}	
	}

}