using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    [SerializeField] GameObject player;
    private bool playerInLava;
    private void Awake()
    {
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            playerInLava = true;
            var healthComponent = collision.GetComponent<PlayerHealth>();
            if (healthComponent != null)
            {
                StartCoroutine(c_Tickinglava());
                //healthComponent.TakeDamage(1);
            }           
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        playerInLava = false;
        StopCoroutine(c_Tickinglava());
    }

    IEnumerator c_Tickinglava()
    {
        while(playerInLava)
        {
            var healthComponent = player.GetComponent<PlayerHealth>();
            if (healthComponent != null)
            {
                healthComponent.TakeDamage(1);
            }
            yield return new WaitForSeconds(0.5f);

        }
    }
}
