﻿using UnityEngine;
using DroneController.Physics;

namespace DroneController
{
    namespace Propelers
    {
        public class ElisaScript : MonoBehaviour
        {

            #region PUBLIC VARIABLES

            [Tooltip("Propellers from your drone. Assing cross propellers. X ")]
            public GameObject[] elisa;
            
            [Tooltip("How fast propellers will rotate when they are idle.")]
            public float idleRotationSpeed = 1000;
            [Tooltip("How fast propellers will rotate when we are moving.")]
            public float movingRotationSpeed = 2000;
            [Tooltip("Fixing propellers rotation if somethign went wrong during import from Blender or 3DsMax.")]
            public float elisaAngle;

            public bool spinDifference = true;

            //[DEPRICATED - will be updated in the future]
            [HideInInspector] public float atWhatSpeedsShowWingtipVortices = 20;//at what speed will this be shown            

            #endregion

            #region PRIVATE VARIABLES

            private float currentYRotation;
            private float rotationSpeed = 1000;

            private int amountOfWingtipVorticesOnElisas = 0;

            private ParticleSystem[] wingtipVortices;

            private float wantedAlpha;
            private float currentAlpha;

            private DroneMovement droneMovementScript;

            #endregion

            #region MONO BEHAVIOUR METHODS

            public virtual void Awake()
            {
                droneMovementScript = GetComponent<DroneMovement>();

                //LocateWintipParticles();//used to determine how many particles on propelers do we have?...
            }

            #endregion

            #region PUBLIC METHODS

            /// <summary>
            /// Detecting user input from settings and spining blades a bit faster, this will get updated...
            /// </summary>
            public void RotationInputs()
            {
                if (Input.GetKey(droneMovementScript.forward) ||
                    Input.GetKey(droneMovementScript.downward) ||
                    Input.GetKey(droneMovementScript.downButton) ||
                    Input.GetKey(droneMovementScript.leftward) ||
                    Input.GetKey(droneMovementScript.rightward) ||
                    Input.GetKey(droneMovementScript.upward) ||
                    Input.GetKey(droneMovementScript.downButton) ||
                    Input.GetKey(droneMovementScript.downward) ||
					droneMovementScript.Vertical_W != 0 ||
					droneMovementScript.Horizontal_D != 0 ||
					droneMovementScript.Vertical_I != 0 ||
					droneMovementScript.Horizontal_L != 0)
                {
                    rotationSpeed = movingRotationSpeed;
                }
                else
                {
                    rotationSpeed = idleRotationSpeed;
                }
            }

            /// <summary>
            /// Using propeler differentials
            /// </summary>
            public void RotationDifferentials()
            {
                currentYRotation += Time.deltaTime * rotationSpeed;
                for (int i = 0; i < elisa.Length; i++)
                {
                    if (spinDifference == true)
                    {
                        if (i % 2 == 0) elisa[i].transform.localRotation = Quaternion.Euler(new Vector3(elisaAngle, currentYRotation, transform.rotation.z));
                        else elisa[i].transform.localRotation = Quaternion.Euler(new Vector3(elisaAngle, -currentYRotation, transform.rotation.z));
                    }
                    else
                    {
                        elisa[i].transform.localRotation = Quaternion.Euler(new Vector3(elisaAngle, currentYRotation, transform.rotation.z));
                    }
                }
            }

            #endregion
            
            #region [DEPRICATED - will get updated in future]

            void LocateWintipParticles()
            {
                amountOfWingtipVorticesOnElisas = 0;
                for (int i = 0; i < elisa.Length; i++)
                {
                    if (elisa[i].GetComponent<ParticleSystem>())
                    {
                        amountOfWingtipVorticesOnElisas++;
                    }
                }
                wingtipVortices = new ParticleSystem[amountOfWingtipVorticesOnElisas];
                for (int i = 0; i < amountOfWingtipVorticesOnElisas; i++)
                {
                    wingtipVortices[i] = elisa[i].GetComponent<ParticleSystem>();
                }
            }
            
            #endregion

        }
    }
}
