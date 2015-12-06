using UnityEngine;
using System.Collections;

public class AmmoReward : MonoBehaviour
{
    public int m_Ammount = 50;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == Tags.Tank)
        {
            TankController tankController = other.gameObject.GetComponent<TankController>();
            if (tankController)
            {
                tankController.AddAmmo(m_Ammount);
            }
            Destroy(gameObject);
        }
    }
}
