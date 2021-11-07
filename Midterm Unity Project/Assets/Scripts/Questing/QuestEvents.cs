using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestEvents {
  public delegate void QuestPropose (QuestGiver questGiver, string[] dialogue);
  public static event QuestPropose QuestProposed;

  public delegate void QuestARD(QuestGiver questGiver);
  public static event QuestARD QuestAccepted;
  public static event QuestARD QuestRejected;

  public delegate void DeliveryCompleted(DeliveryRecipient recipient);
  public static event DeliveryCompleted CompleteDelivery;

  public delegate void Reward (Quest quest);
  public static event Reward GrantReward;

  public delegate void Spawn (FetchQuest quest);
  public static event Spawn SpawnFetchItem;

  public delegate void Item (ItemID id);
  public static event Item ItemPickedUp;


//when player accepts the quest from UI
  public static void AcceptQuest(QuestGiver questGiver) {
    if(QuestAccepted != null) {
      QuestAccepted(questGiver);
    }
  }


//when a quest giver NPC asks the player to do a quest
  public static void ProposeQuest(QuestGiver questGiver, string[] dialogue) {
    if (QuestProposed != null) {
      QuestProposed(questGiver, dialogue);
    }
  }


//when a player rejects the quest from UI
  public static void RejectQuest(QuestGiver questGiver) {
    if (QuestRejected != null) {
      QuestRejected(questGiver);
    }
  }


  public static void DeliveryGoalCompleted(DeliveryRecipient recipient) {
    if (CompleteDelivery != null) {
      CompleteDelivery(recipient);
    }
  }


//when a player turns in a quest
  public static void QuestReward(Quest quest) {
    if (GrantReward != null) {
      GrantReward(quest);
    }
  }


  public static void SpawnItem(FetchQuest quest) {
    if (SpawnFetchItem != null) {
      SpawnFetchItem(quest);
    }
  }


  public static void PickUpItem(ItemID id) {
    if (ItemPickedUp != null) {
      ItemPickedUp(id);
    }
  }
}
