using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    // Method to load a scene asynchronously
    public void CPU()
    {
        SceneManager.LoadSceneAsync(4);
    }
}
