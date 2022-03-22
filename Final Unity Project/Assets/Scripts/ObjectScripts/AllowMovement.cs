using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllowMovement : MonoBehaviour {

    private bool colored;
    public bool falling;
    public bool stacked;
    public bool detected;
    public bool onPlatform;
    private GameObject player;
    private GameObject boxHolder;
    public bool grabbed;
    private bool grabbable;
    private Vector3 stackPosition;
    private Vector3 stackedBox;
    private Rigidbody2D rb;
    private Transform belowBox;
    private BoxCollider2D boxCollider;
    private BoxCollider2D stackCollider;
    [SerializeField]
  //private Rigidbody2D rb;
  //public LayerMask layerMask1;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        boxHolder = GameObject.Find("BoxHolder");
        stackCollider = this.transform.GetChild(0).gameObject.GetComponent<BoxCollider2D>();
        colored = false;
        falling = true;
        stacked = false;
        grabbable = false;
        onPlatform = false;
        rb = gameObject.GetComponent<Rigidbody2D>();
        ShaderEvents.SaturationChange += BoolChange;
        PlayerActions.Grab += GrabObject;
        PlayerActions.Release += ReleaseObject;
        PlayerActions.Drop += DroppedObject;
    }


  void Update() {
        if (!colored && !stacked && !falling)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        else if (stacked && (colored || !colored))
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            //this.transform.parent = belowBox;
        }
        else if (colored && !grabbed && !falling && !stacked)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        else if (grabbed && !colored)
        {
            grabbed = false;
            this.transform.parent = null;
        }
        else if (grabbed == true)
        {
            if (this.gameObject.tag == "box_Small")
            {
                this.transform.position = boxHolder.transform.position;
            }
            else if(this.gameObject.tag == "box_Big")
            {
                rb.constraints = RigidbodyConstraints2D.None;
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                if (player.transform.position.y <= this.gameObject.transform.position.y)
                {
                    if (player.GetComponent<PlayerController>()._bIsGoingRight == false && player.transform.position.x < this.gameObject.transform.position.x)
                    {
                        this.gameObject.transform.position += player.GetComponent<PlayerController>().amountMoved;
                    }
                    else if (player.GetComponent<PlayerController>()._bIsGoingRight == true && player.transform.position.x > this.gameObject.transform.position.x)
                    {
                        this.gameObject.transform.position += player.GetComponent<PlayerController>().amountMoved;
                    }
                    if (player.GetComponent<PlayerController>()._bIsGoingRight == true && player.transform.position.x < this.gameObject.transform.position.x)
                    {
                        this.gameObject.transform.position += player.GetComponent<PlayerController>().amountMoved;
                    }
                    else if (player.GetComponent<PlayerController>()._bIsGoingRight == false && player.transform.position.x > this.gameObject.transform.position.x)
                    {
                        this.gameObject.transform.position += player.GetComponent<PlayerController>().amountMoved;
                    }
                }
                
            }
        }
        else if (falling && !grabbed)
        {
            rb.constraints = RigidbodyConstraints2D.None;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

    }


  private void BoolChange (GameObject obj) {
    if (obj == this.gameObject && !grabbed) {
      colored = !colored;
    }
  }


    private void GrabObject (GameObject obj) {
    if (obj == this.gameObject && colored && detected && grabbable) {
            grabbed = true;
            stacked = false;
            falling = false;
            transform.parent = null;
            if(this.gameObject.tag == "box_Big")
            {
                player.GetComponent<PlayerController>()._bPushingOrPulling = true;
            }
            if (this.gameObject.tag == "box_Small")
            {
                rb.isKinematic = true;
                stackCollider.enabled = false;
                this.transform.position = boxHolder.transform.position;
            }
        }
  }


  private void ReleaseObject (GameObject obj) {
    if (obj == this.gameObject) {
            grabbed = false;
            //Debug.Log("Ran");
            if(this.gameObject.tag == "box_Small")
            {
                rb.isKinematic = false;
                stackCollider.enabled = true;
            }
            if (this.gameObject.tag == "box_Big")
            {
                player.GetComponent<PlayerController>()._bPushingOrPulling = false;
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
            //Debug.Log(collision.gameObject.tag);
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
            else if (collision.collider.tag == "Platform" && !grabbed)
            {
                falling = false;
                this.transform.parent = null;
                this.transform.parent = collision.transform;
            }
            else if (collision.collider.tag == "Terrain" && !grabbed)
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
                belowBox = collision.transform;
                this.transform.parent = belowBox;
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
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "StackPoint" && !grabbed)
        {
            this.transform.parent = null;
            stacked = false;
            falling = true;
        }
    }
    private bool IsGrounded()
    {
        float extraHeightText = .01f;
        RaycastHit2D raycastHit = Physics2D.Raycast(boxCollider.bounds.center, Vector2.down, boxCollider.bounds.extents.y + extraHeightText);
        Color rayColor;
        if (raycastHit.collider != null)
        {
            rayColor = Color.green;
        }
        else
        {
            rayColor = Color.red;
        }
        //Debug.DrawRay(boxCollider.bounds.center, Vector2.down * (boxCollider.bounds.extents.y + extraHeightText));
        //Debug.Log(raycastHit.collider);
        return raycastHit.collider != null;
    }
    private void OnMouseOver()
    {
        detected = true;
    }
    private void OnMouseExit()
    {
        detected = false;
        //Debug.Log("MouseExit");
    }
}
