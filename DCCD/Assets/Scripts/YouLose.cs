using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class YouLose : MonoBehaviour
{
    //H‰viˆpaneeli
    public GameObject GameEndScreen;
    public GameObject UI;
    public GameObject OpenShop;

    //Pause
    public static bool gameIsPaused;

    // Update is called once per frame
    void Update()
    {
        //ESCill‰ p‰‰see p‰‰valikkoon
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }
        //ENTERill‰ ladataan peli alkamaan uudestaan
        if (Input.GetKeyDown(KeyCode.Return))
        {
            //Peli k‰ynnistyy
            Time.timeScale = 1.0f;
            gameIsPaused = false;
            PlayerController.enemyKillCount = 0;
            //Aloitetaan uusi peli
            SceneManager.LoadScene("Game");
        }
    }
    public void TriggerGameOver()
    {
        GameEndScreen.SetActive(true);
        UI.SetActive(false);
        OpenShop.SetActive(false);
        gameIsPaused = true;
        PauseGame();
    }

    /// <summary>
    /// T‰m‰ metodi pys‰ytt‰‰ pelin ja k‰ynnist‰‰ pelin tarvittaessa.
    /// </summary>
    void PauseGame()
    {
        if (gameIsPaused)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
}
