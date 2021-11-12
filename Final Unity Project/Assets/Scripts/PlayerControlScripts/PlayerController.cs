using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum CharacterState
{
    IDLE,
    WALKING,
    JUMPING,
    DEAD
}

public class PlayerController : MonoBehaviour
{
    private CharacterState mPlayerState = CharacterState.IDLE;

    //Movement Settings
    private float mSpeed = 5.0f;
    private float mJumpStrength = 10.0f;

    //State Animation Controller
    //fill in animator stuff here

    private bool _bIsGoingRight = true;
    private bool _bPlayerStateChanged = false;

    private bool _bInputsDisabled = false;

    private bool _bPlayInvincible = false;

    // Start is called before the first frame update
    void Start()
    {
        //set animators and sound managers
    }

    // Update is called once per frame
    void Update()
    {
        if (!_bInputsDisabled)
        {

            _bPlayerStateChanged = false;
            // check state changes
            if (mPlayerState == CharacterState.IDLE)
            {
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
                    gameObject.GetComponent<Rigidbody2D>().velocity = transform.up * mJumpStrength;
                    _bPlayerStateChanged = true;
                    mPlayerState = CharacterState.JUMPING;
                }
            }
            else if (mPlayerState == CharacterState.WALKING)
            {
                if (Input.GetKey(KeyCode.Space))
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



            if (mPlayerState == CharacterState.JUMPING || mPlayerState == CharacterState.WALKING)
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
        }
    }
}
