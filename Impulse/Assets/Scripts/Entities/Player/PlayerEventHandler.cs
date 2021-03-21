using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Managers;

public class PlayerEventHandler : MonoBehaviour
{
    public Text nextWaveUi;
    private void Start()
    {
        WaveManager.Instance.EndOfWave += OnWaveEnd;
    }

    private void OnDisable()
    {
        WaveManager.Instance.EndOfWave -= OnWaveEnd;
    }

    public void OnWaveEnd()
    {
        StartCoroutine(Fade(nextWaveUi));
    }

    IEnumerator Fade(Text go)
    {
        nextWaveUi.text = "Wave " + (WaveManager.Instance.CurrentWave + 1);
        nextWaveUi.gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        nextWaveUi.gameObject.SetActive(false);
    }
}
