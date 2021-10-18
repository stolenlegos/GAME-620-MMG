using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReputationBar : MonoBehaviour
{
    public Slider posSlider;
    public Slider negSlider;
    public Text display;
    public GlobalReputationManager repManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        int rep = repManager.perceivedReputation;
         if(rep>0){
            posSlider.value = rep;
         }
         if (rep<=0){
             negSlider.value = rep*-1;
         }
         
         display.text = "Reputation: " + rep;
         
         
    }
}
