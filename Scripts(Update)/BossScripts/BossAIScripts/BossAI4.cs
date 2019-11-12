using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BossAI4 : MonoBehaviour
{
    /// <summary>
    /// ~Headless Boss
    ///
    /// </summary>
    //VARIABLES														//VARIABLES
    [Header("General Settings")]								    //GENERAL VARIABLES
    public Transform player;                                        //Players transform location
    public GameObject prefab;										//Bullet prefab
    public GameObject boss;                                         //Boss object
    public Transform bossLocation;                                  //Bosses transform location
    public bool bossActive = false;							        //Tells if the boss is active or not
    [Header("Base Stats Settings")]						            //VARIABLES THAT'LL BALANCE THE BOSSES BASE STATS
    public float bulletSpeed = 5f;							        //How fast the bullet will travel
    public float shootDelay = 0.5f;							        //How long the boss will wait to fire again
    public float bulletLifetime = 10.0f;				            //How long the bullet will last before being destroyed
    float shootTimer = 0f;                                          //Timer for shooting  
    [Header("Upgraded Stats Settings")]					            //VARIABLES THAT'LL BALANCE THE BOSSES UPGRADED STATS
    public float bulletSpeedUpgrade = 10f;                          //How fast the bullet will travel after the upgrade
    public float shootDelayUpgrade = 0.3f;			                //How long the boss will wait to fire again after the upgrade
    [Header("Wave Time Settings")] 								    //VARIABLES THAT'll CONTROL WHEN WAVES AND UPGRADES WILL OCCUR
    public int firstWave = 0;										//Time when the first wave will occur
    public int secondWave = 5;									    //Time when the second wave will occur
    public int thirdWave = 15;									    //Time when the third wave will occur
    public int fourthWave = 25;                                     //Time when the fourth wave will occur
    public int reset = 35;                                          //Time when the waves will reset
    float waveTimer = 0f;                                           //Timer for the waves
    [Header("Shoot Pattern Settings")]                              //VARIABLES THAT'LL CONTROL SHOOTING PATTERNS
    public Vector2 startPoint;                                      //Variable for the starting position of the pattern
    public int numberOfBullets = 15;                                //How much bullets the enemy will shoot
    public float radius = 5f;                                       //Radius
    public float angle = 180f;                                      //Angle of bullet direction
    float shootPatternTimer1Var1 = 0f;                              //Timer for shoot pattern one variation one
    float shootPatternTimer1Var2 = 0f;                              //Timer for shoot pattern one variation two
    float shootPatternTimer1Var3 = 0f;                              //Timer for shoot pattern one variation three
    //UPDATE FUNCTION
    void Update()
    {
        if(boss.GetComponent<BossHealth>().bossHealth < boss.GetComponent<BossHealth>().bossHealth / 2)
        {
            shootDelay = 0.4f;
            numberOfBullets = 17;
        }
        else if(boss.GetComponent<BossHealth>().bossHealth < boss.GetComponent<BossHealth>().bossHealth / 4)
        {
            shootDelay = 0.3f;
            numberOfBullets = 20;
        }
        else if(boss.GetComponent<BossHealth>().bossHealth < boss.GetComponent<BossHealth>().bossHealth / 10)
        {
            shootDelay = 0.3f;
            numberOfBullets = 23;
        }
        else
        {
            shootDelay = 0.5f;
            numberOfBullets = 15;
        }
        //WAVES
        if (bossActive == true)
        {
            waveTimer += Time.deltaTime;
            if (waveTimer > firstWave && waveTimer < secondWave)
            {
                Shoot();
                ShootPatternOneVar1(numberOfBullets);
            }
            else if (waveTimer > secondWave && waveTimer < thirdWave)
            {
                Shoot();
                ShootPatternOneVar1(numberOfBullets);
                ShootPatternOneVar2(numberOfBullets);
            }
            else if (waveTimer > thirdWave && waveTimer < fourthWave)
            {
                Shoot();
                ShootPatternOneVar1(numberOfBullets);
                ShootPatternOneVar2(numberOfBullets);
                ShootPatternOneVar3(numberOfBullets);
            }
            else if (waveTimer > fourthWave && waveTimer < reset)
            {
                Shoot();
            }
            else
                waveTimer = 0;
        }
    }
    //SHOOT FUNCTION
    void Shoot()
    {
        shootTimer += Time.deltaTime;
        //Boss Upgrade
        if (boss.GetComponent<BossHealth>().bossHealth < bossPowerUp && shootTimer > shootDelay)
        {
            shootDelay = shootDelayUpgrade;
            if (shootTimer > shootDelayUpgrade)
            {
                shootTimer = 0;
                GameObject bullet = Instantiate(prefab, transform.position, Quaternion.identity);
                Vector3 playerPosition = player.position;
                Vector2 shootDir = new Vector2(playerPosition.x - transform.position.x, playerPosition.y - transform.position.y);
                shootDir.Normalize();
                bullet.GetComponent<Rigidbody2D>().velocity = shootDir * bulletSpeedUpgrade;
                Destroy(bullet, bulletLifetime);
            }
        }
        //Boss Base Stats
        else if (shootTimer > shootDelay)
        {
            shootTimer = 0;
            GameObject bullet = Instantiate(prefab, transform.position, Quaternion.identity);
            Vector3 playerPosition = player.position;
            Vector2 shootDir = new Vector2(playerPosition.x - transform.position.x, playerPosition.y - transform.position.y);
            shootDir.Normalize();
            bullet.GetComponent<Rigidbody2D>().velocity = shootDir * bulletSpeed;
            Destroy(bullet, bulletLifetime);
        }
    }
    //SHOOT PATTERN(ONE)(VARIATION ONE) FUNCTION
    void ShootPatternOneVar1(int numberOfBullets)
    {
        shootPatternTimer1Var1 += Time.deltaTime;
        if (shootPatternTimer1Var1 > 0.5f)
        {
            startPoint = boss.GetComponent<Transform>().position;
            float angleStep = 360f / numberOfBullets;
            angle = 0f;
            for (int i = 0; i <= numberOfBullets - 1; i++)
            {
                shootPatternTimer1Var1 = 0;
                float projectileDirXposition = startPoint.x + Mathf.Sin((angle * Mathf.PI) / 180) * radius;
                float projectileDirYposition = startPoint.y + Mathf.Cos((angle * Mathf.PI) / 180) * radius;
                Vector2 projectileVector = new Vector2(projectileDirXposition, projectileDirYposition);
                Vector2 projectileMoveDirection = (projectileVector - startPoint).normalized * bulletSpeed;
                var bullet = Instantiate(prefab, startPoint, Quaternion.identity);
                bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(projectileMoveDirection.x, projectileMoveDirection.y);
                Destroy(bullet, bulletLifetime);
                angle += angleStep;
            }
        }
    }
    //SHOOT PATTERN(ONE)(VARIATION TWO) FUNCTION
    void ShootPatternOneVar2(int numberOfBullets)
    {
        shootPatternTimer1Var2 += Time.deltaTime;
        if (shootPatternTimer1Var2 > 1.25f)
        {
            startPoint = boss.GetComponent<Transform>().position;
            float angleStep = 360f / numberOfBullets;
            angle = 180;
            for (int i = 0; i <= numberOfBullets - 1; i++)
            {
                shootPatternTimer1Var2 = 0;
                float projectileDirXposition = startPoint.x + Mathf.Sin((angle * Mathf.PI) / 180) * radius;
                float projectileDirYposition = startPoint.y + Mathf.Cos((angle * Mathf.PI) / 180) * radius;
                Vector2 projectileVector = new Vector2(projectileDirXposition, projectileDirYposition);
                Vector2 projectileMoveDirection = (projectileVector - startPoint).normalized * bulletSpeed;
                var bullet = Instantiate(prefab, startPoint, Quaternion.identity);
                bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(projectileMoveDirection.x, projectileMoveDirection.y);
                Destroy(bullet, bulletLifetime);
                angle += angleStep;
            }
        }
    }
    //SHOOT PATTERN(ONE)(VARIATION THREE) FUNCTION
    void ShootPatternOneVar3(int numberOfBullets)
    {
        shootPatternTimer1Var3 += Time.deltaTime;
        if (shootPatternTimer1Var3 > 0.5f)
        {
            float angleStep = 360f / numberOfBullets;
            angle = 90f;
            for (int i = 0; i <= numberOfBullets - 1; i++)
            {
                shootPatternTimer1Var3 = 0;
                float projectileDirXposition = startPoint.x + Mathf.Sin((angle * Mathf.PI) / 180) * radius;
                float projectileDirYposition = startPoint.y + Mathf.Cos((angle * Mathf.PI) / 180) * radius;
                Vector2 projectileVector = new Vector2(projectileDirXposition, projectileDirYposition);
                Vector2 projectileMoveDirection = (projectileVector - startPoint).normalized * bulletSpeed;
                var bullet = Instantiate(prefab, startPoint, Quaternion.identity);
                bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(projectileMoveDirection.x, projectileMoveDirection.y);
                Destroy(bullet, bulletLifetime);
                angle += angleStep;
            }
        }
    }
}
///END OF SCRIPT!
