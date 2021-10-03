using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class rejectButton : MonoBehaviour
{
    public int rejectCount = 0;
    public Text text;

    public void Reject () {
      if (rejectCount == 0) {
        text.text = "But please?";
        rejectCount += 1;
      }

      else if (rejectCount == 1) {
        text.text = "Pretty please?";
        rejectCount += 1;
      }

      else if (rejectCount == 2) {
        text.text = "PLEASE";
        rejectCount += 1;
      }

      else if (rejectCount == 3) {
        rejectCount = 0;
        transform.parent.gameObject.SetActive(false);
      }
    }
}
