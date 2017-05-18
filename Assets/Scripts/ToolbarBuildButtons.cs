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
    public GameObject[] MovementPrefabs;
    public GameObject[] StructurePrefabs;
    public string[] MenuNames;

    /// <summary>
    /// Use this for initialization
    /// </summary>
    void Start ()
    {
        // Find the mouse manager.
        MouseManager mouseManager = GameObject.FindObjectOfType<MouseManager>();

        // Populate the button list
        foreach(string name in MenuNames)
        {
            GameObject buttonGameObject = (GameObject)Instantiate(BuildButtonPrefab, this.transform);
            Text buttonLabel = buttonGameObject.GetComponentInChildren<Text>();
            buttonLabel.text = name;

            Button theButton = buttonGameObject.GetComponent<Button>();

            // This listener will have to go on the sub menus of this.
            //theButton.onClick.AddListener( () => { mouseManager.PrefabToSpawn = shipPart; } );

            theButton.onClick.AddListener( () => { this.OpenMenu(name); } );
        }

    }

    void OpenMenu(string menuName)
    {
        GameObject[] contents;
        switch (menuName)
        {
            case "Structure":
                contents = StructurePrefabs;
                break;
            case "Movement":
                contents = MovementPrefabs;
                break;
        }
    }
	
}
