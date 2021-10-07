using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bear : Enemy {


   private void Start() {
     this.ID = EnemyID.bear;
     this.xp = 200;
     this.health = 100;
     this.inPlayerSpace = false;
     CombatEvents.PlayerDealDamage += TakeDamage;
  }


    private void Update() {
      if (this.health <= 0) {
        Die();
      }

      Debug.Log(this.health);
    }


    protected override void TakeDamage(PlayerCombat player) {
      if (this.inPlayerSpace) {
        this.health -= player.damage;
      }
    }


    protected override void Die() {
      CombatEvents.EnemyDied(this);
      Destroy(gameObject);
    }


    protected override void Attack () {
      //do something
    }


    private void OnTriggerEnter2D (Collider2D other) {
      if (other.tag == "Player") {
        this.inPlayerSpace = true;
      }
    }


    private void OnTriggerExit2D (Collider2D other) {
      if (other.tag == "Player") {
        this.inPlayerSpace = false;
      }
    }
}
