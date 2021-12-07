using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum CharacterState
{
    IDLE,
    WALKING,
    JUMPING,
    FALLING,
    CARRYING,
    JUMPCARRYING,
    EXAMINE,
    DEAD
}

public class PlayerController : MonoBehaviour
{
    private CharacterState mPlayerState = CharacterState.IDLE;

    //Movement Settings
    public float mSpeed = 1.0f;
    public float mJumpStrength = 7.0f;

    //State Animation Controller
    public RuntimeAnimatorController mIdleController;
    public RuntimeAnimatorController mWalkingController;
    public RuntimeAnimatorController mJumpingController;

    private Animator _mAnimatorComponent;
    private bool _bIsGoingRight = true;
    private bool _bPlayerStateChanged = false;

    private bool _bInputsDisabled = false;
    private bool _bMovementDisabled = false;
    private bool _bLeftDirectionInputsDisabled = false;
    private bool _bRightDirectionInputsDisabled = false;

    private bool _bPlayerInvincible = false;

    private SoundManager _mSoundManager;
    private CameraManager _mCameraManager;
    private Rigidbody2D rb2D;

    private float moveHorizontal;
    private float moveVertical;

    // Start is called before the first frame update
    void Start()
    {
        _mAnimatorComponent = gameObject.GetComponent<Animator>();
        _mAnimatorComponent.runtimeAnimatorController = mIdleController;

        _mSoundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
        _mCameraManager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraManager>();

        rb2D = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");
        if (!_bInputsDisabled)
        {
            Debug.Log("PlayerState: " + mPlayerState);
            Debug.Log("ChildCount: " + this.gameObject.transform.childCount);
            //Debug.Log("Movement: " + _bMovementDisabled);
            _bPlayerStateChanged = false;
            // check state changes
            if (!_bMovementDisabled)
            {
                if (mPlayerState == CharacterState.IDLE)
                {
                    _bLeftDirectionInputsDisabled = false;
                    _bRightDirectionInputsDisabled = false;
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
                    else if (Input.GetKey(KeyCode.Space))
                    {
                        //_mSoundManager.Play();
                        gameObject.GetComponent<Rigidbody2D>().velocity = transform.up * mJumpStrength;
                        _bPlayerStateChanged = true;
                        mPlayerState = CharacterState.JUMPING;
                        StartCoroutine("CheckGrounded");
                    }
                    else if (Input.GetMouseButtonDown(1))
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
                }
                else if (mPlayerState == CharacterState.WALKING)
                {
                    _bLeftDirectionInputsDisabled = false;
                    _bRightDirectionInputsDisabled = false;
                    if (Input.GetKey(KeyCode.Space))
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
                    _bMovementDisabled = false;
                    _bLeftDirectionInputsDisabled = false;
                    _bRightDirectionInputsDisabled = false;


                    if (Input.GetKey(KeyCode.D))
                    {
                        _bIsGoingRight = true;
                        //transform.Translate(transform.right * Time.deltaTime * mSpeed);
                        transform.position += new Vector3(moveHorizontal, 0, 0) * Time.deltaTime * (mSpeed - .9f);
                    }
                    else if (Input.GetKey(KeyCode.A))
                    {
                        _bIsGoingRight = false;
                        //transform.Translate(-transform.right * Time.deltaTime * mSpeed);
                        transform.position += new Vector3(moveHorizontal, 0, 0) * Time.deltaTime * (mSpeed - .9f);
                    }
                    if (this.gameObject.transform.childCount > 0)
                    {
                        if (Input.GetKey(KeyCode.Space) && this.gameObject.transform.GetChild(0).tag == "box_Small")
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

                if (/*mPlayerState == CharacterState.JUMPING || */mPlayerState == CharacterState.WALKING)
                {
                    _bMovementDisabled = false;
                    if (Input.GetKey(KeyCode.D))
                    {
                        _bIsGoingRight = true;
                        //transform.Translate(transform.right * Time.deltaTime * mSpeed);
                        transform.position += new Vector3(moveHorizontal, 0, 0) * Time.deltaTime * mSpeed;
                    }
                    else if (Input.GetKey(KeyCode.A))
                    {
                        _bIsGoingRight = false;
                        //transform.Translate(-transform.right * Time.deltaTime * mSpeed);
                        transform.position += new Vector3(moveHorizontal, 0, 0) * Time.deltaTime * mSpeed;
                    }
                }
                if (mPlayerState == CharacterState.JUMPING)
                {
                    if (Input.GetMouseButtonUp(0))
                    {
                        _bPlayerStateChanged = true;
                        mPlayerState = CharacterState.FALLING;
                        //this.gameObject.transform.childCount - 1;
                    }
                    if (Input.GetKey(KeyCode.D) && !_bRightDirectionInputsDisabled)
                    {
                        _bIsGoingRight = true;
                        //transform.Translate(transform.right * Time.deltaTime * mSpeed);
                        transform.position += new Vector3(moveHorizontal, 0, 0) * Time.deltaTime * mSpeed;
                        _bLeftDirectionInputsDisabled = true;
                    }
                    else if (Input.GetKey(KeyCode.A) && !_bLeftDirectionInputsDisabled)
                    {
                        _bIsGoingRight = false;
                        //transform.Translate(-transform.right * Time.deltaTime * mSpeed);
                        transform.position += new Vector3(moveHorizontal, 0, 0) * Time.deltaTime * mSpeed;
                        _bRightDirectionInputsDisabled = true;
                    }
                }
                if (mPlayerState == CharacterState.JUMPCARRYING)
                {
                    if (Input.GetKey(KeyCode.D) && !_bRightDirectionInputsDisabled)
                    {
                        _bIsGoingRight = true;
                        //transform.Translate(transform.right * Time.deltaTime * mSpeed);
                        transform.position += new Vector3(moveHorizontal, 0, 0) * Time.deltaTime * (mSpeed - .9f);
                        _bLeftDirectionInputsDisabled = true;
                    }
                    else if (Input.GetKey(KeyCode.A) && !_bLeftDirectionInputsDisabled)
                    {
                        _bIsGoingRight = false;
                        //transform.Translate(-transform.right * Time.deltaTime * mSpeed);
                        transform.position += new Vector3(moveHorizontal, 0, 0) * Time.deltaTime * (mSpeed - .9f);
                        _bRightDirectionInputsDisabled = true;
                    }
                }
                if (mPlayerState == CharacterState.FALLING)
                {
                    StartCoroutine("CheckGrounded");
                }
            }
            if (mPlayerState == CharacterState.EXAMINE)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    _bMovementDisabled = true;
                    _mCameraManager.PlayerExamineStart();
                    Debug.Log("PlayerExamining");
                }
                if(Input.GetMouseButtonUp(1))
                {
                    _mCameraManager.PlayerExamineEnd();
                    _bPlayerStateChanged = true;
                    mPlayerState = CharacterState.IDLE;
                    _bMovementDisabled = false;
                    Debug.Log("PlayerEndExamining");
                }
            }

            gameObject.GetComponent<SpriteRenderer>().flipX = !_bIsGoingRight;
            if (_bPlayerStateChanged)
            {
                ChangeAnimator();
            }
        }

        if (mPlayerState == CharacterState.WALKING || mPlayerState == CharacterState.JUMPING)
        {
            CheckWall();
        }
    }

