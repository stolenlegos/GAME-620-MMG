using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour {
  public bool spawnerBlocked;


  private void OnTriggerEnter2D (Collider2D other) {
    spawnerBlocked = true;
  }


  private void OnTriggerExit2D (Collider2D other) {
    spawnerBlocked = false;
  }
}
