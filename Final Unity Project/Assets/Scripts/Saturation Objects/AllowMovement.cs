using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllowMovement : MonoBehaviour {

    private bool colored;
    public bool falling;
    public bool stacked;
    public bool detected;
    private GameObject player;
    public bool grabbed;
    private Vector3 offset;
    private Rigidbody2D rb;
    [SerializeField]
  //private Rigidbody2D rb;
  //public LayerMask layerMask1;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        colored = false;
        falling = true;
        stacked = false;
        rb = gameObject.GetComponent<Rigidbody2D>();
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
        else if (colored && grabbed == false && falling == false)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        if (grabbed)
        {
            this.transform.position = player.transform.position - offset;
        }
        else if (falling && !detected)
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
    if (obj == this.gameObject && colored && detected) {
            grabbed = true;
            stacked = false;
            falling = false;
            if (grabbed == true && Input.GetMouseButtonDown(0) == true)
            {
                this.transform.parent = null;
                this.transform.parent = player.transform;
                offset = player.transform.position - this.transform.position;
                rb.constraints = RigidbodyConstraints2D.None;
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                this.transform.position = player.transform.position - offset;
                this.transform.parent = player.transform;
            }
        }
  }


  private void ReleaseObject (GameObject obj) {
    if (obj == this.gameObject) {
            grabbed = false;
            this.transform.parent = null;
            this.transform.position = this.transform.position + Vector3.up * .01f;
            if (this.transform.childCount == 2)
            {
                this.transform.GetChild(1).position = this.transform.GetChild(1).position + Vector3.up * -.01f;
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
    private void OnMouseOver()
    {
        detected = true;
    }
    private void OnMouseExit()
    {
        detected = false;
    }
    /*private void FallUntilCollisionDetected()
   {

       RaycastHit2D hit = Physics2D.Raycast(this.transform.position - Vector3.up * 1f, -Vector2.up, 0.05f, layerMask1);
       Debug.DrawRay(this.transform.position - Vector3.up * 1f, -Vector2.up, Color.green, 45.0f);
       if (hit.transform != null){
           if (hit.transform.tag == "Terrain"){
               falling = false;
               this.transform.parent = null;
               rb.constraints = RigidbodyConstraints2D.FreezeAll;
               Debug.Log("Terrain");
           }
           if (hit.transform.tag == "box_Big" || hit.transform.tag == "box_Small") {
               falling = false;
               stacked = true;
               this.transform.parent = null;
               this.transform.parent = hit.transform;
               //offset = player.transform.position - this.transform.position;
               //this.transform.position = this.transform.parent.position - offset;
               rb.constraints = RigidbodyConstraints2D.FreezeRotation;
               Debug.Log("Box");
           }
           else
           {
               Debug.Log("Failed1");
           }
       }
       else if (hit.transform == null){
           Debug.Log("Failed2");
       }
   }*/
}
