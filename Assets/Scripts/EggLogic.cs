using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggLogic : MonoBehaviour
{
    [SerializeField]
    GameObject alertZone;
    // Start is called before the first frame update
    void Start()
    {
        alertZone.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player" && GetComponent<SpriteRenderer>().enabled)
        {
            ChickenController chic = other.gameObject.GetComponent<ChickenController>();
            if(!chic.hasStolenEgg && !chic.isBurnt)
            {
                //AlertEnemy();
                transform.parent.gameObject.GetComponent<ObstacleLogic>().thiefChicken = other.gameObject;
                chic.StealEgg(GetComponent<SpriteRenderer>().sprite);

                StartCoroutine(transform.parent.gameObject.GetComponent<ObstacleLogic>().Alerting());
            }
            

            //Invoke("AlertEnemy", 1.0f)
        }
    }


    void AlertEnemy()
    {
        transform.parent.gameObject.GetComponent<ObstacleLogic>().SetState(EnemyState.Alert);
    }

    

}
