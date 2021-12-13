using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToEndScreen : MonoBehaviour
{
    [SerializeField]
    private float timer; 
    public GameObject endcard; 
    private bool fadeCalled;
    private float counter;
    private CanvasGroup canvGroup;   
    
    private void OnEnable() {
        canvGroup = GetComponent<CanvasGroup>();
        counter = 0; 
        timer = 10.0f; 
        fadeCalled = false; 
    }
    //public static event Action FadeEndUI; 
    private void Update() { 
        if (timer > 0 ) {
            timer -= Time.deltaTime; 
        } else {
            if (counter <= 1) {
                counter += Time.deltaTime;
                canvGroup.alpha = counter; 
            }
        }
    }
}
