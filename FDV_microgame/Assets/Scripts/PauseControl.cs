using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseControl : MonoBehaviour
{
    public static bool gameIsPaused = false;
    public GameObject pauseMenuCanvas;

    void Start()
    {
        pauseMenuCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            if(gameIsPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f; // Se pausa el juego
        pauseMenuCanvas.SetActive(true); // Activa el menú de pausa
        gameIsPaused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f; // Se reanuda el juego
        pauseMenuCanvas.SetActive(false); // Desactiva el menú de pausa
        gameIsPaused = false;
    }
}
