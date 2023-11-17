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

    [SerializeField] float force;
    [SerializeField] float size = 0.2f;
    [SerializeField] int bounces;   

    [SerializeField] GameObject effect;

    private void Awake()
    {
       
    }


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        m_camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);
        transform.localScale = new Vector3(size, size, 1);
        Vector3 direction = mousePos - transform.position;
        Vector3 rotation = transform.position - mousePos;
        rb.velocity = new Vector2 (direction.x, direction.y).normalized * force;
        float rot = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot);
        Destroy(gameObject, 10);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        
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
            else { bounces--; }

        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
