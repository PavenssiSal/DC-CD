using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public GameObject quest1;
    public GameObject quest2;
    public GameObject quest3;
    public GameObject quest4;

    public PlayerController player;

    public void Start()
    {
        quest1.SetActive(false);
        quest2.SetActive(false);
        quest3.SetActive(false);
        quest4.SetActive(false);
    }
    public void Update()
    {
        if (player.currentMap.tag != "Forest_1" && PlayerController.enemyKillCount < 9)
        {
            quest1.SetActive(true);
        }
        else
        {
            quest1.SetActive(false);
        }
        if (player.currentMap.tag == "Forest_1" && PlayerController.enemyKillCount <= 7)
        {
            quest2.SetActive(true);
        }
        else
        {
            quest2.SetActive(false);
        }
        if (player.currentMap.tag == "Forest_1" && PlayerController.enemyKillCount == 8)
        {
            quest3.SetActive(true);
        }
        else
        {
            quest3.SetActive(false);
        }
        if (PlayerController.enemyKillCount == 9)
        {
            quest4.SetActive(true);
        }
        else
        {
            quest4.SetActive(false);
        }
    }
}
