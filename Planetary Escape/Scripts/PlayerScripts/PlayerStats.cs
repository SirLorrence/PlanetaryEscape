using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PlayerStats : Healable
{
    #region Fields

// variables

    [SerializeField] private int health;
    [SerializeField] private int shield;
    [SerializeField] private float fireRate;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float bulletSpeed;

    private bool autoShoot = false;
    private bool hasDied = false;
    private float playerBulletSpeed = 10;

    [Header("Post Process Information")] public PostProcessProfile HUD_Color;

    //base values | called on reset
    public static int MAX_HEALTH = 10;
    public static int MAX_SHIELD = 0;
    public static float MAX_SPEED = 3f;
    public static float FIRE_RATE = 1f;
    public static int DAMAGE = 1;
    public static float BULLET_SPEED = 10f;
    public static bool UpgradeEnable;

    #endregion

    #region Properties

    public override int Health
    {
        get => health;
        set
        {
            int originalHealth = health;
            health = value;
            if (originalHealth > health)
                StartCoroutine(DamageFlash());
            else
                StartCoroutine(HealFlash());

            if (health >= MAX_HEALTH) health = MAX_HEALTH;

            GameManager.Instance.uiManager.UpdateHealthText();

            if (health <= 0 && !hasDied)
            {
                //Removes the need for it to be called in update
                health = 0;
                GameManager.Instance.PlayerDead();
                hasDied = true;
            }
        }
    }


    public override int Shield
    {
        get => shield;
        set
        {
            int originalShield = shield;
            shield = value;
            if (originalShield > shield) StartCoroutine(ShieldFlash());
            if (shield <= 0) shield = 0;
            GameManager.Instance.uiManager.UpdateShieldText();
            
        }
    }

    public float FireRate
    {
        get => fireRate;
        set => fireRate = value;
    }

    public float BulletSpeed
    {
        get => bulletSpeed;
        set => bulletSpeed = value;
    }

    public float MovementSpeed
    {
        get => movementSpeed;
        set => movementSpeed = value;
    }

    public int Damage
    {
        get => DAMAGE;
        set => DAMAGE = value;
    }

    #endregion

    private void Awake()
    {
        OnReset();
    }

    private void FixedUpdate()
    {
        if (!hasDied)
        {
            if (UpgradeEnable)
            {
                OnReset();
                UpgradeEnable = false;
            }
        }
    }

    public void OnReset()
    {
        hasDied = false;
        health = MAX_HEALTH;
        shield = MAX_SHIELD;
        movementSpeed = MAX_SPEED;
        fireRate = FIRE_RATE;
        bulletSpeed = BULLET_SPEED;
    }

    private IEnumerator DamageFlash()
    {
        Color originalColor = HUD_Color.GetSetting<Vignette>().color.value;
        HUD_Color.GetSetting<Vignette>().color.value = Color.red;

        yield return new WaitForSeconds(0.15f);
        HUD_Color.GetSetting<Vignette>().color.value = originalColor;
        yield return null;
    }

    private IEnumerator ShieldFlash()
    {
        Color originalColor = HUD_Color.GetSetting<Vignette>().color.value;
        HUD_Color.GetSetting<Vignette>().color.value = Color.cyan;

        yield return new WaitForSeconds(0.15f);
        HUD_Color.GetSetting<Vignette>().color.value = originalColor;
        yield return null;
    }

    private IEnumerator HealFlash()
    {
        Color originalColor = HUD_Color.GetSetting<Vignette>().color.value;
        HUD_Color.GetSetting<Vignette>().color.value = Color.green;

        yield return new WaitForSeconds(0.15f);
        HUD_Color.GetSetting<Vignette>().color.value = originalColor;
        yield return null;
    }
}