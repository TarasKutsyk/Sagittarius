using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shredder : MonoBehaviour
{
    [SerializeField] private float yPadding = 0.5f;
    private float xPos;
    private float yPos;

    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Enemy enemy) || collision.gameObject.TryGetComponent(out Player player))
            return;
        
        Destroy(collision.gameObject);
    }
}
