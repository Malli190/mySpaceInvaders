using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameRoket : MonoBehaviour
{
    float speed = 200;
    float minSpeed = 40;
    float liveTime;
    float liveMaxTime = 3f;
    public float cost = 40;
    public bool alive;

    Vector3 tr;

    Rigidbody rb;
    void Start()
    {
        alive = true;
        liveTime = 0;
        rb = transform.gameObject.GetComponent<Rigidbody>();
        tr = new Vector3(1, 0, 0);
        tr.Normalize();
        //player = GameObject.Find("Player");
    }
    void Update()
    {
        liveTime += Time.deltaTime;
        if (liveTime >= liveMaxTime) alive = false;

        rb.AddForce(tr * speed * Time.fixedDeltaTime, ForceMode.Impulse);
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "enemy")
        {
            Vector3 colPos = col.gameObject.transform.position;
            col.gameObject.GetComponent<GameEnemy>().damagePos = colPos;
            col.gameObject.GetComponent<GameEnemy>().sparkDirection = -transform.position;
            col.gameObject.GetComponent<GameEnemy>().transform.position = new Vector3(colPos.x + 1.5f, colPos.y, colPos.z);
            col.gameObject.GetComponent<GameEnemy>().rocketDamage = true;
            col.gameObject.GetComponent<GameEnemy>().damage = true;
            this.alive = false;
        }
    }
}
