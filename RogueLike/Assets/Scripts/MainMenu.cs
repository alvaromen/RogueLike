using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{

    void Awake(){
        gameObject.SetActive(true);
        GameObject.Find("Quick Guide").SetActive(false);
    }

    public void PlayGame(){
        print("Loading Game");
        SceneManager.LoadScene("Rogue Like");
    }

    public void Quit(){
        Application.Quit();
    }
}
