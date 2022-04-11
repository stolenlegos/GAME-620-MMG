using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AllowMovement : MonoBehaviour {

    public LayerMask spirals;
    // States
    private bool colored;
    public bool falling;
    public bool stacked;
    private bool detected;
    private bool onPlatform;
    public bool grabbed;
    private bool grabbable;

    //StatesToSave
    private bool savedColored;
    private bool savedFalling;
    private bool savedStacked;
    private bool savedDetected;
    private bool savedOnPlatform;
    private bool savedGrabbed;
    private bool savedGrabbable;

    private GameObject player;
    private GameObject boxHolder;
    private Vector3 stackPosition;
    private Vector3 stackedBox;
    private Rigidbody2D rb;
    private Transform belowBox;
    private BoxCollider2D boxCollider;
    private BoxCollider2D stackCollider;
    public List<GameObject> groundedObject = new List<GameObject>();
    public List<GameObject> wallObjectsRight = new List<GameObject>();
    public List<GameObject> wallObjectsLeft = new List<GameObject>();
    [SerializeField]
  //private Rigidbody2D rb;
  //public LayerMask layerMask1;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        boxHolder = GameObject.Find("BoxHolder");
        boxCollider = transform.GetComponent<BoxCollider2D>();
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
        else if (grabbed)
        {
            
            if (this.gameObject.tag == "box_Small")
            {
                this.transform.position = boxHolder.transform.position;
            }
            else if(this.gameObject.tag == "box_Big")
            {
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
                //rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                if (player.transform.position.y <= this.gameObject.transform.position.y)
                {
                    if (player.GetComponent<PlayerController>()._bIsGoingRight == false && player.transform.position.x < this.gameObject.transform.position.x && !WallCheckLeft())
                    {
                        Debug.Log("Running1");
                        this.gameObject.transform.position += player.GetComponent<PlayerController>().amountMoved;
                        wallObjectsRight.Clear();
                    }
                    else if (player.GetComponent<PlayerController>()._bIsGoingRight == true && player.transform.position.x > this.gameObject.transform.position.x && !WallCheckRight())
                    {
                        Debug.Log("Running2");
                        this.gameObject.transform.position += player.GetComponent<PlayerController>().amountMoved;
                        wallObjectsLeft.Clear();
                    }
                    if (player.GetComponent<PlayerController>()._bIsGoingRight == true && player.transform.position.x < this.gameObject.transform.position.x && !WallCheckRight())
                    {
                        Debug.Log("Running3");
                        this.gameObject.transform.position += player.GetComponent<PlayerController>().amountMoved;
                        wallObjectsLeft.Clear();
                    }
                    else if (player.GetComponent<PlayerController>()._bIsGoingRight == false && player.transform.position.x > this.gameObject.transform.position.x && !WallCheckLeft())
                    {
                        Debug.Log("Running4");
                        this.gameObject.transform.position += player.GetComponent<PlayerController>().amountMoved;
                        wallObjectsRight.Clear();
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
            groundedObject.Clear();
            wallObjectsRight.Clear();
            wallObjectsLeft.Clear();
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
                //boxHolder.GetComponent<BoxCollider2D>().enabled = true;
            }
        }
  }


  private void ReleaseObject (GameObject obj) {
    if (obj == this.gameObject) {
            grabbed = false;
            if(this.gameObject.tag == "box_Small"){
                rb.isKinematic = false;
                stackCollider.enabled = true;
                //boxHolder.GetComponent<BoxCollider2D>().enabled = false;
            }
            if (this.gameObject.tag == "box_Big"){
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
            if (collision.collider.tag == "StackPoint" && !grabbed && IsGrounded())
            {
                falling = false;
                stacked = true;
                this.transform.parent = null;
                this.transform.parent = collision.transform;
                stackedBox = collision.transform.position;
                stackPosition = stackedBox - this.transform.position;
                this.transform.position = stackedBox - stackPosition;
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                //Debug.Log("StackPoint Collision");
            }
            else if (collision.collider.tag == "Platform" && !grabbed && IsGrounded())
            {
                falling = false;
                this.transform.parent = null;
                this.transform.parent = collision.transform;
                //Debug.Log("Platform Collision");
            }
            else if (collision.collider.tag == "Terrain" && !grabbed && IsGrounded())
            {
                falling = false;
                this.transform.parent = null;
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
                //Debug.Log("Terrain Collision");
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
            if (collision.collider.tag == "StackPoint" && !grabbed && IsGrounded())
            {
                falling = false;
                stacked = true;
                this.transform.parent = null;
                belowBox = collision.transform;
                this.transform.parent = belowBox;
                rb.constraints = RigidbodyConstraints2D.None;
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                //Debug.Log("StackPoint CollisionStay");
            }
            if (collision.collider.tag == "Terrain" && !grabbed && IsGrounded())
            {
                falling = false;
                this.transform.parent = null;
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
                //Debug.Log("Terrain CollisionStay");
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
                ReleaseObject(this.gameObject);
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
        float extraHeightText = .05f;
        RaycastHit2D[] raycastHits;
        raycastHits = Physics2D.RaycastAll(boxCollider.bounds.center, Vector2.down, boxCollider.bounds.extents.y + extraHeightText, ~spirals);
        Color rayColor;
        for (int i = 0; i < raycastHits.Length; i++)
        {
            RaycastHit2D hit = raycastHits[i];
            if (hit.collider.gameObject != this.gameObject)
            {
                if (groundedObject.Count != 1)
                {
                    groundedObject.Add(hit.collider.gameObject);
                    rayColor = Color.green;
                }
            }
            else
            {
                rayColor = Color.red;
            }
            /*if (raycastHits[1] != false)
            {
                rayColor = Color.green;
            }*/
        }
        Debug.DrawRay(boxCollider.bounds.center, Vector2.down * (boxCollider.bounds.extents.y + extraHeightText));
        //Debug.Log(raycastHits[1].collider.tag);
        //Debug.Log(raycastHits[0].collider.tag);
        if (groundedObject.Count == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private bool WallCheckRight()
    {
        float extraHeightText = .05f;
        RaycastHit2D[] raycastHits;
        raycastHits = Physics2D.RaycastAll(boxCollider.bounds.center, Vector2.right, boxCollider.bounds.extents.y + extraHeightText, ~spirals);
        Color rayColor;
        for (int i = 0; i < raycastHits.Length; i++) {
            RaycastHit2D hit = raycastHits[i];
            if (hit.collider.gameObject != this.gameObject) {
                if (wallObjectsRight.Count != 1) {
                    if(hit.collider.gameObject.tag != "Player") {
                        wallObjectsRight.Add(hit.collider.gameObject);
                        rayColor = Color.green;
                    }
                }
            }
            else
            {
                rayColor = Color.red;
            }
        }
        Debug.DrawRay(boxCollider.bounds.center, Vector2.right * (boxCollider.bounds.extents.y + extraHeightText));
        if (wallObjectsRight.Count == 1) {
            return true;
        }
        else { return false; }
    }
    private bool WallCheckLeft()
    {
        float extraHeightText = .05f;
        RaycastHit2D[] raycastHits;
        raycastHits = Physics2D.RaycastAll(boxCollider.bounds.center, Vector2.left, boxCollider.bounds.extents.y + extraHeightText, ~spirals);
        Color rayColor;
        for (int i = 0; i < raycastHits.Length; i++){
            RaycastHit2D hit = raycastHits[i];
            if (hit.collider.gameObject != this.gameObject){
                if (wallObjectsLeft.Count != 1){
                    if (hit.collider.gameObject.tag != "Player"){
                        wallObjectsLeft.Add(hit.collider.gameObject);
                        rayColor = Color.green;
                    }
                }
            }
            else{ rayColor = Color.red; }
        }
        Debug.DrawRay(boxCollider.bounds.center, Vector2.left * (boxCollider.bounds.extents.y + extraHeightText));
        if (wallObjectsLeft.Count == 1) {
            return true;
        }
        else { return false; }
    }
    private void OnMouseOver()
    {
        detected = true;
    }
    private void OnMouseExit()
    {
        detected = false;
    }
    public void SaveCurrentState()
    {
        savedColored = this.colored;
        savedFalling = this.falling;
        savedStacked = this.stacked;
        savedDetected = this.detected;
        savedOnPlatform = this.onPlatform;
        savedGrabbed = this.grabbed;
        savedGrabbable = this.grabbable;
    }
    public void ResetState()
    {
        this.colored = savedColored;
        this.falling = savedFalling;
        this.stacked = savedStacked;
        this.detected = savedDetected;
        this.onPlatform = savedOnPlatform;
        this.grabbed = savedGrabbed;
        this.grabbable = savedGrabbable;
    }
}
