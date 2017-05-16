using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Vector3 spawnPosition;   //发射点
    public GameObject[] hazards;//危险物

    public int totalScore = 0;//总分数变量
    public int damageBoss = 0;//造成的伤害总量
    public int maxBossLife;

    static public float bulletSpeed = 10f;//子弹初始速度

    private float rate = 0.5f;//出怪频率
    private float calculate = 0f;//计算是否到出怪时间

    private UILabel scoreLabel;//UI相关的应该做个脚本文件统一管理
    private UISlider lifeSlider;
    private UILabel logLabel;
    private UILabel gameEnd;

    static public bool GameOver = false;

    //增加总分数
    public void AddScore(int score)
    {
        totalScore += score;
        scoreLabel.text = "分数   " + totalScore;
    }

    void DestroyAllEnemy()
    {
        GameObject[] go = GameObject.FindGameObjectsWithTag("Asteroid");
        for(int i=0; i < go.Length; i++)
        {
            go[i].GetComponent<DataManager>().GetDamage(9999);
        }

        go = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < go.Length; i++)
        {
            go[i].GetComponent<DataManager>().GetDamage(9999);
        }
    }

    //伤害Boss
    public void HurtBoss(int damage)
    {
        damageBoss += damage;
        Debug.Log(damageBoss);
        if (damageBoss >= maxBossLife)
        {
            damageBoss = maxBossLife;
            gameEnd.text = "你赢了";
            GameOver = true;
            DestroyAllEnemy();
        }
        lifeSlider.value = (maxBossLife - damageBoss) * 1.0f/maxBossLife;
    }

    //增加子弹速度
    static public void AddBulletSpeed()
    {
        bulletSpeed += 1;
    }

    void Start()
    {
        
        logLabel = GameObject.Find("GameLog").GetComponent<UILabel>();
        logLabel.text = "游戏开始了";
        gameEnd = GameObject.Find("GameEnd").GetComponent<UILabel>();
        gameEnd.text = "";
        scoreLabel = GameObject.Find("Score").GetComponent<UILabel>();
        scoreLabel.text = "分数   " + totalScore;

        lifeSlider = GameObject.Find("BossLife").GetComponent<UISlider>();
        lifeSlider.value = 1f;
    }

    void Update()
    {
        if (GameOver == true)
            return;
        //更新敌人
        if (calculate >= rate)
        {
            SpawnHazards();
            calculate = 0;
        }
        calculate += Time.deltaTime;
    }
    //更新逻辑
    void SpawnHazards()
    {
        int x;
        if (Random.Range(0, 101) <= 2)//生成物品
            x = Random.Range(4, 15);
        else if (Random.Range(0, 101) <= 10 || GameObject.FindGameObjectsWithTag("Enemy").Length <= 1 )//生成敌人
        {
            x = 3;
            if (GameObject.FindGameObjectsWithTag("Enemy").Length >=3)//敌人生成超过3个就退出
            {
                return;
            }
        }
        else if (Random.Range(0, 101) <= 4)//生成物品
            x = Random.Range(5, 15);
        else if (Random.Range(0, 101) <= 100)//陨石
            x = Random.Range(0, 3);
        else
            return;
        //确定生成
        GameObject hazard = hazards[x];
        Vector3 spawn_point = new Vector3(Random.Range(-spawnPosition.x, spawnPosition.x), spawnPosition.y, spawnPosition.z);
        Instantiate(hazard, spawn_point, Quaternion.identity);
    }
}

