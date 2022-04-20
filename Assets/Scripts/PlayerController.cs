using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    public ChickenController chic;
    [SerializeField]
    float factor = 0.01f;
    [SerializeField]
    float minMove = 0.01f;
    Vector3 curPos;
    Vector3 lastTurningPoint;

    [SerializeField]
    Camera cam;

    [SerializeField]
    float minAmplitude;
    [SerializeField]
    float maxAmplitude;

    Vector3 deltaMovement;
    float amplitude = 0.0f;
    float turnDis = 0.0f;

    void Start()
    {
        curPos = transform.position;
        lastTurningPoint = curPos;

    }

    void Update()
    {
        //Debug.Log(transform.position);
        //transform.position = camera.ScreenToWorldPoint(Input.mousePosition);

        float newMovement = deltaMovement.y * (lastTurningPoint - curPos).y;
        if (Mathf.Abs(deltaMovement.y) > 0.01f)
        {
            curPos = transform.position;
            if (newMovement > 0.0f)
            {
                turnDis = lastTurningPoint.y - transform.position.y;
                amplitude += Mathf.Abs(turnDis);
                
                //hasTurned = !hasTurned;
                //Debug.Log((lastTurningPoint - curPos).y);
                if ((lastTurningPoint - curPos).y < 0)
                {
                    //Debug.Log(amplitude);
                    chic.AddForceToChic(Mathf.Clamp(amplitude, minAmplitude, maxAmplitude) / factor);
                    //chic.AddForceToChic(amplitude / factor);
                    amplitude = 0.0f;
                }
                lastTurningPoint = transform.position;
            }
        }
        deltaMovement = transform.position - curPos;
        
    }

    public void SetPlayer(PlayerType type)
    {
        chic.playerNum = type;
    }
}
