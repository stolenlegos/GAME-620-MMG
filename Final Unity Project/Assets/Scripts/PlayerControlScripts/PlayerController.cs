using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.Rendering.Universal;

public enum CharacterState
{
    IDLE,
    WALKING,
    JUMPING,
    EXAMINE,
    DEAD
}

public class PlayerController : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayerMask;
    [SerializeField] private LayerMask rejectObjectsMask;

    public CharacterState mPlayerState = CharacterState.IDLE;

    //Movement Settings
    public float mSpeed = 3f;
    private float mJumpStrength = 7.1f;
    private float storedSpeed;

    //State Animation Controller
  //  public RuntimeAnimatorController mIdleController;
  //  public RuntimeAnimatorController mWalkingController;
  //  public RuntimeAnimatorController mJumpingController;

    public Animator _mAnimatorComponent;
    public SpriteRenderer rend;
    public Light2D playerLight;
    public bool _bIsGoingRight = true;
    private bool _bPlayerStateChanged = false;

    private bool _bInputsDisabled = false;
    public bool _bMovementDisabled = false;
    public bool _bJumpingDisabled = false;
    private bool _bGrounded = true;
    public bool _bPushing = false;
    public bool _bPulling = false;
    public bool _bPushingOrPulling = false;
    private bool passedThroughCheckpoint = false;
    public bool groundedForDialogue = true;

    //private bool _bPlayerInvincible = false;
    private bool _bPlayerCanExamine = false;

    private SoundManager _mSoundManager;
    private CameraManager _mCameraManager;
    private EnergyManager _mEnergyManager;
    private SaveManager _mSaveManager;
    private Rigidbody2D rb2D;
    private CapsuleCollider2D playerCollider;
    private GameObject boxMovement;

    public List<GameObject> wallObjectsRight = new List<GameObject>();
    public List<GameObject> wallObjectsLeft = new List<GameObject>();

    private float moveHorizontal;
    private float moveVertical;
    public Vector3 previousPos;
    public Vector3 amountMoved;
    public Vector3 savedPosition;

    // Start is called before the first frame update
    void Start()
    {
        //_mAnimatorComponent.runtimeAnimatorController = mIdleController;
        _mAnimatorComponent = gameObject.transform.GetChild(0).gameObject.GetComponent<Animator>();

        _mSoundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
        _mCameraManager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraManager>();
        _mSaveManager = GameObject.FindGameObjectWithTag("SM").GetComponent<SaveManager>();
        
        //_mEnergyManager = GetComponent<EnergyManager>();

        previousPos = transform.position;
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        playerCollider = transform.GetComponent<CapsuleCollider2D>();

        EnergyEvents.EnergyUIChange += energyCheck;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("PlayerState: " + mPlayerState);
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");
        UpdateWalkingAnimation();

        Debug.Log("is grounded" + groundedForDialogue);
        //Debug.Log(mSpeed);
        if (!IsGrounded()) { groundedForDialogue = false; }
        

        if (!_bInputsDisabled)
        {
            _bPlayerStateChanged = false;
            // check state changes
            // save 
            if (_bMovementDisabled && mPlayerState == CharacterState.IDLE)
            {
                _mAnimatorComponent.SetBool("isGrounded_b", true);
                _mAnimatorComponent.SetFloat("Speed_f", 0f);
            }
                if (Input.GetKeyDown(KeyCode.R) && passedThroughCheckpoint){
                _mSaveManager.ResetPositions();
            } 
            if (!_bMovementDisabled)
            {
                if (mPlayerState == CharacterState.IDLE)
                {
                    _mSoundManager.Stop("BoxPush/Pull");
                    _mSoundManager.Stop("Walking");
                    _mAnimatorComponent.SetBool("isGrounded_b", true);
                    _mAnimatorComponent.SetFloat("Speed_f", 0f);
                    if (Input.GetKey(KeyCode.D) || (Input.GetKey(KeyCode.A)))
                    {
                        _mSoundManager.Play("Walking");
                        if (_bPushingOrPulling)
                        {
                            _mSoundManager.Play("BoxPush/Pull");
                        }
                        _bPlayerStateChanged = true;
                            mPlayerState = CharacterState.WALKING;
                            if (Input.GetKey(KeyCode.D))
                            {
                                _bIsGoingRight = true;
                            }
                            else
                            {
                                _bIsGoingRight = false;
                            }
                    }
                    else if (Input.GetKeyDown(KeyCode.Space) && IsGrounded() && !_bPushingOrPulling && !_bPlayerCanExamine)
                    {
                        _mSoundManager.Play("Jumping");
                        gameObject.GetComponent<Rigidbody2D>().velocity = transform.up * mJumpStrength;
                            _bPlayerStateChanged = true;
                            mPlayerState = CharacterState.JUMPING;
                    }
                    else if (Input.GetKeyDown(KeyCode.Space) && _bPlayerCanExamine)
                    {
                        _bPlayerStateChanged = true;
                        mPlayerState = CharacterState.EXAMINE;
                        _mSoundManager.Play("Examining");
                       // Debug.Log("PlayerStateChangedExamine");
                    }
                }

                if (mPlayerState == CharacterState.WALKING)
                {
                    _mAnimatorComponent.SetBool("isGrounded_b", true);
                    _mAnimatorComponent.SetFloat("Speed_f", 1f);
                    _bMovementDisabled = false;
                    if (_bPulling && IsGrounded())
                    {
                        if (Input.GetKey(KeyCode.D) && !WallCheckRight())
                        {
                            _bIsGoingRight = true;
                            transform.position += new Vector3(moveHorizontal, 0, 0) * Time.deltaTime * mSpeed;
                        }
                        else if (Input.GetKey(KeyCode.A) && !WallCheckLeft())
                        {
                            _bIsGoingRight = false;
                            transform.position += new Vector3(moveHorizontal, 0, 0) * Time.deltaTime * mSpeed;
                        }
                    }
                    else if (!_bPulling && IsGrounded())
                    {
                        wallObjectsLeft.Clear();
                        wallObjectsRight.Clear();
                        
                        if (Input.GetKey(KeyCode.D))
                        {
                            _bIsGoingRight = true;
                            transform.position += new Vector3(moveHorizontal, 0, 0) * Time.deltaTime * mSpeed;
                        }
                        else if (Input.GetKey(KeyCode.A))
                        {
                            _bIsGoingRight = false;
                            transform.position += new Vector3(moveHorizontal, 0, 0) * Time.deltaTime * mSpeed;
                        }
                    }
                    if (Input.GetKeyDown(KeyCode.Space) && IsGrounded() && !_bPushingOrPulling)
                    {
                        _mSoundManager.Play("Jumping");
                        _mSoundManager.Stop("Walking");
                        gameObject.GetComponent<Rigidbody2D>().velocity = transform.up * mJumpStrength;
                        _bPlayerStateChanged = true;
                        mPlayerState = CharacterState.JUMPING;
                    }
                    else if (!Input.GetKey(KeyCode.D) && (!Input.GetKey(KeyCode.A)) && IsGrounded())
                    {
                        _bPlayerStateChanged = true;
                        mPlayerState = CharacterState.IDLE;
                    }
                    else if (!IsGrounded())
                    {
                        _mSoundManager.Stop("Walking");
                        _bPlayerStateChanged = true;
                        mPlayerState = CharacterState.JUMPING;
                    }

                }
                if (mPlayerState == CharacterState.JUMPING)
                {
                    _mAnimatorComponent.SetBool("isGrounded_b", false);
                    if (Input.GetKey(KeyCode.D))
                    {
                        _bIsGoingRight = true;
                        transform.position += new Vector3(moveHorizontal, 0, 0) * Time.deltaTime * mSpeed;
                    }
                    else if (Input.GetKey(KeyCode.A))
                    {
                        _bIsGoingRight = false;
                        transform.position += new Vector3(moveHorizontal, 0, 0) * Time.deltaTime * mSpeed;
                    }
                }
            }
            if (mPlayerState == CharacterState.EXAMINE)
            {
              _mAnimatorComponent.SetBool("isGrounded_b", true);
              _bMovementDisabled = true;
              _mCameraManager.PlayerExamineStart();
              //Debug.Log("PlayerExamining");
                if(Input.GetKeyUp(KeyCode.Space))
                {
                    while (_mCameraManager.zoomBackTimer > 0)
                    {
                        _mCameraManager.PlayerExamineEnd();
                    }
                    _mSoundManager.Stop("Examining");
                    _bPlayerStateChanged = true;
                    mPlayerState = CharacterState.IDLE;
                    _bMovementDisabled = false;
                    //Debug.Log("PlayerEndExamining");
                }
            }
            if (_bIsGoingRight)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            if (!_bIsGoingRight)
            {
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }

        if (mPlayerState == CharacterState.WALKING || mPlayerState == CharacterState.JUMPING)
        {
            
        }
        if(_bPushingOrPulling && !IsGrounded())
        {
            _bPushingOrPulling = false;
        }
        //Debug.Log("Push/Pull" + _bPushingOrPulling);
        //pushing big boxes code
        amountMoved = transform.position - previousPos;
        previousPos = transform.position;
    }

    private void UpdateWalkingAnimation() {
      if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A)) && !_bMovementDisabled){
        _mAnimatorComponent.SetFloat("Speed_f", 1);
        if ((Input.GetKey(KeyCode.A) && _bPulling) || (Input.GetKey(KeyCode.D) && _bPulling)) {
          rend.flipX = true;
            } else {
          rend.flipX = false;
            }
      } else {
        _mAnimatorComponent.SetFloat("Speed_f", 0);
      }
    }

    public void BlockPlayerInputs()
    {
        _bInputsDisabled = true;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Platform")
        {
            //Debug.Log("Triggered");
            this.transform.parent = other.gameObject.transform;
        }
        if (other.gameObject.tag == "Checkpoint")
        {
            passedThroughCheckpoint = true;
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.tag == "Examiner")
        {
            _bPlayerCanExamine = true;
            //Debug.Log("ExamineReady");
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Examiner")
        {
            _bPlayerCanExamine = false;
        }
        if (other.gameObject.tag == "Platform")
        {
            this.transform.parent = null;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform != null)
        {
            if ((collision.transform.tag == "Terrain" || collision.transform.tag == "box_Big" || collision.transform.tag == "box_Small" || collision.transform.tag == "Platform" || collision.transform.tag == "Door"))
            {
                if (!IsGrounded())
                {
                    _bGrounded = false;
                }
                if (collision.collider.tag == "StackPoint")
                {
                    this.transform.parent = collision.transform;
                }
                else
                {
                    //_bcancelJumpCarry = false;
                }
                if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A)) && IsGrounded())
                {
                    _mSoundManager.Play("Landing");
                    _mSoundManager.Play("Walking");
                    _bMovementDisabled = false;
                    _bPlayerStateChanged = true;
                    mPlayerState = CharacterState.WALKING;
                }
                else if (IsGrounded())
                {
                    _mSoundManager.Play("Landing");
                    mPlayerState = CharacterState.IDLE;
                }
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.collider.tag == "StackPoint")
        {
            this.transform.parent = null;
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if ((collision.transform.tag == "Terrain" || collision.transform.tag == "box_Big" || collision.transform.tag == "box_Small" || collision.transform.tag == "Platform") && _bGrounded == false)
        {
            IsGrounded();
            if (IsGrounded())
            {
                _bGrounded = true;
            }
            if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A)) && _bGrounded == true)
            {
                _mSoundManager.Play("Walking");
                _bMovementDisabled = false;
                _bPlayerStateChanged = true;
                mPlayerState = CharacterState.WALKING;
            }
            else if (_bGrounded == true)
            {
                mPlayerState = CharacterState.IDLE;
            }
        }
    }
    private void energyCheck(int currentEnergy, int maxEnergy)
    {
      if (currentEnergy == (maxEnergy - 1))
      {
          //Debug.Log("less fast");
          mSpeed = 3.0f * .75f;
          mJumpStrength = 7.1f * .80f;
          playerLight.pointLightOuterRadius = 2.15f * .75f;
            float pitchChange = 0.8f;
            _mSoundManager.PitchChange("Walking", pitchChange);
      }
      else if (currentEnergy == (maxEnergy - 2))
      {
          //Debug.Log("more less fast");
          mSpeed = 3.0f * .5f;
          mJumpStrength = 7.1f * .75f;
            playerLight.pointLightOuterRadius = 2.15f * .5f;
            float pitchChange = 0.7f;
            _mSoundManager.PitchChange("Walking", pitchChange);
        }
      else if (currentEnergy == (maxEnergy - 3))
      {
          //Debug.Log("the slowest of the fast");
          mSpeed = 3.0f * .33f;
          mJumpStrength = 7.1f * .65f;
            playerLight.pointLightOuterRadius = 2.15f * .33f;
            float pitchChange = 0.6f;
            _mSoundManager.PitchChange("Walking", pitchChange);
        }
      else if (currentEnergy == (maxEnergy - 4))
      {
          //Debug.Log("the slowest of the fast");
          mSpeed = 3.0f * .25f;
          mJumpStrength = 7.1f * .65f;
            playerLight.pointLightOuterRadius = 2.15f * .25f;
            float pitchChange = 0.5f;
            _mSoundManager.PitchChange("Walking", pitchChange);
        }
      else if (currentEnergy == maxEnergy)
      {
          //Debug.Log("Fast");
          mSpeed = 3.0f;
          mJumpStrength = 7.1f;
            playerLight.pointLightOuterRadius = 2.15f;
            float pitchChange = 0.85f;
            _mSoundManager.PitchChange("Walking", pitchChange);
        }
    }
    private bool IsGrounded()
    {
        float extraHeightText = .01f;
        RaycastHit2D raycastHit = Physics2D.Raycast(playerCollider.bounds.center, Vector2.down, playerCollider.bounds.extents.y + extraHeightText, ~playerLayerMask);
        Color rayColor;
        if (raycastHit.collider != null)
        {
            rayColor = Color.green;
        }
        else
        {
            rayColor = Color.red;
        }
        Debug.DrawRay(playerCollider.bounds.center, Vector2.down * (playerCollider.bounds.extents.y + extraHeightText));
        //Debug.Log(raycastHit.collider);
        if (raycastHit.collider != null)
        {
            groundedForDialogue = true;
            return true;
        }
        else { return false; }
        //return raycastHit.collider != null;
    }
    private bool WallCheckRight()
    {
        float extraHeightText = -.3f;
        RaycastHit2D[] raycastHits;
        Physics2D.queriesHitTriggers = false;
        raycastHits = Physics2D.RaycastAll(playerCollider.bounds.center, Vector2.right, playerCollider.bounds.extents.y + extraHeightText, ~playerLayerMask);
        Color rayColor;
        for (int i = 0; i < raycastHits.Length; i++)
        {
            RaycastHit2D hit = raycastHits[i];
            if (hit.collider.gameObject != this.gameObject)
            {
                if (wallObjectsRight.Count != 1)
                {
                    if (hit.collider.gameObject.tag != "Player")
                    {
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
        Debug.DrawRay(playerCollider.bounds.center, Vector2.right * (playerCollider.bounds.extents.y + extraHeightText));
        if (wallObjectsRight.Count == 1)
        {
            return true;
        }
        else { return false; }
    }
    private bool WallCheckLeft()
    {
        float extraHeightText = -.3f;
        RaycastHit2D[] raycastHits;
        Physics2D.queriesHitTriggers = false;
        raycastHits = Physics2D.RaycastAll(playerCollider.bounds.center, Vector2.left, playerCollider.bounds.extents.y + extraHeightText, ~playerLayerMask);
        Color rayColor;
        for (int i = 0; i < raycastHits.Length; i++)
        {
            RaycastHit2D hit = raycastHits[i];
            if (hit.collider.gameObject != this.gameObject)
            {
                if (wallObjectsLeft.Count != 1)
                {
                    if (hit.collider.gameObject.tag != "Player")
                    {
                        wallObjectsLeft.Add(hit.collider.gameObject);
                        rayColor = Color.green;
                    }
                }
            }
            else { rayColor = Color.red; }
        }
        Debug.DrawRay(playerCollider.bounds.center, Vector2.left * (playerCollider.bounds.extents.y + extraHeightText));
        if (wallObjectsLeft.Count == 1)
        {
            return true;
        }
        else { return false; }
    }
    public void SaveCurrentState()
    {
        savedPosition = this.transform.position;
    }
    public void ResetState()
    {
        this.transform.position = savedPosition;
    }

}
