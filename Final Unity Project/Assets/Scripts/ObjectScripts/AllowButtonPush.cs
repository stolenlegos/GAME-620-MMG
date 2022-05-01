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
    private bool noRepeatButtonT = false;
    private Coroutine ButtonPressCoroutine;
    private UIManager _mUIManager;
  [SerializeField] private GameObject door;
  public float timer;
    public static event Action buttonTutorialPush;


  void Start() {
        _mUIManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
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
                _mUIManager.DisplayTimer(this.gameObject.transform);
            }
            if(ButtonPressCoroutine != null)
            {
                StopCoroutine("ButtonTimer");
            }
            if (this.gameObject.name == "Button (2)" && !noRepeatButtonT)
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

}
