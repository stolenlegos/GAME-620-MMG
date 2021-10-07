using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEvents : MonoBehaviour {
  [SerializeField]
  private Text questProposalText;
  [SerializeField]
  private GameObject proposalTextUI;
  private QuestGiver temp;


  private void Start() {
    QuestEvents.QuestProposed += AcceptUI;
    QuestEvents.QuestAccepted += DeactivateAcceptUI;
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
}
