using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] PlayerInput m_playerInput;
    [SerializeField] Camera m_camera;
    [SerializeField] CameraShake m_shake;
    [SerializeField] AudioSource m_Shoot;
    
    public GameObject bullet;
    public Transform bulletTransform;
    public bool canFire;
    public float fireTimer;

    private float resetTimer;



    private void Awake()
    {
        m_playerInput = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        m_playerInput.actions.FindAction("MousePosition").performed += MousePosition;
        m_playerInput.actions.FindAction("Shoot").performed += Handle_ShootPerformed;
        m_playerInput.actions.FindAction("Shoot").canceled += Handle_ShootCancelled;

    }


    private void OnDisable()
    {
        m_playerInput.actions.FindAction("MousePosition").performed -= MousePosition;
        m_playerInput.actions.FindAction("Shoot").performed -= Handle_ShootPerformed;
        m_playerInput.actions.FindAction("Shoot").canceled -= Handle_ShootCancelled;
    }
    private void Start()
    {
        resetTimer = fireTimer;
    }

    private void MousePosition(InputAction.CallbackContext context)
    {
        Vector3 mousePos = m_camera.ScreenToWorldPoint(context.ReadValue<Vector2>());
        Vector3 rotation = mousePos - transform.position;

        float rotz = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, rotz);
    }
    bool hold;
    Coroutine c_fire;

    void Handle_ShootPerformed(InputAction.CallbackContext context)
    {
        if(canFire)
        {
                     
            //StartCoroutine(m_shake.Shake(.15f, .4f));
        }

        hold = true;
        //StartCoroutine(c_Recoil());   

        //checks if coroutine is running
        if (c_fire == null)
            {
                c_fire = StartCoroutine(c_FireTimer());
                canFire = false;
            }
    }
    void Handle_ShootCancelled(InputAction.CallbackContext context)
    {        

        if(c_fire != null)
        {
            hold = false;        
            StopCoroutine(c_fire);
            c_fire = null;

        }
        
    }

    IEnumerator c_FireTimer()
    {     
        while(hold)
        {
            Instantiate(bullet, bulletTransform.position, Quaternion.identity);          
            yield return new WaitForSeconds(fireTimer);
            Debug.Log("test");
            canFire = true;
        }

        
    }
    IEnumerator c_Recoil()
    {
        transform.localPosition += new Vector3(0.2f, 0, 0);
        m_Shoot.Play();
        yield return new WaitForSeconds(0.03f);
        transform.localPosition -= new Vector3(0.2f, 0, 0);
        yield return new WaitForSeconds(0.03f);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
