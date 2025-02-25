﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	public Transform target;

	//[Range(0, 1)]
	public float smoothSpeed;
	public Vector3 offset;

	void LateUpdate(){

		Vector3 desiredPosition = target.position + offset;
		Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
		transform.position = smoothedPos;
	}
}
