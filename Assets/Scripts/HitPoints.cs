using UnityEngine;
using System.Collections;
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

    public void AddHitPoints(float ammount)
    {
        m_HitPoints += ammount;
        if (m_HitPoints > m_MaxHitPoints)
        {
            m_MaxHitPoints = m_HitPoints;
        }
        UpdateLifeBar();
    }

    public void TakeDamage(float amount)
    {
        m_HitPoints -= amount;
        if (m_HitPoints <= 0)
        {
            m_HitPoints = 0;
            Utils.DestroyWithExplosion(gameObject, m_Explosion);
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
