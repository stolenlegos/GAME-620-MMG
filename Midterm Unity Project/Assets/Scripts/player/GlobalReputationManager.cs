using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalReputationManager : MonoBehaviour {
  public int realReputation;
  public int perceivedReputation;

    private void Start() {
      QuestEvents.GrantReward += IncreaseRep;
      QuestEvents.QuestRejected += DecreaseRepByRejecting;
      UIEvents.QuestRemoved += DecreaseRepByDropping;
      realReputation = 0;
      perceivedReputation = 0;
    }


    private void DecreaseRepByDropping(Quest quest) {
      perceivedReputation -= 1;
      realReputation -= 1;
    }


    private void DecreaseRepByRejecting (QuestGiver questGiver) {
      perceivedReputation -= 1;
    }


    private void IncreaseRep(Quest quest) {
      realReputation += 2;
      perceivedReputation += 1;
    }
}
