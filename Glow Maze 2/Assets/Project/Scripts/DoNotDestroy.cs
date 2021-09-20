using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoNotDestroy : MonoBehaviour
{
    void Awake()
    {
        GameObject[] gameManager = GameObject.FindGameObjectsWithTag("Game Manager"); //Get all GameObjects with tag "Game Manager" in Current Scene

        if (gameManager.Length > 1) //Check if no more than 1 gameManager GameObject in the Current Scene
        {
            Destroy(this.gameObject); //If more than 1 found then Destroy current gameManager
        }
        DontDestroyOnLoad(this.gameObject); //If not don't Destroy
    }
}