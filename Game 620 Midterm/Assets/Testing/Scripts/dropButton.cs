using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dropButton : MonoBehaviour
{
  public questManagerScript qManage;
  public int buttonNumber;

    public void DropQuest() {
      qManage.questList.RemoveAt(buttonNumber);
    }
}
