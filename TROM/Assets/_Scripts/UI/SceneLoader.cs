using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Singleton;
    public GameObject loadingScreen; // Assign your loading screen GameObject
    public Slider slider; 
    public TMP_Text text;

    public void Awake()
    {
        Singleton = this;
        loadingScreen.SetActive(false);
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadAsynchronously(sceneName));
    }

    IEnumerator LoadAsynchronously(string sceneName)
    {
        // Display the loading screen
        loadingScreen.SetActive(true);

        // Start loading the scene
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        // While the scene is still loading
        while (!operation.isDone)
        {
            // Update the UI slider. This assumes that you have a slider element in your loading screen to represent progress.
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            slider.value = progress;

            yield return null;
        }
    }
}