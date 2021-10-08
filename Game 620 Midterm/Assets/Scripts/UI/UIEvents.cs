using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEvents {
  public delegate void QuestUIHandler (Quest quest);
  public static event QuestUIHandler QuestListAdd;
  public static event QuestUIHandler QuestRemoved;


  public static void QuestCreated(Quest quest) {
    if (QuestListAdd != null) {
      QuestListAdd(quest);
    }
  }


  public static void QuestListRemove (Quest quest) {
    if (QuestRemoved != null) {
      QuestRemoved(quest);
    }
  }
}
