// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI; 

// public class PlayerEffects : MonoBehaviour, Observer
// {
//   public int health;
//   private bool noDam;
//   private float damTimer;
//   public Sprite deathSprite; 

//   void Start() {
//     health = 100;
//     noDam = false;
//     damTimer = 5f;

//     foreach (SubjectBeingObserved subject in FindObjectsOfType<SubjectBeingObserved>()) {
//         subject.AddObserver(this);
//     }
//   }


//   void Update() {
//     InvincibilityCheck();
//     KillPlayer();
//   }


//   public void OnNotify (Object obj, NotificationType noTy) {
//     if (noTy == NotificationType.healthPotion) {
//       health += 10;
//     } else if (noTy == NotificationType.star) {
//       noDam = true;
//     } else if (noTy == NotificationType.takeDamage) {
//       TakeDamage();
//     }
//   }


//   private void TakeDamage(){
//     health -= 15;
//   }


//   private void InvincibilityCheck() {
//     if (noDam) {
//       damTimer -= Time.deltaTime;
//     } else if (!noDam) {
//       damTimer = 5f;
//     }

//     if (damTimer < 0) {
//       noDam = false;
//     }
//   }


//   private void KillPlayer() {
//     if (health <= 0) {
//       //Destroy(gameObject);
//       GetComponent<SpriteRenderer>().sprite = deathSprite;
//       //Time.timeScale = 0;
//     }
//   }
// }
