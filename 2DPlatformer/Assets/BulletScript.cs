using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class BulletScript : MonoBehaviour
{  
    private Camera m_camera;
    private Rigidbody2D rb;
    public float force;
    private Vector3 mousePos;

    private void Awake()
    {
       
    }


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        m_camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - transform.position;
        Vector3 rotation = transform.position - mousePos;
        rb.velocity = new Vector2 (direction.x, direction.y).normalized * force;
        float rot = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot);
        Destroy(gameObject, 2);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
