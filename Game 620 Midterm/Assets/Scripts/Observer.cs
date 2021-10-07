using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Observer {
  void OnNotify(GameObject obj, NotificationType noTy);
}


public enum NotificationType {
  TakeDamage
}
