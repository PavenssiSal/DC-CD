using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // K‰ynnistett‰v‰n Scenen nimi (Eka taso)
    public string Game;
    public string Credits;
    public string Alkuruutu;
    public void NewGame()
    {
        // Aloitetaan uusi peli
        SceneManager.LoadScene(Game);
    }

    public void Creditit()
    {
        // Menn‰‰n creditseihin
        SceneManager.LoadScene(Credits);
    }
    public void Mainmenu()
    {
        // Menn‰‰n creditseihin
        SceneManager.LoadScene(Alkuruutu);
    }

    public void QuitGame()
    {
        //Lopettaa pelin
        Application.Quit();
    }
}
