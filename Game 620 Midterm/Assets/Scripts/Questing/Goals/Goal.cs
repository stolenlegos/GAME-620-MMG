using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal {
  protected Quest Quest { get; set; }
  protected string Description { get; set; }
  protected int CurrentAmount { get; set; }
  protected int RequiredAmount { get; set; }
  public bool Completed { get; set; }


  public virtual void Initialise (){
    //default init stuff
  }


  protected void Evaluate() {
    if (CurrentAmount >= RequiredAmount) {
      Complete();
    }
  }


  protected void Complete () {
    Completed = true;
    Quest.CheckGoals();
  }
}
