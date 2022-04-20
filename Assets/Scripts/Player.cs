using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField]
    PlayerType type;
    [SerializeField]
    Text playerText;

    [SerializeField]
    Transform eggSpawnPoint;

    public int eggCnt = 4;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
