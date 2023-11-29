using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSizeBuff : MonoBehaviour
{
    [SerializeField] BulletScript bullet;
    [SerializeField] float amount;

    private void Awake()
    {
        bullet.size = 0.2f;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            bullet.size += amount;

        }
    }
}
