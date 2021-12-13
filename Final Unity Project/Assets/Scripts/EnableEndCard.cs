using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableEndCard : MonoBehaviour
{
    public GameObject endCard; 
    private void OnEnable() { 
        endCard.SetActive(true);
        
    }
}
