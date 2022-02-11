using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum CharacterState
{
    IDLE,
    WALKING,
    JUMPING,
    //FALLING,
    CARRYING,
    JUMPCARRYING,
    EXAMINE,
    DEAD
}

public class PlayerController : MonoBehaviour
{
    public CharacterState mPlayerState = CharacterState.IDLE;

    //Movement Settings
    public float mSpeed = 3.0f;
    public float mJumpStrength = 7.1f;
    private int playerCollisionCount;

    //State Animation Controller
  //  public RuntimeAnimatorController mIdleController;
  //  public RuntimeAnimatorController mWalkingController;
  //  public RuntimeAnimatorController mJumpingController;

    public Animator _mAnimatorComponent;
    public SpriteRenderer rend;
    private bool _bIsGoingRight = true;
    private bool _bPlayerStateChanged = false;

    private bool _bInputsDisabled = false;
    private bool _bMovementDisabled = false;
    //private bool _bLeftDirectionInputsDisabled = false;
    //private bool _bRightDirectionInputsDisabled = false;
    private bool _bGrounded = true;
    private bool _bcancelJumpCarry = false;
    private bool _bcancelPush = false;

    //private bool _bPlayerInvincible = false;
    private bool _bPlayerCanExamine = false;

    private SoundManager _mSoundManager;
    private CameraManager _mCameraManager;
    private EnergyManager _mEnergyManager;
    private Rigidbody2D rb2D;

    private float moveHorizontal;
    private float moveVertical;

    // Start is called before the first frame update
    void Start()
    {
      //  _mAnimatorComponent.runtimeAnimatorController = mIdleController;
        _mAnimatorComponent = gameObject.transform.GetChild(0).gameObject.GetComponent<Animator>();

        _mSoundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
        _mCameraManager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraManager>();
        _mEnergyManager = GetComponent<EnergyManager>();

        rb2D = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");
        playerCollisionCount = GameObject.FindGameObjectWithTag("Player").GetComponent<playerCollisionCounter>().collisionCount;

        UpdateWalkingAnimation();

        if (playerCollisionCount >= 1)
        {
            _bGrounded = true;
        }
        else
        {
            _bGrounded = false;
        }
        /*if(_mEnergyManager.currentEnergy == (_mEnergyManager.maxEnergy - 1))
        {
            Debug.Log("less fast");
            mSpeed = mSpeed * .75f;
            mJumpStrength = mJumpStrength * .75f;
        }
        else if (_mEnergyManager.currentEnergy == (_mEnergyManager.maxEnergy - 2))
        {
            Debug.Log("more less fast");
            mSpeed = mSpeed * .5f;
            mJumpStrength = mJumpStrength * .5f;
        }
        else if (_mEnergyManager.currentEnergy == (_mEnergyManager.maxEnergy - 3))
        {
            Debug.Log("the slowest of the fast");
            mSpeed = mSpeed * .25f;
            mJumpStrength = mJumpStrength * .25f;
        }
        else
        {
            Debug.Log("Fast");
            mSpeed = 1.0f;
            mJumpStrength = 7.1f;
        }*/
        Debug.Log ("CollisionCount: " + playerCollisionCount);
        if (!_bInputsDisabled)
        {
            //Debug.Log("PlayerState: " + mPlayerState);
            //Debug.Log("ChildCount: " + this.gameObject.transform.childCount);
            //Debug.Log("Movement: " + _bMovementDisabled);
            _bPlayerStateChanged = false;
            // check state changes
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
                    else if (Input.GetKeyDown(KeyCode.Space))
                    {
                        //_mSoundManager.Play();
                        gameObject.GetComponent<Rigidbody2D>().velocity = transform.up * mJumpStrength;
                        _bPlayerStateChanged = true;
                        mPlayerState = CharacterState.JUMPING;
                        StartCoroutine("CheckGrounded");
                    }
                    else if (Input.GetMouseButtonDown(1) && _bPlayerCanExamine)
                    {
                        _bPlayerStateChanged = true;
                        mPlayerState = CharacterState.EXAMINE;
                        Debug.Log("PlayerStateChangedExamine");
                    }
                    else if (Input.GetMouseButtonDown(0))
                    {
                        _bPlayerStateChanged = true;
                        mPlayerState = CharacterState.CARRYING;
                    }
                    else if (_bGrounded == false)
                    {
                        //_bPlayerStateChanged = true;
                        //mPlayerState = CharacterState.FALLING;
                    }
                }
                else if (mPlayerState == CharacterState.WALKING)
                {
                    _mAnimatorComponent.SetBool("isGrounded_b", true);
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        //_mSoundManager.Play();
                        gameObject.GetComponent<Rigidbody2D>().velocity = transform.up * mJumpStrength;
                        _bPlayerStateChanged = true;
                        mPlayerState = CharacterState.JUMPING;
                        StartCoroutine("CheckGrounded");
                    }
                    else if (!Input.GetKey(KeyCode.D) && (!Input.GetKey(KeyCode.A)))
                    {
                        _bPlayerStateChanged = true;
                        mPlayerState = CharacterState.IDLE;
                    }
                    else if (Input.GetMouseButtonDown(0))
                    {
                        _bPlayerStateChanged = true;
                        mPlayerState = CharacterState.CARRYING;
                    }
                }

