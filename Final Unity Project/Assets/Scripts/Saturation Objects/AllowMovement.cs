using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllowMovement : MonoBehaviour {
  private bool colored;
  private GameObject player;
  public bool grabbable;
  private Vector3 offset;
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
        if (grabbable)
        {
            this.transform.position = player.transform.position - offset;
        }
    }


  private void BoolChange (GameObject obj) {
    if (obj == this.gameObject) {
      colored = !colored;
    }
  }


  private void GrabObject (GameObject obj) {
    if (obj == this.gameObject && colored) {
            grabbable = true;
            rb.mass = 20;
            this.transform.parent = player.transform;
            offset = player.transform.position - this.transform.position;
            rb.constraints = RigidbodyConstraints2D.None;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            this.transform.position = player.transform.position - offset;
            this.transform.parent = player.transform;
        }
  }


  private void ReleaseObject (GameObject obj) {
    if (obj == this.gameObject) {
            grabbable = false;
            rb.mass = 1000;
            this.transform.parent = null;
            //rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }
  }
    private void DroppedObject(GameObject obj)
    {
        if (obj == this.gameObject)
        {
            StartCoroutine("FreezeObject");
        }
    }

    IEnumerator FreezeObject()
    {
        //Debug.Log("Coroutine Started");
        yield return new WaitForSeconds(1.0f);

        while (true)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position - Vector3.up * 1f, -Vector2.up, 0.05f);
            Debug.DrawRay(transform.position - Vector3.up * 1, -Vector2.up - new Vector2(0, 1f), Color.green, 45.0f);
            if (hit.collider != null)
            {
                if (hit.transform.tag == "Terrain")// || hit.transform.tag == "box_Big" || hit.transform.tag == "box_Small")
                {
                    this.transform.parent = null;
                    rb.constraints = RigidbodyConstraints2D.FreezeAll;
                    break;
                }
                if (hit.transform.tag == "box_Big" || hit.transform.tag == "box_Small")
                {
                    this.transform.parent = null;
                    rb.constraints = RigidbodyConstraints2D.FreezeAll;
                    break;
                }
            }

            yield return new WaitForSeconds(0.45f);
        }
    }
}
