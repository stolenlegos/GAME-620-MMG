using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerLevelUpdate : MonoBehaviour
{

    // Update is called once per frame
    public Text text; 
    public GameObject player; 
    void State() {
        text = this.gameObject.GetComponent<Text>(); 
    }
    void Update()
    {
        text.text = ("Player Level: " + player.GetComponent<PlayerCombat>().damage/2);
    }
}
