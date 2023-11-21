using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour
{
    [SerializeField] private float explosionRange = 2f;
    [SerializeField] private LayerMask explodable;
    [SerializeField] CameraShake shake;
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

        foreach(Collider2D obj in objects)
        {
            StartCoroutine(shake.Shake(.15f, .4f));
            Destroy(obj.gameObject);
        }
    }
}
