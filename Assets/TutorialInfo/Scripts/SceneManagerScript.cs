using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    public void LoadScene(string sceneName) {
        Debug.Log("Button clicked! Loading scene: " + sceneName);
        SceneManager.LoadScene(sceneName);
    }
}
