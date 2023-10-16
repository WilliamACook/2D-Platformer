using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    [SerializeField] PlayerInput m_playerInput;

    bool isGrounded;

    private Rigidbody2D rb;

    float m_f_Axis;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        m_playerInput = GetComponent<PlayerInput>();
    }
 

    // Start is called before the first frame update
    void Start()
    {
        m_playerInput.actions.FindAction("Jump").performed += Jump;
        m_playerInput.actions.FindAction("Move").performed += Handle_MovedPerformed;
        m_playerInput.actions.FindAction("Move").canceled += Handle_MovedCancelled;
    }

    bool m_b_InMoveActive;
    Coroutine c_RMove;


   void Handle_MovedPerformed(InputAction.CallbackContext context)
    {
        m_f_Axis = context.ReadValue<float>();
        m_b_InMoveActive = true;
        if(c_RMove == null)
        {
            c_RMove = StartCoroutine(C_MoveUpdate());
        }
    }

    void Handle_MovedCancelled(InputAction.CallbackContext context)
    {
        m_f_Axis = context.ReadValue<float>();
        m_b_InMoveActive = false;
        if(c_RMove != null) 
        {
            StopCoroutine(c_RMove);
            c_RMove = null;
        }
    }

    IEnumerator C_MoveUpdate()
    {
        while(m_b_InMoveActive)
        {
            Debug.Log($"Move Input = {m_f_Axis}");
            //rb.AddForce(new Vector2((m_f_Axis * m_fMovement), 0), ForceMode2D.Force);
            rb.velocity = new Vector2(m_f_Axis * m_fMovement, rb.velocity.y);
            yield return new WaitForFixedUpdate();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.CircleCast(m_castPos.position, m_castradius, Vector2.zero, 0, m_layerMask);
        //rb.velocity = new Vector2 (1 * m_fConstantSpeed, rb.velocity.y);  

    }
    //public float constantMove;

    private void FixedUpdate()
    {
        //rb.AddForce(new Vector2(constantMove, 0), ForceMode2D.Force) ;
        
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (isGrounded && context.performed)
        {
            rb.AddForce(Vector2.up * m_fJump, ForceMode2D.Impulse);
        }
        
    }

    //public void Move(InputAction.CallbackContext context)
    //{
        //m_f_Axis = context.ReadValue<float>();
    //}

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

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("KillObject"))
        {
            Debug.Log("Die");
        }
    }
}
