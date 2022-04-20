using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns3dRudder
{
	public class FireController : ILocomotion
	{
		private Vector4 axes;
        private Transform trans;

        List<float> deltaMovements = new List<float>();
        float lastRot = 0.0f;

        [SerializeField]
        float cd = 10.0f;
        [SerializeField]
        float t = 0.0f;

        Animator animator;
        AudioSource audioSource;

		// Use this for initialization
		void Start()
		{
            axes = Vector4.zero;
            trans = transform;
            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();

        }

		// Vector4 X = Left/Right, Y = Up/Down, Z = Forward/Backward, W = Rotation
		public override void UpdateAxes(Controller3dRudder controller3dRudder, Vector4 axesWithFactor)
		{
            // save axesWithFactor like you want
            axes = axesWithFactor;
		}

		// Update is called once per frame
		void FixedUpdate()
		{
            // mutliply saved axesWithFactor with the Time.deltaTime to apply translation or rotation            
        	if(deltaMovements.Count >= 60)    
        	{
        		deltaMovements.RemoveAt(0);
        	}
        	deltaMovements.Add(Mathf.Abs(axes.x - lastRot));
        	lastRot = axes.x;
        	float sum = 0.0f;

        	for(int i = 0; i < deltaMovements.Count; i++)
        	{
        		sum += deltaMovements[i];
        	}

            if (t <= 0.0f)
            {
                if (sum >= 70)
                {
                    audioSource.Play();
                    animator.SetBool("isFireOn", true);
                }
                else if (sum <= 35)
                {
                    audioSource.Stop();
                    animator.SetBool("isFireOn", false);
                }
                t = cd;
            }
            t -= Time.deltaTime;
            
        }


        void Update()
        {
            Vector3 newPos = transform.position;
            newPos.x = axes.w / 9;
            transform.position = newPos;
        }
	}
}