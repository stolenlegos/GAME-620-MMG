using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyObject : MonoBehaviour {
  protected bool inPlayerSpace;
  public Enemy enemyData;

  protected abstract void Die();
  protected abstract void Attack();
  protected abstract void Wander();
  protected abstract void TakeDamage(PlayerCombat player);
}
