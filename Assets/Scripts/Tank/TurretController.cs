using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.Networking;

public class TurretController : MonoBehaviour
{
    public Transform m_ShellSpawner;
    public GameObject m_Projectile;
    public GameObject m_LaunchFlash;

    public int m_ShellsCount = 30;

    public float m_FireRate = 2.0f; // projectiles per second
    private float m_ReloadTime = 0f;

    private GameObject m_Turret;
    public GameObject Turret
    {
        get { return m_Turret; }
    }

    private bool m_IsTurningTurret = false;
    private float m_TurnAngleRadiansDelta = 0.1f;

    public void Start()
    {
        m_Turret = m_ShellSpawner.transform.parent.gameObject;
    }

    public void AddAmmo(int ammo)
    {
        m_ShellsCount += ammo;
    }

    public void Update()
    {
        if (m_ReloadTime > 0)
        {
            m_ReloadTime -= Time.deltaTime;
        }
        if (m_IsTurningTurret)
        {
            Vector3 forward = m_Turret.transform.parent.transform.forward;
            m_Turret.transform.forward = Vector3.RotateTowards(m_Turret.transform.forward, forward, m_TurnAngleRadiansDelta, 0.0f);
            float angleDegress = Vector3.Angle(m_Turret.transform.forward, forward);
            if (Mathf.Approximately(angleDegress, 0.0f))
            {
                m_IsTurningTurret = false;
            }
        }
    }

    public bool CanFire()
    {
        return ((m_ReloadTime <= 0) && HasAmmo());
    }

    public bool HasAmmo()
    {
        return (m_ShellsCount > 0);
    }

    public float GetReloadFactor()
    {
        return (1f - m_ReloadTime * m_FireRate);
    }

    public void FireProjectile(float launchSpeed)
    {
        ShellController controller = PrepareToFire();
        if (controller)
        {
            controller.Launch(gameObject, launchSpeed);
        }
    }

    public void FireProjectile(Vector3 target)
    {
        ShellController controller = PrepareToFire();
        if (controller)
        {
            controller.Launch(gameObject, target);
        }
    }

    private ShellController PrepareToFire()
    {
        ShellController controller = null;
        if (CanFire())
        {
            --m_ShellsCount;
            m_ReloadTime = 1 / m_FireRate;
            GameObject projectile = GameObject.Instantiate(m_Projectile, m_ShellSpawner.position, m_ShellSpawner.rotation) as GameObject;
            if (projectile)
            {
                GameObject.Instantiate(m_LaunchFlash, m_ShellSpawner.position, m_ShellSpawner.rotation);
                controller = projectile.GetComponent<ShellController>();
            }
        }
        return controller;
    }

    public void TurnTurretForward()
    {
        m_IsTurningTurret = true;
    }

    public void TurnTurret(float angle)
    {
        m_IsTurningTurret = false;
        m_Turret.transform.Rotate(Vector3.up, angle, Space.Self);
    }

    public void TurretLookAt(Vector3 targetPos)
    {
        m_IsTurningTurret = false;
        m_Turret.transform.LookAt(targetPos);
    }
}
