using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneManager : MonoBehaviour
{
    sceneManager man;
    public void SceneLoad(int level)
    {
        SceneManager.LoadScene(level);
    }
    public void exit_game()
    {
        Application.Quit();
    }
}
