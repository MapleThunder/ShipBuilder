using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {

    /**
     *  Private Properties:
     *      _MenuExpanded       A boolean to dictate whether the menu is open or closed.
     *      _Content            The content array for the submenu
     */
    private bool _MenuExpanded = false;
    private GameObject[] _Content;

    /// <summary>
    /// Opens or closes the menu.
    /// </summary>
    void ActivateMenu()
    {
        _MenuExpanded = !_MenuExpanded;

        if(_MenuExpanded)
        {
            // Show the menu
        }
        else
        {
            // Hide the menu
        }
    }
   
    public void SetContent(GameObject[] content)
    {
        _Content = content;
    }
}
