using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour
{
    [SerializeField] private float explosionRange = 2f;
    [SerializeField] private LayerMask explodable;
    [SerializeField] CameraShake shake;
    [SerializeField] bool DestroyObjects;
    [SerializeField] float force;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Detonate()
    {
        Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, explosionRange, explodable);
        StartCoroutine(shake.Shake(.15f, .4f));

        foreach(Collider2D obj in objects)
        {
            if (DestroyObjects)
            {
                Destroy(obj.gameObject);

            }
            else
            {
                Vector2 dir = obj.transform.position - transform.position;
                obj.GetComponent<Rigidbody2D>().AddForce(dir * force);
            }
        }
    }
}
