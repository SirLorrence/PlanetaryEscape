using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Slider slider;
    public Text text;

    public void LoadLevel(int levelIndex)
    {
        slider.gameObject.SetActive(true);
        StartCoroutine(LoadAsync(levelIndex));
    }

    IEnumerator LoadAsync(int levelIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(levelIndex);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f) * 100;
            slider.value = progress;
            text.text = progress + "%";
            yield return null;
        }
    }
}
