using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour {
  // this is a generic class, not meant to be instantiated. Please derive
  //specific NPCs from this class.
  protected bool playerNear;

  protected void Update() {
    if (playerNear && Input.GetKeyDown(KeyCode.F)) {
      Interact();
    }
  }


//generic interact if the same thing needs to happen each time the player talks
//to an NPC. Not sure if we need to use it but is set up if we do.
  public virtual void Interact() {

  }

//the following two functions check if the player is in talking distance using
//a trigger collider on the NPC game object.
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

//list of unique NPCs in the game. assign these in the subclass using a public
//variable and the inspector. Keep GenericNPC at the top of the list.
public enum NpcID {
  GenericNPC,
  MailBox,
  Tom,
  Gregg,
  Satan
}
