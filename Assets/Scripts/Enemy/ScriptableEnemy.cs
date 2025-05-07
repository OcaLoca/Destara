using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Game.BattleController;

namespace Game
{
    public abstract class ScriptableEnemy : ScriptableObject
    {
        /// <summary>
        /// Serve sia per returnare il nome nella localizzazion che l'SO nel database
        /// </summary>
        [Tooltip("Stringa per importare lo SO del nemico e per localizzare il nome")]
        public string enemyName;
        public float constitution;
        public float dexterity;
        public float strength;
        public float inteligence;

        /// <summary>
        /// Nei nemici per evitare di assegnare equip. ho creato una variabile da assegnare direttamente al nemico
        /// </summary>
        public float defence;
        public TypeDatabase.DefenseType defenseType;
        
        /// <summary>
        /// Si riferisce al tipo di attacco dell'attacco base. Le abilità ne avranno uno loro.
        /// </summary>
        public TypeDatabase.AttackType baseAttackType;

        /// <summary>
        /// Si riferisce che l'Unità vincerà sempre
        /// </summary>
        public bool indestructible;
        public float buffLimit;
        public Difficult difficult;
        public List<ScriptableAbility> ability;
        public bool isHumanoid;
        public bool isBoss;
        public bool canAttack = false;
        public float hp;
        public float baseAttack;
        public int level;
        public bool isDead;
        //public bool isAttacking;
        public Sprite enemyImage;
        public Material normalMap;
        public List<Weapon> equippedWeapon;
        public List<ScriptableItem> myItems;
        public Dictionary<ScriptableItem, int> itemInventory = new Dictionary<ScriptableItem, int>();
        public float elementalLevel;
        public RuntimeAnimatorController animController;
        public GameObject enemyPSB;
        public GameObject enemyShadePrefab;

        [Tooltip("Dimensioni dell'immagine")]
        /// <summary>
        /// Si riferisce allo scale dell'immagime del nemico al caricamentoi
        /// </summary>
        public Vector3 scale;
        [Tooltip("Posizione dell'immagine")]
        public Vector3 localPosition;

        public ScriptableAbility finalAbility;
        public bool noStatus;
        public string baseAttackName;
        public AudioClip baseAttackSound;
        public AudioClip dodgeSound;
        public AudioClip battleScreamSound;
        public AudioClip hitSound;

        public Sprite enemyUISprite;

        public enum Difficult
        {
            Easy,
            Medium,
            Hard
        }

        public Dictionary<PlayerManager.Stats, int> healedUnit = new Dictionary<PlayerManager.Stats, int>();

        void CreateStats()
        {
            healedUnit.Add(PlayerManager.Stats.burned, 0);
            healedUnit.Add(PlayerManager.Stats.confused, 0);
            healedUnit.Add(PlayerManager.Stats.paralyzed, 0);
            healedUnit.Add(PlayerManager.Stats.freezed,0);
            healedUnit.Add(PlayerManager.Stats.poisoned, 0);
            healedUnit.Add(PlayerManager.Stats.invurneable, 0);
            healedUnit.Add(PlayerManager.Stats.unable, 0);
        }

        private void OnEnable()
        {
            hp = constitution * 10;
            CreateStats();
        }

       
    }
}