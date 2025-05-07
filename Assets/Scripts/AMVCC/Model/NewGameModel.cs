using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmartMVC;

namespace Game
{
    /// <summary>
    /// 
    /// </summary>
    public class NewGameModel : Model<GameApplication>
    {
        public PlayerManager.Difficulty selectedDifficulty = PlayerManager.Difficulty.Coward;
    }
}