using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class BulletScript : MonoBehaviour
{  
    private Camera m_camera;
    private Rigidbody2D rb;
    private Vector3 mousePos;
    private AudioSource m_bounceBullet;
    private SpriteRenderer sr;
    private TrailRenderer tr;
    private PlayerHealth player;

    [SerializeField] float force;
    [SerializeField] public float size = 0.2f;
    [SerializeField] public int bounces;
    [SerializeField] public float damage;

    [SerializeField] GameObject effect;
    //[SerializeField] CameraShake CameraShake;

    private void Awake()
    {
    }


    void Start()
    {
        tr = GetComponent<TrailRenderer>();
        rb = GetComponent<Rigidbody2D>();
        m_bounceBullet = GetComponent<AudioSource>();
        sr = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        m_camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);
        transform.localScale = new Vector3(size, size, 1);
        Vector3 direction = mousePos - transform.position;
        Vector3 rotation = transform.position - mousePos;
        rb.velocity = new Vector2 (direction.x, direction.y).normalized * force;
        float rot = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot);
        tr.startWidth = size;
        Destroy(gameObject, 10);
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("ExplodingBarrel"))
        {
            Debug.Log("Boom");
            Instantiate(effect, transform.position, Quaternion.identity);
            var healthComponent = other.GetComponent<Health>();
            if (healthComponent != null)
            {
                healthComponent.TakeDamage(damage);
            }
            Destroy(gameObject);
        }

        if (other.gameObject.CompareTag("Player"))
        {
            var healthComponent = other.GetComponent<PlayerHealth>();
            if (healthComponent != null)
            {
                healthComponent.TakeDamage(damage);
            }
            Destroy(gameObject);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Hit");
            if (bounces == 0) 
            { 
                Instantiate(effect, transform.position, Quaternion.identity);                
                Destroy(gameObject); 
            
            }
            else 
            {
                bounces--; 
                m_bounceBullet.Play();
            }

        }
        if (collision.gameObject.CompareTag("Player"))
        {
            var healthComponent = player.GetComponent<PlayerHealth>();
            if (healthComponent != null)
            {
                healthComponent.TakeDamage(damage);
            }
            Destroy(gameObject);
        }

    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
