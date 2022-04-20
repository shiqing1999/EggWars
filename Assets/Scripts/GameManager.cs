using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    [SerializeField]
    ChickenController[] players = new ChickenController[2];

    [SerializeField]
    float gameTime = 300.0f;
    float t = 0.0f;

    [SerializeField]
    GameObject eggParent;
    List<GameObject> eggs = new List<GameObject>();

    [SerializeField]
    Transform[] eggSpawnPoints = new Transform[2];
    [SerializeField]
    public float spaceDis = 1.5f;
    [SerializeField]
    Text timeText;

    AudioSource audioSource;

    [SerializeField]
    GameObject cloud;
    [SerializeField]
    GameObject fire;

    [SerializeField]
    GameObject snake;
    [SerializeField]
    GameObject eagle;

    [SerializeField]
    Slider slider;

    [SerializeField]
    Text henScore;
    [SerializeField]
    Text roosterScore;
    [SerializeField]
    GameObject restartButton;

    public bool hasWin = false;

    public int cloudCnt = 0;
    public int fireCnt = 0;

    public bool isGameEnd = false;


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
        restartButton.SetActive(false);
        snake.SetActive(false);
        eagle.SetActive(false);
        for(int i = 0; i < 2; i++)
        {
            players[i].otherPlayer = players[1 - i];
            players[i].gameObject.SetActive(false);
        }
        
        for(int i = 0; i < eggParent.transform.childCount; i++)
        {
            eggs.Add(eggParent.transform.GetChild(i).gameObject);
        }

        for(int i = 0; i < players[0].eggCnt; i++)
        {
            Vector3 newPos = eggSpawnPoints[0].position;
            newPos.x += i * spaceDis;
            eggs[i].transform.position = newPos;
            players[0].playerEggs.Add(eggs[i]);
        }

        for(int i = players[0].eggCnt; i < eggs.Count; i++)
        {
            Vector3 newPos = eggSpawnPoints[1].position;
            newPos.x -= (i - players[0].eggCnt) * spaceDis;
            eggs[i].transform.position = newPos;
            players[1].playerEggs.Add(eggs[i]);
        }
        timeText.text = "";
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(GameManager.Instance.StartGame());
        slider.value = players[0].eggCnt;

        Invoke("AddObstacle", 60.0f);
        //StartGame();
    }

    IEnumerator StartGame()
    {
        UIController.Instance.startBackground.SetActive(false);
        audioSource.Play();
        yield return StartCoroutine(OpeningCountDown());
        

        for (int i = 0; i < 2; i++)
        {
            players[i].otherPlayer = players[1 - i];
            players[i].gameObject.SetActive(true);
            players[i].gameObject.GetComponent<Animator>().SetBool("isFlying", true);
            players[i].ResetHorizontalForce(999f);
        }
        timeText.text = "Time: 3:00";
        t = gameTime;
        
        yield return StartCoroutine(CountDown());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Win(int player)
    {
        isGameEnd = true;
        for (int i = 0; i < 2; i++)
        {
            players[i].gameObject.SetActive(false);
        }
        for(int i = 0; i < eggs.Count; i++)
        {
            eggs[i].SetActive(false);
        }
        hasWin = true;
        UIController.Instance.Show(player);
        /*        fire.SetActive(false);
                cloud.SetActive(false);*/
        StartCoroutine(fire.GetComponent<FInalDrop>().showResult());
        StartCoroutine(cloud.GetComponent<FInalDrop>().showResult());
        snake.SetActive(false);
        eagle.SetActive(false);
    }

    IEnumerator CountDown()
    {
        while(true)
        {
            if(hasWin)
            {
                yield break;
            }
            t -= Time.fixedDeltaTime;
            string minutes = Mathf.Floor(t / 60).ToString("00");
            string seconds = (t % 60).ToString("00");

            timeText.text = "Time: " + minutes + ":" + seconds;

            if (t <= 0)
            {
                CheckResult();
                timeText.text = "Time's Up";
                yield break;
            }

            yield return new WaitForFixedUpdate();

            
        }
    }

    void CheckResult()
    {
        if(players[0].eggCnt > players[1].eggCnt)
        {
            Win(0);
        }
        else if(players[0].eggCnt < players[1].eggCnt)
        {
            Win(1);
        }
        else if(players[0].eggCnt == players[1].eggCnt)
        {
            Win(2);
        }
        restartButton.SetActive(true);
    }

    IEnumerator OpeningCountDown()
    {
        timeText.text = "3";
        yield return new WaitForSeconds(1.0f);
        timeText.text = "2";
        yield return new WaitForSeconds(1.0f);
        timeText.text = "1";
        yield return new WaitForSeconds(1.0f);
    }

    public void GMStartGame()
    {
        StartCoroutine(StartGame());
    }

    public void SetSlider()
    {
        henScore.text = "" + players[0].eggCnt;
        roosterScore.text = "" + players[1].eggCnt;
        slider.maxValue = players[0].eggCnt + players[1].eggCnt;
        slider.value = players[0].eggCnt;
    }

    void AddObstacle()
    {
        snake.SetActive(true);
        eagle.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    // public void Score(Player player)
    // {
    //     if(player == Player.Player1)
    //     {

    //     }
    // }
}
