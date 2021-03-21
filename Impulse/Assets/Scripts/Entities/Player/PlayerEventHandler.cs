using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Managers;

public class PlayerEventHandler : MonoBehaviour
{
    [Header("Assignables")]
    public Text nextWaveUi;
    public GameObject resultsScreen;
    private void Start()
    {
        WaveManager.Instance.EndOfWave += OnWaveEnd;
        GameManager.Instance.playerDeath += OnPlayerDeath;
    }

    private void OnDisable()
    {
        WaveManager.Instance.EndOfWave -= OnWaveEnd;
        GameManager.Instance.playerDeath -= OnPlayerDeath;
    }

    private void OnWaveEnd()
    {
        StartCoroutine(AppearAndDissapear(nextWaveUi, 3));
    }

    private void OnPlayerDeath()
    {
        resultsScreen.gameObject.SetActive(true);
        //StartCoroutine(Appear(resultsScreen, 4));
    }

    IEnumerator AppearAndDissapear(Text go, float time)
    {
        go.text = "Wave " + (WaveManager.Instance.CurrentWave + 1);
        go.gameObject.SetActive(true);
        yield return new WaitForSeconds(time);
        go.gameObject.SetActive(false);
    }

    IEnumerator Appear(GameObject go, float delay)
    {
        yield return new WaitForSeconds(delay);
        nextWaveUi.gameObject.SetActive(true);
    }
}
