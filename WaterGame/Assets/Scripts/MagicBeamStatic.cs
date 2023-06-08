using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Scripts
{

public class MagicBeamStatic : MonoBehaviour
{

        [SerializeField]
        private string fire2String;

        [Header("Prefabs")]
    public GameObject beamLineRendererPrefab; //Put a prefab with a line renderer onto here.
    //public GameObject beamStartPrefab; //This is a prefab that is put at the start of the beam.
    public GameObject beamEndPrefab; //Prefab put at end of beam.
    public Slider waterTank;

    //private GameObject beamStart;
    private GameObject beamEnd;
    private GameObject beam;
    private LineRenderer line;
    private Vector3 end;
        private bool _beam = false;

    [Header("Beam Options")]
    //public bool alwaysOn = true; //Enable this to spawn the beam when script is loaded.
    public bool beamCollides = true; //Beam stops at colliders
    public float beamLength = 100; //Ingame beam length
    public float beamEndOffset = 0f; //How far from the raycast hit point the end effect is positioned
    public float textureScrollSpeed = 0f; //How fast the texture scrolls along the beam, can be negative or positive.
    public float textureLengthScale = 1f;   //Set this to the horizontal length of your texture relative to the vertical. 
                                            //Example: if texture is 200 pixels in height and 600 in length, set this to 


    void FixedUpdate()
    {
            //ビームが発射されているかどうか
            if(Input.GetButton(fire2String)&& waterTank.value > 10.0f&&!_beam)
            {
                SpawnBeam();
                _beam = true;
            }
            if(Input.GetButtonUp(fire2String))
            {
                RemoveBeam();
                _beam=false;
            }
        if (beam) //Updates the beam
        {
            waterTank.value -= 0.3f;
            
            if (waterTank.value <= 2.0f)
            {
                RemoveBeam();
            }
            line.SetPosition(0, transform.position);

            RaycastHit hit;
                if (beamCollides && Physics.Raycast(transform.position, transform.forward, out hit)) //Checks for collision
                    end = hit.point; //- (transform.forward * beamEndOffset);
            else
                end = transform.position + (transform.forward * beamLength);

                float distance = Vector3.Distance(transform.position, end);
                if(distance>10f)
                {
                    end = transform.position + (transform.forward * beamLength);
                }
                line.SetPosition(1, end);
            /*
            if (beamStart)
            {
                beamStart.transform.position = transform.position;
                beamStart.transform.LookAt(end);
            }
            */
            if (beamEnd)
            {
                beamEnd.transform.position = end;
                //beamEnd.transform.LookAt(beamStart.transform.position);
            }

            line.material.mainTextureScale = new Vector2(distance / textureLengthScale, 1); //This sets the scale of the texture so it doesn't look stretched
            line.material.mainTextureOffset -= new Vector2(Time.deltaTime * textureScrollSpeed, 0); //This scrolls the texture along the beam if not set to 0
        }
    }

        /*
        public void OnEnable(InputAction.CallbackContext context)
        {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                if(waterTank.value>10.0f)
                {
                    SpawnBeam();
                }
                break;

            case InputActionPhase.Canceled:
                RemoveBeam();
                break;
        }
        }
        */


        public void SpawnBeam() //This function spawns the prefab with linerenderer
    {
        if (beamLineRendererPrefab)
        {
            if (beamEndPrefab)
            beamEnd = Instantiate(beamEndPrefab);
            beam = Instantiate(beamLineRendererPrefab);
            beam.transform.position = transform.position;
            beam.transform.parent = transform;
            beam.transform.rotation = transform.rotation;
            line = beam.GetComponent<LineRenderer>();
            line.useWorldSpace = true;
            #if UNITY_5_5_OR_NEWER
			line.positionCount = 2;
			#else
			line.SetVertexCount(2); 
			#endif
        }
        else
            print("Add a prefab with a line renderer to the MagicBeamStatic script on " + gameObject.name + "!");
    }

    public void RemoveBeam() //This function removes the prefab with linerenderer
    {
        if (beam)
            Destroy(beam);
        /*
        if (beamStart)
            Destroy(beamStart);
        */
        if (beamEnd)
            Destroy(beamEnd);
    }
}
}