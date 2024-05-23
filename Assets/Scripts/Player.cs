using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] float sensitivity;
    [SerializeField] float padding = 0.5f;
    [SerializeField] int hp = 300;
    [SerializeField] float gameOverDelay = 1.5f;
    [SerializeField] GameObject lowhpVFX;
    [SerializeField] GameObject explosionVFX;
    [SerializeField] float m_lowhpVFXzPadding = 1;
    [SerializeField] int lowhpValue;

    [Header("Sounds")]
    [SerializeField] AudioClip deathSound;
    [Range(0, 1)] [SerializeField] float deathSoundVolume = 1f;

    [Header("Projectile")]
    [SerializeField] private float projectileSpeed = 25f;
    [FormerlySerializedAs("projectileFiringSpeed")] [SerializeField] private float projectileFiringDelay = 2f;
    [FormerlySerializedAs("maxFiringSpeed")] [SerializeField] float minFiringDelay = 0.1f;
    [SerializeField] GameObject laserPrefab;

    GameObject s_lowhpVFX;
    int maxHP;

    HealthDisplay healthDisplay;

    private float yMin;
    private float yMax;

    private float xMin;
    private float xMax;

    private bool mayFire = true;
    private bool lowhpVFXisTriggered = false;

    public int Hp { get => hp; set => hp = value; }
    public float ProjectileFiringDelay { get => projectileFiringDelay; set => projectileFiringDelay = value; }
    public float MaxFiringSpeed { get => minFiringDelay; set => minFiringDelay = value; }

    void Start()
    {
        maxHP = hp;
        healthDisplay = FindObjectOfType<HealthDisplay>();
        SetUpMoveBounaries();
    }

    void Update()
    {
        ManageLifeStatus();
        Move();
        if (mayFire)
            StartCoroutine(FireContinuosly());
        if (s_lowhpVFX != null)
        {
            s_lowhpVFX.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - m_lowhpVFXzPadding);
        }
    }

    public void DecreaseHealthAndManageLifeStatus(int hp)
    {
        Hp -= hp;
        ManageLifeStatus();
    }

    private void ManageLifeStatus()
    {
        if (hp <= 0f)
        {
           Die();
        }
        else if (hp <= lowhpValue && !lowhpVFXisTriggered)
        {
            lowhpVFXisTriggered = true;
            s_lowhpVFX = Instantiate(lowhpVFX, 
            new Vector3 (transform.position.x, transform.position.y, transform.position.z - m_lowhpVFXzPadding), Quaternion.identity);
        }
        else if (hp > lowhpValue && lowhpVFXisTriggered)
        {
            lowhpVFXisTriggered = false;
            Destroy(s_lowhpVFX);
        }

        if(hp > maxHP)
        {
            hp = maxHP;
        }
    }

    private void Die()
    {
        Instantiate(explosionVFX, transform.position, Quaternion.identity);
        Destroy(gameObject);
        AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, deathSoundVolume);
        FindObjectOfType<GameSession>().GetScore();
        FindObjectOfType<Level>().LoadGameOver();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out Enemy enemy))
        {
            DecreaseHealthAndManageLifeStatus(other.gameObject.GetComponent<Enemy>().Hp);
            other.gameObject.GetComponent<Enemy>().Die();
        }
        else if (other.gameObject.TryGetComponent(out DamageDealer damageDealer))
        {
            DecreaseHealthAndManageLifeStatus(other.gameObject.GetComponent<DamageDealer>().Damage);
            other.gameObject.GetComponent<DamageDealer>().OnHit();
        }
        healthDisplay.CurrentHp = hp;
    }

    private IEnumerator FireContinuosly()
    {
        if (Input.GetButton("Fire1"))
        {
           GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity) as GameObject;
           laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
        }

        mayFire = false;

        yield return new WaitForSeconds(projectileFiringDelay);

        mayFire = true;
    }

    private void Move()
    {
        var deltaX = Input.GetAxisRaw("Horizontal") * Time.deltaTime * sensitivity;
        var deltaY = Input.GetAxisRaw("Vertical") * Time.deltaTime * sensitivity;

        var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
        var newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);

        transform.position = new Vector2(newXPos, newYPos);
    }


    private void SetUpMoveBounaries()
    {
        Camera camera = Camera.main;
        xMin = camera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        xMax = camera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;

        yMin = camera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = camera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
    }
}
