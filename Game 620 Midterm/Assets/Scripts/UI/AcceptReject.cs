using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AcceptReject : MonoBehaviour {
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
  private int dialogueCount;


  private void Start() {
    QuestEvents.QuestProposed += AcceptUI;
    QuestEvents.QuestAccepted += DeactivateAcceptUI;
    proposalTextUI.SetActive(false);
    proposalInProgress = false;
    dialogueCount = 0;
  }

  private void Update() {
    if(proposalInProgress && dialogueCount == 0) {
      if (temp.questType == "KillQuest") {
        questProposalText.text = DialogueManager.KillProposalDialogue[Random.Range(0,1)];
      }
      else if (temp.questType == "FetchQuest") {
        questProposalText.text = DialogueManager.FetchProposeDialogue[Random.Range(0,1)];
      }
      else if (temp.questType == "DeliverQuest") {
        questProposalText.text = DialogueManager.DeliveryProposalDialogue[Random.Range(0,1)];
      }
    }
  }


  private void AcceptUI(QuestGiver questGiver) {
    temp = questGiver;
    proposalTextUI.SetActive(true);
    proposalInProgress = true;
  }


  public void AcceptTheQuest() {
    QuestEvents.AcceptQuest(temp);
  }


  private void DeactivateAcceptUI(QuestGiver questGiver) {
    proposalTextUI.SetActive(false);
  }


  public void RejectQuest() {
    if (rejectCount == 0) {
      questProposalText.text = "These old bones can't do it themselves.";
      rejectCount++;
    }
    else if (rejectCount == 1) {
      questProposalText.text = "There will be a great reward.";
      rejectCount++;
    }
    else if (rejectCount == 2) {
      questProposalText.text = "Udělej to, ty parchante.";
      rejectCount++;
    }
    else if (rejectCount == 3) {
      ResetVariables();
      QuestEvents.RejectQuest(temp);
    }
  }


  private void ResetVariables() {
    proposalTextUI.SetActive(false);
    rejectCount = 0;
  }


  public void ContinueButton(){
    dialogueCount = 2;
  }
}
