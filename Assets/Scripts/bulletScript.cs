using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletScript : MonoBehaviour
{
    float speed = 120;
    float minSpeed = 30;
    float liveTime;
    float liveMaxTime = 3f;
    public bool alive;

    public Vector3 direction;
    GameObject player;
    Vector3 tr;

    Rigidbody rb;
    void Start()
    {
        alive = true;
        liveTime = 0;
        rb = transform.gameObject.GetComponent<Rigidbody>();
        tr = direction - transform.position;
        tr.Normalize();
        player = GameObject.Find("Player");
    }
    void Update()
    {
        speed -= 1.7f;
        if (speed <= minSpeed) speed = minSpeed;

        liveTime += Time.deltaTime;
        if (liveTime >= liveMaxTime) alive = false;
    }
    void FixedUpdate()
    {
        rb.AddForce(tr * speed * Time.fixedDeltaTime, ForceMode.Impulse);
    }
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            player.GetComponent<GamePlayer>().sparkDirection = -transform.position;
            player.GetComponent<GamePlayer>().turnDamage = true;
            this.alive = false;
        }
    }
}
