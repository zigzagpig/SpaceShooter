using System.Collections;
using System.Collections.Generic;
//using UnityEditor;
using UnityEngine;

public class Mover : MonoBehaviour
{
    public float speed = 20;//移动速度

    private Rigidbody bolt_rigidbody;
	void Start ()
	{
	    bolt_rigidbody = gameObject.GetComponent<Rigidbody>();
        //小行星运动代码，if是遗留，Player已不适用
        if (gameObject.name != "Player")
	    {
            Debug.Log("正在执行");
            Vector3 vector = Vector3.forward;
            vector = Quaternion.Euler(0, Random.Range(-45f, 45f), 0) * vector;
            bolt_rigidbody.velocity = vector * speed;
        }
	    else
	    {
            bolt_rigidbody.velocity = transform.forward * speed;
        }
        
    }
}
