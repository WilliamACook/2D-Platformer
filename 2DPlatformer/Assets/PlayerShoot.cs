using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] PlayerInput m_playerInput;
    [SerializeField] Camera m_camera;

    public GameObject bullet;
    public Transform bulletTransform;
    public bool canFire;
    public float fireTimer;



    private void Awake()
    {
        m_playerInput = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        m_playerInput.actions.FindAction("MousePosition").performed += MousePosition;
        m_playerInput.actions.FindAction("Shoot").performed += Shoot;
        m_playerInput.actions.FindAction("Shoot").started += PointerHoldBegin;
        m_playerInput.actions.FindAction("Shoot").canceled += PointerHoldEnd;

    }

    private void OnDisable()
    {
        m_playerInput.actions.FindAction("MousePosition").performed -= MousePosition;
        m_playerInput.actions.FindAction("Shoot").performed -= Shoot;
        m_playerInput.actions.FindAction("Shoot").started -= PointerHoldBegin;
        m_playerInput.actions.FindAction("Shoot").canceled -= PointerHoldEnd;
    }
    private void Start()
    {
        
    }

    private void MousePosition(InputAction.CallbackContext context)
    {
        Vector3 mousePos = m_camera.ScreenToWorldPoint(context.ReadValue<Vector2>());
        Vector3 rotation = mousePos - transform.position;

        float rotz = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, rotz);
    }
    bool hold;

    private void Shoot(InputAction.CallbackContext context)
    {
        Debug.Log("Fire!");
        Debug.Log(hold);
        if(context.performed && canFire)
        {
                canFire = false;
                StartCoroutine(c_FireTimer());
                Instantiate(bullet, bulletTransform.position, Quaternion.identity);
            
            

        }
        else if(context.canceled)
        {
            StopCoroutine(c_FireTimer());
        }

    }

    IEnumerator c_FireTimer()
    {
        yield return new WaitForSeconds(fireTimer);
        canFire = true;
    }
    public void PointerHoldBegin(InputAction.CallbackContext context)
    {
        Debug.Log("PointerHoldBegin");
        hold = true;
        
    }

    public void PointerHoldEnd(InputAction.CallbackContext context)
    {
        Debug.Log("PointerHoldEnd");
        hold = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
