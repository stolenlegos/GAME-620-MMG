using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
  private float moveSpeed = 5f;
  private Vector2 movement;

  [SerializeField]
  private Rigidbody2D rb;
  private Animator anim; 

    void Start() {
      anim = GetComponent<Animator>(); 
    }


    void Update() {
      movement.x = Input.GetAxisRaw("Horizontal");
      movement.y = Input.GetAxisRaw("Vertical");
      Animate();
    }


    void FixedUpdate() {
      rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    private void Animate(){
      anim.SetFloat("MovementX", movement.x);
      anim.SetFloat("MovementY", movement.y);
    }
}

