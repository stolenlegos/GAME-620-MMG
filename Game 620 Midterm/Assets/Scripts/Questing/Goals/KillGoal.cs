using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillGoal : Goal {
//goal specific variables not shared by another type of goal.
  EnemyID EnemyID { get; set; }


  //constructor for a kill goal. all variables are passed from a newly
  //instantiated kill quest.
  public KillGoal(Quest quest, EnemyID enemyID, string description, bool completed, int currentAmount, int requiredAmount) {
    this.Quest = quest;
    this.EnemyID = enemyID;
    this.Description = description;
    this.Completed = completed;
    this.CurrentAmount = currentAmount;
    this.RequiredAmount = requiredAmount;
  }


  //goal specific Initialise function. good for adding functions to delegates.
  public override void Initialise() {
    base.Initialise();
    CombatEvents.OnEnemyDeath += EnemyDied;
  }


  //Function called when enemy dies, check to see if it is the enemy type assiged
  //to the goal. if so increases the progress of the goal.
  private void EnemyDied(Enemy enemy) {
    if (enemy.ID == this.EnemyID) {
      this.CurrentAmount++;
      Evaluate();
    }
  }
}
