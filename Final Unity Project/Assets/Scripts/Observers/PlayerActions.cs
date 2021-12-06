using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions {
public delegate void PlayerAction (GameObject obj);
public static event PlayerAction Grab;
public static event PlayerAction Release;
public static event PlayerAction Drop;

  public static void ObjectGrab(GameObject obj) {
    if (Grab != null) {
      Debug.Log(obj);
      Grab(obj);
    }
  }

  public static void ObjectRelease(GameObject obj) {
    if (Release != null) {
      Debug.Log(obj);
      Release(obj);
    }
  }
  public static void ObjectDropped(GameObject obj)
    {
        if (Drop != null)
        {
            Debug.Log(obj);
            Drop(obj);
        }
    }
}
