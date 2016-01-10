using UnityEngine;
using UnityEngine.UI;

public class HitPoints : MonoBehaviour
{
    public float m_MaxHitPoints = 100.0f;
    public float m_HitPoints = 100.0f;
    public GameObject m_Explosion;
    public Slider m_LifeBar;

    public void Start()
    {
        if (m_LifeBar)
        {
            m_LifeBar.enabled = true;
            UpdateLifeBar();
        }
    }

    public void Update()
    {
    }

    public bool CanAddHitPoints()
    {
        return (m_HitPoints < m_MaxHitPoints);
    }

    public void AddHitPoints(float ammount)
    {
        m_HitPoints += ammount;
        if (m_HitPoints > m_MaxHitPoints)
        {
            m_HitPoints = m_MaxHitPoints;
        }
        UpdateLifeBar();
    }

    public void TakeDamage(float amount, GameObject attacker)
    {
        m_HitPoints -= amount;
        if (m_HitPoints <= 0)
        {
            m_HitPoints = 0;
            Utils.DestroyWithExplosion(gameObject, m_Explosion);
            if (gameObject.tag == Tags.Tank)
            {
                GameManager.Instance.OnTankDestroyed(gameObject, attacker);
            }
        }
        UpdateLifeBar();
    }

    private void UpdateLifeBar()
    {
        if (m_LifeBar)
        {
            m_LifeBar.value = m_HitPoints/m_MaxHitPoints;
        }
    }
}
