using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
  public int damage = 12;
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
    }
}
