using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{

    public int hp;//当前生命值
    public int maxHP;//最大生命值
    public int attack;//物品攻击力
    public int score;//物品分数价值
    public string name;//物品分数价值

    public GameObject explosion;//爆炸特效变量

    private UISlider playerSlider;
    private UILabel logLabel;

    void Start ()
    {   //开场满血
        hp = maxHP;
        logLabel = GameObject.Find("GameLog").GetComponent<UILabel>();
    }
	
	void Update () {
        if (GameObject.Find("Player") == null)
        {
            return;
        }

        //更新子弹攻击力
        if (gameObject.tag == "Bolt") 
            attack = GameObject.Find("Player").GetComponent<DataManager>().attack;

        DataManager player = GameObject.Find("Player").GetComponent<DataManager>();
        {
            playerSlider = GameObject.Find("PlayerLife").GetComponent<UISlider>();
            playerSlider.value = player.hp * 1f / player.maxHP;
        }
    }

    //生命值处理
    private void CheckHP()
    {
        if (hp <= 0)
        {
            hp = 0;
            //子弹死亡无特效
            if (gameObject.tag != "Bolt" && gameObject.tag != "EnemyBall")
            {
                //logLabel.text = "消灭了" + name;
                //消灭物品得分
                GameObject.Find("Game Controller").GetComponent<GameController>().AddScore(score);
                //分数大于500升级
                
                if (GameObject.Find("Game Controller").GetComponent<GameController>().totalScore >= 300)
                {
                    if (GameObject.Find("Player") != null && GameObject.Find("Player").GetComponent<DataManager>().attack == 35)
                    {
                        logLabel.text = "获得200分，攻击提升为11";
                        GameObject.Find("Player").GetComponent<DataManager>().attack += 5;
                    }
                }
                //死亡爆炸
                Instantiate(explosion, transform.position, transform.rotation);
            }
            //死亡消失
            if(gameObject.tag == "Player")
            {
                GameObject.Find("GameEnd").GetComponent<UILabel>().text = "Game Over";
                GameController.GameOver = true;
            }
            Destroy(gameObject); 
        }
        else if (hp > maxHP)//生命值溢出
        {
            hp = maxHP;
            Debug.Log("超出最大生命上限");
        }
    }
    //收到伤害
    public void GetDamage(int damage)
    {
        hp = hp - damage;
        CheckHP();
        if (gameObject.tag == "Player")
            logLabel.text = "受到了"+ damage + "点伤害";
    }
    //获得治疗
    public void Cure(int curePoint)
    {
        hp = hp + curePoint;
        CheckHP();
    }
}
