using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns3dRudder
{

	public class CollectionLogic : ILocomotion
	{
		private Vector4 axes;
        private Transform trans;


        [SerializeField]
        List<Sprite> stages;

        [SerializeField]
       	List<GameObject> collectibles;

       	[SerializeField]
       	List<BoxCollider2D> colliders;

       	SpriteRenderer renderer;
       	public int stage = 0;

        bool isFunctioning = false;

        Vector3 newPos;

        [SerializeField]
        KeyCode[] keycodes = new KeyCode[2];

        [SerializeField]
        float yOffset = 0.0f;
        [SerializeField]
        float xOffset = 2.0f;




		// Use this for initialization
		void Start()
		{
            
            axes = Vector4.zero;
            trans = transform;
            renderer = GetComponent<SpriteRenderer>();
            renderer.sprite = stages[stage];
            colliders[0].enabled = false;
            for(int i = 1; i < colliders.Count; i++)
            {
            	colliders[i].enabled = false;
            }
            StartCoroutine(spawnNewCollectibles());
            stage = 0;
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
            Vector3 newPos = transform.position;
            newPos.x = axes.x + xOffset;
            transform.position = newPos;

            // if(Input.GetKey(keycodes[0]))
            // {
            // 	newPos = transform.position;
	           //  newPos.x -= 8.0f * Time.deltaTime;
	           //  transform.position = newPos;
            // }
            // else if(Input.GetKey(keycodes[1]))
            // {
            // 	newPos = transform.position;
	           //  newPos.x += 8.0f * Time.deltaTime;
	           //  transform.position = newPos;
            // }
            

        }

        void OnTriggerEnter2D(Collider2D other)
        {	
        	if(other.tag == "Collectibles")
        	{
                Destroy(other.gameObject);
        		colliders[stage].enabled = false;
        		
        		stage++;

        		
        		renderer.sprite = stages[stage];
        		if(stage == 3)
        		{
                    colliders[stage].enabled = true;
                    StartCoroutine(startFunction());
        		}
        	}
        }

        IEnumerator startFunction()
        {
            if (!isFunctioning)
            {
                isFunctioning = true;
                for (int i = 0; i < 10; i++)
                {
                    renderer.sprite = stages[3 + i % 2];
                    yield return new WaitForSeconds(0.5f);
                }
                colliders[stage].enabled = false;
                stage = 0;

                //colliders[stage].enabled = true;
                renderer.sprite = stages[stage];
                isFunctioning = false;
            }
            else
            {
                yield break;
            }
        	
        }

        IEnumerator spawnNewCollectibles()
        {
        	yield return new WaitForSeconds(10.0f);
        	while(!GameManager.Instance.isGameEnd)
        	{
        		if(stage < 3)
        		{
        			Instantiate(collectibles[Random.Range(0, collectibles.Count)], new Vector3(Random.Range(-15.0f, 15.0f), transform.position.y + yOffset, 0.0f), Quaternion.identity);
        		}
                yield return new WaitForSeconds(8.0f);
        	}
        }

        


	}
}