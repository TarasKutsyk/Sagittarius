using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy")]
    [SerializeField] float explosionTime = 0.3f;
    [SerializeField] int hp = 100;
    [SerializeField] int scorePerKill = 50;
    [SerializeField] bool noneMissileProjectile = true;
    [SerializeField] bool missileProjectile = false;

    [Header("Laser/Bomb")]
    float shotCounter;
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 1f;
    [SerializeField] GameObject projPrefab;
    [SerializeField] private float projSpeed = 25f;
    [SerializeField] float projOffset = 1f;

    [Header("Missiles")]
    [SerializeField] GameObject missilePrefab;
    [SerializeField] float missileOffsetX = 1f;
    //[SerializeField] float missileOffsetY = 0.5f;
    [SerializeField] float missilePossibility = 0.5f;
    [SerializeField] int missileNumber;
    [SerializeField] float missilesStartDelay = 2f;
    [SerializeField] float missilesLaunchSFXvolume = 2f;

    [Header("Sound/Video FX")]
    [SerializeField] GameObject explosionVFX;
    [SerializeField] AudioClip firingSound;
    [SerializeField] AudioClip deathSound;
    [Range(0, 1)][SerializeField] float deathSoundVolume = 0.5f;
    [Range(0, 1)][SerializeField] float firingSoundVolume = 0.5f;

    [Header("Power-ups")]
    [SerializeField] bool generateFiringPowerUp;
    [SerializeField] GameObject firingPowerUp;
    [SerializeField] float firingPowerUpChance = 100f;
    [SerializeField] bool generateHpPowerUp;
    [SerializeField] GameObject hpPowerUp;
    [SerializeField] float hpPowerUpChance = 100f;
    [SerializeField] float powerUpSpeed = 5f;

    Score score;
    bool missileShoot = false;

    public int Hp { get => hp; set => hp = value; }

    private void Start()
    {
        shotCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        score = FindObjectOfType<Score>();
    }

    private void Update()
    {
        CountDownAndShoot();
    }

    private void CountDownAndShoot()
    {
        missilesStartDelay -= Time.deltaTime;
        shotCounter -= Time.deltaTime;
        if (shotCounter <= 0f)
        {
            if (missileProjectile && missilesStartDelay <= 0f && !missileShoot)
            {
                MissilesFire();
            }
            if (noneMissileProjectile)
            {
                DefaultFire();
            }
            
            shotCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }

    private void DefaultFire()
    {
        var projPos = new Vector2(transform.position.x, transform.position.y - projOffset);
        GameObject laser = Instantiate(projPrefab, projPos, Quaternion.identity) as GameObject;
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projSpeed);
        AudioSource.PlayClipAtPoint(firingSound, Camera.main.transform.position, firingSoundVolume);
    }

    void MissilesFire()
    {
        if (tag != "Missile Enemy")
            missileShoot = true;

        LaunchMissileWithPossibility();
    }

    private bool LaunchMissileWithPossibility()
    {
        if (UnityEngine.Random.Range(0, 101) <= missilePossibility)
        {
            for (int missileCount = 1; missileCount <= missileNumber; missileCount++)
            {
                var projPos = new Vector2(transform.position.x + ((float)Math.Pow(-1, missileCount) * missileOffsetX), transform.position.y);

                GameObject missile = Instantiate(missilePrefab, projPos, missilePrefab.transform.rotation) as GameObject;
                AudioSource.PlayClipAtPoint(missile.GetComponent<AudioSource>().clip, Camera.main.transform.position, firingSoundVolume);
            }
            
            return true;
        }
        
        return false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
            DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
            if(!damageDealer) { return; }
            damageDealer.OnHit();
            ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        Hp -= damageDealer.Damage;
        if (Hp <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        GeneratePowerUp();
        score.IncreaseScore(scorePerKill);
        Destroy(gameObject);
        GameObject explosion = Instantiate(explosionVFX, transform.position, Quaternion.identity);
        Destroy(explosion, explosionTime);
        AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, deathSoundVolume);
    }

    void GeneratePowerUp()
    {
        if (generateFiringPowerUp && UnityEngine.Random.Range(0, 101) <= firingPowerUpChance)
        {
            GameObject l_firingPowerUp = Instantiate(firingPowerUp, transform.position, Quaternion.identity) as GameObject;
            l_firingPowerUp.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -powerUpSpeed);
        }
        else if (generateHpPowerUp && UnityEngine.Random.Range(0, 101) <= hpPowerUpChance)
        {
            GameObject l_hpPowerUp = Instantiate(hpPowerUp, transform.position, Quaternion.identity) as GameObject;
            l_hpPowerUp.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -powerUpSpeed);
        }
    }
}
