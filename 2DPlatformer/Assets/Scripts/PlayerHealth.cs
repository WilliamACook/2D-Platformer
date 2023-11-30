using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] float maxHealth;
    [SerializeField] AudioSource snd_hurt;
    //[SerializeField] GameObject effect;
    private SpriteRenderer sr;
    private HealthBar healthBar;
    private float currentHealth;
    private bool died;
    // Start is called before the first frame update

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        healthBar = GetComponentInChildren<HealthBar>();
    }
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.UpdateHealth(currentHealth, maxHealth);
        died = false;
    }

    // Update is called once per frame

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        healthBar.UpdateHealth(currentHealth, maxHealth);
        snd_hurt.Play();

        if (currentHealth <= 0 && !died)
        {
            died = true;
            Die();
        }
    }

    void Die()
    {
        SceneManager.LoadScene("TestingGround");
        //Instantiate(effect, transform.position, Quaternion.identity);
        //sr.enabled = false;
        //Destroy(gameObject);
    }
}


