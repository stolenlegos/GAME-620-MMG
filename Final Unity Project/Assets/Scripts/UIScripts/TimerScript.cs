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
            timeStart = this.gameObject.transform.parent.GetComponent<AllowButtonPush>().timer;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.parent != null)
        {
            timeEnd = (/*this.gameObject.transform.parent.GetComponent<AllowButtonPush>().timer*/ timeStart -= (Time.deltaTime));
            buttonTime.text = ":" + (int) timeEnd;
            if(timeEnd <= 0f || !this.transform.parent.GetComponent<AllowButtonPush>().colored)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