    public void ChangeAnimator()
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
    }

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
                    //Debug.Log("Hitting");
                    if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A)) && mPlayerState != CharacterState.JUMPCARRYING)
                    {
                        Debug.Log("Running");
                        _bMovementDisabled = false;
                        _bPlayerStateChanged = true;
                        mPlayerState = CharacterState.WALKING;
                    }
                    else if (mPlayerState == CharacterState.FALLING)
                    {
                        _bMovementDisabled = false;
                        mPlayerState = CharacterState.IDLE;
                    }
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

        ChangeAnimator();
        yield return null;
    }

    //Sent when an incoming collider makes contact with this object's
    // collider (2D physics only)
    // <param name="other"> The Collision2D data associated with this collision. </params>
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.transform.tag == "Monster" && !_bPlayerInvincible)
        {
            //_mSoundManager.Play();
            Vector3 heading = other.transform.position - transform.position;
            float magnitude = heading.magnitude;
            gameObject.GetComponent<Rigidbody2D>().velocity = -10f * heading / magnitude;
            StartCoroutine("Coroutine_BlaockPlayerInputs");
            StartCoroutine("Coroutine_SetPlayerInvincible");

        }
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
        _bPlayerInvincible = true;
        yield return new WaitForSeconds(1.5f);

        _bPlayerInvincible = false;
        yield return null;
    }

    public void BlockPlayerInputs()
    {
        _bInputsDisabled = true;
    }

    public void SetStatePlayerInvincible(bool newState)
    {
        _bPlayerInvincible = newState;
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
                        _bMovementDisabled = true;
                        _bPlayerStateChanged = true;
                        mPlayerState = CharacterState.FALLING;
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
}
