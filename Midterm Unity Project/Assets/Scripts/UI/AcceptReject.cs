using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AcceptReject : MonoBehaviour {
  [SerializeField]
  private GameObject leaveDialogueButton;
  [SerializeField]
  private GameObject acceptButton;
  [SerializeField]
  private GameObject denyButton;
  [SerializeField]
  private GameObject continueButton;
  [SerializeField]
  private Text questProposalText;
  [SerializeField]
  private GameObject proposalTextUI;
  private QuestGiver temp;
  private int rejectCount;
  private bool proposalInProgress;
  private string[] tempDialogue;
  private int dialogueCount;


  private void Start() {
    QuestEvents.QuestProposed += AcceptUI;
    proposalTextUI.SetActive(false);
    proposalInProgress = false;
    dialogueCount = 0;
  }

  private void Update() {
    if (proposalInProgress) {
      questProposalText.text = tempDialogue[dialogueCount];
    }
  }


  private void AcceptUI(QuestGiver questGiver, string[] dialogue) {
    temp = questGiver;
    tempDialogue = dialogue;
    proposalTextUI.SetActive(true);
    dialogueCount = 0;
    proposalInProgress = true;
  }


  public void AcceptButtonFuntion(){
    dialogueCount = 6;
    AcceptTheQuest();
    leaveDialogueButton.SetActive(true);
    acceptButton.SetActive(false);
    denyButton.SetActive(false);
  }


  private void AcceptTheQuest() {
    QuestEvents.AcceptQuest(temp);
  }


  public void RejectQuest() {
    if (rejectCount == 0) {
      dialogueCount = 2;
      rejectCount++;
    }
    else if (rejectCount == 1) {
      dialogueCount = 3;
      rejectCount++;
    }
    else if (rejectCount == 2) {
      dialogueCount = 4;
      rejectCount++;
    }
    else if (rejectCount == 3) {
      dialogueCount = 5;
      leaveDialogueButton.SetActive(true);
      acceptButton.SetActive(false);
      denyButton.SetActive(false);
      QuestEvents.RejectQuest(temp);
    }
  }


  public void leaveDialogue() {
    ResetVariables();
  }


  private void ResetVariables() {
    acceptButton.SetActive(false);
    denyButton.SetActive(false);
    continueButton.SetActive(true);
    leaveDialogueButton.SetActive(false);
    proposalTextUI.SetActive(false);
    rejectCount = 0;
    dialogueCount = 0;
  }


  public void ContinueButton(){
    dialogueCount = 1;
    acceptButton.SetActive(true);
    denyButton.SetActive(true);
    continueButton.SetActive(false);
  }
}
