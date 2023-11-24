using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;

public class SizeText : MonoBehaviour
{

    [SerializeField] float increament = 0.02f;
    private float resize;
    private TextMeshPro text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshPro>();
        StartCoroutine(C_Size());
    }

    // Update is called once per frame
    void Update()
    {
        //text.transform.localScale = new Vector3 (10, 10, 10);
    }

    IEnumerator C_Size()
    {
        //    resize = 1;
        //    while (true)
        //    {
        //    //resize = 1 + resize * Mathf.Sin(Time.deltaTime);
        //    resize++;
        //    text.transform.localScale = new Vector3(resize, resize, resize);
        //    yield return new WaitForSeconds(0.1f);
        //    resize--;
        //    text.transform.localScale = new Vector3(resize, resize, resize);
        //        yield return new WaitForSeconds(0.1f);
        //    }
        //}

        resize = 1;

        while (true)
        {
            while (true)
            {
                resize += increament;
                text.transform.localScale = new Vector3(resize, resize, resize);
                if (resize >= 1) break;
                yield return new FixedUpdate();
            }
            while (true)
            {
                resize -= increament;
                text.transform.localScale = new Vector3(resize, resize, resize);
                if (resize <= 1) break;
                yield return new FixedUpdate();
            }
        }
    }
}
