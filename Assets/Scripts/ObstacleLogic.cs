using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    Rest, LayEgg, Alert, Attack
}
public class ObstacleLogic : MonoBehaviour
{
    [SerializeField]
    public Transform spawnPoint;

    List<Transform> spawnPoints = new List<Transform>();

    Animator animator;
    SpriteRenderer renderer;

    bool isAttacking = false;

    EnemyState obstacleState = EnemyState.Rest;

    AudioSource audioSource;
    [SerializeField]
    AudioClip sfx;

    float t = 0.0f;

    [SerializeField]
    GameObject egg;
    [SerializeField]
    List<Sprite> eggs = new List<Sprite>();

    BoxCollider2D playerDetection;

    [SerializeField]
    GameObject angryMark;

    [SerializeField]
    GameObject questionMark;

    [SerializeField]
    GameObject alertZone;
    [SerializeField]
    Transform center;
    [SerializeField]
    float spanAngle;
    [SerializeField]
    int rayDensity;
    [SerializeField]
    float radius;

    List<ChickenController> detectedChickens = new List<ChickenController>();

    public GameObject thiefChicken;


    // Start is called before the first frame update
    void Start()
    {
        alertZone.SetActive(false);
        angryMark.SetActive(false);
        questionMark.SetActive(false);
        egg.SetActive(true);
        egg.GetComponent<SpriteRenderer>().enabled = false;
        //egg.SetActive(false);
        animator = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        playerDetection = GetComponent<BoxCollider2D>();
        playerDetection.enabled = false;
        //renderer.enabled = false;

        for(int i = 0; i < spawnPoint.childCount; i++)
        {
            spawnPoints.Add(spawnPoint.GetChild(i));
        }
    }

    void Update()
    {
        switch(obstacleState)
        {
            case (EnemyState.Rest):
                Rest();
                break;

            case (EnemyState.LayEgg):

                break;

            case (EnemyState.Alert):
                //Alert();
                break;

            case (EnemyState.Attack):
                Attack();
                break;
        }
        t -= Time.deltaTime;
    }

    public void SetState(EnemyState state)
    {
        obstacleState = state;
        switch(obstacleState)
        {
            case (EnemyState.Rest):
                animator.SetTrigger("isRest");
                playerDetection.enabled = false;
                t = 5.0f;
                break;

            case (EnemyState.LayEgg):
                playerDetection.enabled = false;
                egg.GetComponent<SpriteRenderer>().sprite = eggs[Random.Range(0, eggs.Count)];
                egg.GetComponent<SpriteRenderer>().enabled = true;
                //egg.SetActive(true);
                break;

            case (EnemyState.Alert):
                detectedChickens.Clear();
                questionMark.SetActive(true);
                animator.SetTrigger("isAlert");
                //Invoke("SearchForPlayer", 0.5f);
                //egg.SetActive(false);
                egg.GetComponent<SpriteRenderer>().enabled = false;
                t = 10.0f;
                break;

            case (EnemyState.Attack):

                playerDetection.enabled = false;
                animator.SetTrigger("isAttacking");
                
                t = 0.5f;
                break;
        }

    }

    void Rest()
    {
        if(t < 0)
        {
            SetState(EnemyState.LayEgg);
        }
    }

    void NewEgg()
    {

    }

    void Alert()
    {
        if(t < 0)
        {
            alertZone.SetActive(false);
            SetState(EnemyState.Rest);
            angryMark.SetActive(false);
        }
    }
    void SearchForPlayer()
    {
        questionMark.SetActive(false);
        angryMark.SetActive(true);
        playerDetection.enabled = true;
        animator.SetTrigger("isAlert");
    }

    void Attack()
    {
        if(t <= 0)
        {
            angryMark.SetActive(false);
            SetState(EnemyState.Rest);
        }
    }

    // Update is called once per frame
    // void FixedUpdate()
    // {   
    //     t -= Time.fixedDeltaTime;
    //     if(t <= 0.0f)
    //     {
    //         StartCoroutine(Spawn());
    //     }
    // }

    // void OnTriggerEnter2D(Collider2D other)
    // {
    //     if(other.tag == "Player")
    //     {
    //         SetState(EnemyState.Attack);
    //     }
    // }


    public IEnumerator Spawn()
    {
        if(!isAttacking)
        {
            isAttacking = true;
            //renderer.enabled = true;
            int i = Random.Range(0, spawnPoints.Count);
            transform.position = spawnPoints[i].position;
            transform.rotation = spawnPoints[i].rotation;
            yield return new WaitForSeconds(Random.Range(2, 5));
            animator.SetTrigger("isAttacking");
            yield return new WaitForSeconds(2.0f);
            //renderer.enabled = false;
            t = Random.Range(1, 3);
            isAttacking = false;
        }
        
    }

    public IEnumerator Alerting()
    {
        alertZone.SetActive(true);
        SetState(EnemyState.Alert);
        // float t = 0.0f;
        // while(t < 10.0f)
        // {
        //     Debug.Log(Look());
        //     t += Time.deltaTime;
        //     yield return null;
        // }
        yield return new WaitForSeconds(1.5f);
        alertZone.SetActive(false);
        questionMark.SetActive(false);
        if(Look())
        {
            angryMark.SetActive(true);
            SetState(EnemyState.Attack);
            yield return new WaitForSeconds(0.3f);
            for (int i = 0; i < detectedChickens.Count; i++)
            {
                detectedChickens[i].isKilled();
            }
            yield return new WaitForSeconds(0.7f);
            
            angryMark.SetActive(false);

            if(!detectedChickens.Contains(thiefChicken.GetComponent<ChickenController>()))
            {
                StartCoroutine(thiefChicken.GetComponent<ChickenController>().SuccessStealEgg());
            }
        }
        else
        {   
            SetState(EnemyState.Rest);
            StartCoroutine(thiefChicken.GetComponent<ChickenController>().SuccessStealEgg());
        }

    }

    public bool Look()
    {
        if (LookAround(Quaternion.identity, Color.green))
        {
            return true;
        }

        float subAngle = (spanAngle / 2) / rayDensity;
        for (int i = 0; i < rayDensity; i++)
        {
            if (LookAround(Quaternion.Euler(0, 0, -1 * subAngle * (i + 1)), Color.green) || LookAround(Quaternion.Euler(0, 0, subAngle * (i + 1)), Color.green))
            {
                return true;
            }
        }

        return false;
    }

    bool LookAround(Quaternion eulerAnger,Color DebugColor)
    {
        Debug.DrawRay(center.position, eulerAnger * -center.up.normalized * radius, DebugColor);

        RaycastHit2D hit;
        hit = Physics2D.Raycast(center.position, eulerAnger * -center.up.normalized, radius);
        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            if(!detectedChickens.Contains(hit.collider.gameObject.GetComponent<ChickenController>()))
            {
                detectedChickens.Add(hit.collider.gameObject.GetComponent<ChickenController>());
            }
            return true;
        }
        return false;
    }


}
