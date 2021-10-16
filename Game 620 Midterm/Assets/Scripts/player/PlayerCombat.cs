using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
  [SerializeField]
  private Transform respawnPoint;
  public int health = 200;
  public int damage = 6;
  private float timeBtwAttack;
  private float startTimeBtwAttack;


    void Update()
    {
      if (timeBtwAttack <= 0) {
        if (Input.GetKeyDown(KeyCode.Space)) {
          CombatEvents.PlayerAttack(this);
          timeBtwAttack = startTimeBtwAttack;
        }
      } else{
        timeBtwAttack -= Time.deltaTime;
      }

      if (health <= 0) {
        Die();
      }
    }


    private void Die() {
      gameObject.transform.position = respawnPoint.position;
      health = 200;
    }


    private void OnCollisionEnter2D (Collision2D other) {
      if (other.gameObject.tag == "Enemy") {
        health -= 10;
      }
    }
}
