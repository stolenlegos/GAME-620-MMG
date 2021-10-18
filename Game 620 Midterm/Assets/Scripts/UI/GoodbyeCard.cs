using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoodbyeCard : MonoBehaviour
{
    public GameObject qManager;
    public GameObject MQ;
    // Start is called before the first frame update
    void Start()
    {
       gameObject.SetActive(false); 
    }

    // Update is called once per frame
    void Update()
    {
      if (MQ.GetComponent<MainQuest>().GetStage() == 5 && qManager.GetComponent<MQDManager>().GetLine() == 6) {
        gameObject.SetActive(true);
      }
    }
}
