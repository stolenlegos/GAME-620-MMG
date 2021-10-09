using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal {
  //generic goal class. not meant to be instantiated. derive specific goals from
  //this class
  protected Quest Quest { get; set; }
  protected string Description { get; set; }
  protected int CurrentAmount { get; set; }
  protected int RequiredAmount { get; set; }
  public bool Completed { get; set; }


//generic initialise function if the same thing needs to happen for each goal.
//probably no need for it to be used but here if we need it.
  public virtual void Initialise (){

  }


//evaluates if the specific goal is completed, not the quest as a whole.
  protected void Evaluate() {
    if (CurrentAmount >= RequiredAmount) {
      Complete();
    }
  }


//marks if this specific goal is completed not the quest a whole.
  protected void Complete () {
    Completed = true;
    Quest.CheckGoals();
  }
}
