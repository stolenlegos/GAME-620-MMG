using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

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

    public CharacterState mPlayerState = CharacterState.IDLE;

    //Movement Settings
    private float mSpeed = 2.5f;
    private float mJumpStrength = 7.1f;

    //State Animation Controller
  //  public RuntimeAnimatorController mIdleController;
  //  public RuntimeAnimatorController mWalkingController;
  //  public RuntimeAnimatorController mJumpingController;

    public Animator _mAnimatorComponent;
    public SpriteRenderer rend;
    public bool _bIsGoingRight = true;
    private bool _bPlayerStateChanged = false;

    private bool _bInputsDisabled = false;
    private bool _bMovementDisabled = false;
    private bool _bGrounded = true;
    public bool _bPushingOrPulling = false;

    //private bool _bPlayerInvincible = false;
    private bool _bPlayerCanExamine = false;

    private SoundManager _mSoundManager;
    private CameraManager _mCameraManager;
    private EnergyManager _mEnergyManager;
    private SaveManager _mSaveManager;
    private Rigidbody2D rb2D;
    private CapsuleCollider2D playerCollider;
    private GameObject boxMovement;

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
        //Debug.Log("PlayerState: " + mPlayerState);
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");
        UpdateWalkingAnimation();

        //Debug.Log("Grounded Animation: " + _mAnimatorComponent.GetBool("isGrounded_b"));
        //Debug.Log("Animation " + _mAnimatorComponent.GetCurrentAnimatorStateInfo(0).fullPathHash);

        if (!_bInputsDisabled)
        {
            _bPlayerStateChanged = false;
            // check state changes
            if (Input.GetKeyDown(KeyCode.R)){
                _mSaveManager.ResetPositions();
            } 
            if (!_bMovementDisabled)
            {
                if (mPlayerState == CharacterState.IDLE)
                {
                    _mAnimatorComponent.SetBool("isGrounded_b", true);
                    _mAnimatorComponent.SetFloat("Speed_f", 0f);
                    if (Input.GetKey(KeyCode.D) || (Input.GetKey(KeyCode.A)))
                    {
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
                            gameObject.GetComponent<Rigidbody2D>().velocity = transform.up * mJumpStrength;
                            _bPlayerStateChanged = true;
                            mPlayerState = CharacterState.JUMPING;
                    }
                    else if (Input.GetKeyDown(KeyCode.Space) && _bPlayerCanExamine)
                    {
                        _bPlayerStateChanged = true;
                        mPlayerState = CharacterState.EXAMINE;
                       // Debug.Log("PlayerStateChangedExamine");
                    }
                }

                if (mPlayerState == CharacterState.WALKING)
                {
                    _mAnimatorComponent.SetBool("isGrounded_b", true);
                    _mAnimatorComponent.SetFloat("Speed_f", 1f);
                    _bMovementDisabled = false;
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
                    if (Input.GetKeyDown(KeyCode.Space) && IsGrounded() && !_bPushingOrPulling)
                    {
                            gameObject.GetComponent<Rigidbody2D>().velocity = transform.up * mJumpStrength;
                            _bPlayerStateChanged = true;
                            mPlayerState = CharacterState.JUMPING;
                    }
                    else if (!Input.GetKey(KeyCode.D) && (!Input.GetKey(KeyCode.A)))
                    {
                        _bPlayerStateChanged = true;
                        mPlayerState = CharacterState.IDLE;
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
        //pushing big boxes code
        amountMoved = transform.position - previousPos;
        previousPos = transform.position;
    }

    private void UpdateWalkingAnimation() {
      if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A)){
        _mAnimatorComponent.SetFloat("Speed_f", 1);
        if (Input.GetKey(KeyCode.A)) {
          //rend.flipX = true;
            } else {
          //rend.flipX = false;
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
            if ((collision.transform.tag == "Terrain" || collision.transform.tag == "box_Big" || collision.transform.tag == "box_Small" || collision.transform.tag == "Platform"))
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
                    _bMovementDisabled = false;
                    _bPlayerStateChanged = true;
                    mPlayerState = CharacterState.WALKING;
                }
                else if (IsGrounded())
                {
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
      }
      else if (currentEnergy == (maxEnergy - 2))
      {
          //Debug.Log("more less fast");
          mSpeed = 3.0f * .5f;
          mJumpStrength = 7.1f * .75f;
      }
      else if (currentEnergy == (maxEnergy - 3))
      {
          //Debug.Log("the slowest of the fast");
          mSpeed = 3.0f * .25f;
          mJumpStrength = 7.1f * .65f;
      }
      else if (currentEnergy == maxEnergy)
      {
          //Debug.Log("Fast");
          mSpeed = 3.0f;
          mJumpStrength = 7.1f;
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
        return raycastHit.collider != null;
    }
    public void SaveCurrentState()
    {
        savedPosition = this.transform.position;
    }
    public void ResetState()
    {
        this.transform.position = savedPosition;
    }
    /*public void ChangeAnimator()
{
    RuntimeAnimatorController newAnimator = mIdleController;

    if(mPlayerState == CharacterState.WALKING || mPlayerState == CharacterState.JUMPING)
    {
        newAnimator = mWalkingController;
        if(_bIsGoingRight)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    gameObject.GetComponent<Animator>().runtimeAnimatorController = newAnimator;
}*/

}
