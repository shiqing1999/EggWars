using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using UnityEngine.UI;

public class CheckDevice : MonoBehaviour
{
    private static CheckDevice _instance;
    public static CheckDevice Instance { get { return _instance; } }
    // Start is called before the first frame update

    [SerializeField]
    GameObject[] controllers = new GameObject[2];

    [SerializeField]
    ChickenController hen;
    [SerializeField]
    ChickenController rooster;

    [SerializeField]
    GameObject trackerPrefab;

    [SerializeField]
    SpriteRenderer henHead;
    [SerializeField]
    SpriteRenderer roosterHead;

    [SerializeField]
    List<Sprite> henHeads;
    [SerializeField]
    List<Sprite> roosterHeads;

    Vector3[] lastPos = new Vector3[2];

    [SerializeField]
    Text instruction;
    [SerializeField]
    GameObject startLogo;
    [SerializeField]
    GameObject gameLogo;
    [SerializeField]
    GameObject fightScene;
    bool isDevicesDetected = false;
    [SerializeField]
    List<GameObject> texts = new List<GameObject>();

    List<GameObject> allTrackers = new List<GameObject>();

    AudioSource audioSource;
    [SerializeField]
    AudioClip[] connectedSounds = new AudioClip[3];

    [SerializeField]
    GameObject score;

    [SerializeField]
    AudioClip fightSound;

