using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerType
{
    Rooster, Hen
}
public class ChickenController : MonoBehaviour
{
    public PlayerController playerController;
    [SerializeField]
    public PlayerType playerNum;
    [SerializeField]
    KeyCode input;
    [SerializeField]
    bool isUsingKeyboard = true;

    [SerializeField]
    float horizontalForce = 1.0f;

    [SerializeField]
    float verticalForce = 1.0f;



    Vector3 curPos;

    Rigidbody2D rig;
    Vector3 initPos;

    AudioSource audioSource;
    Animator animator;

    [SerializeField]
    AudioClip hitSound;
    [SerializeField]
    AudioClip successSound;
    [SerializeField]
    AudioClip flipSound;
    [SerializeField]
    AudioClip burntSound;
    [SerializeField]
    AudioClip disappointedSound;
    [SerializeField]
    AudioClip yaySound;

    bool isRespawning = false;

    [SerializeField]
    Text playerText;

    [SerializeField]
    Transform eggSpawnPoint;
    public List<GameObject> playerEggs = new List<GameObject>();

    public int eggCnt = 4;

    [SerializeField]
    Slider slider;

    public ChickenController otherPlayer;

    [SerializeField]
    List<GameObject> carriedEggs;
    public bool hasStolenEgg = false;

    [SerializeField]
    float wetParam = 1.5f;
    float nowParam = 1.0f;

    public GameObject eggStolen;

    [SerializeField]
    GameObject bumpEffect;
    [SerializeField]
    AudioClip bounceSound;

    [SerializeField]
    GameObject nestEggsParent;
    List<GameObject> nestEggs = new List<GameObject>();

    bool isWin = false;

    public Sprite newEggSprite;

    public bool isWet = false;
    public bool isBurnt = false;
    public bool hasStealed = false;
    // Start is called before the first frame update
    void Start()
    {
        if(bumpEffect)
        {
            bumpEffect.SetActive(false);
        }
        for(int i = 0; i < carriedEggs.Count; i++)
        {
            carriedEggs[i].SetActive(false);
        }
        //stolenEgg.SetActive(false);
        rig = GetComponent<Rigidbody2D>();
        ResetHorizontalForce(horizontalForce);
        
        curPos = transform.position;
        initPos = transform.position;

        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        animator.SetBool("isFlying", true);
        //StartCoroutine(DisableCollider());


        for(int i = 0; i < nestEggsParent.transform.childCount; i++)
        {
            nestEggs.Add(nestEggsParent.transform.GetChild(i).gameObject);
            nestEggs[i].SetActive(i < eggCnt);
        }


    }

