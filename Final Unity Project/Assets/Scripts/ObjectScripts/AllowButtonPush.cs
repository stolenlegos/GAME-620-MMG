using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class AllowButtonPush : MonoBehaviour {
  public bool colored;
  private GameObject player;
  private bool playerNear;
  private bool doorOpen;
    private bool storedColor;
    private bool noRepeatButtonT = false;
    public Coroutine ButtonPressCoroutine;
    private UIManager _mUIManager;
    private DialogueManager _mDialogueManager;
  [SerializeField] private GameObject door;
  public float timer;
    public static event Action buttonTutorialPush;


  void Start() {
        _mUIManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        _mDialogueManager = GameObject.FindGameObjectWithTag("DialogueManager").GetComponent<DialogueManager>();
        player = GameObject.FindGameObjectWithTag("Player");
    colored = false;
    doorOpen = false;
    playerNear = false;
    ShaderEvents.SaturationChange += BoolChange;
  }


  void Update() {
    if (colored && Input.GetKeyDown(KeyCode.E) && playerNear) {
      PlayerActions.ButtonPushed(door);
      doorOpen = true;
            if (doorOpen)
            {
                _mUIManager.DisplayTimer(this.gameObject.transform, timer);
            }
            if(ButtonPressCoroutine != null)
            {
                StopCoroutine("ButtonTimer");
            }
            if (this.gameObject.name == "Button (2)" && !noRepeatButtonT && _mDialogueManager.buttonTutorial1Occured)
            {
                buttonTutorialPush.Invoke();
                noRepeatButtonT = true;
            }
            ButtonPressCoroutine = StartCoroutine("ButtonTimer");
            //Debug.Log("PUSHED THE BUTTON");
        }
    else if (!colored && doorOpen)
        {
            PlayerActions.ButtonPushed(door);
            doorOpen = false;
        }
    //Debug.Log("Door open is: " + doorOpen.ToString());
  }


  private void BoolChange (GameObject obj) {
    if (obj == this.gameObject) {
      colored = !colored;
            ShaderEvents.ChangeColor(door);
    }
  }


  private void OnTriggerEnter2D (Collider2D other) {
    if (other.gameObject == player) {
      playerNear = true;
    }
  }


  private void OnTriggerExit2D (Collider2D other) {
    if (other.gameObject == player) {
      playerNear = false;
    }
  }


  IEnumerator ButtonTimer() {
    yield return new WaitForSeconds(timer);

    PlayerActions.ButtonPushed(door);
    doorOpen = false;
    if (colored) {
      EnergyEvents.ChangeColor(this.gameObject);
    }
    if (!colored)
        {
            StopCoroutine(ButtonPressCoroutine);
        }
  }
    public void SaveButtonPush()
    {
        storedColor = colored;
    }

    public void ResetButtonPush()
    {
        if(ButtonPressCoroutine != null)
        {
            StopCoroutine(ButtonPressCoroutine);
        }
        if(this.gameObject.transform.childCount == 3)
        {
            Destroy(this.gameObject.transform.GetChild(2).gameObject);
        }
        colored = storedColor;
    }

}
