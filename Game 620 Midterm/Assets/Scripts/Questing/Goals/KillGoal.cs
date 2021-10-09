using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillGoal : Goal {
  EnemyID EnemyID { get; set; }


  public KillGoal(Quest quest, EnemyID enemyID, string description, bool completed, int currentAmount, int requiredAmount) {
    this.Quest = quest;
    this.EnemyID = enemyID;
    this.Description = description;
    this.Completed = completed;
    this.CurrentAmount = currentAmount;
    this.RequiredAmount = requiredAmount;
  }


  public override void Initialise() {
    base.Initialise();
    CombatEvents.OnEnemyDeath += EnemyDied;
  }


  private void EnemyDied(Enemy enemy) {
    if (enemy.ID == this.EnemyID) {
      this.CurrentAmount++;
      Evaluate();
    }
  }
}
