using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 10f;
    public HealthManager healthManager;
    private Rigidbody2D rb;
    private void Start()
    {
        healthManager = GameObject.Find("HealthManager").GetComponent<HealthManager>();
        // Get the rigidbody2D component of the bullet
        rb = GetComponent<Rigidbody2D>();
        // Set the velocity of the rigidbody2D component to move the bullet in a straight line
        //GetComponent<Rigidbody2D>().velocity = transform.right * speed;
        //rb.AddForce(transform.right * speed);
    }
    private void Update()
    {
        // Move the bullet in a straight line
        rb.velocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            // Apply damage to the object with the specific tag
            // ...
            healthManager.PlayerHealth--;
            // Destroy the bullet
            Destroy(gameObject);
        }    
        }

  
}
