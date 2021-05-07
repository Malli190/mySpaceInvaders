using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnemy : MonoBehaviour
{
    bool move;
    float speed = 3;
    public float ySpeed = 12;
    float posSteep = 4;
    float rotationSteep = 3f;
    float startYpos;
    float yPos;

    int shootCount;
    int shootMaxCount = 3;
    float firstShootime;
    //float waightTime = 15f;
    float shootTime;
    float shootMaxTime = 0.5f;

    public float[] health = new float[] { 200, 250, 300, 400, 500, 750 };
    public float h;

    bool upOrDown;
    bool firstmove;
    bool shoot;
    bool clampMode;
    public bool alive;
    public bool damage;
    public bool rocketDamage;
    bool damageMove;
    public Vector3 damagePos;
    public Vector3 sparkDirection;

    GameObject playerPosition;
    public GameObject sparkPrefab;
    public GameObject bulletPrefab;
    public GameObject damageTextPrefab; 
    TextMesh enemyText;
 
    void Start()
    {
        alive = true;
        move = false;
        firstmove = true;
        firstShootime = Random.Range(10, 30) / 10;
        playerPosition = GameObject.Find("Player");
        enemyText = transform.Find("textObj").GetComponent<TextMesh>();
    }
    void Update()
    {
        Vector3 direction = transform.position - playerPosition.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float bulletAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90f;
        Quaternion rotation = Quaternion.AngleAxis(angle, new Vector3(0, 0, 1));

        if (damage)
        {
            float sc = 0.02f;
            if (rocketDamage) { sc = 0.033f; h -= playerPosition.gameObject.GetComponent<GamePlayer>().weaponsCost[playerPosition.gameObject.GetComponent<GamePlayer>().weapons] * 1.3f; rocketDamage = false; }
            h -= playerPosition.gameObject.GetComponent<GamePlayer>().weaponsCost[playerPosition.gameObject.GetComponent<GamePlayer>().weapons] * 1.3f;
            transform.localScale = new Vector3(transform.localScale.x - sc, transform.localScale.y, transform.localScale.z);
            if (h <= 0) alive = false;
            CreateSparks(Random.Range(5, 10));
            damageMove = true;
            damage = false;
        }
        if (damageMove)
        {
            if (transform.position.x > damagePos.x) transform.position = new Vector3(transform.position.x - 1.2f * Time.deltaTime, transform.position.y, transform.position.z);
            else
            {
                transform.position = damagePos;
                damageMove = false;
            }
        }
        if (transform.position.x <= startYpos && !firstmove)
        {
            move = false;
            startYpos -= posSteep;
            upOrDown = UporDown();
            StartCoroutine("waitForNewMove");
        }
        if (shoot)
        {
            shootTime += Time.deltaTime;
            enemyText.text = shootTime + " c: " + shootCount;
            if (shootTime >= shootMaxTime)
            {
                shootTime = 0;
                var bul = bulletPrefab.gameObject;
                bul.GetComponent<bulletScript>().direction = playerPosition.transform.position;
                Instantiate(bul, transform.position, Quaternion.AngleAxis(bulletAngle, new Vector3(0, 0, 1)));
                if (shootCount >= shootMaxCount)
                {
                    shoot = false;
                    shootCount = 0;
                    StartCoroutine("waitToShoot");
                }
                shootCount++;
            }
        }
        if (firstmove)
        {
            transform.position = new Vector3(transform.position.x - (2 * ySpeed * Time.deltaTime), transform.position.y, 0);
            
            if (ySpeed <= 0)
            {
                startYpos = transform.position.x - posSteep;
                firstmove = false;
                clampMode = true;
                //shoot = true;
                StartCoroutine("firstSoot");
                StartCoroutine("waitForNewMove");
            }
            ySpeed -= 0.22f;
        }
        if (move && !firstmove)
            transform.position = new Vector3(transform.position.x - (speed * Time.deltaTime), transform.position.y, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSteep * Time.deltaTime);
        if (clampMode)
        {
            transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, -21, 20),
            Mathf.Clamp(transform.position.y, -10, 9), 0);
        }
    }
    IEnumerator waitForNewMove()
    {
        yield return new WaitForSeconds(10f);
        move = true;
    }
    IEnumerator waitToShoot()
    {
        yield return new WaitForSeconds(nextShootTime());
        shoot = true;
    }
    IEnumerator firstSoot()
    {
        yield return new WaitForSeconds(firstShootime);
        shoot = true;
    }
    void CreateSparks(int sparksCount)
    {
        for (int i = 0; i < sparksCount; i++)
        {
            var sp = sparkPrefab;
            sp.GetComponent<gameSpark>().direction = new Vector3(sparkDirection.x - 10, Random.Range(sparkDirection.y - 15, sparkDirection.y + 15), 0);
            Instantiate(sp, transform.position, Quaternion.identity);
        }
    }
    float nextShootTime()
    {
        return Random.Range(10, 400) / 100;
    }
    bool UporDown()
    {
        int a = Random.Range(0,1);

        if (a == 0) return true;
        else return false;
    }
}
