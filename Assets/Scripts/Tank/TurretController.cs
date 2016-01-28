using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class TurretController : MonoBehaviour
{
    public Transform m_ShellSpawner;
    public GameObject m_Projectile;
    public GameObject m_LaunchFlash;

    public int m_ShellsCount = 30;

    public float m_FireRate = 2.0f; // projectiles per second
    private float m_ReloadTime = 0f;

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
}
