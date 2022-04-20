using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{    
    private static UIController _instance;
    public static UIController Instance { get { return _instance; } }


    [SerializeField]
    SpriteRenderer henHead;
    [SerializeField]
    SpriteRenderer henUI;
    [SerializeField]
    SpriteRenderer roosterHead;
    [SerializeField]
    SpriteRenderer roosterUI;

    [SerializeField]
    List<Sprite> henHeads;
    [SerializeField]
    List<Sprite> roosterHeads;
    [SerializeField]
    List<Sprite> UIs;

    [SerializeField]
    GameObject henOtherUI;
    [SerializeField]
    GameObject roosterOtherUI;

    [SerializeField]
    public GameObject startBackground;

    [SerializeField]
    GameObject credits;

    [SerializeField]
    List<GameObject> listIDontWantAtTheEnd;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
        }
        else
        {
            _instance = this;
        }
       
    }
    void Start()
    {
        credits.SetActive(false);
        henHead.gameObject.SetActive(false);
        henUI.gameObject.SetActive(false);
        roosterHead.gameObject.SetActive(false);
        roosterUI.gameObject.SetActive(false);

        startBackground.SetActive(true);
        audioSource = GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void Show(int winner)
    {
        audioSource.Play();
        henOtherUI.SetActive(false);
        roosterOtherUI.SetActive(false);
        henHead.gameObject.SetActive(true);
        henUI.gameObject.SetActive(true);
        roosterHead.gameObject.SetActive(true);
        roosterUI.gameObject.SetActive(true);


        henHead.sprite = henHeads[winner];
        roosterHead.sprite = roosterHeads[2 - winner];
        if(winner == 2)
        {
            henUI.sprite = UIs[winner];
            roosterUI.sprite = UIs[winner];
        }
        else
        {
            henUI.sprite = UIs[winner];
            roosterUI.sprite = UIs[1 - winner];
        }

        StartCoroutine(ShowCredits());
        
    }

    IEnumerator ShowCredits()
    {
        yield return new WaitForSeconds(10.0f);
        credits.SetActive(true);
        henHead.gameObject.SetActive(false);
        henUI.gameObject.SetActive(false);
        roosterHead.gameObject.SetActive(false);
        roosterUI.gameObject.SetActive(false);

        for(int i = 0; i < listIDontWantAtTheEnd.Count; i++)
        {
            listIDontWantAtTheEnd[i].SetActive(false);
        }
    }
}
