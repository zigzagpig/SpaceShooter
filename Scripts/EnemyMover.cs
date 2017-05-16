using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMover : MonoBehaviour {

    private Rigidbody enemy_Rigidbody;
    public GameObject prefab_EmemyBullet;
    public Transform enemyBulletPoint;
    private Transform enemy_Transform;

    public float changeVelocityRate = 1f;//敌人子弹改变速度频率
    public float attackRate = 6f;//多久攻击一次
    private float time_Count = 0;//改变速度计时
    private float attack_Count = 0;//是否攻击计时
    public float speed = 20;//敌人移动速度

    Boundary boundary;//边界

    Vector3 vector = Vector3.back;//速度方向

    void Start () {
        enemy_Rigidbody = gameObject.GetComponent<Rigidbody>();
        enemy_Transform = gameObject.GetComponent<Transform>();
        if (GameObject.Find("Player") == null)
            return;
        boundary = GameObject.Find("Player").GetComponent<PlayerController>().Bg_Boundary;
        enemy_Rigidbody.velocity = (Quaternion.Euler(0, Random.Range(0f, 360f), 0) * vector).normalized * speed;
        enemy_Transform.rotation = Quaternion.Euler(180, 0, 0);
    }
	
	void Update () {
        if (GameObject.Find("Player") == null)
            return;

        time_Count = Time.deltaTime + time_Count;
        attack_Count = Time.deltaTime + attack_Count;
        //根据改变速度的频率或者是否出界设定速度方向
        if ( (time_Count >= changeVelocityRate) || isOut())
        {
            ChangeVelocity();
            time_Count = 0;
        }

        if (attack_Count >= attackRate)
        {
            Debug.Log(attack_Count + " " + attackRate);
            //75%直线攻击
            if(Random.Range(0,100)%4 != 0)
            {
                AttackOne();
            }
            else
            {
                AttackTwo();
            }
            attack_Count = 0;
        }

    }

    //攻击方式1：直线攻击
    void AttackOne()
    {
        GameObject enemyBullet;
        enemyBullet = Instantiate(prefab_EmemyBullet, enemyBulletPoint.position, Quaternion.identity);
        enemyBullet.GetComponent<Rigidbody>().velocity = GameObject.Find("Player").GetComponent<Transform>().position - enemyBulletPoint.position;
    }

    //攻击方式2：跟踪
    void AttackTwo()
    {
        GameObject enemyBullet;
        enemyBullet = Instantiate(prefab_EmemyBullet, enemyBulletPoint.position, Quaternion.identity);
        Destroy(enemyBullet, 7f);
        StartCoroutine(AttackPlayer(enemyBullet.GetComponent<Rigidbody>()));//协程跟踪
    }

    //敌人跟踪弹
    IEnumerator AttackPlayer(Rigidbody bullet_Rigidbody) //跟踪
    {
        Transform player_Transform = null;
        Transform bullet_Transform = bullet_Rigidbody.gameObject.GetComponent<Transform>();
        if (GameObject.Find("Player").GetComponent<Transform>() != null)
            player_Transform = GameObject.Find("Player").GetComponent<Transform>();
        //获取玩家和敌人子弹位置，实施跟踪，确保物体都不为空
        while (bullet_Rigidbody != null && player_Transform != null)
        {
            player_Transform = GameObject.Find("Player").GetComponent<Transform>();
            bullet_Transform = bullet_Rigidbody.gameObject.GetComponent<Transform>();
            bullet_Rigidbody.velocity = (player_Transform.position - bullet_Transform.position).normalized * 4;

            yield return new WaitForSeconds(Time.deltaTime * 1);
        }
 
    }

    //改变敌人运动方向
    void ChangeVelocity()
    {
        if( isOut())//出界直接向前飞走
        {
            enemy_Rigidbody.velocity = vector * speed;
        }
        else//否则随便向一个速度方向飞
        {
            enemy_Rigidbody.velocity = (Quaternion.Euler(0, Random.Range(0f, 360f), 0) * vector).normalized * speed;
        }
    }
    
    //是否在画面处理范围
    bool isOut()
    {
        //太过复杂的代码多看几遍，很容易有错
        if (   (enemy_Transform.position.z < boundary.zMax - 1f)
            && (enemy_Transform.position.z > boundary.zMin + 1f)
            && (enemy_Transform.position.x < boundary.xMax - 2f)
            && (enemy_Transform.position.x > boundary.xMin + 2f))
        {
            //Debug.Log("界内"+ enemy_Transform.position);
            return false;
        }
        else
        {
            //Debug.Log("界外" + enemy_Transform.position);
            return true;
        } 
    }
}
