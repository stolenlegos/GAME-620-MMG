using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPatrol : MonoBehaviour
{
    // Start is called before the first frame update
    public float walkSpeed;
    public Rigidbody2D rb;
    private float timer; 
    public float maxTime; 

    // Update is called once per frame
    void Start() {
        timer = maxTime;
    }

    void Update()
    { 
        Patrol(); 
        if (timer <= 0) {
            Flip(); 
            timer = Random.Range(1,maxTime);
        } else {
            timer -= Time.deltaTime;
        }
    }

    private void OnCollissionEnter2D(Collider2D other) { 
        Debug.Log("This code is reached");
        Flip();
    }

    void Patrol() {
        rb.velocity = new Vector2(walkSpeed * Time.fixedDeltaTime, rb.velocity.y);  
    }

    private void Flip() {
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y); 
        walkSpeed *= -1;
    }
}
