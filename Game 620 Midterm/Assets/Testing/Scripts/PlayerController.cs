using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
  public float moveSpeed = 5f;
  public Rigidbody2D rb;
  Vector2 movement;
  public GameObject sword;

  public Camera mainCam;


    void Update() {
      movement.x = Input.GetAxisRaw("Horizontal");
      movement.y = Input.GetAxisRaw("Vertical");
    }


    void FixedUpdate() {
      rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    void OnTriggerEnter2D(Collider2D other){
      if(other.tag == "AreaOne") {
        mainCam.transform.position = new Vector3 (0f, 0f, -10f);
      } else if(other.tag == "AreaTwo") {
        mainCam.transform.position = new Vector3(0f, -11.543f, -10f);
      } else if (other.tag == "AreaThree") {
        mainCam.transform.position = new Vector3(-20.52f, -11.543f, -10f);
      } else if(other.tag == "AreaFour") {
        mainCam.transform.position = new Vector3(-20.52f, 0f, -10f);
      }
    }
}
