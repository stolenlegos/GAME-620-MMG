using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {
  private Collider2D col;
  private SpriteRenderer sprite;
  private bool open;

  void Start() {
    col = GetComponent<Collider2D>();
    sprite = GetComponent<SpriteRenderer>();
    open = false;
    PlayerActions.PushButton += ChangeBool;
  }


  void Update() {
    if (open) {
      col.enabled = false;
      sprite.enabled = false;
    } else {
      col.enabled = true;
      sprite.enabled = true;
    }
  }


  private void ChangeBool(GameObject obj) {
    if (obj == this.gameObject){
      open = !open;
    }
  }
}
