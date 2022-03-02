using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllowMovement : MonoBehaviour {

    private bool colored;
    public bool falling;
    public bool stacked;
    public bool detected;
    private GameObject player;
    private GameObject boxHolder;
    public bool grabbed;
    private bool grabbable;
    private Vector3 stackPosition;
    private Vector3 stackedBox;
    private Rigidbody2D rb;
    [SerializeField]
  //private Rigidbody2D rb;
  //public LayerMask layerMask1;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        boxHolder = GameObject.Find("BoxHolder");
        colored = false;
        falling = true;
        stacked = false;
        grabbable = false;
        rb = gameObject.GetComponent<Rigidbody2D>();
        ShaderEvents.SaturationChange += BoolChange;
        PlayerActions.Grab += GrabObject;
        PlayerActions.Release += ReleaseObject;
        PlayerActions.Drop += DroppedObject;
    }


  void Update() {
        if (!colored && !stacked)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        else if (stacked && (colored || !colored))
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            this.transform.position = stackedBox - stackPosition;
        }
        else if (colored && !grabbed && !falling && !stacked)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        if (grabbed == true)
        {
            if(this.gameObject.tag == "box_Small")
            {
                this.transform.position = boxHolder.transform.position;
            }
            else if(this.gameObject.tag == "box_Big")
            {
                rb.constraints = RigidbodyConstraints2D.None;
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
        }
        else if (falling && !grabbed)
        {
            rb.constraints = RigidbodyConstraints2D.None;
        }

    }


  private void BoolChange (GameObject obj) {
    if (obj == this.gameObject) {
      colored = !colored;
    }
  }


    private void GrabObject (GameObject obj) {
    if (obj == this.gameObject && colored && detected && grabbable) {
            grabbed = true;
            stacked = false;
            falling = false;
            transform.parent = null;
            if (this.gameObject.tag == "box_Small")
            {
                rb.isKinematic = true;
            }
            /*if (grabbed == true && Input.GetMouseButtonDown(0) == true)
            {
                if (this.gameObject.tag == "box_Small")
                {
                    this.transform.parent = null;
                    this.gameObject.transform.parent = player.gameObject.transform;
                    offset.Set(player.gameObject.transform.position.x -.5f, player.gameObject.transform.position.y, player.gameObject.transform.position.z);
                    rb.constraints = RigidbodyConstraints2D.None;
                    rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                    this.gameObject.transform.position = player.gameObject.transform.position - offset;
                    this.transform.parent = player.transform;
                }
                else
                {
                //Debug.Log("Continuing");
                this.transform.parent = null;
                this.transform.parent = player.transform;
                offset = player.transform.position - this.transform.position;
                rb.constraints = RigidbodyConstraints2D.None;
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                this.transform.position = player.transform.position - offset;
                this.transform.parent = player.transform;
                }
            }*/
        }
  }


  private void ReleaseObject (GameObject obj) {
    if (obj == this.gameObject) {
            grabbed = false;
            if(this.gameObject.tag == "box_Small")
            {
                rb.isKinematic = false;
            }
            falling = true;
        }
  }
    private void DroppedObject(GameObject obj){
        if (obj == this.gameObject)
        {

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null)
        {
            if (collision.collider.tag == "StackPoint" && !grabbed)
            {
                falling = false;
                stacked = true;
                this.transform.parent = null;
                this.transform.parent = collision.transform;
                stackedBox = collision.transform.position;
                stackPosition = stackedBox - this.transform.position;
                this.transform.position = stackedBox - stackPosition;
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
            if (collision.collider.tag == "Terrain" && !grabbed)
            {
                falling = false;
                this.transform.parent = null;
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
            }
        }
        else{
            Debug.Log("FailedTofALL");
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision != null)
        {
            if (collision.collider.tag == "StackPoint" && !grabbed)
            {
                falling = false;
                stacked = true;
                this.transform.parent = null;
                this.transform.parent = collision.transform;
                rb.constraints = RigidbodyConstraints2D.None;
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
            if (collision.collider.tag == "Terrain" && !grabbed)
            {
                falling = false;
                this.transform.parent = null;
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
            }
        }
        else
        {
            Debug.Log("FailedTofALL");
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.tag == "Player")
            {
                grabbable = true;
            }
            else
            {

            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.tag == "Player")
            {
                grabbable = false;
            }
            else
            {

            }
        }
    }
    private void OnMouseOver()
    {
        detected = true;
    }
    private void OnMouseExit()
    {
        detected = false;
    }
}
