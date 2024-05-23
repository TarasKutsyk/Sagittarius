using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] int damage = 100;

    [Header("Explosive")]
    [SerializeField] GameObject explosionVFX;
    [SerializeField] float explosionTime;
    [SerializeField] float followingTime = 3f;
    [SerializeField] float followSpeed = 2;
    [SerializeField] float activationTime = 0.5f;
    [SerializeField] float guidanceSpeed = 2f;
    [SerializeField] AudioClip explosionSFX;
    [Range(0, 1)] [SerializeField] float explosionSFXVolume = 0.5f;

    [Header("PowerUp")]
    [SerializeField] int hpRestore = 500;
    [SerializeField] float firingBonus = 1f;
    [SerializeField] float PowerUpVolume = 2f;

    Player player;

    public int Damage => damage;

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }

    private void Update()
    {
        if (tag == "Missile")
        {
            activationTime-= Time.deltaTime;
            if (activationTime >= 0)
            {
                DefaultBehavior();
            }
            else
            {
                followingTime -= Time.deltaTime;
                if (followingTime >= 0)
                {
                    if (player == null)
                    {
                        DefaultBehavior();
                        return;
                    }
                    
                    GuidedBehavior();
                }
                else
                {
                    DefaultBehavior();
                }
            }
        }
    }

    private void GuidedBehavior()
    {
        var newPos = Vector2.MoveTowards(transform.position, player.transform.position, followSpeed * Time.deltaTime);
        transform.position = newPos;
        
        Quaternion rotation = Quaternion.LookRotation(player.transform.position - transform.position,
            transform.TransformDirection(Vector3.forward));
        transform.rotation = Quaternion.Lerp(transform.rotation, new Quaternion(0, 0, rotation.z, rotation.w), Time.deltaTime * guidanceSpeed); 
    }

    private void DefaultBehavior()
    {
        transform.position += -transform.up * Time.deltaTime * followSpeed;
    }

    public void OnHit()
    {
        if (tag == "Missile" || tag == "Bomb")
        {
            GameObject explosion = Instantiate(explosionVFX, transform.position, Quaternion.identity);
            AudioSource.PlayClipAtPoint(explosionSFX, Camera.main.transform.position, explosionSFXVolume);
            Destroy(explosion, explosionTime);
        } 
        
        else if (tag == "HP Powerup")
        {
            player.DecreaseHealthAndManageLifeStatus(-hpRestore);
            AudioSource.PlayClipAtPoint(GetComponent<AudioSource>().clip, Camera.main.transform.position, PowerUpVolume);
        }

        else if (tag == "Weapon Powerup")
        {
            if (player.ProjectileFiringDelay >= player.MaxFiringSpeed)
                player.ProjectileFiringDelay -= firingBonus;
            AudioSource.PlayClipAtPoint(GetComponent<AudioSource>().clip, Camera.main.transform.position, PowerUpVolume);
        }

        Destroy(gameObject);
    }

}
