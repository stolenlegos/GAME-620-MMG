using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
  public questManagerScript qMan;
  private giveQuest quest;
  public Text text;

    // Start is called before the first frame update
    public void giveThisQuest() {
      quest = new giveQuest();
      quest.doThis();
      qMan.questList.Add(quest);
      transform.parent.gameObject.SetActive(false);
    }

    void Awake() {
      text.text = "Hello can you help?";
    }
}