    [SerializeField]
    GameObject startBackground;

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
        Restart();
    }

    public void Restart()
    {
        for(int i = 0; i < texts.Count; i++)
        {
            texts[i].SetActive(false);
        }
        score.SetActive(false);
        startLogo.SetActive(false);
        fightScene.SetActive(false);
        controllers[0].GetComponent<PlayerController>().enabled = false;
        controllers[1].GetComponent<PlayerController>().enabled = false;
        StartCoroutine(PlayLogo());

        henHead.sprite = henHeads[0];
        roosterHead.sprite = roosterHeads[0];
        audioSource = GetComponent<AudioSource>();

        instruction.text = "";
        henHead.gameObject.SetActive(false);
        roosterHead.gameObject.SetActive(false);
        //StartCoroutine(SearchForControllers());
    }

    IEnumerator PlayLogo()
    {
        gameLogo.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        gameLogo.SetActive(false);
        henHead.gameObject.SetActive(true);
        roosterHead.gameObject.SetActive(true);
        yield return StartCoroutine(SearchForControllers());
    }


    IEnumerator SearchForControllers()
    {
        while(true)
        {
            //instruction.text = "Searching for devices...";
            uint index = 0;
            var error = ETrackedPropertyError.TrackedProp_Success;
            for (uint i = 0; i < 16; i++)
            {
                var result = new System.Text.StringBuilder((int)64);
                OpenVR.System.GetStringTrackedDeviceProperty(i, ETrackedDeviceProperty.Prop_RenderModelName_String, result, 64, ref error);

                if (result.ToString().Contains("tracker"))
                {
                    GameObject newTracker = Instantiate(trackerPrefab, Vector3.zero, Quaternion.identity) as GameObject;
                    newTracker.GetComponent<SteamVR_TrackedObject>().index = (SteamVR_TrackedObject.EIndex)i;
                    allTrackers.Add(newTracker);
                }
            }

            if(allTrackers.Count >= 2)
            {
                break;
            }
            allTrackers.Clear();

            yield return new WaitForSeconds(1.0f);
        }

        yield return StartCoroutine(AssignTrackers());


    }

    // Update is called once per frame
    void Update()
    {
        
    }


    IEnumerator AssignTrackers()
    {
        texts[0].SetActive(true);
        //instruction.text = "Player on the left flip your arm like a chicken!!!";

        yield return StartCoroutine(SearchForViveTrackers(0));
        audioSource.PlayOneShot(connectedSounds[0]);
        henHead.sprite = henHeads[1];
        texts[0].SetActive(false);
        texts[1].SetActive(true);
        //instruction.text = "Player on the right flip your arm like a chicken!!!";
        yield return StartCoroutine(SearchForViveTrackers(1));
        audioSource.PlayOneShot(connectedSounds[1]);
        roosterHead.sprite = roosterHeads[1];
        yield return new WaitForSeconds(2.0f);
        texts[1].SetActive(false);

        // instruction.text = "All Set!! Let's do a fist bump!!";

        // while(true)
        // {
        //     if(Vector3.Distance(controllers[1].transform.position, controllers[0].transform.position) <= 0.1f)
        //     {
        //         break;
        //     }
        //     yield return null;
        // }


        texts[2].SetActive(true);
        //instruction.text = "Fly to the other side and rob the eggs!! Ready?";
        audioSource.PlayOneShot(connectedSounds[2]);
        startLogo.SetActive(true);
        henHead.gameObject.SetActive(false);
        roosterHead.gameObject.SetActive(false);
        yield return new WaitForSeconds(5.0f);
        texts[2].SetActive(false);

        audioSource.PlayOneShot(fightSound);
        fightScene.SetActive(true);
        instruction.gameObject.SetActive(false);
        startLogo.SetActive(false);
        yield return new WaitForSeconds(2.6f);
        score.SetActive(true);

        controllers[0].GetComponent<PlayerController>().enabled = true;
        controllers[1].GetComponent<PlayerController>().enabled = true;

        instruction.gameObject.SetActive(false);
        GameManager.Instance.GMStartGame();
        startBackground.SetActive(false);

        int j = allTrackers.Count;
        for(int i = 0; i < j; i++)
        {
            GameObject tracker = allTrackers[0];
            allTrackers.RemoveAt(0);
            Destroy(tracker);
        }
        
        yield break;
        
    }


    // IEnumerator SearchForControllers()
    // {
    //     while(true)
    //     {
    //         instruction.text = "Searching for devices...";
    //         uint index = 0;
    //         var error = ETrackedPropertyError.TrackedProp_Success;
    //         for (uint i = 0; i < 16; i++)
    //         {
    //             var result = new System.Text.StringBuilder((int)64);
    //             OpenVR.System.GetStringTrackedDeviceProperty(i, ETrackedDeviceProperty.Prop_RenderModelName_String, result, 64, ref error);

    //             if (result.ToString().Contains("tracker"))
    //             {
    //                 if(index == 0)
    //                 {
    //                     controllers[0].GetComponent<SteamVR_TrackedObject>().index = (SteamVR_TrackedObject.EIndex)i;
    //                     index = i;
    //                 }
    //                 else
    //                 {
    //                     controllers[1].GetComponent<SteamVR_TrackedObject>().index = (SteamVR_TrackedObject.EIndex)i;
    //                     SetLastPos();
    //                     isDevicesDetected = true;
    //                     break;
    //                 }
    //             }
    //         }

    //         if (isDevicesDetected)
    //         {
    //             break;
    //         }
    //         yield return new WaitForSeconds(1.0f);
    //     }

    //     yield return StartCoroutine(AssignTrackers());


    // }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }


    // IEnumerator AssignTrackers()
    // {
    //     float movement1 = 0.0f;
    //     float movement2 = 0.0f;
    //     instruction.text = "Player 1 wave your Vive Tracker!!!";
    //     // movement1 
    //     float t = 3.0f;
    //     while(true)
    //     {
    //         while(t >= 0)
    //         {
    //             movement1 += Vector3.Distance(lastPos[0], controllers[0].transform.position);
    //             movement2 += Vector3.Distance(lastPos[1], controllers[1].transform.position);
    //             SetLastPos();
    //             yield return new WaitForFixedUpdate();
    //             t -= Time.fixedDeltaTime;
    //         }
    //         if(Mathf.Max(movement1, movement2) >= 5.0f)
    //         {
    //             break;
    //         }
    //         instruction.text = "Player1 not detected. Please try again!";
    //         yield return new WaitForSeconds(3.0f);
    //         t = 3.0f;
    //         movement1 = 0.0f;
    //         movement2 = 0.0f;
    //     }
    //     int player1;

    //     if(movement1 > movement2)
    //     {
    //         controllers[0].GetComponent<PlayerController>().chic = hen;
    //         player1 = 0;
    //     }
    //     else
    //     {
    //         controllers[1].GetComponent<PlayerController>().chic = hen;
    //         player1 = 1;
    //     }

    //     movement1 = 0.0f;
    //     t = 3.0f;
    //     instruction.text = "Player 2 wave your Vive Tracker!!!";
    //     yield return new WaitForSeconds(1.0f);
    //     while(true)
    //     {
    //         while(t >= 0)
    //         {
    //             movement1 += Vector3.Distance(lastPos[1 - player1], controllers[1 - player1].transform.position);
    //             SetLastPos();
    //             yield return new WaitForFixedUpdate();
    //             t -= Time.fixedDeltaTime;
    //         }
    //         if(movement1 >= 5.0f)
    //         {
    //             break;
    //         }
    //         instruction.text = "Player2 not detected. Please try again!";
    //         yield return new WaitForSeconds(3.0f);
    //         t = 3.0f;
    //         movement1 = 0.0f;
    //     }
    //     controllers[1 - player1].GetComponent<PlayerController>().chic = rooster;

    //     instruction.text = "All Set!! Let's do a fist bump!!";
    //     while(true)
    //     {
    //         if(Vector3.Distance(controllers[1].transform.position, controllers[0].transform.position) <= 0.1f)
    //         {
    //             break;
    //         }
    //         yield return null;
    //     }

    //     instruction.text = "Ready? Set? Go!!!";

    //     yield return new WaitForSeconds(3.0f);

    //     controllers[0].GetComponent<PlayerController>().enabled = true;
    //     controllers[1].GetComponent<PlayerController>().enabled = true;

    //     instruction.gameObject.SetActive(false);
    //     GameManager.Instance.GMStartGame();
    //     transform.parent.gameObject.SetActive(false);
        
    //     yield break;
        
    // }

    // void SetLastPos()
    // {
    //     lastPos[0] = controllers[0].transform.position;
    //     lastPos[1] = controllers[1].transform.position;
    // }




    IEnumerator SearchForViveTrackers(int playerNum)
    {
        List<float> movements = new List<float>();
        for(int i = 0; i < allTrackers.Count; i++)
        {
            movements.Add(0);
        }
        //float[] movements = new float[allTrackers.Count];
        Vector3[] lastPoses = new Vector3[allTrackers.Count];

        float t = 3.0f;
        while(true)
        {
            while(t >= 0)
            {
                
                
                for(int i = 0; i < allTrackers.Count; i++)
                {
                    movements[i] += Vector3.Distance(lastPoses[i], allTrackers[i].transform.position);
                    lastPoses[i] = allTrackers[i].transform.position;
                }

                yield return new WaitForFixedUpdate();
                t -= Time.fixedDeltaTime;
            }

            if (Mathf.Max(movements.ToArray()) >= 5.0f)
            {
                break;
            }

            //instruction.text = "Controller not detected. Please try again!";
            //yield return new WaitForSeconds(3.0f);
            t = 3.0f;
            for(int i = 0; i < allTrackers.Count; i++)
            {
                movements[i] = 0.0f;
            }
        }

        GameObject controller = allTrackers[movements.IndexOf(Mathf.Max(movements.ToArray()))];
        controllers[playerNum].GetComponent<SteamVR_TrackedObject>().index = controller.GetComponent<SteamVR_TrackedObject>().index;
        Debug.Log((int)controller.GetComponent<SteamVR_TrackedObject>().index);
        int idx = allTrackers.IndexOf(controller);
        allTrackers.RemoveAt(idx);
        
        //controllers[i].GetComponent<PlayerController>().chic = Playe;
        yield return null;
    }

}
