using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolbarBuildButtons : MonoBehaviour
{
    /**
     *  Private Properties:
     *      _OpenMenu       An int to indicate which menu is currently open.
     *                      It relates to the MenuNames index, -1 is none open.
     */
    int _OpenMenu = -1;
    MouseManager _MM;
    Canvas _Canvas;
    RectTransform _SubMenuArea;

    /**
     *  Public Properties:
     *      BuildButtonPrefab   Prefab for the Menu buttons in the toolbar.
     *      ShipPartPrefabs     A list of Ship Part Prefabs
     */
    public GameObject BuildButtonPrefab;
    public GameObject SubMenuButtonPrefab;
    public Text CurrentPartText;
    public GameObject[] ShipPartPrefabs;
    public GameObject[] MovementPrefabs;
    public GameObject[] StructurePrefabs;
    public string[] MenuNames;
    


    /// <summary>
    /// Opens the sub menu for a toolbar button.
    /// </summary>
    /// <param name="menuName"></param>
    /// <param name="theButton"></param>
    void OpenMenu(Button theButton)
    {
        GameObject[] contents = null;
        /** To get the 4 corners for the clicked button I use the 
         * RectTransform.GetWorldCorners(Vector3[] fourCornersArray).
         * World corners are clockwise starting at Bottom Left.
         * worldCorners[0] == Bottom Left
         * worldCorners[1] == Top Left
         * worldCorners[2] == Top Right
         * worldCorners[3] == Bottom Right
         */
        RectTransform btnTrans = (RectTransform)theButton.transform;
        Vector3[] buttonCorners = new Vector3[4];
        btnTrans.GetWorldCorners(buttonCorners);
        // Find corners of the sub menu area
        Vector3[] subMenuAreaCorners = new Vector3[4];
        _SubMenuArea.GetWorldCorners(subMenuAreaCorners);

        string menuName = theButton.name;

        Debug.Log("ToolbarBuildButtons::OpenMenu ->>> Button Coords: " + buttonCorners[1]);

        switch (menuName)
        {
            case "Structure":
                contents = StructurePrefabs;
                break;
            case "Movement":
                contents = MovementPrefabs;
                break;
        }

        if (contents == null)
        {
            // No sub-menu contents
            Debug.LogError("ToolbarBuildButtons::OpenMenu ->>> contents == null");
            return;
        }

        Text txt = CurrentPartText.GetComponent<Text>();
        float btnHeight = buttonCorners[1].y;

        for (int i = 0; i < contents.Length; i++)
        {
            GameObject buttonGameObject = Instantiate(SubMenuButtonPrefab, Vector3.zero, Quaternion.identity, _SubMenuArea);
            // Set the names and labels for the button.
            buttonGameObject.name = contents[i].name;
            Text buttonLabel = buttonGameObject.GetComponentInChildren<Text>();
            buttonLabel.text = contents[i].name;

            RectTransform btnRect = (RectTransform)buttonGameObject.transform;
            // Set the anchors of the button to the top left of the screen.
            btnRect.anchorMin = new Vector2(0.0f, 1.0f);
            btnRect.anchorMax = new Vector2(0.0f, 1.0f);

            // Set pivot to bottom left from center and adjust position.
            btnRect.anchoredPosition = new Vector2(0.0f, 0.0f);
            Debug.Log("ToolbarBuildButtons::OpenMenu ->>> Sub button rect: " + buttonCorners[1].x);
            btnRect.position = new Vector3(buttonCorners[1].x, btnHeight * i);

            // Add OnClickListener to change the PrefabToSpawn and CurrentPartText.
            Button partSelection = buttonGameObject.GetComponent<Button>();
            partSelection.onClick.AddListener(() => { _MM.SetPrefabToSpawn(contents[i]); });

        }

    } // End OpenMenu

    /// <summary>
    /// Use this for initialization
    /// </summary>
    void Start ()
    {
        // Grab all needed private variables
        _MM = GameObject.FindObjectOfType<MouseManager>();
        _Canvas = GameObject.FindObjectOfType<Canvas>();
        _SubMenuArea = (RectTransform)GameObject.Find("Sub Menu Area").transform;

        // Populate the category menu buttons
        for (int i = 0; i < MenuNames.Length; i++)
        {
            GameObject buttonGameObject = Instantiate(BuildButtonPrefab, this.transform);
            buttonGameObject.name = MenuNames[i];

            Text buttonLabel = buttonGameObject.GetComponentInChildren<Text>();
            buttonLabel.text = MenuNames[i];

            Button theButton = buttonGameObject.GetComponent<Button>();
            
            // Set up the OnClickListener
            theButton.onClick.AddListener( () => { this.OpenMenu(theButton); } );
        }

    }

   
	
}
