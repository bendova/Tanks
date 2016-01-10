using UnityEngine;
using UnityEngine.UI;

public class UiManager : Singleton<UiManager>
{
    public Text[] m_PlayerScoreTexts;
    
    void Start()
    {
	
	}
	
	void Update()
    {
	
	}

    public void SetScore(TankController.PlayerIndex playerIndex, int score)
    {
        switch (playerIndex)
        {
            case TankController.PlayerIndex.Player_01:
                SetScoreText(m_PlayerScoreTexts[0], "Red Player", score);
            break;
            case TankController.PlayerIndex.Player_02:
                SetScoreText(m_PlayerScoreTexts[1], "Blue Player", score);
            break;
        }
    }

    private void SetScoreText(Text text, string playerName, int score)
    {
        text.text = playerName + ": " + score;
    }
}
