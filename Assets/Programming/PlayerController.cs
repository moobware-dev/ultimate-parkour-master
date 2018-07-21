﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float moveSensitivity = 1f;
    public float leftMostBound = -1.5f;
    public float rightMostBound = 1.5f;

    Rigidbody playerRigidBody;

	void Start () {
		
	}
	
	void FixedUpdate () {
        var movementChangeInput = Input.GetAxis("Mouse ScrollWheel");
        var movement = movementChangeInput * moveSensitivity;

        var newHorizontalPosition = Mathf.Clamp(transform.position.x + movement, leftMostBound, rightMostBound);
        transform.position = new Vector3(newHorizontalPosition, transform.position.y, transform.position.z);
    }
}