using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBytime : MonoBehaviour
{
    //定义物品生存时间，爆炸特效用
    public float lifetime;

	void Start () {
		Destroy(gameObject, lifetime);
	}
}
