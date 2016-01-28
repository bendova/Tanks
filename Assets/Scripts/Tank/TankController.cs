using UnityEngine;
using UnityEngine.UI;

public class TankController : MonoBehaviour
{
    public enum PlayerIndex
    {
        None = 0,
        Player_01 = 1,
        Player_02 = 2,
    }

    public float m_MoveSpeedFactor = 0.2f;
    public float m_ProjectileAccMin = 5.0f;
    public float m_ProjectileAccMax = 30.0f;
    public float m_ShotPowerUpFactor = 2.0f;
    public float m_FireRate = 2.0f; // projectiles per second
    public int m_ShellsCount = 100;
    public PlayerIndex m_PlayerIndex = PlayerIndex.None;
    public Transform m_ShellSpawner;
    public GameObject m_Projectile;
    public GameObject m_LaunchFlash;
    public Slider m_ReloadSlider;
    public GameObject m_PowerUp;
    public GameObject m_Turret;
    public Text m_Ammo;
    public SpriteRenderer m_PlayerIndicator;

    private Slider m_PowerUpSlider;
    private float m_ProjectileAcc = 0f;
    private float m_PowerUpTime = 0f;
    private bool m_IsPoweringUp = false;
    private float m_ReloadTime = 0f;

    void Start()
    {
        if (m_ReloadSlider)
        {
            m_ReloadSlider.enabled = true;
            m_ReloadSlider.value = 1f;
        }
        if (m_PowerUp)
        {
            m_PowerUp.SetActive(false);
            m_PowerUpSlider = m_PowerUp.GetComponent<Slider>();
            m_PowerUpSlider.value = 0f;
        }
        UpdateAmmoText();

        if (m_PlayerIndex == PlayerIndex.Player_01)
        {
            m_PlayerIndicator.color = Color.red;
        }
        else if (m_PlayerIndex == PlayerIndex.Player_02)
        {
            m_PlayerIndicator.color = Color.blue;
        }
    }

    public void AddAmmo(int ammo)
    {
        m_ShellsCount += ammo;
        UpdateAmmoText();
    }

    void Update()
	{
        if (m_PlayerIndex != PlayerIndex.None)
        {
            UpdateMovement();
            UpdateTurret();
            UpdateFiring();
        }
    }
    
    private void UpdateMovement()
    {
        float hInput = GetAxis("Horizontal");
        float vInput = GetAxis("Vertical");
        
        Vector3 forwardDirection = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;
        transform.position = transform.position + forwardDirection * vInput * m_MoveSpeedFactor;
        transform.Rotate(Vector3.up, hInput, Space.Self);
    }

    private void UpdateTurret()
    {
        float hInput = GetAxis("HorizontalRight");
        float vInput = GetAxis("VerticalRight");

        m_Turret.transform.Rotate(Vector3.up, hInput, Space.Self);
    }

    private void UpdateFiring()
    {
        if ((m_ReloadTime <= 0) && (m_ShellsCount > 0))
        { 
            m_IsPoweringUp = m_IsPoweringUp || GetButtonDown("Fire");
            if (m_IsPoweringUp)
            {
                m_PowerUpTime += m_ShotPowerUpFactor * Time.deltaTime;
                m_ProjectileAcc = Mathf.Lerp(m_ProjectileAccMin, m_ProjectileAccMax, m_PowerUpTime);
                UpdatePowerUpSlider();
            }

            bool fireUp = GetButtonUp("Fire");
            if (m_IsPoweringUp && fireUp)
            {
                FireProjectile(m_ProjectileAcc);
                m_PowerUp.SetActive(false);
                m_IsPoweringUp = false;
                m_ProjectileAcc = 0f;
                m_PowerUpTime = 0f;
                m_ReloadTime = 1 / m_FireRate;
                --m_ShellsCount;
                UpdateAmmoText();
            }
        }
        else
        {
            m_ReloadTime -= Time.deltaTime;
        }
        UpdateReloadSlider();
    }
    
    private void FireProjectile(float projectileAcc)
    {
        GameObject projectile = GameObject.Instantiate(m_Projectile, m_ShellSpawner.position, m_ShellSpawner.rotation) as GameObject;
        if (projectile)
        {
            GameObject.Instantiate(m_LaunchFlash, m_ShellSpawner.position, m_ShellSpawner.rotation);
            ShellController controller = projectile.GetComponent<ShellController>();
            if (controller)
            {
                controller.m_Launcher = gameObject;
                controller.m_LaunchSpeed = projectileAcc;
            }
        }
    }

    private void UpdatePowerUpSlider()
    {
        if (m_PowerUp)
        {
            m_PowerUp.SetActive(true);
            m_PowerUpSlider.value = m_ProjectileAcc / (m_ProjectileAccMax - m_ProjectileAccMin);
        }
    }

    private void UpdateReloadSlider()
    {
        m_ReloadSlider.value = (1f - m_ReloadTime * m_FireRate);
    }

    private void UpdateAmmoText()
    {
        m_Ammo.text = "Shells: " + m_ShellsCount;
    }

    private float GetAxis(string axis)
    {
        return Input.GetAxis(GetInputName(axis));
    }

    private bool GetButtonDown(string button)
    {
        return Input.GetButtonDown(GetInputName(button));
    }

    private bool GetButtonUp(string button)
    {
        return Input.GetButtonUp(GetInputName(button));
    }

    private string GetInputName(string input)
    {
        return input + (int) m_PlayerIndex;
    }
}
