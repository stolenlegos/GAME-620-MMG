using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions {
public delegate void PlayerAction (GameObject obj);
public static event PlayerAction Grab;
public static event PlayerAction Release;

  public static void ObjectGrab(GameObject obj) {
    if (Grab != null) {
      Debug.Log(obj);
      Grab(obj);
    }
  }
//butts

  public static void ObjectRelease(GameObject obj) {
    if (Release != null) {
      Debug.Log(obj);
      Release(obj);
    }
  }
}
