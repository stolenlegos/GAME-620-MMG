using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatEvents {
  public delegate void EnemyEventHandler(Enemy enemy);
  public static event EnemyEventHandler OnEnemyDeath;

  public delegate void PlayerEventHandler(PlayerCombat player);
  public static event PlayerEventHandler PlayerDealDamage;


  public static void EnemyDied(Enemy enemy) {
    if (OnEnemyDeath != null) {
      OnEnemyDeath(enemy);
    }
  }


  public static void PlayerAttack(PlayerCombat player) {
    if (PlayerDealDamage != null) {
      PlayerDealDamage(player);
    }
  }
}
