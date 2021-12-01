using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : NPC {
  public bool AssignedQuest { get; set; }

  [SerializeField]
  private GameObject quests;

  [SerializeField]
  private string questType;
  public Quest Quest { get; set; }

  private void Start() {
    QuestEvents.QuestAccepted += AssignQuest;
    UIEvents.QuestRemoved += DroppedQuest;
  }


  public override void Interact() {
    if (!AssignedQuest) {
      base.Interact();
      QuestEvents.ProposeQuest(this);
      //AssignQuest();
    }
    else if (AssignedQuest) {
      CheckQuest();
    }
  }


  private void AssignQuest(QuestGiver questGiver) {
    if (questGiver.tag == this.tag) {
      AssignedQuest = true;
      Quest = (Quest)quests.AddComponent(System.Type.GetType(questType));
    }
  }

  private void CheckQuest() {
    if (Quest.Completed) {
      Quest.GiveReward();
      AssignedQuest = false;
      //pass completed dialogue string here
    }
    else {
      Debug.Log("Finish your task, butthole.");
      //pass in progress dialogue string here
    }
  }


  private void DroppedQuest(Quest quest) {
    if (quest == this.Quest) {
      AssignedQuest = false;
      Quest = null;
    }
  }
}
