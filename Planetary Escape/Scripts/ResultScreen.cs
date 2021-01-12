using UnityEngine;
using UnityEngine.UI;

public class ResultScreen : MonoBehaviour
{
    public Text scoreText;
    public Text enemiesText;
    public Text currencyCollected;
    public Text TotalCurrencyText;

    void DisplayResults()
    {

    }

    //enemies killed
    public void UpdateKills()
    {
        enemiesText.text = GameManager.Instance.enemiesKilled.ToString();
    }

    //total score
    public void UpdateScore()
    {
        scoreText.text = GameManager.Instance.score.ToString();
    }

    //skill points collected
    public void UpdateCollected()
    {

    }

    //total skill points
    public void UpdateTotal()
    {

    }
}
