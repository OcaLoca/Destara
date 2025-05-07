/* ---------------------------------------------- 
 * Copyright � Mobybit
 * ----------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{

    public class SlidedObject : MonoBehaviour
    {
        Touch touch;
        float speed;

        private void Start()
        {
            speed = 0.01f;
        }
        void Update()
        {
            if (Input.touchCount > 0)
            {

                touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Moved)
                {
                    transform.position = new Vector3(
                        transform.position.x + touch.deltaPosition.x * speed,
                         transform.position.y + touch.deltaPosition.y * speed,
                         transform.position.z + touch.deltaPosition.y * 0);
                }
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Vector3 position = this.transform.position;
                position.x--;
                this.transform.position = position;
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                Vector3 position = this.transform.position;
                position.x++;
                this.transform.position = position;
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Vector3 position = this.transform.position;
                position.y++;
                this.transform.position = position;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                Vector3 position = this.transform.position;
                position.y--;
                this.transform.position = position;
            }
        }

        private void OnTriggerEnter2D()
        {
            Debug.Log("EEEE");
        }


    }

}
