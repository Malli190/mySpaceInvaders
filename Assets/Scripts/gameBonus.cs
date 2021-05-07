using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameBonus : MonoBehaviour
{
    float speed = 3f; 
    float liveTime;
    float liveMaxTime = 13f;


    public bool alive = true;

    Vector3 tr;
    void Start()
    {
        tr = new Vector3(-1, 0, 0);
        tr.Normalize();
        transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
    }

    // Update is called once per frame
    void Update()
    {
        liveTime += Time.deltaTime;
        if (liveTime >= liveMaxTime) alive = false;
        transform.Translate(tr * speed * Time.deltaTime);
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            //Vector3 colPos = col.gameObject.transform.position;
            //col.gameObject.GetComponent<GameEnemy>().damagePos = colPos;
            //col.gameObject.GetComponent<GameEnemy>().sparkDirection = -transform.position;
            //col.gameObject.GetComponent<GameEnemy>().transform.position = new Vector3(colPos.x + 0.5f, colPos.y, colPos.z);
            col.gameObject.GetComponent<GamePlayer>().getBonus = true;
            this.alive = false;
        }
    }
}
