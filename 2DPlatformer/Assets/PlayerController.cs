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
    [SerializeField] LayerMask m_layerMaskCorner;
    [SerializeField] PlayerInput m_playerInput;

    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private TrailRenderer tr;

    private float coyoteTime = 0.3f;
    private float coyoteTimeCounter;

    private float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;

    //Dashing
    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 24f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1.0f;

    float m_f_Axis;
    bool isCrouched;
    bool nearEdge = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        m_playerInput = GetComponent<PlayerInput>();
        boxCollider = GetComponent<BoxCollider2D>();
        tr = GetComponent<TrailRenderer>();
    }
 

    // Start is called before the first frame update
    void OnEnable()
    {
        m_playerInput.actions.FindAction("Jump").performed += Jump;
        m_playerInput.actions.FindAction("Move").performed += Handle_MovedPerformed;
        m_playerInput.actions.FindAction("Move").canceled += Handle_MovedCancelled;
        m_playerInput.actions.FindAction("Jump").canceled += Jump;
        m_playerInput.actions.FindAction("Crouch").performed += Crouch;
        m_playerInput.actions.FindAction("Crouch").canceled += Crouch;
        m_playerInput.actions.FindAction("Dash").performed += Dash;
    }

    private void OnDisable()
    {
        m_playerInput.actions.FindAction("Jump").performed -= Jump;
        m_playerInput.actions.FindAction("Move").performed -= Handle_MovedPerformed;
        m_playerInput.actions.FindAction("Move").canceled -= Handle_MovedCancelled;
        m_playerInput.actions.FindAction("Jump").canceled -= Jump;
        m_playerInput.actions.FindAction("Crouch").performed -= Crouch;
        m_playerInput.actions.FindAction("Crouch").canceled -= Crouch;
        m_playerInput.actions.FindAction("Dash").performed -= Dash;
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
            rb.velocity = new Vector2(0 * 0, rb.velocity.y);

           
            //Sets Collision box size to normal
            boxCollider.size = new Vector2(1f, 1f);
            boxCollider.offset = new Vector2(0f, 0f);


        }
    }

    IEnumerator C_MoveUpdate()
    {
        while(m_b_InMoveActive)
        {
            //Debug.Log($"Move Input = {m_f_Axis}");
            //rb.AddForce(new Vector2((m_f_Axis * m_fMovement), 0), ForceMode2D.Force);
            if(!nearEdge)
            {
                if (IsGrounded())
                {
                    coyoteTimeCounter = coyoteTime;

                    //Sets Collision box size to normal
                    boxCollider.size = new Vector2(1f, 1f);
                    boxCollider.offset = new Vector2(0f, 0f);
                    tr.emitting = false;

                }
                else
                {
                    coyoteTimeCounter -= Time.deltaTime;
                    tr.emitting = true;
                }

                //Corner clips on jumps
                if (rb.velocity.y > 0f || !IsGrounded())
                {
                    boxCollider.size = new Vector2(1f, 0.5f);
                    boxCollider.offset = new Vector2(0f, 0.28f);
                    //m_castPos.transform.position = (0.478, -0.483, 0);
                }
                rb.velocity = new Vector2(m_f_Axis * m_fMovement, rb.velocity.y);
                //JumpBuffer();
                yield return new WaitForFixedUpdate();

            }
            else
            {
                m_b_InMoveActive = false;
                //Debug.Log("Stop");
                StopCoroutine(c_RMove);
                c_RMove = null;
                rb.velocity = new Vector2(0 * 0, rb.velocity.y);
                yield return new WaitForFixedUpdate();            
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        //isGrounded = Physics2D.CircleCast(m_castPos.position, m_castradius, Vector2.zero, 0, m_layerMask);
        
        //rb.velocity = new Vector2 (1 * m_fConstantSpeed, rb.velocity.y);  

        //JumpBuffer();

        //Debug.Log(jumpBufferCounter);
    }
    //public float constantMove;

    private void FixedUpdate()
    {
        //rb.AddForce(new Vector2(constantMove, 0), ForceMode2D.Force) ;
    }


    private bool IsGrounded()
    {
        return Physics2D.CircleCast(m_castPos.position, m_castradius, Vector2.zero, 0, m_layerMask);
    }    
    
    private bool IsCorner()
    {
        return Physics2D.CircleCast(m_castPos.position, m_castradius, Vector2.zero, 0, m_layerMaskCorner);
    }

    Coroutine c_JumpBuffer;
    public void Jump(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            jumpBufferCounter = jumpBufferTime;

            if (!IsGrounded())
            {
                StartCoroutine(C_JumpBuffered());
            }

            if(coyoteTimeCounter > 0f && jumpBufferCounter > 0f || IsGrounded())
            {
                //rb.AddForce(Vector2.up * m_fJump, ForceMode2D.Impulse);
                rb.velocity = new Vector2(rb.velocity.x, m_fJump);               
                //Debug.Log(jumpBufferCounter);
                //JumpBuffer();
                if (c_JumpBuffer != null)
                {
                    StopCoroutine(c_JumpBuffer);
                    //Debug.Log("JumpBuffered");
                    c_JumpBuffer= null;

                }
           
                //jumpBufferCounter= 0f;
            }

        }
        else
        {
            //if(c_JumpBuffer == null)
            //{
            //    c_JumpBuffer = StartCoroutine(C_JumpBuffered());

            //}
        }




        if (context.canceled && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            coyoteTimeCounter = 0;
        
        }

    }
        bool canBufferJump;
    IEnumerator C_JumpBuffered()
    {
        //jumpBufferCounter -= Time.deltaTime;
        //Debug.Log(jumpBufferCounter);
        //Debug.Log("Player Can jump");
        canBufferJump = true;
        yield return new WaitForSeconds(0.3f);
        canBufferJump = false;
        //Debug.Log("Player Cannot jump");
    }

    public void JumpBuffer()
    {
        if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, m_fJump);

            if (c_JumpBuffer != null)
            {
                StopCoroutine(c_JumpBuffer);
                //Debug.Log("JumpBuffered");
                c_JumpBuffer = null;

            }

            jumpBufferCounter = 0f;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }
    }

    public void Crouch(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
        Debug.Log("Crouch pressed");
            isCrouched = true;

        }

        if(context.canceled)
        {
            Debug.Log("Crouch Cancelled");
            isCrouched = false;
            nearEdge = false;
        }
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if(canDash)
        {
            //Debug.Log("Dashing");
            StartCoroutine(Dash());
        }
    }

    IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        //float NormalGravity = rb.gravityScale;
        //rb.gravityScale = 0f;
        //if (rb.velocity.x > 0f)
        //{
        //    rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        //}
        //else { rb.velocity = new Vector2(rb.velocity.x * -dashingPower, 0f); }
        yield return new WaitForSeconds(dashingTime);
        //rb.gravityScale = NormalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
        
    }

    private void OnDrawGizmos()
    {
        //if (isGrounded)
        //{
           // Gizmos.color = Color.yellow;
            //Gizmos.DrawSphere(m_castPos.position, m_castradius);
       // }
        //else
       // {
           // Gizmos.color = Color.red;
            //Gizmos.DrawSphere(m_castPos.position, m_castradius);
       // }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("KillObject"))
        {
            Debug.Log("Die");
        }

        if(other.gameObject.CompareTag("Corner"))
        {    
            if(isCrouched) { nearEdge = true; } else { nearEdge = false; }
            
                       
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Corner"))
        {
           nearEdge = false;


        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(canBufferJump) 
        {
            Debug.Log("Jump");
            rb.velocity = new Vector2(rb.velocity.x, m_fJump);

        }
    }
}
