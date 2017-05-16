using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    private UISlider targetSlider;
    private UILabel targetLabel;
    private UILabel playerLabel;
    private UILabel logLabel;

    void Start()
    {
        if (GameObject.Find("Player") == null)
            return;
        playerState(GameObject.Find("Player").GetComponent<DataManager>());
        logLabel = GameObject.Find("GameLog").GetComponent<UILabel>();
        targetLabel = GameObject.Find("TargetState").GetComponent<UILabel>();
        targetSlider = GameObject.Find("TargetLife").GetComponent<UISlider>();
        if (gameObject.tag == "Player")
        {
            targetLabel.text = "";
            targetSlider.value = 1f;
        }
    }

    string WeaponType(GameObject player)
    {
        BulletManager bulletManager = player.GetComponent<BulletManager>();
        if (bulletManager.bulletType == BulletManager.BulletType.DEFAULT)
            return "豆豆弹";
        else if (bulletManager.bulletType == BulletManager.BulletType.LEVEL1)
            return "小散弹";
        else if (bulletManager.bulletType == BulletManager.BulletType.LEVEL2)
            return "大散弹";
        else if (bulletManager.bulletType == BulletManager.BulletType.LEVEL3)
            return "跟踪弹";
        else if (bulletManager.bulletType == BulletManager.BulletType.LEVEL4)
            return "五点哦";
        else if (bulletManager.bulletType == BulletManager.BulletType.LEVEL5)
            return "保护弹";
        else if (bulletManager.bulletType == BulletManager.BulletType.ROTATE)
            return "跳舞弹";
        else
            return "错误了呃";
    }

    void playerState(DataManager player_DataManager)
    {
        playerLabel = GameObject.Find("PlayerState").GetComponent<UILabel>();
        //生命 33   攻击 22   速度 15   武器 散弹
        playerLabel.text = "生命值 " + player_DataManager.hp + "/" + player_DataManager.maxHP + "  攻击 " + player_DataManager.attack + "   射速 "+ GameController.bulletSpeed + "   武器 " + WeaponType(player_DataManager.gameObject);
    }

    void TargetHP(DataManager target_DataManager)
    {
        targetSlider.value = target_DataManager.hp * 1.0f / target_DataManager.maxHP;
        targetLabel.text = target_DataManager.name + "   生命值 " + target_DataManager.hp + "/" + target_DataManager.maxHP + "   攻击力" + target_DataManager.attack;
    }

    void OnTriggerEnter(Collider other )
    {
        string other_tag = other.gameObject.tag;
        //同类或者触碰到约束边界就直接退出
        if (other_tag == "Boundary" || other_tag == gameObject.tag)
        {
            return;
        }

        DataManager now_DataManager;//当前游戏物体数据
        now_DataManager = gameObject.GetComponent<DataManager>();
        DataManager other_DataManager;//其他游戏物体数据
        other_DataManager = other.gameObject.GetComponent<DataManager>();

        

        

        if (gameObject.tag == "Player")//玩家碰到的目标
        {
            BulletManager player_BulletManager = gameObject.GetComponent<BulletManager>();
            DataManager player_DataManager = gameObject.GetComponent<DataManager>();

            if (other_tag == "Bolt")//自己的子弹
            {
                return;
            }
            else if (other_tag == "Asteroid")//小行星
            {
                other_DataManager.GetDamage(9999);
                now_DataManager.GetDamage(other_DataManager.attack);
                TargetHP(other_DataManager);
            }
            else if (other_tag == "Enemy")//碰到敌人
            {
                other_DataManager.GetDamage(9999);
                now_DataManager.GetDamage(100);
            }
            else if (other_tag == "EnemyBall")//敌人子弹
            {
                other_DataManager.GetDamage(now_DataManager.attack);
                now_DataManager.GetDamage(other_DataManager.attack);
            }
            else if(true)//触碰到物品
            {
                GameController.AddBulletSpeed();
                player_DataManager.attack += 1;
                logLabel.text = "射速加1 攻击加1    ";
                if (other_tag == "nn")
                {
                    player_BulletManager.bulletType = BulletManager.BulletType.ROTATE;
                    Destroy(other.gameObject);
                    logLabel.text += "获得跳舞弹";
                }
                else if (other_tag == "dd")
                {
                    player_BulletManager.bulletType = BulletManager.BulletType.LEVEL1;
                    Destroy(other.gameObject);
                    logLabel.text += "获得小散弹";
                }
                else if (other_tag == "dls")
                {
                    player_BulletManager.bulletType = BulletManager.BulletType.LEVEL2;
                    Destroy(other.gameObject);
                    logLabel.text += "获得大散弹";
                }
                else if (other_tag == "ds")
                {
                    player_BulletManager.bulletType = BulletManager.BulletType.LEVEL3;
                    Destroy(other.gameObject);
                    logLabel.text += "获得跟踪弹";
                }
                else if (other_tag == "yy")
                {
                    player_BulletManager.bulletType = BulletManager.BulletType.LEVEL4;
                    Destroy(other.gameObject);
                    logLabel.text += "获得五点弹";
                }
                else if (other_tag == "yh")
                {
                    player_BulletManager.bulletType = BulletManager.BulletType.LEVEL5;
                    Destroy(other.gameObject);
                    logLabel.text += "获得保护弹";
                }
                else if (other_tag == "tx")
                {
                    player_DataManager.attack += 1;
                    Destroy(other.gameObject);
                    logLabel.text += "攻击力加1";
                }
                else if (other_tag == "xg")
                {
                    player_DataManager.Cure(100);
                    Destroy(other.gameObject);
                    logLabel.text += "恢复100点生命值";
                }
                else if (other_tag == "zz")
                {
                    player_DataManager.attack += 2;
                    Destroy(other.gameObject);
                    logLabel.text += "攻击力加2";
                }
                else if (other_tag == "jj")
                {
                    player_DataManager.maxHP += 100;
                    player_DataManager.Cure(20);
                    Destroy(other.gameObject);
                    logLabel.text = "生命上限加100 恢复生命20";
                }
                else if (other_tag == "dog")
                {
                    player_BulletManager.bulletType = BulletManager.BulletType.DEFAULT;
                    Destroy(other.gameObject);
                    logLabel.text = "获得豆豆弹";
                }  
            }
            if(player_DataManager != null)
                playerState(player_DataManager);
        }
        else if (gameObject.tag == "Bolt")//己方子弹
        {
            if (other_tag == "Asteroid")//攻击小行星
            {
                other_DataManager.GetDamage(now_DataManager.attack);
                now_DataManager.GetDamage(other_DataManager.attack);
                TargetHP(other_DataManager);
            }
            else if (other_tag == "dog")//狗吸收子弹
            {
                now_DataManager.GetDamage(9999);
            }
            else if (other_tag == "Enemy")//敌人
            {
                GameObject.Find("Game Controller").GetComponent<GameController>().HurtBoss(now_DataManager.attack);
                other_DataManager.GetDamage(now_DataManager.attack);
                now_DataManager.GetDamage(other_DataManager.attack);
                TargetHP(other_DataManager);
            }
        }
    }
}
