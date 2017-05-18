using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneFromButton : MonoBehaviour {

	public void LoadSceneFromIndex(bool preMadeShip)
    {
        if (preMadeShip) PlayerPrefs.SetInt("shipIndex", 1);
        else PlayerPrefs.SetInt("shipIndex", 0);

        SceneManager.LoadScene(1);
    }
}
