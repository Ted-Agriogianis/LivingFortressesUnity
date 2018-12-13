using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour {
    public Rigidbody2D rb;
    public float speed, lifeTime, damage;
    public bool isLaser;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        lifeTime = 5;
	}
	
	// Update is called once per frame
	void Update () {
        if (isLaser == false)
        {
            rb.MovePosition(transform.position + transform.up * -1 * speed * Time.deltaTime);
            lifeTime -= Time.deltaTime;
            if (lifeTime <= 0)
            {
                Destroy(gameObject);
            }
        }
	}
    void OnTriggerEnter2D(Collider2D other){
        if (isLaser == false)
        {
            Destroy(gameObject);
        }
    }
}