                else if (mPlayerState == CharacterState.CARRYING)
                {
                    _mAnimatorComponent.SetBool("isGrounded_b", true);
                    _bMovementDisabled = false;


                    if (Input.GetKey(KeyCode.D) && _bcancelJumpCarry == false && !_bcancelPush)
                    {
                        _bIsGoingRight = true;
                        transform.position += new Vector3(moveHorizontal, 0, 0) * Time.deltaTime * (mSpeed - .9f);
                    }
                    else if (Input.GetKey(KeyCode.A) && _bcancelJumpCarry == false && !_bcancelPush)
                    {
                        _bIsGoingRight = false;
                        transform.position += new Vector3(moveHorizontal, 0, 0) * Time.deltaTime * (mSpeed - .9f);
                    }
                    if (this.gameObject.transform.childCount > 1)
                    {
                        if (Input.GetKey(KeyCode.Space) && this.gameObject.transform.GetChild(1).tag == "box_Small" && _bcancelJumpCarry == false)
                        {
                            gameObject.GetComponent<Rigidbody2D>().velocity = transform.up * (mJumpStrength - 1.5f);
                            _bPlayerStateChanged = true;
                            mPlayerState = CharacterState.JUMPCARRYING;
                            StartCoroutine("CheckGrounded");
                        }
                    }
                    if (Input.GetMouseButtonUp(0) || this.gameObject.transform.childCount == 0)
                    {
                        _bPlayerStateChanged = true;
                        mPlayerState = CharacterState.IDLE;
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
                }
                if (mPlayerState == CharacterState.JUMPING)
                {
                    _mAnimatorComponent.SetBool("isGrounded_b", false);
                    if (Input.GetMouseButtonUp(0))
                    {
                        //_bPlayerStateChanged = true;
                       //mPlayerState = CharacterState.FALLING;
                        //this.gameObject.transform.childCount - 1;
                    }
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
                if (mPlayerState == CharacterState.JUMPCARRYING)
                {
                    _mAnimatorComponent.SetBool("isGrounded_b", false);
                    if (Input.GetKey(KeyCode.D))
                    {
                        _bIsGoingRight = true;
                        transform.position += new Vector3(moveHorizontal, 0, 0) * Time.deltaTime * (mSpeed - .9f);
                    }
                    else if (Input.GetKey(KeyCode.A))
                    {
                        _bIsGoingRight = false;
                        transform.position += new Vector3(moveHorizontal, 0, 0) * Time.deltaTime * (mSpeed - .9f);
                    }
                }
                /*if (mPlayerState == CharacterState.FALLING)
                {
                    _mAnimatorComponent.SetBool("isGrounded_b", false);
                    StartCoroutine("CheckGrounded"); 
                }*/
            }
            if (mPlayerState == CharacterState.EXAMINE)
            {
              _mAnimatorComponent.SetBool("isGrounded_b", true);
              _bMovementDisabled = true;
              _mCameraManager.PlayerExamineStart();
              Debug.Log("PlayerExamining");
                if(Input.GetMouseButtonUp(1))
                {
                    while (_mCameraManager.zoomBackTimer > 0)
                    {
                        _mCameraManager.PlayerExamineEnd();
                    }
                    _bPlayerStateChanged = true;
                    mPlayerState = CharacterState.IDLE;
                    _bMovementDisabled = false;
                    Debug.Log("PlayerEndExamining");
                }
            }

            gameObject.GetComponent<SpriteRenderer>().flipX = !_bIsGoingRight;
            if (_bPlayerStateChanged)
            {
                //ChangeAnimator();
            }
        }

