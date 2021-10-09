using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestEvents {
  public delegate void QuestHandler(QuestGiver questGiver);
  public static event QuestHandler QuestAccepted;
  public static event QuestHandler QuestProposed;
  public static event QuestHandler QuestRejected;

  public delegate void Reward (Quest quest);
  public static event Reward GrantReward;


  public static void AcceptQuest(QuestGiver questGiver) {
    if(QuestAccepted != null) {
      QuestAccepted(questGiver);
    }
  }


  public static void ProposeQuest(QuestGiver questGiver) {
    if (QuestProposed != null) {
      QuestProposed(questGiver);
    }
  }


  public static void RejectQuest(QuestGiver questGiver) {
    if (QuestRejected != null) {
      QuestRejected(questGiver);
    }
  }


  public static void QuestReward(Quest quest) {
    if (GrantReward != null) {
      GrantReward(quest);
    }
  }
}
