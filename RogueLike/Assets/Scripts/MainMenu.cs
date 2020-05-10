using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public void PlayGame(){
        print("Loading Game");
        SceneManager.LoadScene("Rogue Like");
    }

    

    public void Quit(){
        Application.Quit();
    }
}
