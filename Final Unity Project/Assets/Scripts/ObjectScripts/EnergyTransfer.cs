using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyTransfer : MonoBehaviour
{
    public bool returning;
    public bool giving;
    private float speed;
    private Vector3 playerPosition;
    public GameObject targetObject;
    public Animator _mAnimatorComponent;
    private SoundManager _mSoundManager;

    // Start is called before the first frame update
    void Start()
    {
        _mAnimatorComponent = gameObject.GetComponent<Animator>();
        _mSoundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
        playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        speed = 6.5f;
        if (this.gameObject.name != "ColorEnergy 1")
        {
            _mSoundManager.Play("EnergyTransfer");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //_mAnimatorComponent.Play();
        playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        if (returning){
            transform.position = Vector3.MoveTowards(this.transform.position, playerPosition, speed * Time.deltaTime);
            if(transform.position == playerPosition){
                Destroy(gameObject);
                _mSoundManager.Stop("EnergyTransfer");
            }
        }
        else if (giving){
            if(EnergyEvents.objectsColored.Count >= 1){
                transform.position = Vector3.MoveTowards(this.transform.position, targetObject.transform.position, speed * Time.deltaTime);
                if (transform.position == EnergyEvents.objectsColored[EnergyEvents.objectsColored.Count - 1].transform.position)
                {
                    Destroy(gameObject);
                    _mSoundManager.Stop("EnergyTransfer");
                }
            }
            else if(EnergyEvents.objectsColored.Count <= 0)
            {
                Destroy(gameObject);
                _mSoundManager.Stop("EnergyTransfer");
            }
        }
    }
}
