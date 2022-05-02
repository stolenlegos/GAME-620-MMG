using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerScript : MonoBehaviour
{
    public TMP_Text buttonTime;
    float timeStart;
    float timeEnd;
    //float timer;
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.transform.position += Vector3.up * 1f;
        if (this.transform.parent != null)
        {
            if (this.gameObject.transform.parent.childCount <= 3)
            {
                timeStart = this.gameObject.transform.parent.GetComponent<AllowButtonPush>().timer;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.parent != null)
        {
            timeEnd = (/*this.gameObject.transform.parent.GetComponent<AllowButtonPush>().timer*/ timeStart -= (Time.deltaTime));
            if (timeEnd <= 10)
            {
                buttonTime.text = ":0" + (int)timeEnd;
            }
            else { buttonTime.text = ":" + (int)timeEnd; }
            if(timeEnd <= 0f || !this.transform.parent.GetComponent<AllowButtonPush>().colored)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
