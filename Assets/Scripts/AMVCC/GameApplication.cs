using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SmartMVC;
using AV;

namespace Game
{
    /// <summary>
    /// Il cuore dell'AMVCC. L'applicazione regola la comunicazione tra tutte le componenti MVC
    /// </summary>
    public class GameApplication : BaseApplication<GameModel, GameView, GameController>
    {
        public static GameApplication Singleton;
        internal SoundsTable Sounds => model.sounds;

        protected override void Awake()
        {
            base.Awake();
            if (Singleton != null && Singleton != this)
            {
                Destroy(gameObject);
                return;
            }
            Singleton = this;
        }
    }
}