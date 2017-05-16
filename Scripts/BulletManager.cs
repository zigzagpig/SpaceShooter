using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    private float speed;//速度
    private Rigidbody bullet_Rigidbody;
    //子弹种类
    public enum BulletType
    {
        DEFAULT,
        LEVEL1,
        LEVEL2,
        LEVEL3,
        LEVEL4,
        LEVEL5,
        ROTATE
    }

    public BulletType bulletType;

    void Start ()
    {
        bullet_Rigidbody = gameObject.GetComponent<Rigidbody>();
    }

    //发射子弹
    public void Shoot(GameObject prefab_bullet, Vector3 position, Quaternion rotation)
    {
        GameObject bullet;
        speed = GameController.bulletSpeed;

        if (bulletType == BulletType.DEFAULT)//普通直接发射
        {
            bullet = Instantiate(prefab_bullet, position, rotation);
            bullet.GetComponent<Rigidbody>().velocity = rotation * Vector3.forward * speed; 
        }
        else if (bulletType == BulletType.LEVEL1)//小发散枪
        {
            for(int i = 0; i < 3; i++)
            {
                rotation = Quaternion.Euler(0, 10 * i - 10, 0);
                bullet = Instantiate(prefab_bullet, position, rotation);
                bullet.GetComponent<Rigidbody>().velocity = rotation * Vector3.forward * speed;
            }
            bullet = Instantiate(prefab_bullet, position, rotation);
            bullet.GetComponent<Rigidbody>().velocity = rotation * Vector3.left * speed;
            bullet = Instantiate(prefab_bullet, position, rotation);
            bullet.GetComponent<Rigidbody>().velocity = rotation * Vector3.right * speed;
        }
        else if (bulletType == BulletType.LEVEL2)//大发散枪
        {
            for (int i = 0; i < 5; i++)
            {
                rotation = Quaternion.Euler(0, 15 * i - 30, 0);
                bullet = Instantiate(prefab_bullet, position, rotation);
                bullet.GetComponent<Rigidbody>().velocity = rotation * Vector3.forward * speed;
            }
        }
        else if (bulletType == BulletType.LEVEL3)//跟踪弹
        {
            for (int i = 0; i < 5; i++)
            {
                bullet = Instantiate(prefab_bullet, position, rotation);
                StartCoroutine(AttackEnemy(bullet.GetComponent<Rigidbody>()));//协程跟踪
            }
        }
        else if (bulletType == BulletType.LEVEL4)//5点弹
        {
            float changeX = 0f; //用于记录变化值，修改了发射点之后再改回来
            float changeZ = 0f; //用于记录变化值，修改了发射点之后再改回来
            float distance = 2f; //子弹间距

            for (int i = 0; i < 5; i++)
            {
                if(i == 0)
                {
                    changeX = -distance;
                    changeZ = -distance;
                }
                else if(i == 1)
                {
                    changeX = -distance;
                    changeZ = distance;
                }
                else if (i == 2)
                {
                    changeX = 0f;
                    changeZ = 0f;
                }
                else if (i == 3)
                {
                    changeX = distance;
                    changeZ = -distance;
                }
                else if (i == 4)
                {
                    changeX = distance;
                    changeZ = distance;
                }

                position.x = position.x + changeX;
                position.z = position.z + changeZ;

                bullet = Instantiate(prefab_bullet, position, rotation);
                bullet.GetComponent<Rigidbody>().velocity = rotation * Vector3.forward * speed;

                position.x = position.x - changeX;
                position.z = position.z - changeZ;
            }
        }
        else if (bulletType == BulletType.LEVEL5)//守护弹
        {
            for (int i = 0; i < 3; i++)
            {
                rotation = Quaternion.Euler(0, 10 * i - 10, 0);
                bullet = Instantiate(prefab_bullet, position, rotation);
                bullet.GetComponent<Rigidbody>().velocity = rotation * Vector3.forward * speed;
            }

            bullet = Instantiate(prefab_bullet, position, rotation);
            StartCoroutine(BulletProtect(bullet.GetComponent<Rigidbody>()));//协程旋转
        }
        else if (bulletType == BulletType.ROTATE)//旋转弹
        {
            for (int i = 0; i < 5; i++)
            {
                Debug.Log("什么情况"+ prefab_bullet);
                bullet = Instantiate(prefab_bullet, position + new Vector3(i - 1, 0, 0), rotation);
                StartCoroutine(BulletRotate(bullet.GetComponent<Rigidbody>()));//协程乱转
            }
            
        }
    }
    //查找存在的敌人
    private GameObject FindEnemy()
    {
        GameObject[] enemyByFind = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] asteroidByFind = GameObject.FindGameObjectsWithTag("Asteroid");
        //随机查找目标，每一次都切换目标
        if (Random.Range(0,2)==1 && enemyByFind.Length != 0)
        {
            return enemyByFind[Random.Range(0, enemyByFind.Length)];
        }
        else if(asteroidByFind.Length != 0)
        {
            return asteroidByFind[Random.Range(0, asteroidByFind.Length)];
        }
        else
        {
            return null;
        }
    }

    IEnumerator AttackEnemy(Rigidbody bullet_Rigidbody) //跟踪
    {
        GameObject go = FindEnemy();
        Transform enemy_Transform = null;
        Transform bullet_Transform = null;

        if (go == null)//无目标时向前攻击
        {
            bullet_Rigidbody.velocity = Vector3.forward * speed;
        }
        else
        {
            enemy_Transform = go.GetComponent<Transform>();
            bullet_Transform = bullet_Rigidbody.gameObject.GetComponent<Transform>();
        }   

        while (enemy_Transform != null && bullet_Transform != null && go != null)
        {   //获取子弹和敌人位置，实时跟踪
            enemy_Transform = enemy_Transform.gameObject.GetComponent<Transform>();
            bullet_Transform = bullet_Rigidbody.gameObject.GetComponent<Transform>();
            bullet_Rigidbody.velocity = (enemy_Transform.position - bullet_Transform.position).normalized * speed;

            yield return new WaitForSeconds(Time.deltaTime * 1);
        }

    }

    IEnumerator BulletProtect(Rigidbody bullet_Rigidbody) //旋转
    {
        Rigidbody player_Rigidbody = GameObject.Find("Player").GetComponent<Rigidbody>();
        float degree = 0f;//初始旋转度数

        while (player_Rigidbody != null && bullet_Rigidbody != null)
        {
            player_Rigidbody = GameObject.Find("Player").GetComponent<Rigidbody>();
            bullet_Rigidbody.position = player_Rigidbody.position + Quaternion.Euler(0, degree, 0) * Vector3.forward * 6;

            degree = degree + 5f;//每帧涨3度

            if (degree >= 360f)
                degree = 0f;

            if (bullet_Rigidbody == null)
                break;

            yield return new WaitForSeconds(Time.deltaTime);//处理完之后暂停等下一帧
        }
            
    }

    IEnumerator BulletRotate(Rigidbody bullet_Rigidbody) //延迟旋转弹
    {
        bullet_Rigidbody.velocity = Vector3.forward * speed;
        //计时切换状态
        float timeCount = 0f;
        float switch_velocity = 0f;
        while (true)
        {
            if(switch_velocity <= 0.2f)//左旋
            {
                bullet_Rigidbody.velocity = bullet_Rigidbody.velocity.normalized * speed;
                bullet_Rigidbody.AddForce(Quaternion.Euler(0, -90, 0) * bullet_Rigidbody.velocity * 15);
            }
            else if(switch_velocity <= 0.3f)//直线
            {
                bullet_Rigidbody.velocity = bullet_Rigidbody.velocity.normalized * speed;
            }
            else if (switch_velocity <= 0.5f)//右旋
            {
                bullet_Rigidbody.velocity = bullet_Rigidbody.velocity.normalized * speed;
                bullet_Rigidbody.AddForce(Quaternion.Euler(0, 90, 0) * bullet_Rigidbody.velocity * 30);
            }

            timeCount = timeCount + Time.deltaTime;
            switch_velocity += Time.deltaTime;
            if (switch_velocity >= 0.5f)
                switch_velocity = 0f;

            yield return new WaitForSeconds(Time.deltaTime);
            //超过3秒发射
            if (timeCount > 3f || bullet_Rigidbody == null)
                break;
        }
        //最后直线发射
        if (bullet_Rigidbody != null)
            bullet_Rigidbody.velocity = Vector3.forward * speed;
    }
}
