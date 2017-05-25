using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets._2D;

//namespace UnityStandardAssets._2D
//{
    [RequireComponent(typeof (PlatformerCharacter2D))]
    public class Platformer2DUserControl : MonoBehaviour
    {
        private PlatformerCharacter2D m_Character;
        private bool m_Jump;
        private float h;

        public bool regJump;
        public float regH;

        private void Awake()
            {
                m_Character = GetComponent<PlatformerCharacter2D>();
            }


        private void Update()
        {
            if (!m_Jump)
            {

            // Read the jump input in Update so button presses aren't missed.
            m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }
        }


        private void FixedUpdate()
        {
            // Read the inputs.
            regH = h;
            regJump = m_Jump;      
            
            bool crouch = Input.GetKey(KeyCode.LeftControl);
            h = CrossPlatformInputManager.GetAxis("Horizontal");
            // if(h != 0) {TODO: Start timer}
            // Pass all parameters to the character control script.
            m_Character.Move(h, crouch, m_Jump);
        // if(m_Jump == true) {TODO: Start timer}

        m_Jump = false;
        }
    }
//}
