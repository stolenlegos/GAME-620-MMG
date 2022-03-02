using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllowButtonPush : MonoBehaviour {
  private bool colored;
  private GameObject player;
  private bool playerNear;
  private bool doorOpen;
  [SerializeField] private GameObject door;
  [SerializeField] private float timer;


  void Start() {
    player = GameObject.FindGameObjectWithTag("Player");
    colored = false;
    doorOpen = false;
    playerNear = false;
    ShaderEvents.SaturationChange += BoolChange;
  }


  void Update() {
    if (colored && Input.GetKeyDown(KeyCode.E) && playerNear) {
      PlayerActions.ButtonPushed(door);
      doorOpen = true;
      StartCoroutine("ButtonTimer");
      //Debug.Log("PUSHED THE BUTTON");
    }
    //Debug.Log("Door open is: " + doorOpen.ToString());
  }


  private void BoolChange (GameObject obj) {
    if (obj == this.gameObject) {
      colored = !colored;
            ShaderEvents.ChangeColor(door);
    }
  }


  private void OnTriggerEnter2D (Collider2D other) {
    if (other.gameObject == player) {
      playerNear = true;
    }
  }


  private void OnTriggerExit2D (Collider2D other) {
    if (other.gameObject == player) {
      playerNear = false;
    }
  }


  IEnumerator ButtonTimer() {
    yield return new WaitForSeconds(timer);

    PlayerActions.ButtonPushed(door);
    doorOpen = false;
    if (colored) {
      EnergyEvents.ChangeColor(this.gameObject);
    }
  }

}
