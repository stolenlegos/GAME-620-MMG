using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    public Transform pos1, pos2;
    public float speed;
    public Transform startPos;
    private bool colored;
    private bool savedColored;
    private bool transport = false;
    private Animator _mAnimatorComponent;
    private SoundManager _mSoundManager;

    Vector3 nextPos;

    // Start is called before the first frame update
    void Start()
    {
        nextPos = startPos.position;
        colored = false;
        ShaderEvents.SaturationChange += BoolChange;
        _mAnimatorComponent = this.gameObject.GetComponent<Animator>();
        _mSoundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (colored)
        {
            _mAnimatorComponent.SetBool("Colored", true);
            if (transform.position == pos1.position)
            {
                nextPos = pos2.position;
            }
            if (transform.position == pos2.position)
            {
                nextPos = pos1.position;
            }
        }
        if (!colored && !transport)
        {
            _mAnimatorComponent.SetBool("Colored", false);
            if (!colored && transform.position != pos1.position)
            {
                nextPos = pos1.position;
            }
        }
        transform.position = Vector3.MoveTowards(transform.position, nextPos, speed * Time.deltaTime);
    }

    private void BoolChange(GameObject obj)
    {
        if (obj == this.gameObject)
        {
            colored = !colored;
            if (colored)
            {
                _mSoundManager.Play("PlatformMoving");
            }
            else if (!colored)
            {
                _mSoundManager.Stop("PlatformMoving");
            }
        }
    }
    public void SaveCurrentState()
    {
        savedColored = this.colored;
    }
    public void ResetState()
    {
        this.colored = savedColored;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Door" && colored)
        {
            if (nextPos == pos2.position)
            {
                nextPos = pos1.position;
            }
            else if (nextPos == pos1.position)
            {
                nextPos = pos2.position;
            }
        }
        else if (collision.gameObject.tag == "Door" && !colored)
        {

            this.transform.position = startPos.position;
        }
    }
}
