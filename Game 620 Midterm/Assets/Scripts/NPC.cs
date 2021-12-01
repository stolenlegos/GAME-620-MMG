using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour {
  protected bool playerNear;

  protected void Update() {
    if (playerNear && Input.GetKeyDown(KeyCode.F)) {
      Interact();
    }
  }

  public virtual void Interact() {

  }


  protected void OnTriggerEnter2D (Collider2D other) {
    if (other.tag == "Player") {
      playerNear = true;
    }
  }


  protected void OnTriggerExit2D (Collider2D other) {
    if (other.tag == "Player") {
      playerNear = false;
    }
  }
}


public enum NpcID {
  generic,
  Tom,
  Gregg,
  Satan
}
