using UnityEngine;

public class RewardSpawningManager: Singleton<RewardSpawningManager>
{
    public GameObject[] m_Rewards;
    public GameObject m_SpawnPointsContainer;
    public float m_SpawnInterval = 5f;
    public int m_MaxSpawnedRewards = 5;

    private float m_TimeSinceLastSpawn = 0f;
    private int m_SpawnedRewards = 0;

    void Start()
    {
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
            GameObject spawnedReward = GameObject.Instantiate(reward, spawnPoint.transform.position, reward.transform.rotation) as GameObject;
            spawnedReward.transform.parent = gameObject.transform;
        }
    }

    public void OnRewardPickedUp()
    {
        --m_SpawnedRewards;
        Debug.Assert((m_SpawnedRewards >= 0), "OnRewardPickedUp() was called too often!");
    }
}
