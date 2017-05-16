using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Boundary
{
    public float xMin;
    public float xMax;
    public float zMin;
    public float zMax;
}

public class PlayerController : MonoBehaviour
{

    public float speed = 10f;//角色速度

    public Boundary Bg_Boundary;//背景边界

    private Rigidbody player_Rigidbody;
    private AudioSource fire_AudioSource;//子弹音效
    public AudioSource power_AudioSource;//升级音效

    public GameObject bolt;//三种子弹
    public GameObject bullet;
    public GameObject ball;
    private GameObject bullet_select;

    private DataManager player_DataManager;

    public Transform spawn_Transform;//枪口

    public float fireRate = 0.6f;//发射频率
    private float nextFire;//控制发射频率临时变量

    private BulletManager bulletManager;//发射子弹组件

    private bool isMaxPower = false;
    private bool isMidPower = false;

    void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(h, 0f, v);
        player_Rigidbody.velocity = speed * move;//角色移动
        //防止超出边界，Clamp限制
        player_Rigidbody.position = new Vector3(
            Mathf.Clamp(player_Rigidbody.position.x, Bg_Boundary.xMin, Bg_Boundary.xMax),
            0,
            Mathf.Clamp(player_Rigidbody.position.z, Bg_Boundary.zMin, Bg_Boundary.zMax)
            );
    }

	void Start ()
	{
	    player_Rigidbody = gameObject.GetComponent<Rigidbody>();
        fire_AudioSource = gameObject.GetComponent<AudioSource>();
        bulletManager = gameObject.GetComponent<BulletManager>();
        player_DataManager = gameObject.GetComponent<DataManager>();
        power_AudioSource = GameObject.Find("LevelUpMusic").GetComponent<AudioSource>();
    }
	
	void Update ()
    {
        //空格或者鼠标左键发射子弹
	    if ((Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space)) &&  Time.time > nextFire)
	    {
	        nextFire = Time.time + fireRate;
            //攻击力决定子弹类型
            if (player_DataManager.attack < 40)
            {
                bullet_select = bolt;
            }
            else if (player_DataManager.attack < 50 && player_DataManager.attack >= 40)
            {
                if (isMidPower == false)
                {
                    power_AudioSource.Play();
                    player_DataManager.attack += 5;
                    isMidPower = true;
                }
                bullet_select = bullet;
            }
            else if (player_DataManager.attack >= 50)
            {
                if (isMaxPower ==false)
                {
                    power_AudioSource.Play();
                    player_DataManager.attack += 10;
                    isMaxPower = true;
                }
                    
                bullet_select = ball;
            }
            else
                Debug.Log("没有给子弹获取预制体");

            bulletManager.Shoot(bullet_select, spawn_Transform.position, spawn_Transform.rotation);
            
            fire_AudioSource.Play();
        }
	}
}
