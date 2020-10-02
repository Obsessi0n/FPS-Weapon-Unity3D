using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    float speed = 220f;


    void Update()
    {
        Move();
    }


    void Move()
    {
        this.GetComponent<Rigidbody>().velocity = transform.forward*speed;
    }
}
