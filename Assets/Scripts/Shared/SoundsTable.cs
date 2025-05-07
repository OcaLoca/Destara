/* ----------------------------------------------
 * 
 * 				Ariokan
 * 
 * Creation Date: 07/09/2021 17:21:48
 * 
 * Copyright � AheadGames
 * ----------------------------------------------
 */

using UnityEngine;

namespace AV
{
    ///<summary>
    ///Holds all the sounds of the Application
    ///</summary>
    [System.Serializable, CreateAssetMenu(fileName = "SoundsTable", menuName = "Game/SoundsTable")]
    public class SoundsTable : ScriptableObject
    {
        [Header("Soundtrack")]
        public AudioClip MetagameSoundtrack;
        public AudioClip GameSoundtrack;
        public AudioClip AllaSaluteSoundtrack;
        public AudioClip ScappaSoundtrack;
        public AudioClip InVinoVeritasSoundtrack;
        public AudioClip GenericFightSoundtrack;
        public AudioClip DeathSoundtrack;

        [Header("MenuUI")]
        public AudioClip GenericMenuButton;
        public AudioClip GenericBackButton;
        public AudioClip CowardButton;
        public AudioClip FearlessButton;
        public AudioClip InsaneButton;
        public AudioClip ConfirmDifficultButton;
        public AudioClip TutorialClickButton;
        public AudioClip BookmarkOpened;

        [Header("BattleUI")]
        public AudioClip VictorySound;
        public AudioClip LoseSound;

        [Header("UIGame")]
        public AudioClip changeGenericPage;
        public AudioClip Courage;//diamanti in alto a dx
        public AudioClip DiamondSuperstition;
        public AudioClip DiamondLucky;
        public AudioClip DiamondDiceRoll;

        //questi dovrebberò andare sotto un header classe
        public AudioClip EquipmentButton;
        public AudioClip ObjectsButton;

        //questi dovrebberò andare sotto un header menufight
        public AudioClip SwitchWeapon;
        public AudioClip AbilityOption;
        public AudioClip BagOpen;

        //effetti particolari pulsanti
        public AudioClip scendiEseguilo;
        public AudioClip beviEseguilo;
        public AudioClip OptionButton;

        [Header("Effects")]
        public AudioClip restAudio;
        public AudioClip torchAudio;
    }
}