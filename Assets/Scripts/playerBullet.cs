using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerBullet : MonoBehaviour
{
    float speed = 120;
    float minSpeed = 30;
    float liveTime;
    float liveMaxTime = 3f;
    public float cost = 10;
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
            col.gameObject.GetComponent<GameEnemy>().damage = true;
            col.gameObject.GetComponent<GameEnemy>().transform.position = new Vector3(colPos.x + 0.5f, colPos.y, colPos.z);
            this.alive = false;
        }
    }
}
