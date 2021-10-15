using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatObject : EnemyObject {

  private void Start() {
    CombatEvents.PlayerDealDamage += TakeDamage;
    this.inPlayerSpace = false;
  }


  private void Update() {
    if (enemyData.health <= 0) {
      Die();
    }
  }


  protected override void Die() {
    CombatEvents.EnemyDied(this);
    Destroy(gameObject);
  }


  protected override void Attack() {
    //do something
  }


  protected override void Wander() {
    //wander
  }


  protected override void TakeDamage (PlayerCombat player){
    if (this.inPlayerSpace) {
      enemyData.health -= player.damage;
    }
  }


  private void OnTriggerEnter2D (Collider2D other) {
    if (other.tag == "PlayerHitBox") {
      inPlayerSpace = true;
    }
  }


  private void OnTriggerExit2D (Collider2D other) {
    if (other.tag == "PlayerHitBox") {
      inPlayerSpace = false;
    }
  }
}
