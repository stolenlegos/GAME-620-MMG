using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEvents {
  public delegate void QuestUIHandler (Quest quest);
  public static event QuestUIHandler QuestListAdd;
  public static event QuestUIHandler QuestRemoved;

  public delegate void RandomTextEvents (string dialogue);
  public static event RandomTextEvents QuestInProgress;
  public static event RandomTextEvents QuestFinished;


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

  public static void QuestStillInProgress(string dialogue) {
    if (QuestInProgress != null) {
      QuestInProgress(dialogue);
    }
  }

  public static void QuestHasFinished(string dialogue) {
    if (QuestFinished != null) {
      QuestFinished(dialogue);
    }
  }
}
