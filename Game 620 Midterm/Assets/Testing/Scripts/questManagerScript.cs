using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class questManagerScript : MonoBehaviour
{
    public List<giveQuest> questList = new List<giveQuest>();
    public Text questLogText;
    public GameObject questLogObj;
    private bool logOpen;

    void Start()
    {
      logOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
      if (Input.GetKeyDown(KeyCode.E) && !logOpen) {
        logOpen = true;
        questLogObj.SetActive(true);
      } else if (Input.GetKeyDown(KeyCode.E) && logOpen) {
        logOpen = false;
        questLogObj.SetActive(false);
      }
      questLogText.text = ListToText(questList);

      /*foreach (var x in questList) {
        Debug.Log(x.nameThis);
        Debug.Log(x.killThis);
      }*/
    }

    public string ListToText(List<giveQuest> list) {
      string result = "";
      foreach(var listMember in list) {
        result += listMember.nameThis + "\n";
      }

      return result;
    }
}
