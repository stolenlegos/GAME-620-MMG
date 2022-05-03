using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {
  private Collider2D col;
  private Collider2D trigger;
  private SpriteRenderer sprite;
  private bool open;
    public bool storedOpen;
    private SoundManager _mSoundManager;
    private SaveManager _mSaveManager;


  void Start() {
        _mSoundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
        _mSaveManager = GameObject.FindGameObjectWithTag("SM").GetComponent<SaveManager>();
        col = GetComponent<Collider2D>();
    trigger = GetComponent<CapsuleCollider2D>();
    sprite = GetComponent<SpriteRenderer>();
    open = false;
    PlayerActions.PushButton += ChangeBool;
  }


  void Update() {
    if (open) {
      col.enabled = false;
      sprite.enabled = false;
      trigger.enabled = false;
    } else {
      col.enabled = true;
      sprite.enabled = true;
      trigger.enabled = true;
    }
        
    if(this.transform.parent.GetComponent<AllowButtonPush>().colored == false)
        {
            open = false;
        }
  }


  private void ChangeBool(GameObject obj) {
    if (obj == this.gameObject){
      open = !open;
            Debug.Log("Ran");
            if (open)
            {
                _mSoundManager.Play("DoorOpen");
            }
            else if (!open)
            {
                _mSoundManager.Play("DoorOpen");
            }
    }
  }
    public void SaveDoorState()
    {
        storedOpen = open;
    }
    public void ReloadDoorState()
    {
        open = storedOpen; 
    }
}
