using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : NPC {
  public NpcID id;
  private int localReputation;
  public bool AssignedQuest { get; set; }

  [SerializeField]
  private GameObject quests;

  [SerializeField]
  private string questType;
  public Quest Quest { get; set; }


//assigns functions to delegates
  private void Start() {
    QuestEvents.QuestAccepted += AssignQuest;
    UIEvents.QuestRemoved += DroppedQuest;
    localReputation = 0;
  }


//checks if player has quest from this NPC. Pass relevant non quest dialogue here.
  public override void Interact() {
    if (!AssignedQuest) {
      base.Interact();
      QuestEvents.ProposeQuest(this);
    }
    else if (AssignedQuest) {
      CheckQuest();
    }
  }


//pulls up the quest accept UI screen
  private void AssignQuest(QuestGiver questGiver) {
    if (questGiver.tag == this.tag) {
      AssignedQuest = true;
      Quest = (Quest)quests.AddComponent(System.Type.GetType(questType));
      //pass world slower mechanic here
    }
  }


//checks if quest goals have all be completed by checking quest completion bool
//and decides appropriate dialogue options.
  private void CheckQuest() {
    if (Quest.Completed) {
      Quest.GiveReward(Quest);
      AssignedQuest = false;
      localReputation += 1;
      //pass quest completed dialogue string here
    }
    else {
      Debug.Log("Finish your task, please.");
      //pass quest in progress dialogue string here
    }
  }


//changes variables if player drops the quest assigned by this person.
//do not pass dialogue here.
  private void DroppedQuest(Quest quest) {
    if (quest == this.Quest) {
      AssignedQuest = false;
      Quest = null;
      localReputation -= 1;
      //pass world quicker mechanic here
    }
  }
}
