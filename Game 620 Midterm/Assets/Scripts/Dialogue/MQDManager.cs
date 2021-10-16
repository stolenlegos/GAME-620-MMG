using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MQDManager : MonoBehaviour {
  [SerializeField]
  private GameObject textObj;
  [SerializeField]
  private Text textGoHere;

  private int numOfLines;
  private int currentLine;
  private string[] currentDialogue;


    private void Start() {
      currentLine = 0;
      textObj.SetActive(false);
      UIEvents.MQDialogue += RecieveDialogue;
    }


    private void Update() {
      textGoHere.text = currentDialogue[currentLine];
      Reset();
    }


    public void Reset(){
      if (currentLine >= numOfLines) {
        Debug.Log("I have reset");
        textObj.SetActive(false);
        currentLine = 0;
      }
    }


    public void NextLine() {
      currentLine++;
    }


    private void RecieveDialogue(string[] dialogue, int lines) {
      currentDialogue = dialogue;
      numOfLines = lines;
      textObj.SetActive(true);
    }
}
