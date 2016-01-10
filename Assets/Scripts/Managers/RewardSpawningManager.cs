using UnityEngine;

public class RewardSpawningManager: MonoBehaviour
{
    public GameObject[] m_Rewards;
    public GameObject m_SpawnPointsContainer;
    public float m_SpawnInterval = 5f;
    public int m_MaxSpawnedRewards = 5;

    private float m_TimeSinceLastSpawn = 0f;
    private int m_SpawnedRewards = 0;

    private static RewardSpawningManager s_Instance = null;
    public static RewardSpawningManager Instance
    {
        get
        {
            return s_Instance;
        }
    }

    void Start()
    {
        s_Instance = this;
    }

    void Update()
	{
        if (m_SpawnedRewards < m_MaxSpawnedRewards)
        {
            m_TimeSinceLastSpawn += Time.deltaTime;
            if (m_TimeSinceLastSpawn > m_SpawnInterval)
            {
                SpawnRandomReward();
                m_TimeSinceLastSpawn = 0f;
                ++m_SpawnedRewards;
            }
        }
	}

    private void SpawnRandomReward()
    {
        if (m_Rewards.Length > 0 && m_SpawnPointsContainer.transform.childCount > 0)
        {
            GameObject reward = m_Rewards[Random.Range(0, m_Rewards.Length)];
            int spawnPointIndex = Random.Range(0, m_SpawnPointsContainer.transform.childCount);
            GameObject spawnPoint = m_SpawnPointsContainer.transform.GetChild(spawnPointIndex).gameObject;
            GameObject.Instantiate(reward, spawnPoint.transform.position, reward.transform.rotation);
        }
    }

    public void OnRewardPickedUp()
    {
        --m_SpawnedRewards;
        Debug.Assert((m_SpawnedRewards >= 0), "OnRewardPickedUp() was called too often!");
    }
}
