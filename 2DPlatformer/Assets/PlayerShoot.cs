using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] PlayerInput m_playerInput;
    [SerializeField] Camera m_camera;
    [SerializeField] CameraShake m_shake;

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

    private void Handle_ShootPerformed(InputAction.CallbackContext context)
    {
        if(canFire)
        {
            canFire = false;
            hold = true;
            StartCoroutine(c_FireTimer());         
            //StartCoroutine(m_shake.Shake(.15f, .4f));
        }

    }
    private void Handle_ShootCancelled(InputAction.CallbackContext context)
    {    
        StopCoroutine(c_FireTimer());
        hold = false;
        fireTimer = resetTimer;
        
    }
    Coroutine fire;
    //Do what i did with player controller check if coroutine exists / running

    IEnumerator c_FireTimer()
    {
        fireTimer = resetTimer;
        while(hold)
        {
            Instantiate(bullet, bulletTransform.position, Quaternion.identity);
            yield return new WaitForSeconds(fireTimer);          
            canFire = true;
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
