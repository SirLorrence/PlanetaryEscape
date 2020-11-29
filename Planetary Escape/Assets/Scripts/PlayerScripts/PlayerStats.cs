using System;
using System.Collections;
using UnityEngine;

public class PlayerStats : Healable
{
    #region Fields

// variables

    [SerializeField] private int health;
    [SerializeField] private float shield;
    [SerializeField] private float fireRate;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private int damage = 1;

    private bool autoShoot = false;
    private bool hasDied = false;
    private float playerBulletSpeed = 10;

    [Header("Material Info")] 
    public SkinnedMeshRenderer smr;
    public Material greenFlash;
    public Material redFlash;

    //base values | called on reset
    public static int MAX_HEALTH = 10;
    public static float MAX_SHIELD = 0;
    public static float MAX_SPEED = 3f;
    public static float FIRE_RATE = 1f;
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
                GameManager.Instance.PlayerDead();
                hasDied = true;
            }
        }
    }

    public float Shield
    {
        get => shield;
        set => shield = value;
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
        get => damage;
        set => damage = value;
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
        Material[] originalMaterials = smr.materials;
        Material[] redMaterials = new Material[originalMaterials.Length];

        for (int i = 0; i < originalMaterials.Length; i++)
            redMaterials[i] = redFlash;

        smr.materials = redMaterials;

        yield return new WaitForSeconds(0.15f);
        smr.materials = originalMaterials;
        yield return null;
    }
    private IEnumerator HealFlash()
    {
        Material[] originalMaterials = smr.materials;
        Material[] greenMaterials = new Material[originalMaterials.Length];

        for (int i = 0; i < originalMaterials.Length; i++)
            greenMaterials[i] = greenFlash;

        smr.materials = greenMaterials;

        yield return new WaitForSeconds(0.15f);
        smr.materials = originalMaterials;
        yield return null;
    }
}