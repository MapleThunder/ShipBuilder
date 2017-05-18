using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComponentKeybindDialog : MonoBehaviour {

    /**
     *  Private Properties:
     *      keybindableComponent    A placeholder for a KeybindableComponent.
     */
    KeybindableComponent keybindableComponent;

    public void OpenDialog( KeybindableComponent keybindableComponent )
    {
        this.keybindableComponent = keybindableComponent;
        gameObject.SetActive(true);

        transform.Find("Keybind").GetComponent<Text>().text = keybindableComponent.keyCode.ToString();
    }


	// Update is called once per frame
	void Update ()
    {
        // Close window on escape key.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameObject.SetActive(false);
            return;
        }

        // While the window is open, listen for a key press.	
        if (Input.anyKeyDown)
        {
            // A key was pressed, but WHICH key ?
            foreach(KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
            {
                // If the KeyCode matches and is NOT Left/Right/Center-Mouse button AND not Escape
                if (!Input.GetKeyDown(KeyCode.Mouse0) && !Input.GetKeyDown(KeyCode.Mouse1) 
                    && !Input.GetKeyDown(KeyCode.Mouse2) && !Input.GetKeyDown(KeyCode.Escape)
                    && Input.GetKeyDown(keyCode))
                {
                    // This is the key pressed
                    keybindableComponent.keyCode = keyCode;
                    transform.Find("Keybind").GetComponent<Text>().text = keybindableComponent.keyCode.ToString();
                    return;
                }
            }
        }
	}
}
