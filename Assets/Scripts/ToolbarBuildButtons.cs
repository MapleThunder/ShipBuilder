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
    MouseManager _MM;
    Canvas _Canvas;
    RectTransform _SubMenuArea;

    /**
     *  Public Properties:
     *      BuildButtonPrefab   Prefab for the Menu buttons in the toolbar.
     *      
     */
    public GameObject BuildButtonPrefab;
    public GameObject SubMenuButtonPrefab;
    public GameObject[] MovementPrefabs;
    public GameObject[] StructurePrefabs;
    public string[] MenuNames;
    


    /// <summary>
    /// Opens the sub menu for a toolbar button.
    /// </summary>
    /// <param name="menuName"></param>
    /// <param name="btnGameObject"></param>
    void OpenMenu(GameObject btnGameObject)
    {
        GameObject[] contents = null;
        // Clear any buttons that may be in the _SubMenuArea
        int childCount = _SubMenuArea.childCount;
        if(childCount > 0)
        {
            for(int i = 0; i < childCount; i++)
            {
                Destroy(_SubMenuArea.GetChild(i).gameObject);
            }
        }

        /** To get the 4 corners for the clicked button I use the 
         * RectTransform.GetWorldCorners(Vector3[] fourCornersArray).
         * World corners are clockwise starting at Bottom Left.
         * worldCorners[0] == Bottom Left
         * worldCorners[1] == Top Left
         * worldCorners[2] == Top Right
         * worldCorners[3] == Bottom Right
         */
        RectTransform btnTrans = (RectTransform)btnGameObject.transform;
        Vector3[] buttonCorners = new Vector3[4];
        btnTrans.GetWorldCorners(buttonCorners);
        // Find corners of the sub menu area
        Vector3[] subMenuAreaCorners = new Vector3[4];
        _SubMenuArea.GetWorldCorners(subMenuAreaCorners);

        string menuName = btnGameObject.name;

        Debug.Log("ToolbarBuildButtons::OpenMenu ->>> Sub Menu WorldCorner[0]: " + subMenuAreaCorners[0]);

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
            Debug.LogError("ToolbarBuildButtons::OpenMenu ->>> Sub-menu content array is null.");
            return;
        }
        
        float btnHeight = buttonCorners[1].y;

        for (int i = 0; i < contents.Length; i++)
        {
            GameObject buttonGameObject = Instantiate(SubMenuButtonPrefab, _SubMenuArea);
            Debug.Log("ToolbarBuildButtons::OpenMenu ->>> Sub Menu Button["+ i +"] transform: " + buttonGameObject.transform.position);
            // Set the names and labels for the button.
            buttonGameObject.name = contents[i].name;
            Text buttonLabel = buttonGameObject.GetComponentInChildren<Text>();
            buttonLabel.text = contents[i].name;

            RectTransform btnRect = (RectTransform)buttonGameObject.transform;

            // Set pivot to bottom left from center and adjust position.
            btnRect.pivot = new Vector2(0.0f, 0.0f);
            btnRect.anchoredPosition = new Vector2(0.0f, 0f);
            btnRect.position = new Vector3(buttonCorners[0].x, btnHeight + (btnHeight * i));

            // Add OnClickListener to change the PrefabToSpawn and CurrentPartText.
            Button partSelection = buttonGameObject.GetComponent<Button>();
            Debug.Log("ToolbarBuildButtons:: OpenMenu ->>> Contents[" + i + "]: " + contents[i]);
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
            theButton.onClick.AddListener( () => { this.OpenMenu(buttonGameObject); } );
        }

    }

   
	
}
