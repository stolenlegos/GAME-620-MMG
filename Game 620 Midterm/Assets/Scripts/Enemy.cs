using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour {
  public EnemyID ID;
  public int xp;
  protected int health;
  protected bool inPlayerSpace;

  protected abstract void Die();
  protected abstract void Attack();
  protected abstract void TakeDamage (PlayerCombat player);
}


public enum EnemyID {
  bananas,
  apples,
  lottoWinner,
  bear
}
