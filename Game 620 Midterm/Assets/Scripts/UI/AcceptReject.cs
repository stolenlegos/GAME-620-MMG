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
}
