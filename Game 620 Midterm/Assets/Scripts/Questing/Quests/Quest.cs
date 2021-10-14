using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Quest : MonoBehaviour {
  protected List<Goal> Goals { get; set; } = new List<Goal>();
  public string QuestName { get; set; }
  public string Description { get; set; }
  public bool Completed { get; set; }
  public bool MainQuest { get; set; }
  public string QuestType { get; set; }


  public void CheckGoals() {
    Completed = Goals.All(g => g.Completed);
  }


  public void GiveReward(Quest quest) {
    QuestEvents.QuestReward(quest);
  }


  protected void AddToList(Quest quest) {
    UIEvents.QuestCreated(quest);
  }
}
