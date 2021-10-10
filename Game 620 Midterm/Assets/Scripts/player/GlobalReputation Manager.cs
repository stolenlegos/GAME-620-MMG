using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalReputationManager : MonoBehaviour {
  public int globalReputation;

    private void Start() {
      QuestEvents.GrantReward += IncreaseRep;
      globalReputation = 0;
    }


    private void DecreaseRep(Quest quest) {

    }


    private void IncreaseRep(Quest quest) {
      globalReputation++;
    }
}
