using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour {
  public EnemyID ID;
  public int xp;
  protected int health;
  [SerializeField]
  protected bool inPlayerSpace;

  protected abstract void Die();
  protected abstract void Attack();
  protected abstract void TakeDamage (PlayerCombat player);
}


public enum EnemyID {
  Evil_Beings,
  Things_that_will_attack_you,
  Lotto_Winners,
  Bears
}
