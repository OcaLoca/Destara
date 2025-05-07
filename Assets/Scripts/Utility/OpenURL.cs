/* ---------------------------------------------- 
 * Copyright � Mobybit
 * ----------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{

    public class OpenURL : MonoBehaviour
    {
        [SerializeField] string link;

        public void OpenLinkURL()
        {
            Application.OpenURL(link);
        }

    }

}
