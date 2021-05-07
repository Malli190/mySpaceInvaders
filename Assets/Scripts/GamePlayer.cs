using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayer : MonoBehaviour
{
    public float speed = 3;
    float speedOffset;
    public GameObject enemyPrefab;
    public GameObject sparkPrefab;
    public GameObject playerBulletPrefab;
    public GameObject playerRocketPrefab;
    public GameObject playerBonusPrefab;
    GameObject[] bonuses;
    GameObject[] enemies;
    GameObject[] enemyBullets;
    GameObject[] playerBullets;
    GameObject[] playerRockets;
    GameObject[] sparks;
    public Rigidbody rb;
    public Vector3 sparkDirection;
    Text healthText;
    Text weaponsText;
    Text[] damageText;

    Color bonusColor;

    int enemyStart;
    int shootCount;
    int maxShootCount;
    int minEnemy, maxEnemy, levelEnemy = 0;
    float shootTime;
    float maxShootTime;
    public int health;
    public int weapons;
    public int bonusType;
    public int[] weaponsCost = new int[] { 10, 40 };
    int maxWeaponPow;
    float powSpeed, weaponPow;

    bool createEnemies;
    bool showEnemyText;
    public bool turnDamage;

    public bool getBonus;
    bool firsWave;
    bool nextWaveE;
    
    void Start()
    {
        weapons = 0;
        weaponPow = 100;
        maxWeaponPow = 100;
        powSpeed = 0.15f;
        rb = this.GetComponent<Rigidbody>();
        healthText = GameObject.Find("player_lives").GetComponent<Text>();
        weaponsText = GameObject.Find("weaponsText").GetComponent<Text>();
        health = 300;
        maxShootCount = 5;
        maxShootTime = 0.1f;
        minEnemy = 1;
        maxEnemy = 2;
        enemyStart = Random.Range(minEnemy, maxEnemy);
        showEnemyText = false;
        StartCoroutine("startWave");
    }
    void Update()
    {
        
        if (weaponPow < 0) weaponPow = 0;
        if (Input.GetKey(KeyCode.Space))
        {
            shootTime += Time.deltaTime;
            if (shootTime >= maxShootTime)
            {
                shootTime = 0;
                if (shootCount < maxShootCount)
                {
                    playerShoot();
                    shootCount++;
                }
                else shootCount = 0;
            }
        }
        if (Input.GetKey(KeyCode.Alpha1)) weapons = 0;
        if (Input.GetKey(KeyCode.Alpha2)) weapons = 1;
        speedOffset = speed * Time.deltaTime;

        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x + Input.GetAxis("Horizontal") * speedOffset, -21, 21), 
            Mathf.Clamp(transform.position.y + Input.GetAxis("Vertical") * speedOffset, -10, 9), 0);
        checkBullets();
        checkPlayerBullets();
        checkPlayerRockets();
        checkBonuses();
        checkEnemies();
        CheckSparks();
        if (nextWaveE) // следующая волна 
        {
            nextWaveE = false;
            StartCoroutine("nextWave");
        }
        healthText.text = "Жизни: " + health + " противники: " + enemyStart + " уровень: " + levelEnemy;
        weaponsText.text = "Оружие: " + weapons + " энергия: " + Mathf.Round(weaponPow) + "/" + maxWeaponPow;

        if (getBonus)
        {
            switch (bonusType)
            {
                case 0: health += Random.Range(5, 50);
                    break;
                case 1: weaponPow += Random.Range(5, 50);
                    break;
                default:
                    break;
            }
            getBonus = false;
        }
        if (turnDamage)//если попал
        {
            health -= 3;
            transform.position = new Vector3(transform.position.x - 0.5f, transform.position.y, transform.position.z);
            CreateSparks(Random.Range(3, 10));
            turnDamage = false;
        }
        if (weaponPow < maxWeaponPow)
            weaponPow += powSpeed;
        
    }
    IEnumerator startWave()
    {
        yield return new WaitForSeconds(2f);
        createWave(enemyStart);
        if (!firsWave) firsWave = true;
    }
    IEnumerator nextWave()
    {
        yield return new WaitForSeconds(3f);
        createWave(enemyStart);
    }
    void createWave(int enemyCount) {
        int randY = -(enemyCount - 1);
        if (enemyStart >= 8 && levelEnemy < enemyPrefab.GetComponent<GameEnemy>().health.Length ||
            enemyStart == 2 || enemyStart == 5) levelEnemy++;
        for (int i = 0; i < enemyCount; i++)
        {
            TextMesh tt = enemyPrefab.transform.Find("textObj").GetComponent<TextMesh>();
            tt.text = "enemy " + i;
            tt.gameObject.active = showEnemyText;
            enemyPrefab.GetComponent<GameEnemy>().h = enemyPrefab.GetComponent<GameEnemy>().health[levelEnemy];
            Instantiate(enemyPrefab, new Vector3(22, randY, 0), Quaternion.identity);
            randY = randY + 2;
        }
    }
    void CreateSparks(int sparksCount)
    {
        for (int i = 0; i < sparksCount; i++)
        {
            var sp = sparkPrefab;
            sp.GetComponent<gameSpark>().direction = new Vector3(sparkDirection.x + 10, Random.Range(sparkDirection.y - 15, sparkDirection.y + 15), 0);
            Instantiate(sp, transform.position, Quaternion.identity);
        }
    }
    void playerShoot()
    {
        if (weapons == 0 && weaponPow > weaponsCost[weapons])
        {
            weaponPow -= weaponsCost[weapons];
            Instantiate(playerBulletPrefab, transform.position, Quaternion.AngleAxis(90, new Vector3(0, 0, 1)));
        }
        else if (weapons == 1 && weaponPow > weaponsCost[weapons])
        {
            weaponPow -= weaponsCost[weapons];
            Instantiate(playerRocketPrefab, transform.position, Quaternion.AngleAxis(90, new Vector3(0, 0, 1)));
        }
    }
    void checkBullets()
    {
        enemyBullets = GameObject.FindGameObjectsWithTag("enemyBullet");

        foreach (var bullet in enemyBullets)
        {
            if (!bullet.GetComponent<bulletScript>().alive) Destroy(bullet);
        }
    }
    void checkPlayerBullets()
    {
        playerBullets = GameObject.FindGameObjectsWithTag("playerBullet");

        foreach (var bullet in playerBullets)
        {
            if (!bullet.GetComponent<playerBullet>().alive) Destroy(bullet);
        }
    }
    void checkPlayerRockets()
    {
        playerRockets = GameObject.FindGameObjectsWithTag("playerRocket");
        foreach (var rocket in playerRockets)
        {
            if (!rocket.GetComponent<gameRoket>().alive) Destroy(rocket);
        }
    }
    void checkBonuses()
    {
        bonuses = GameObject.FindGameObjectsWithTag("bonus");
        foreach (var bonus in bonuses)
        {
            if (!bonus.GetComponent<gameBonus>().alive) Destroy(bonus);
        }
    }
    void checkEnemies() //проверка инвайлеров
    {
        enemies = GameObject.FindGameObjectsWithTag("enemy");
        foreach (var enemy in enemies)
        {
            if (!enemy.GetComponent<GameEnemy>().alive)
            {
                for (int i = 0; i < 30; i++)
                {
                    sparkPrefab.GetComponent<gameSpark>().direction = new Vector3(Random.Range(sparkDirection.x - 15, sparkDirection.x + 15), Random.Range(sparkDirection.y - 15, sparkDirection.y + 15), 0);
                    Instantiate(sparkPrefab, enemy.transform.position, Quaternion.identity);
                }
                if (enemies.Length <= 1 && firsWave)
                {
                    if (minEnemy < 8) minEnemy++;
                    if (maxEnemy < 9) maxEnemy++;
                    enemyStart = Random.Range(minEnemy, maxEnemy);
                    nextWaveE = true;
                }
                int b = Random.Range(1, 2);
                bonusType = b;
                if (b == 1)
                {
                    bonusColor = new Color(0, 255, 0);
                    Color hdrColor = new Color(11, 118, 0);
                    //playerBonusPrefab.GetComponent<Material>().SetColor(0, bonusColor);
                    Instantiate(playerBonusPrefab, enemy.transform.position, Quaternion.identity);
                }
                Destroy(enemy);
                
            }
        }
        
    }
    void CheckSparks()
    {
        sparks = GameObject.FindGameObjectsWithTag("sparks");
        foreach (var spark in sparks)
            if (!spark.GetComponent<gameSpark>().alive) Destroy(spark);
    }
}
