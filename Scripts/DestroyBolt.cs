using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBolt : MonoBehaviour
{
    //销毁子弹，子弹碰到东西。已废弃。
    void OnTriggerExit(Collider other)
    {
        Destroy(other.gameObject);
        Debug.Log("销毁了");
    }
}
