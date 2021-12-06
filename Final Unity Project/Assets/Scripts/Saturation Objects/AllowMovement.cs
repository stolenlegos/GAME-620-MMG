using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllowMovement : MonoBehaviour {
  private bool colored;
  private GameObject player;
  [SerializeField]
  private Rigidbody2D rb;

  void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        colored = false;
        ShaderEvents.SaturationChange += BoolChange;
        PlayerActions.Grab += GrabObject;
        PlayerActions.Release += ReleaseObject;
    }


  void Update() {
        if (!colored)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
      //  else if (colored && Input.GetMouseButtonDown(0))
      //  {
          //  rb.constraints = RigidbodyConstraints2D.None;
          //  rb.constraints = RigidbodyConstraints2D.FreezeRotation;
          //  this.transform.parent = player.transform;
      //  }
      //  else if (colored && Input.GetMouseButtonUp(0))
      //  {
          //  this.transform.parent = null;
          //  rb.constraints = RigidbodyConstraints2D.FreezeAll;
      //  }
  }


  private void BoolChange (GameObject obj) {
    if (obj == this.gameObject) {
      colored = !colored;
    }
  }


  private void GrabObject (GameObject obj) {
    if (obj == this.gameObject && colored) {
      rb.constraints = RigidbodyConstraints2D.None;
      rb.constraints = RigidbodyConstraints2D.FreezeRotation;
      this.transform.parent = player.transform;
    }
  }


  private void ReleaseObject (GameObject obj) {
    if (obj == this.gameObject && colored) {
      this.transform.parent = null;
      rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }
  }
}