        if (mPlayerState == CharacterState.WALKING || mPlayerState == CharacterState.JUMPING)
        {
            CheckWall();
        }
    }

    private void UpdateWalkingAnimation() {
      if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A)){
        _mAnimatorComponent.SetFloat("Speed_f", 1);
        if (Input.GetKey(KeyCode.A)) {
          rend.flipX = true;
        } else {
          rend.flipX = false;
        }
      } else {
        _mAnimatorComponent.SetFloat("Speed_f", 0);
      }
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

    IEnumerator CheckGrounded()
    {
        if (mPlayerState == CharacterState.JUMPCARRYING)
        {
            yield return new WaitForSeconds(1.2f);
        }
        else
        {
            yield return new WaitForSeconds(1.0f);
        }

        while (true)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position - Vector3.up * 1f, -Vector2.up, 0.05f);
            if(hit.collider != null)
            {
                if(hit.transform.tag == "Terrain" || hit.transform.tag == "box_Big" || hit.transform.tag == "box_Small")
                {
                    if(hit.transform.tag == "box_Small")
                    {
                        _bcancelJumpCarry = true;
                    }
                    else
                    {
                        _bcancelJumpCarry = false;
                    }
                    if (hit.transform.tag == "box_Big")
                    {
                        _bcancelPush = true;
                    }
                    else
                    {
                        _bcancelPush = false;
                    }
                    if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A)) && mPlayerState != CharacterState.JUMPCARRYING)
                    {
                        //Debug.Log("Running");
                        _bMovementDisabled = false;
                        _bPlayerStateChanged = true;
                        mPlayerState = CharacterState.WALKING;
                    }
                    /*else if (mPlayerState == CharacterState.FALLING)
                    {
                        _bMovementDisabled = false;
                        mPlayerState = CharacterState.IDLE;
                    }*/
                    else if (mPlayerState == CharacterState.JUMPCARRYING)
                    {
                        mPlayerState = CharacterState.CARRYING;
                    }
                    else
                    {
                        mPlayerState = CharacterState.IDLE;
                    }
                    break;
                }
            }
                yield return new WaitForSeconds(0.45f);

        }

        //ChangeAnimator();
        yield return null;
    }

    IEnumerator Coroutine_BlockPlayerInputs()
    {
        _bInputsDisabled = true;
        yield return new WaitForSeconds(0.5f);
        _bInputsDisabled = false;
        yield return null;
    }

    IEnumerator Coroutine_SetPlayerInvincible()
    {
        //_bPlayerInvincible = true;
        yield return new WaitForSeconds(1.5f);

        //_bPlayerInvincible = false;
        yield return null;
    }

    public void BlockPlayerInputs()
    {
        _bInputsDisabled = true;
    }

    public void CheckWall()
    {
        List<float> directions = new List<float> { -.2375f, .2375f };

        float distance = 0.005f;

        for (int i = 0; i < directions.Count; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position + transform.right * directions[i], transform.right * directions[i], distance);
            Debug.DrawRay(transform.position + transform.right * directions[i], transform.right  * directions[i], Color.green, 45.0f);
            if(hit.collider != null)
            {
                if(hit.transform.tag == "Terrain" || hit.transform.tag == "box_Big")
                {
                    transform.Translate(-.0000000000001f * transform.right * directions[i] * 0.025f);
                    if(hit.collider.tag == "Terrain" && mPlayerState == CharacterState.JUMPING)
                    {
                        //Debug.Log("Wall Hit");
                        //_bMovementDisabled = true;
                        //_bPlayerStateChanged = true;
                        //mPlayerState = CharacterState.FALLING;
                    }
                    else if (hit.collider.tag == "Terrain" && mPlayerState == CharacterState.WALKING)
                    {

                    }
                    else if(hit.collider.tag == "Terrain" && mPlayerState == CharacterState.IDLE)
                    {
                        Debug.Log("Reset");
                        _bMovementDisabled = false;
                    }
                }
            }
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.tag == "Examiner")
        {
            _bPlayerCanExamine = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Examiner")
        {
            _bPlayerCanExamine = false;
        }
    }
}
