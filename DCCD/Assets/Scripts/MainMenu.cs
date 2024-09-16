using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Käynnistettävän Scenen nimi (Eka taso)
    public string Game;
    public string Credits;
    public void NewGame()
    {
        // Aloitetaan uusi peli
        SceneManager.LoadScene(Game);
    }

    public void Creditit()
    {
        // Mennään creditseihin
        SceneManager.LoadScene(Credits);
    }

    public void QuitGame()
    {
        //Lopettaa pelin
        Application.Quit();
    }
}
