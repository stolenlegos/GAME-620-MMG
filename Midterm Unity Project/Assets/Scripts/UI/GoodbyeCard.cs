using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoodbyeCard : MonoBehaviour
{
    public GameObject qManager;
    public GameObject MQ;
    public GlobalReputationManager RepManager;
    public Text Goodbye;
    // Start is called before the first frame update
    void Start()
    {
        Goodbye.text = "Don’t be too hard on yourself. \n You've done better than you thought; You were selling yourself short all along.";
        Goodbye.text +=  "\n Your real reputation was " + RepManager.realReputation + "\n Thank you for playing our game demo!";
       transform.Find("EndDialogue").gameObject.SetActive(false); 
    }

    // Update is called once per frame
    void Update()
    {   
        if (qManager.GetComponent<MQDManager>().GetLine() == 5 && MQ.GetComponent<MainQuest>().GetStage() == 6 ) {
           // Goodbye.text = "Don’t be too hard on yourself. \n You've done better than you thought you did; You've been selling yourself short all along.";
           // Goodbye.text +=  "Your real reputation was " + RepManager.realReputation + "\n Thank you for playing our game demo!";


            if (Input.GetMouseButtonUp(0)){
                transform.Find("EndDialogue").gameObject.SetActive(true);
            }
        
      }
    }
}
