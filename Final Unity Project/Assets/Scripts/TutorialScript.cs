using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialScript : MonoBehaviour
{
   // [SerializeField] private GameObject button;
    public GameObject popup;
    private bool on;
    // Start is called before the first frame update
    void Start()
    {
        //on = false;
        popup.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
       // if (!on)
      //  {
     //       popup.SetActive(false);
      //  }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
       // on = true;
        popup.SetActive(true);
    }

    void OnTriggerExit2D(Collider2D other)
    {
       // on = false;
        popup.SetActive(false);
    }
}
