using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameSpark : MonoBehaviour
{
    public Vector3 direction;
    Rigidbody rb;
    float aliveTime;
    float maxAliveTime;
    float scale;
    float speed;
    float minScale;
    public bool alive;
    void Start()
    {
        speed = Random.Range(80, 150);
        scale = Random.RandomRange(0.1f, 0.13f);
        minScale = Random.RandomRange(0.05f, 0.11f);
        alive = true;
        maxAliveTime = Random.Range(30, 500) / 100;
        transform.localScale = new Vector3(scale, scale, scale);
        rb = this.GetComponent<Rigidbody>();
    }
    void Update()
    {
        speed -= 5.5f;
        if (speed < 0 && speed <= -1.5f) speed = -1.5f;
        if (scale > minScale) scale -= 0.005f;
        else scale = minScale;
        transform.localScale = new Vector3(scale, scale, scale);
        rb.AddForce(direction * speed * Time.deltaTime, ForceMode.Force);
        aliveTime += Time.deltaTime;
        if (aliveTime >= maxAliveTime) alive = false;
    }
}
