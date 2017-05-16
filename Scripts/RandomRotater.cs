using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotater : MonoBehaviour
{
    public float rotateAngularVelocityle = 5;

	void Start ()
	{   //让物体旋转
	    gameObject.GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * rotateAngularVelocityle;
	}
}
