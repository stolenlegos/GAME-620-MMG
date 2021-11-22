using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllowMovement : MonoBehaviour {
  private bool colored;
  [SerializeField]
  private Rigidbody2D rb;

  void Start() {
    colored = false;
    ShaderEvents.SaturationChange += BoolChange;
  }


  void Update() {
    if (!colored) {
      rb.constraints = RigidbodyConstraints2D.FreezeAll;
    } else if (colored) {
      rb.constraints = RigidbodyConstraints2D.None;
    }
  }


  private void BoolChange (GameObject obj) {
    if (obj == this.gameObject) {
      colored = !colored;
    }
  }
}
