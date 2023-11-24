using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] float maxHealth;
    [SerializeField] GameObject effect;
    private ExplosiveBarrel ExplosiveBarrel;
    private SpriteRenderer sr;
    private float currentHealth;
    private bool died;
    // Start is called before the first frame update

    private void Awake()
    {
        ExplosiveBarrel = GetComponent<ExplosiveBarrel>();
        sr = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        currentHealth = maxHealth;
        died = false;
    }

    // Update is called once per frame

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if(currentHealth <= 0 && !died) 
        {
            died = true;
            ExplosiveBarrel.Detonate();
            Die();
        }
    }

    void Die()
    {
        Instantiate(effect, transform.position, Quaternion.identity);
        sr.enabled = false;
        Destroy(gameObject, 3);
    }
}
