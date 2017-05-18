using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolbarBuildButtons : MonoBehaviour {

    /**
     *  Public Properties:
     *      BuildButtonPrefab   Prefab for the Menu buttons in the toolbar.
     *      ShipPartPrefabs     A list of Ship Part Prefabs
     */
    public GameObject BuildButtonPrefab;
    public GameObject[] ShipPartPrefabs;

    /// <summary>
    /// Use this for initialization
    /// </summary>
    void Start ()
    {
        // Find the mouse manager.
        MouseManager mouseManager = GameObject.FindObjectOfType<MouseManager>();

        // Populate the button list
        foreach(GameObject shipPart in ShipPartPrefabs)
        {
            GameObject buttonGameObject = (GameObject)Instantiate(BuildButtonPrefab, this.transform);
            Text buttonLabel = buttonGameObject.GetComponentInChildren<Text>();
            buttonLabel.text = shipPart.name;

            Button theButton = buttonGameObject.GetComponent<Button>();

            theButton.onClick.AddListener( () => { mouseManager.PrefabToSpawn = shipPart; } );
        }
		
	}
	
}
