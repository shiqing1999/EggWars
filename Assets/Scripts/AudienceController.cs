using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns3dRudder
{
	public class AudienceController : ILocomotion
	{
		private Vector4 axes;
        private Transform trans;

        Animator animator;


        [SerializeField]
        float cd = 10.0f;

        [SerializeField]
        float rainTime = 3.0f;

        float cdTime = 0.0f;

        AudioSource audioSource;
        // [SerializeField]
        // AudioClip clip;

		// Use this for initialization
		void Start()
		{
			audioSource = GetComponent<AudioSource>();
            axes = Vector4.zero;
            trans = transform;
            animator = GetComponent<Animator>();
		}

		// Vector4 X = Left/Right, Y = Up/Down, Z = Forward/Backward, W = Rotation
		public override void UpdateAxes(Controller3dRudder controller3dRudder, Vector4 axesWithFactor)
		{
            // save axesWithFactor like you want
            axes = axesWithFactor;
		}

		// Update is called once per frame
		void Update()
		{
			// mutliply saved axesWithFactor with the Time.deltaTime to apply translation or rotation            
			//trans.Translate(axes * Time.deltaTime);
			//trans.Rotate(0, 0, axes.w * Time.deltaTime);
			Vector3 newRot = new Vector3(0, 0, -axes.x * 3);
			trans.rotation = Quaternion.Euler(newRot);

			if(axes.z > 10.0f && cdTime <= 0.0f)
            {
            	StartCoroutine(Raining());
            	cdTime = 10.0f;
				//FIRE!!
            }
            cdTime -= Time.deltaTime;

			Vector3 newPos = transform.position;
			newPos.x = axes.w / 9;
			transform.position = newPos;
		}

        IEnumerator Raining()
		{
			audioSource.Play();
			animator.SetBool("isCloudRain", true);
			yield return new WaitForSeconds(3.0f);
			animator.SetBool("isCloudRain", false);
			audioSource.Stop();
		}


	}

	
}