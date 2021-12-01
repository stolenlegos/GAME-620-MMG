using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AcceptReject : MonoBehaviour {
  [SerializeField]
  private Text questProposalText;
  [SerializeField]
  private GameObject proposalTextUI;
  private QuestGiver temp;
  private int rejectCount;


  private void Start() {
    QuestEvents.QuestProposed += AcceptUI;
    QuestEvents.QuestAccepted += DeactivateAcceptUI;
    proposalTextUI.SetActive(false);
  }


  private void AcceptUI(QuestGiver questGiver) {
    temp = questGiver;
    proposalTextUI.SetActive(true);
    questProposalText.text = "Can u help these old bones?";
  }


  public void AcceptTheQuest() {
    QuestEvents.AcceptQuest(temp);
  }


  private void DeactivateAcceptUI(QuestGiver questGiver) {
    proposalTextUI.SetActive(false);
  }


  public void RejectQuest() {
    if (rejectCount == 0) {
      questProposalText.text = "But please?";
      rejectCount++;
    }
    else if (rejectCount == 1) {
      questProposalText.text = "Pretty please?";
      rejectCount++;
    }
    else if (rejectCount == 2) {
      questProposalText.text = "Do it ya bastard.";
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
}
