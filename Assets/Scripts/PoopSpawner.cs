﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoopSpawner : MonoBehaviour {

	[SerializeField]
	GameObject poopPrefab;
	GameObject poopClone;
	// Use this for initialization
	void Start () {
		for (int y = 0; y < 2; y++) {
            for (int x = 0; x < 5; x++) {
                poopClone = Instantiate(poopPrefab, transform.position, Quaternion.identity) as GameObject;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
