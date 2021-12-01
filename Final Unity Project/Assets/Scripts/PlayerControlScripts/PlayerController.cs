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
    private CharacterState mPlayerState = CharacterState.IDLE;

    //Movement Settings
    public float mSpeed = 2.0f;
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

    // Start is called before the first frame update
    void Start()
    {
        _mAnimatorComponent = gameObject.GetComponent<Animator>();
        _mAnimatorComponent.runtimeAnimatorController = mIdleController;

        _mSoundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
        _mCameraManager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_bInputsDisabled)
        {
            Debug.Log("PlayerState: " + mPlayerState);
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
                }



                if (/*mPlayerState == CharacterState.JUMPING || */mPlayerState == CharacterState.WALKING)
                {
                    if (Input.GetKey(KeyCode.D))
                    {
                        _bIsGoingRight = true;
                        transform.Translate(transform.right * Time.deltaTime * mSpeed);
                    }
                    else if (Input.GetKey(KeyCode.A))
                    {
                        _bIsGoingRight = false;
                        transform.Translate(-transform.right * Time.deltaTime * mSpeed);
                    }
                }
                if (mPlayerState == CharacterState.JUMPING)
                {
                    if (Input.GetKey(KeyCode.D) && !_bRightDirectionInputsDisabled)
                    {
                        _bIsGoingRight = true;
                        transform.Translate(transform.right * Time.deltaTime * mSpeed);
                        _bLeftDirectionInputsDisabled = true;
                    }
                    else if (Input.GetKey(KeyCode.A) && !_bLeftDirectionInputsDisabled)
                    {
                        _bIsGoingRight = false;
                        transform.Translate(-transform.right * Time.deltaTime * mSpeed);
                        _bRightDirectionInputsDisabled = true;
                    }
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

        CheckWall();
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
        yield return new WaitForSeconds(1.0f);

        while (true)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position - Vector3.up * 1f, -Vector2.up, 0.05f);
            if(hit.collider != null)
            {
                if(hit.transform.tag == "Terrain" || hit.transform.tag == "box_Big" || hit.transform.tag == "box_Small")
                {
                    Debug.Log("Hitting");
                    if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
                    {
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
        List<float> directions = new List<float> { -1, 1 };

        for (int i = 0; i < directions.Count; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position + transform.right * directions[i], transform.right * directions[i], 0.05f);
            if(hit.collider != null)
            {
                if(hit.transform.tag == "Terrain" || hit.transform.tag == "box_Big")
                {
                    transform.Translate(-.00000001f * transform.right * directions[i] * 0.025f);
                }
            }
        }
    }
}
