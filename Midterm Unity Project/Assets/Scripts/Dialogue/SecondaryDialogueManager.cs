using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SecondaryDialogueManager : MonoBehaviour
{
  [SerializeField]
  GameObject parentObject;
  [SerializeField]
  Text textField;


    void Start() {
      UIEvents.QuestInProgress += DisplayInProgressDialogue;
      UIEvents.QuestFinished += DisplayFinishedDialogue;
      parentObject.SetActive(false);
    }


    private void DisplayInProgressDialogue(string dialogue) {
      parentObject.SetActive(true);
      textField.text = dialogue;
    }


    private void DisplayFinishedDialogue(string dialogue) {
      parentObject.SetActive(true);
      textField.text = dialogue;
    }


    public void CloseUI() {
      parentObject.SetActive(false);
    }
}
