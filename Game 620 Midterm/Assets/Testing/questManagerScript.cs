using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class questManagerScript : MonoBehaviour
{
    public List<QuestInterface> questList = new List<QuestInterface>();
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
      foreach (var x in questList) {
        Debug.Log(x.nameThis);
        Debug.Log(x.killThis);
      }
    }
}
