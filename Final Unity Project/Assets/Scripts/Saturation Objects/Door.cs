using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {
  private Collider2D col;
  private SpriteRenderer sprite;
  private bool open;
  [SerializeField] private float setTimer;
  private float timer;

  void Start() {
    col = GetComponent<Collider2D>();
    sprite = GetComponent<SpriteRenderer>();
    open = false;
    timer = setTimer;
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

    if (timer <= 0 && open) {
      ResetTimer();
      open = false;
    }

    timer -= Time.deltaTime;
  }


  private void ChangeBool(GameObject obj) {
    if (obj == this.gameObject){
      open = !open;
      ResetTimer();
    }
  }


  private void ResetTimer() {
    timer = setTimer;
  }
}
