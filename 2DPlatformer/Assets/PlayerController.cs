using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float m_fMovement;
    [SerializeField] float m_fConstantSpeed;
    [SerializeField] float m_fJump;

    [SerializeField] Transform m_castPos;
    [SerializeField] float m_castradius;
    [SerializeField] LayerMask m_layerMask;

    bool isGrounded;

    private Rigidbody2D rb;

    float m_f_Axis;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.CircleCast(m_castPos.position, m_castradius, Vector2.zero, 0, m_layerMask);
       //rb.velocity = new Vector2 (m_f_Axis * m_fMovement, rb.velocity.y);
       rb.velocity = new Vector2 (1 * m_fConstantSpeed, rb.velocity.y);  
       
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (isGrounded && context.performed)
        {
            rb.AddForce(Vector2.up * m_fJump, ForceMode2D.Impulse);
        }
        
    }

    public void Move(InputAction.CallbackContext context)
    {
        m_f_Axis = context.ReadValue<float>();
    }

    private void OnDrawGizmos()
    {
        if (isGrounded)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(m_castPos.position, m_castradius);
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(m_castPos.position, m_castradius);
        }
    }

}
