using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectableLogic : MonoBehaviour
{

    SpriteRenderer renderer;
    Color c;
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        c = renderer.color;
        StartCoroutine(lifeTime());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator lifeTime()
    {
        yield return new WaitForSeconds(5.0f);
        for(int i = 0; i < 3; i++)
        {
            while(true)
            {
                c.a -= 0.05f;
                if(c.a < 0)
                {
                    c.a = 0;
                    renderer.color = c;
                    break;
                }
                renderer.color = c;
                yield return null;
            }
            yield return new WaitForSeconds(0.1f);
            c.a = 1.0f;
            renderer.color = c;
        }

        Destroy(gameObject);
        
    }
}
