using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScore
{
    private readonly TankController.PlayerIndex m_PlayerIndex;
    private int m_Score = 0;

    public PlayerScore(TankController.PlayerIndex playerIndex)
    {
        m_PlayerIndex = playerIndex;
        UpdateScoreText();
    }

    public void IncScore()
    {
        ++m_Score;
        UpdateScoreText();
    }

    public void UpdateScoreText()
    {
        UiManager.Instance.SetScore(m_PlayerIndex, m_Score);
    }

}

public class GameManager: PersistentSingleton<GameManager>
{
    private Dictionary<TankController.PlayerIndex, PlayerScore> m_PlayerScore;

    private void Start()
    {
        if (EnsureUnique(this))
        {
            m_PlayerScore = new Dictionary<TankController.PlayerIndex, PlayerScore>();
            m_PlayerScore[TankController.PlayerIndex.Player_01] = new PlayerScore(TankController.PlayerIndex.Player_01);
            m_PlayerScore[TankController.PlayerIndex.Player_02] = new PlayerScore(TankController.PlayerIndex.Player_02);
        }
    }
	
	void Update()
    {
	    
	}

    public void OnTankDestroyed(GameObject tank, GameObject attacker)
    {
        TankController controller = tank.GetComponent<TankController>();
        TankController attackerController = attacker.GetComponent<TankController>();

        if (IsTankPlayer(controller) &&
            IsTankPlayer(attackerController))
        {
            m_PlayerScore[attackerController.m_PlayerIndex].IncScore();
            StartCoroutine("ReloadLevel");
        }
    }

    private bool IsTankPlayer(TankController controller)
    {
        return (controller && (controller.m_PlayerIndex != TankController.PlayerIndex.None));
    }

    private IEnumerator ReloadLevel()
    {
        yield return new WaitForSeconds(3f);
        Application.LoadLevel(Application.loadedLevel);
    }

    public void OnLevelWasLoaded(int level)
    {
        if (m_PlayerScore != null)
        {
            foreach (KeyValuePair<TankController.PlayerIndex, PlayerScore> pair in m_PlayerScore)
            {
                pair.Value.UpdateScoreText();
            }
        }
    }
}
