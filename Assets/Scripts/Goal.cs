using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {

	[SerializeField]
	GameObject poop;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider poop)
	{
		if(poop.tag == "PoopBall")
			Debug.Log("GOL !!!");
	}
}
