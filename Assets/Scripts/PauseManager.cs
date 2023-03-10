using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    Canvas pauseMenu;
    public bool isPaused;

    void Start()
    {
        pauseMenu = GetComponent<Canvas>();
        isPaused= false;
    }

    void Update()
    {
        if (isPaused) {
            pauseMenu.enabled = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            pauseMenu.enabled = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void Unpause()
    {
        isPaused = false;
    }

    public void Pause()
    {
        isPaused= true;
    }
}
