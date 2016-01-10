using UnityEngine;

public class HitPointsReward : MonoBehaviour
{
    public float m_HitPointsReward = 20f;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == Tags.Tank)
        {
            HitPoints hitPoints = other.gameObject.GetComponent<HitPoints>();
            if (hitPoints && hitPoints.CanAddHitPoints())
            {
                hitPoints.AddHitPoints(m_HitPointsReward);
                RewardSpawningManager.Instance.OnRewardPickedUp();
                Destroy(gameObject);
            }
        }
    }
}
