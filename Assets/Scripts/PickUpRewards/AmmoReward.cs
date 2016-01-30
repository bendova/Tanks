using UnityEngine;
using System.Collections;

public class AmmoReward : MonoBehaviour
{
    public int m_Ammount = 50;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == Tags.Tank)
        {
            TurretController turretController = other.gameObject.GetComponent<TurretController>();
            if (turretController)
            {
                turretController.AddAmmo(m_Ammount);
            }
            RewardSpawningManager.Instance.OnRewardPickedUp();
            Destroy(gameObject);
        }
    }
}
