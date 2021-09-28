using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuestInterface : MonoBehaviour
{
  public int killThis;
  public string nameThis;

  public abstract int killNeed();
  public abstract string questName();
}
