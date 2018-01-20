using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeplacementMenu : MonoBehaviour {

	[SerializeField] GameObject positionDepart;
	float speed;

	// Use this for initialization
	void Start () {
		speed = Random.Range (0.9f, 2.2f);
	}

	// Update is called once per frame
	void Update () {
		transform.position = new Vector3 (transform.position.x - speed*Time.deltaTime, transform.position.y, transform.position.z);

		if (transform.position.x - positionDepart.transform.position.x < -60) {
			transform.position = positionDepart.transform.position;
			speed = Random.Range (0.9f, 1.5f);
		}
	}
}