    // Update is called once per frame
    void Update()
    {
        //rig.AddForce(new Vector2(horizontalForce, 0.0f));
        //curPos -= new Vector3(1.0f, 0.0f, 0.0f) * Time.deltaTime;

        //transform.position = curPos;

        if(isUsingKeyboard && !isRespawning && Input.GetKeyDown(input))
        {
            ResetVelocity(false);
            rig.AddForce(new Vector2(0.0f, verticalForce), ForceMode2D.Impulse);
        }

        if(transform.position.y <= -10.0f)
        {
/*            Vector3 newPos = transform.position;
            newPos.y += 20f;
            transform.position = newPos;*/
            StartCoroutine(Reborn());
        }
        else if (transform.position.y >= 10.0f)//15
        {
/*            Vector3 newPos = transform.position;
            newPos.y -= 20f;
            transform.position = newPos;*/
            StartCoroutine(Reborn());
        }

        if(transform.position.x <= -20.0f)
        {
            Vector3 newPos = transform.position;

            newPos.x += 35.5f;
            //Debug.Log(newPos.x);
            transform.position = newPos;
        }
        else if(transform.position.x >= 20.0f)
        {
            Vector3 newPos = transform.position;
            newPos.x -= 35.5f;
            //Debug.Log(newPos.x);
            transform.position = newPos;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(!hasStolenEgg && (playerNum == PlayerType.Hen && other.tag == "Rooster" || playerNum == PlayerType.Rooster && other.tag == "Hen"))
        {
            eggStolen = otherPlayer.LoseEgg();
            for(int i = 0; i < carriedEggs.Count; i++)
            {
                carriedEggs[i].SetActive(true);
            }
            //GetEgg(eggStolen);

            //hasStolenEgg = true;
            //StartCoroutine(Reborn());

            StartCoroutine(SuccessStealEgg());
            //GetEgg(otherPlayer.LoseEgg());
            
            // GameManager.Instance
        }
        // else if(hasStolenEgg && (playerNum == PlayerType.Hen && other.tag == "Hen" || playerNum == PlayerType.Rooster && other.tag == "Rooster"))
        // {

        //     for(int i = 0; i < carriedEggs.Count; i++)
        //     {
        //         carriedEggs[i].SetActive(false);
        //     }
        //     hasStolenEgg = false;
        //     GetEgg(eggStolen);

        // }
        // else if(other.tag == "Obstacle")
        // {
        //     // audioSource.PlayOneShot(hitSound);
        //     // StartCoroutine(Reborn());
        // }
        else if(other.tag == "Player" && !hasStealed)
        {

            //audioSource.PlayOneShot(hitSound);
            StartCoroutine(HitOtherPlayer());
            //StartCoroutine(Reborn());
        }

        else if(other.tag == "Rain")
        {
            if(!isBurnt && !hasStealed)
            {
                GameManager.Instance.cloudCnt++;
                isWet = true;
                StartCoroutine(Wet());
            }
            
        }

        else if(other.tag == "Fire")
        {
            if (!isBurnt && !hasStealed)
            {
                isBurnt = true;
                GameManager.Instance.fireCnt++;
                ResetVelocity(true, false);
                animator.SetTrigger("isBurnt");
                nowParam = 1000.0f;
            }
            for (int i = 0; i < carriedEggs.Count; i++)
            {
                carriedEggs[i].SetActive(false);
            }
            
        }
    }

    IEnumerator HitOtherPlayer()
    {

        if(bumpEffect)
        {
            
            bumpEffect.SetActive(true);
        }

        if(bounceSound)
        {
            audioSource.PlayOneShot(bounceSound);
        }

        rig.AddForce(new Vector2(0.0f, -15 * ((int)playerNum - 0.5f)), ForceMode2D.Impulse);
        nowParam = 1000.0f;
        ResetVelocity(true);
        ResetHorizontalForce(-horizontalForce);
        yield return new WaitForSeconds(0.75f);
        if(bumpEffect)
        {
            bumpEffect.SetActive(false);
        }
        yield return new WaitForSeconds(0.25f);
        rig.AddForce(new Vector2(0.0f, 15 * ((int)playerNum - 0.5f)), ForceMode2D.Impulse);
        nowParam = 1.0f; 
        ResetVelocity(true, true, true);
        ResetHorizontalForce(horizontalForce);
    }


    IEnumerator Reborn()
    {
        isBurnt = false;
        hasStealed = false;
        if(isWin)
        {
            isWin = false;
        }
        else
        {
            audioSource.PlayOneShot(disappointedSound);
        }
        
        //audioSource.PlayOneShot(hitSound);
        //animator.SetBool("isBurnt", false);
        nowParam = 1000.0f;
        animator.SetBool("isFlying", false);
        //GetComponent<Collider2D>().enabled = false;
        isRespawning = true;
        transform.position = initPos;
        ResetVelocity(true, true, true);
        rig.gravityScale = 0.0f;
        rig.isKinematic = true;
        animator.SetTrigger("Reborn");

        for(int i = 0; i < carriedEggs.Count; i++)
        {
            carriedEggs[i].SetActive(false);
        }
        hasStolenEgg = false;

        yield return new WaitForSeconds(1.0f);
        nowParam = 1.0f;
        horizontalForce = 3.0f;
        rig.isKinematic = false;
        ResetHorizontalForce(horizontalForce);
        rig.gravityScale = 1.0f;
        
        isRespawning = false;
        animator.SetBool("isFlying", true);


        //yield return new WaitForSeconds(2.0f);
        //GetComponent<Collider2D>().enabled = true;
    }

    IEnumerator Wet()
    {
        ResetVelocity(true, false);
        horizontalForce /= 1.5f;
        ResetHorizontalForce(horizontalForce);
        nowParam *= wetParam;
        animator.SetBool("isWet", true);
        float wetT = 0.0f;
        while(!isBurnt && !hasStealed)
        {
            wetT += Time.deltaTime;
            if(wetT > 5.0f)
            {
                break;
            }
            yield return null;
        }
        //yield return new WaitForSeconds(5.0f);
        if(nowParam == wetParam)
        {
            nowParam /= wetParam;
        }
        animator.SetBool("isWet", false);
        ResetVelocity(true, false);
        horizontalForce *= 1.5f;
        ResetHorizontalForce(horizontalForce);
        isWet = false;
    }

    IEnumerator DisableCollider()
    {
        GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(2.0f);
        GetComponent<Collider2D>().enabled = true;
    }

    void ResetVelocity(bool isHorizontal, bool isVertical = true, bool isReborn = false)
    {

        Vector2 newVelocity = rig.velocity;
        if(isHorizontal)
        {
            newVelocity.x = 0.0f;
        }
        if(isVertical)
        {
            if(!isReborn)
            {
                if (newVelocity.y < 0)
                {
                    newVelocity.y = 0.0f;
                }
            }
            else
            {
                newVelocity.y = 0.0f;
            }
        }
        
        rig.velocity = newVelocity;
    }

    public void ResetHorizontalForce(float f)
    {
        if(f > 100)
        {
            f = horizontalForce;
        }
        if((int)playerNum == 0)
        {
            rig.AddForce(new Vector2(-f, 0.0f), ForceMode2D.Impulse);
        }
        else
        {
            rig.AddForce(new Vector2(f, 0.0f), ForceMode2D.Impulse);
        }
    }


    public void AddForceToChic(float force)
    {
        if(!isRespawning)
        {
            ResetVelocity(false);
            rig.AddForce(new Vector2(0.0f, force / nowParam), ForceMode2D.Impulse);
        }
    }

    // void GetEgg(GameObject egg)
    // {
        
    //     audioSource.PlayOneShot(successSound);
    //     Vector3 newPos = playerEggs[playerEggs.Count - 1].transform.position;

    //     newPos.x += ((float)playerNum - 0.5f) * 2 * GameManager.Instance.spaceDis;
    //     egg.transform.position = newPos;
    //     playerEggs.Add(egg);

    //     if(this.name == "Hen")
    //     {
    //         slider.value = eggCnt;
    //     }

    //     if(playerEggs.Count == 8)
    //     {
    //         GameManager.Instance.Win((int)playerNum);
    //     }
    //     eggCnt = playerEggs.Count;
    //     GameManager.Instance.SetSlider();

    // }

    void GetEgg(GameObject egg)
    {
        
        audioSource.PlayOneShot(successSound);
        playerEggs.Add(egg);
        eggCnt = playerEggs.Count;
        if(eggCnt <= nestEggs.Count)
        {
            nestEggs[eggCnt - 1].SetActive(true);
            nestEggs[eggCnt - 1].GetComponent<SpriteRenderer>().sprite = newEggSprite;
        }


        if(otherPlayer.eggCnt == 0)
        //if(playerEggs.Count == 8 || otherPlayer.eggCnt == 0)
        {
            GameManager.Instance.Win(1 - (int)playerNum);
        }
        
        GameManager.Instance.SetSlider();
    }

    GameObject LoseEgg()
    {
        GameObject lostEgg = playerEggs[playerEggs.Count - 1];
        if(eggCnt <= nestEggs.Count)
        {
            otherPlayer.newEggSprite = nestEggs[eggCnt - 1].GetComponent<SpriteRenderer>().sprite;
            nestEggs[eggCnt - 1].SetActive(false);
        }
        
        // if(playerEggs.Count == 1)
        // {
        //     return null;
        // }
        playerEggs.RemoveAt(playerEggs.Count - 1);
        eggCnt = playerEggs.Count;
        return lostEgg;
    }

    public void PlayWingSound()
    {
        audioSource.PlayOneShot(flipSound);
    }

    public void StealEgg(Sprite egg)
    {

        hasStolenEgg = true;
        eggStolen = Instantiate(carriedEggs[0]) as GameObject;
        eggStolen.SetActive(false);
        newEggSprite = egg;

        for(int i = 0; i < carriedEggs.Count; i++)
        {
            carriedEggs[i].SetActive(true);
            carriedEggs[i].GetComponent<SpriteRenderer>().sprite = egg;
        }
        //stolenEgg.SetActive(true);
        //stolenEgg.GetComponent<SpriteRenderer>().sprite = egg;
    }

    public void PlayOvenSound()
    {
        audioSource.PlayOneShot(burntSound);
    }

    public void isKilled()
    {
        audioSource.PlayOneShot(hitSound);
        StartCoroutine(Reborn());
    }

    public IEnumerator SuccessStealEgg()
    {
        isWin = true;
        hasStealed = true;
        for (int i = 0; i < carriedEggs.Count; i++)
        {
            carriedEggs[i].SetActive(false);
        }
        ResetVelocity(true, true, true);
        nowParam = 1000f;
        rig.gravityScale = 0.0f;
        rig.isKinematic = true;
        animator.SetTrigger("isThief");
        audioSource.PlayOneShot(yaySound);
        yield return new WaitForSeconds(2.0f);
        nowParam = 1.0f;
        rig.gravityScale = 1.0f;
        rig.isKinematic = false;
        yield return StartCoroutine(Reborn());
        GetEgg(eggStolen);
        hasStealed = false;
    }
        
}
