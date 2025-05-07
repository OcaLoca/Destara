using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{

    [CreateAssetMenu(fileName = "Enemy", menuName = "Enemy/Enemy", order = 0)]

    public class HumanoidEnemy : ScriptableEnemy
    {
        public string battleScream;

        public Equipment equipment;
        public Weapon weapon;
       

    }

    
   
}
