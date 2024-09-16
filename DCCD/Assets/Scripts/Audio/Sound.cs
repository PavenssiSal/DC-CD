using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Audio;    // Tarvitaan äänen kanssa

[System.Serializable] // Näkyy Inspectorissa
public class Sound
{
    // Ääni
    public AudioClip clip;

    // Äänen nimi
    public string name;

    // liukukytkimet
    [Range(0f, 1f)]
    public float volume; // voimakkuus ( 0 - 1)
    [Range (0.1f, 3f)]
    public float pitch; // sävelkorkeus (0.1 - 3)

    // Merkkilippu joka kertoo soitetaanko ääni loopissa
    public bool loop;

    // Äänen lähde, joka piilotetaan
    [HideInInspector]
    public AudioSource source; // AudioManager -luokka käyttää tätä muuttujaa

}   // Sound.cs päättyy
