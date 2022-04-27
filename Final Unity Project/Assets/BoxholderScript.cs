using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxholderScript : MonoBehaviour
{
    public bool doNotPickUp;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Terrain")
        {
            doNotPickUp = true;
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Terrain")
        {
            doNotPickUp = false;
        }
    }
}
