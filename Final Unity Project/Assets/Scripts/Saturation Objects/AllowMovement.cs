using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllowMovement : MonoBehaviour {
  private bool colored;
  private GameObject player;
  private bool grabbable;
  [SerializeField]
  private Rigidbody2D rb;

  void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        colored = false;
        ShaderEvents.SaturationChange += BoolChange;
        PlayerActions.Grab += GrabObject;
        PlayerActions.Release += ReleaseObject;
        PlayerActions.Drop += DroppedObject;
    }


  void Update() {
        if (!colored)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
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
    private void DroppedObject(GameObject obj)
    {
        StartCoroutine("FreezeObject");
    }

    IEnumerator FreezeObject()
    {
        yield return new WaitForSeconds(2.0f);

        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }
}
