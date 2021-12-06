using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllowMovement : MonoBehaviour {
  private bool colored;
  private GameObject player;
  public bool grabbable;
  private Transform location;
  [SerializeField]
  private Rigidbody2D rb;

  void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        colored = false;
        ShaderEvents.SaturationChange += BoolChange;
    }


  void Update() {
        Debug.Log("Grabbable: " + grabbable);

        if (!colored)
        {
            //Debug.Log("Drop");
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        if (colored && Input.GetMouseButtonDown(0) && grabbable)
        {
            Debug.Log("Pickup");
            rb.constraints = RigidbodyConstraints2D.None;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            this.transform.parent = player.transform;
        }
        if ((colored && Input.GetMouseButtonUp(0)) || !colored)
        {
            Debug.Log("Drop"); 
            this.transform.parent = null;

            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
}


  private void BoolChange (GameObject obj) {
    if (obj == this.gameObject) {
      colored = !colored;
    }
  }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.tag == "Player")
            {
                grabbable = true;
            }
            else
            {

            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision != null)
        {
            if (collision.gameObject.tag == "Player")
            {
                grabbable = false;
            }
            else
            {

            }
        }
    }
    IEnumerator CheckGrounded()
    {
        yield return new WaitForSeconds(1.0f);

        while (true)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position - Vector3.up * 1f, -Vector2.up, 0.05f);
            if (hit.collider != null)
            {
                if (hit.transform.tag == "Terrain" || hit.transform.tag == "box_Big" || hit.transform.tag == "box_Small")
                {
                    break;
                }
            }

            yield return new WaitForSeconds(0.45f);
        }
    }
}
