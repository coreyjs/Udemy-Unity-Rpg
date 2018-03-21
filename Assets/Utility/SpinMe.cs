using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinMe : MonoBehaviour {

	[SerializeField] float xRotationsPerMinute = 1f;
	[SerializeField] float yRotationsPerMinute = 1f;
	[SerializeField] float zRotationsPerMinute = 1f;
	
	void Update () {

        //xdergreesPerFrame = TIme.DeltaTime, 60, 360, xRotationsPerMInute
        // degrees frame^-1 = seconds frame^-1, seconds minute^-1

        // degress frame^-1 = frame^-1 * degress

	    float xDegreesPerFrame = Time.deltaTime / 60 * 360 * xRotationsPerMinute;
        transform.RotateAround (transform.position, transform.right, xDegreesPerFrame);

		float yDegreesPerFrame = 0; // TODO COMPLETE ME
        transform.RotateAround (transform.position, transform.up, yDegreesPerFrame);

        float zDegreesPerFrame = 0; // TODO COMPLETE ME
        transform.RotateAround (transform.position, transform.forward, zDegreesPerFrame);
	}
}
