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
    private bool colliding = false;
    private Vector3 lastKnownSafePosition;
    private bool notPickedUp = true;
    private bool noRepeatSmallBoxT = false;
    private bool noRepeatBigBoxT = false;

    //StatesToSave
    private bool savedColored;
    private bool savedFalling;
    private bool savedStacked;
    private bool savedDetected;
    private bool savedOnPlatform;
    private bool savedGrabbed;
    private bool savedGrabbable;

    private SoundManager _mSoundManager;
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
    public static event Action smallBoxGrabActivated;
    public static event Action bigBoxGrabActivated;

    void Start()
    {
        _mSoundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
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
        if (!colored && !stacked && !falling){
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        else if (stacked && (colored || !colored)){
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            //this.transform.parent = belowBox;
        }
        else if (colored && !grabbed && !falling && !stacked){
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        else if (grabbed && !colored){
            Debug.Log("Release");
            grabbed = false;
            this.transform.parent = null;
            ReleaseObject(this.gameObject);
        }
        else if (grabbed)
        {
            //rb.velocity = Vector2.zero;
            if (this.gameObject.tag == "box_Small"){
                if (!boxHolder.GetComponent<BoxholderScript>().doNotPickUp && notPickedUp){
                    //Debug.Log("Running");
                    notPickedUp = false;
                    this.transform.position = boxHolder.transform.position;
                }
                else if ((!boxHolder.GetComponent<BoxholderScript>().doNotPickUp || boxHolder.GetComponent<BoxholderScript>().doNotPickUp) && !notPickedUp){
                    //Debug.Log("Running2");
                    this.transform.position = boxHolder.transform.position;
                    if (player.GetComponent<PlayerController>()._bIsGoingRight == true){
                        //Debug.Log("WalkingRight");
                        if (WallCheckRight()){
                            //Debug.Log("Running1");
                            rb.isKinematic = false;
                        }
                        else if (!WallCheckRight()){
                            rb.isKinematic = true;
                        }
                    }
                    if (player.GetComponent<PlayerController>()._bIsGoingRight == false){
                        //Debug.Log("WalkingLeft");
                        if (WallCheckLeft()){
                            //Debug.Log("Running2");
                            rb.isKinematic = false;
                        }
                        else if (!WallCheckLeft()){
                            rb.isKinematic = true;
                        }
                    }
                    else if (IsGrounded()){
                        rb.isKinematic = false;
                    }
                    else if (!IsGrounded() && (!WallCheckLeft() || !WallCheckRight())){
                        //Debug.Log("Running3");
                        rb.isKinematic = true;
                    }
                }
            }
            
            else if (this.gameObject.tag == "box_Big" && player.GetComponent<PlayerController>()._bPushingOrPulling == true){
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
                //rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                if (player.transform.position.y <= this.gameObject.transform.position.y){
                    if (player.GetComponent<PlayerController>()._bIsGoingRight == false && player.transform.position.x < this.gameObject.transform.position.x && !WallCheckLeft() && colored){
                        //Debug.Log("Running1");
                        this.gameObject.transform.position += player.GetComponent<PlayerController>().amountMoved;
                        wallObjectsRight.Clear();
                        player.GetComponent<PlayerController>()._bPulling = true;
                        player.GetComponent<PlayerController>()._bPushing = false;
                    }
                    else if (player.GetComponent<PlayerController>()._bIsGoingRight == true && player.transform.position.x > this.gameObject.transform.position.x && !WallCheckRight() && colored){
                        //Debug.Log("Running2");
                        this.gameObject.transform.position += player.GetComponent<PlayerController>().amountMoved;
                        wallObjectsLeft.Clear();
                        player.GetComponent<PlayerController>()._bPulling = true;
                        player.GetComponent<PlayerController>()._bPushing = false;
                    }
                    if (player.GetComponent<PlayerController>()._bIsGoingRight == true && player.transform.position.x < this.gameObject.transform.position.x && !WallCheckRight() && colored){
                        //Debug.Log("Running3");
                        this.gameObject.transform.position += player.GetComponent<PlayerController>().amountMoved;
                        wallObjectsLeft.Clear();
                        player.GetComponent<PlayerController>()._bPushing = true;
                        player.GetComponent<PlayerController>()._bPulling = false;
                    }
                    else if (player.GetComponent<PlayerController>()._bIsGoingRight == false && player.transform.position.x > this.gameObject.transform.position.x && !WallCheckLeft() && colored){
                        //Debug.Log("Running4");
                        this.gameObject.transform.position += player.GetComponent<PlayerController>().amountMoved;
                        wallObjectsRight.Clear();
                        player.GetComponent<PlayerController>()._bPushing = true;
                        player.GetComponent<PlayerController>()._bPulling = false;
                    }
                    else{

                    }
                    if (!colored){
                        grabbed = false;
                        player.GetComponent<PlayerController>()._bPushing = false;
                        player.GetComponent<PlayerController>()._bPulling = false;
                        player.GetComponent<PlayerController>()._bPushingOrPulling = false;
                        ReleaseObject(this.gameObject);
                    }
                }
            }

        }
        else if (falling && !grabbed){
            rb.constraints = RigidbodyConstraints2D.None;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        if (!colliding){
            lastKnownSafePosition = transform.position;
        }
        //Debug.Log(rb.isKinematic);
    }


  private void BoolChange (GameObject obj) {
    if (obj == this.gameObject) {
      colored = !colored;
    }
  }


    private void GrabObject (GameObject obj) {
    if (obj == this.gameObject && colored && detected && grabbable) {
            _mSoundManager.Play("BoxPickUp");
            groundedObject.Clear();
            wallObjectsRight.Clear();
            wallObjectsLeft.Clear();
            grabbed = true;
            stacked = false;
            falling = false;
            transform.parent = null;
            if (this.gameObject.tag == "box_Big"){
                player.GetComponent<PlayerController>()._bPushingOrPulling = true;
            }
            if (this.gameObject.tag == "box_Small"){
                if (!boxHolder.GetComponent<BoxholderScript>().doNotPickUp){
                    rb.isKinematic = true;
                    stackCollider.enabled = false;
                    this.transform.position = boxHolder.transform.position;
                }
                else if (boxHolder.GetComponent<BoxholderScript>().doNotPickUp){
                    grabbed = false;
                    ReleaseObject(this.gameObject);
                }
            }
            if(this.gameObject.name == "box_Small 7" && !noRepeatSmallBoxT)
            {
                smallBoxGrabActivated.Invoke();
                noRepeatSmallBoxT = true;
            }
            if(this.gameObject.name == "box_Big 1" && !noRepeatBigBoxT)
            {
                bigBoxGrabActivated.Invoke();
                noRepeatBigBoxT = true;
            }
        }
  }


  private void ReleaseObject (GameObject obj) {
    if (obj == this.gameObject) {
            //Debug.Log("Release");
            //_mSoundManager.Play("BoxRelease");
            player.GetComponent<PlayerController>()._bPushing = false;
            player.GetComponent<PlayerController>()._bPulling = false;
            player.GetComponent<PlayerController>()._bPushingOrPulling = false;
            grabbed = false;
            notPickedUp = true;
            if(this.gameObject.tag == "box_Small"){
                //Debug.Log(boxHolder.GetComponent<BoxCollider2D>().enabled);
                rb.isKinematic = false;
                stackCollider.enabled = true;
                if (WallCheckLeft()){
                    transform.position = lastKnownSafePosition;
                    wallObjectsLeft.Clear();
                }
                if (WallCheckRight()){
                    transform.position = lastKnownSafePosition;
                    wallObjectsRight.Clear();
                }
            }
            if (this.gameObject.tag == "box_Big"){
                player.GetComponent<PlayerController>()._bPushingOrPulling = false;
            }
            falling = true;
        }
  }
    private void DroppedObject(GameObject obj){
        if (obj == this.gameObject){

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null){
            //Debug.Log(collision.gameObject.tag);
            if (collision.collider.tag == "StackPoint" && !grabbed && IsGrounded()){
                _mSoundManager.Play("BoxDrop");
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
            else if (collision.collider.tag == "Platform" && !grabbed && IsGrounded()){
                _mSoundManager.Play("BoxDrop");
                falling = false;
                this.transform.parent = null;
                this.transform.parent = collision.transform;
                //Debug.Log("Platform Collision");
            }
            else if (collision.collider.tag == "Terrain" && !grabbed && IsGrounded()){
                _mSoundManager.Play("BoxDrop");
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
        if (collision != null){
            colliding = false;
            if(collision.collider.tag == "box_Big" || collision.collider.tag == "box_Small" || collision.collider.tag == "Terrain"){
                colliding = true;
            }
            if (collision.collider.tag == "StackPoint" && !grabbed && IsGrounded()){
                falling = false;
                stacked = true;
                this.transform.parent = null;
                belowBox = collision.transform;
                this.transform.parent = belowBox;
                rb.constraints = RigidbodyConstraints2D.None;
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                //Debug.Log("StackPoint CollisionStay");
            }
            if (collision.collider.tag == "Terrain" && !grabbed && IsGrounded()){
                falling = false;
                this.transform.parent = null;
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
                //Debug.Log("Terrain CollisionStay");
            }
        }
        else{
            Debug.Log("FailedTofALL");
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.tag == "Player"){
                grabbable = true;
            }
            else{

            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null){
            if (collision.tag == "Player"){
                grabbable = false;
                ReleaseObject(this.gameObject);
            }
            else{

            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "StackPoint" && !grabbed){
            this.transform.parent = null;
            stacked = false;
            falling = true;
        }
    }
    private bool IsGrounded()
    {
        float extraHeightText = .05f;
        RaycastHit2D[] raycastHits;
        Physics2D.queriesHitTriggers = true;
        raycastHits = Physics2D.RaycastAll(boxCollider.bounds.center, Vector2.down, boxCollider.bounds.extents.y + extraHeightText, ~spirals);
        Color rayColor;
        for (int i = 0; i < raycastHits.Length; i++){
            RaycastHit2D hit = raycastHits[i];
            if (hit.collider.gameObject != this.gameObject){
                if (groundedObject.Count != 1){
                    groundedObject.Add(hit.collider.gameObject);
                    rayColor = Color.green;
                }
            }
            else{
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
        if (groundedObject.Count == 1){
            return true;
        }
        else{
            return false;
        }
    }
    private bool WallCheckRight()
    {
        float extraHeightText = .05f;
        RaycastHit2D[] raycastHits;
        Physics2D.queriesHitTriggers = false;
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
            rb.velocity = Vector3.zero;
            return true;
        }
        else { return false; }
    }
    private bool WallCheckLeft()
    {
        float extraHeightText = .05f;
        RaycastHit2D[] raycastHits;
        Physics2D.queriesHitTriggers = false;
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
            rb.velocity = Vector3.zero;
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
