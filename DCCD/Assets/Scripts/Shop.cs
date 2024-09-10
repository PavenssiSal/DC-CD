using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public PlayerController player;
    //Kauppa
    public GameObject shop;
    public GameObject HahBroke;
    public GameObject OpenShop;

    private bool ShopOpened = false;
    private bool Broke = false;

    // Update is called once per frame
    void Update()
    {
        //B avaa kaupan, jos se ei ole jo auki
        if (Input.GetKeyDown(KeyCode.B) && ShopOpened == false)
        {
            OpenShop.SetActive(false);
            shop.SetActive(true);
            ShopOpened = true;
        }
        //B sulkee kaupan, jos se on auki
        else if (Input.GetKeyDown(KeyCode.B) && ShopOpened == true)
        {
            OpenShop.SetActive(true);
            shop.SetActive(false);
            ShopOpened = false;
            if (Broke == true) 
            {
                HahBroke.SetActive(false);
                Broke = false;
            }
        }
        if (ShopOpened)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) && player.money > 0)
            {
                player.money--;
                player.damage++;
                Debug.Log("Bought sword");
            }

            if (Input.GetKeyDown(KeyCode.Alpha2) && player.money > 0 && player.currentHealth < player.MaxHealth)
            {
                player.money--;
                player.currentHealth++;
                Debug.Log("Bought potion");
            }

            if (Input.GetKeyDown(KeyCode.Alpha3) && player.money > 0)
            {
                player.money--;
                player.MaxHealth++;
                Debug.Log("Bought heart");
            }
            //Broke
            if (Input.GetKey(KeyCode.Alpha1) && player.money == 0 || Input.GetKey(KeyCode.Alpha2) && player.money == 0 || Input.GetKey(KeyCode.Alpha3) && player.money == 0)
            {
                HahBroke.SetActive(true);
                Broke = true;
            }
        }
    }

}

