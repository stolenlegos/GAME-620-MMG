using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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
    public CharacterState mPlayerState = CharacterState.IDLE;

    //Movement Settings
    private float mSpeed = 2.5f;
    private float mJumpStrength = 7.1f;
    private int playerCollisionCount;

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
    private bool _bCarrying = false;

    //private bool _bPlayerInvincible = false;
    private bool _bPlayerCanExamine = false;

    private SoundManager _mSoundManager;
    private CameraManager _mCameraManager;
    private EnergyManager _mEnergyManager;
    private Rigidbody2D rb2D;

    private float moveHorizontal;
    private float moveVertical;
    public Vector3 previousPos;
    public Vector3 amountMoved;

    // Start is called before the first frame update
    void Start()
    {
        //_mAnimatorComponent.runtimeAnimatorController = mIdleController;
        _mAnimatorComponent = gameObject.transform.GetChild(0).gameObject.GetComponent<Animator>();

        _mSoundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
        _mCameraManager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraManager>();
        //_mEnergyManager = GetComponent<EnergyManager>();

        previousPos = transform.position;
        rb2D = gameObject.GetComponent<Rigidbody2D>();

        EnergyEvents.EnergyUIChange += energyCheck;
    }

    // Update is called once per frame
    void Update()
    {
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");
        playerCollisionCount = GameObject.FindGameObjectWithTag("Player").GetComponent<playerCollisionCounter>().collisionCount;
        Debug.DrawRay(transform.position, transform.forward * 5, Color.red);
        UpdateWalkingAnimation();

        if (playerCollisionCount >= 1)
        {
            _bGrounded = true;
        }
        else
        {
            _bGrounded = false;
        }
        //Debug.Log ("CollisionCount: " + playerCollisionCount);
        if (!_bInputsDisabled)
        {
            //Debug.Log("Carrying: " + _bCarrying);
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
                            gameObject.GetComponent<Rigidbody2D>().velocity = transform.up * mJumpStrength;
                            _bPlayerStateChanged = true;
                            mPlayerState = CharacterState.JUMPING;
                            StartCoroutine("CheckGrounded");
                    }
                    else if (Input.GetMouseButtonDown(2) && _bPlayerCanExamine)
                    {
                        _bPlayerStateChanged = true;
                        mPlayerState = CharacterState.EXAMINE;
                        Debug.Log("PlayerStateChangedExamine");
                    }
                    else if (_bGrounded == false)
                    {

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
                            StartCoroutine("CheckGrounded");
                        }
                        else if (Input.GetKey(KeyCode.A))
                        {
                            _bIsGoingRight = false;
                            transform.position += new Vector3(moveHorizontal, 0, 0) * Time.deltaTime * mSpeed;
                            StartCoroutine("CheckGrounded");
                        }
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
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
            CheckWall();
        }
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
            yield return new WaitForSeconds(1.0f);

        while (true)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position - Vector3.up * 1f, -Vector2.up, 0.05f);
            /*float extensionHeight = .02f;
            RaycastHit2D hit = Physics2D.Raycast(this.GetComponent<CapsuleCollider2D>().bounds.center, Vector2.down, this.GetComponent<CapsuleCollider2D>().bounds.extents.y + extensionHeight);
            Debug.DrawRay(this.GetComponent<CapsuleCollider2D>().bounds.center, Vector2.down * (this.GetComponent<CapsuleCollider2D>().bounds.extents.y + extensionHeight));
            Physics2D.IgnoreLayerCollision(8, 8);*/
            if (hit.collider != null)
            {
                Debug.Log("Landed " + hit.collider.tag);
                if (hit.transform.tag == "Terrain" || hit.transform.tag == "box_Big" || hit.transform.tag == "box_Small" || hit.transform.tag == "Platform")
                {
                    if(hit.transform.tag == "box_Small" || hit.transform.tag == "box_Big")
                    {
                        //_bcancelJumpCarry = true;
                    }
                    else
                    {
                        //_bcancelJumpCarry = false;
                    }
                    if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A)))
                    {
                        _bMovementDisabled = false;
                        _bPlayerStateChanged = true;
                        mPlayerState = CharacterState.WALKING;
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
            //Debug.DrawRay(transform.position + transform.right * directions[i], transform.right  * directions[i], Color.green, 45.0f);
            if(hit.collider != null)
            {
                if(hit.transform.tag == "Terrain" || hit.transform.tag == "box_Big")
                {
                    transform.Translate(-.0000000000001f * transform.right * directions[i] * 0.025f);
                    if(hit.collider.tag == "Terrain" && mPlayerState == CharacterState.JUMPING)
                    {

                    }
                    else if (hit.collider.tag == "Terrain" && mPlayerState == CharacterState.WALKING)
                    {

                    }
                    else if(hit.collider.tag == "Terrain" && mPlayerState == CharacterState.IDLE)
                    {
                        //Debug.Log("Reset");
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
    private void energyCheck(int currentEnergy, int maxEnergy)
    {
      if(currentEnergy == (maxEnergy - 1))
      {
          //Debug.Log("less fast");
          mSpeed = 3.0f * .75f;
          mJumpStrength = 7.1f * .85f;
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
    private void energyPulse()
    {
        //if 
    }
}
