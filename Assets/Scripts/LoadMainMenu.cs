using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMainMenu : MonoBehaviour
{
    public void NavigateToMainMenu()
    {
        PlayerPrefs.SetInt("shipIndex", -1);

        SceneManager.LoadScene(0);
    }
	
}
