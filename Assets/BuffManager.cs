/* ----------------------------------------------
 * 
 * 				MobyBit
 * 
 * Creation Date: 01/09/2020 00:10:30
 * 
 * Copyright � MobyBit
 * ----------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Game.BattleController;

namespace Game
{
    public class BuffManager : MonoBehaviour
    {
        public const int MAXFIGHTBUFF = 5;
        public Image[] powerUpImages = new Image[MAXFIGHTBUFF];
        public GameObject[] powerUpParticles;
       // public ParticleSystem[] playerEmission;
        [SerializeField] BattleController battleController;
        [SerializeField] BattlePlayerManager battlePlayerManager;

        public void TurnOffPowerupTxt()
        {
            battlePlayerManager.txtAvaiablePowerUpCount.gameObject.SetActive(false);
        }

        public static int powerupInUse;
        public static int powerupAvaiable;
    }
}