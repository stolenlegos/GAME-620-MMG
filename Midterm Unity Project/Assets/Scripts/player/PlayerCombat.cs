using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
  [SerializeField]
  private Transform respawnPoint;

  public int health = 200;
  public int damage;
  private float timeBtwAttack;
  private float startTimeBtwAttack;
  public Animator anim;
  public Transform sword;


    void Update()
    {
      if (timeBtwAttack <= 0) {
        if (Input.GetKeyDown(KeyCode.Space)) {
          CombatEvents.PlayerAttack(this);
          timeBtwAttack = startTimeBtwAttack;
          AttackAnim();
          anim.Play("swordSwing");
          //anim.Play("idle");
        }
      } else{
        timeBtwAttack -= Time.deltaTime;
      }

      if (health <= 0) {
        Die();
      }
    }

    void AttackAnim(){
      if (Input.GetKey(KeyCode.W)) {
        sword.transform.rotation = Quaternion.Euler(0, 0, 0);
      }
      else if (Input.GetKey(KeyCode.A)) {
        sword.transform.rotation = Quaternion.Euler(0, 0, 90);
      }
      else if (Input.GetKey(KeyCode.D)) {
        sword.transform.rotation = Quaternion.Euler(0, 0, -90);
      }
      else if (Input.GetKey(KeyCode.S)) {
        sword.transform.rotation = Quaternion.Euler(0, 0, 180);
      }
      else {
        sword.transform.rotation = Quaternion.Euler(0, 0, 180);
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
