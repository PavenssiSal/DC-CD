using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warp : MonoBehaviour
{
    // Kartta, jonne siirryt‰‰n
    public GameObject targetMap;
    //Kohde, joka siirtyy. Eli pelaaja
    public GameObject target;

    private void Awake()
    {
        //Piilotetaan Warp GameObjekti
        GetComponent<SpriteRenderer>().enabled = false;
        //Piilotetaan Exit GameObjekti
        transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Pelaaja siirtyy Exit-pisteeseen
            collision.transform.position = target.transform.GetChild(0).transform.position;
            //Siirt‰‰ kameran
            Camera.main.GetComponent<MainCamera>().SetBound(targetMap);
        }
    }
}
