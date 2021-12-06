using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllowButtonPush : MonoBehaviour {
  private bool colored;
  private GameObject player;
  private bool playerNear;
  [SerializeField] private GameObject door;
  [SerializeField] private float timer;


  void Start() {
    player = GameObject.FindGameObjectWithTag("Player");
    colored = false;
    playerNear = false;
    ShaderEvents.SaturationChange += BoolChange;
  }


  void Update() {
    if (colored && Input.GetKeyDown(KeyCode.E) && playerNear) {
      PlayerActions.ButtonPushed(door);
      StartCoroutine("ButtonTimer");
      Debug.Log("PUSHED THE BUTTON");
    }
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
    EnergyEvents.ChangeColor(this.gameObject);
  }

}
