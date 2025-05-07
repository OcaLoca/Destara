/* ---------------------------------------------- 
 * Copyright � Mobybit
 * ----------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// /Contain all ID off Localization
    /// </summary>
    public class LocalizationIDDatabase : MonoBehaviour
    {
        public static LocalizationIDDatabase Singleton { get; set; }

        public const string FEM_PREFIX = "feM";
        public const string DEATH_BUTTON_TEXT = "youAreDead";

        public const string CRITICAL_POPUP = "criticalDamage";
        public const string CRITICAL_LOG = "criticalLog";
        public const string EMPTY_HIT_LOG = "";
        public const string EFFECTIVE_LOG = "effectiveLog";
        public const string NOT_EFFECTIVE_LOG = "notEffectiveLog";
        public const string MISS_LOG = "missLog";
        public const string DODGE_LOG = "dodgeLog";
        public const string MISS_POPUP = "missPopup";
        public const string WHAT_DO_BATTLE_LOG = "playerMove";
        public const string BUFF_BATTLE_LOG = "buffBattleLog";
        public const string BUFF_TWO_BATTLE_LOG = "buffTwoBattleLog";
        public const string DEBUFF_BATTLE_LOG = "debuffBattleLog";
        public const string USE_BATTLE_LOG = "useBattleLog";
        public const string CHANGE_DEFENCE_LOG = "changeDefenceLog";
        public const string ZERO_DAMAGE = "zeroDamage";
        public const string DEATH_BATTLE_LOG = "deathBattleLog";
        public const string IMMUNE_BATTLE_LOG = "immuneBattleLog";
        public const string PLAYER_IS_TIRED = "playerTired";
        public const string RECOVER_STAMINA = "playerRecoverStamina";
        public const string RECOVER_X_STAMINA = "playerRecoverXStamina";
        public const string WEAPON_COST_TOOMUCH = "weaponCostToMuch";
        public const string DEFENCE_COST_TOOMUCH = "defenceCostToMuch";
        public const string STAMINA_IS_MAX = "staminaIsMax";
        public const string ABILITY_COST_TOMUCH = "abilityCostToMuch";
        public const string ABILITY_USELESS = "abilityUseless";
        public const string DONT_USE_OBJECT = "dontUseObject";
        public const string DONT_USE_OBJECT_HERE = "dontUseObjectHere";
        public const string DEFENCE_ALREADY_USED = "defenceAlreadyEquipped";
        public const string ENEMY_DEFENCE_STRONG = "enemyDefenceStrong";
        public const string PLAYER_DEFENCE_STRONG = "playerDefenceStrong";
        public const string PLAYER_DEFENCE_WEAK = "playerDefenceWeak";
        public const string ABILITY_GIVE_DAMAGE = "abilityGenericDamageLog";
        public const string HIT_X_TIMES = "hitXTimesLog";
        public const string NOT_FIND_OBJECT = "notFindObject";
        public const string NO_OBJECT_IN_PAGE = "noObjectInPage";
        public const string BTN_ATTACK = "Attack";
        public const string BTN_ANALYZE = "analyze";
        public const string PLAYERRECOVERXHP = "abilityRecoverLifeAmount";
        public const string PLAYERRECOVERXAP = "abilityRecoverAPAmount";
        public const string ABBOTADVICE = "abbotAdvice";
        public const string CRONEADVICE = "croneAdvice";
        public const string BOUNTYADVICE = "bountyAdvice";
        public const string TRAFFICKERADVICE = "traffickerAdvice";

        /// <summary>
        /// Passare Nome Target e da cosa è affetto, veleno evcc
        /// </summary>
        public const string TARGET_AFFECTED_BY_STATUS = "targetAffectedByStatusLog";
        public const string CANT_USE_LEVEL_LOW = "cantUseLevelLow";

        //PALYERPREFS //0 false //1 true
        internal const string CHECKFIRSTGAMELAUNCH = "FirstStartup";
        internal const string CHECKACTIVETUTORIALONFIRSTLAUNCH = "ActiveTutorialLaunch";
        internal const string CHECKPLAYERACTIVETUTORIAL = "PlayerActiveTutorial";
        internal const string CHECKPLAYERINVENTORYTUTORIAL = "PlayerInventoryTutorial";
        public const string PRIVACYPOLICY = "privacyPolicyAccepted";


        //LINGUE
        internal const string ENGLISH = "English";
        internal const string ITALIAN = "Italian";
        internal const string FRENCH = "French";
        internal const string SPANISH = "Spanish";
        internal const string GERMAN = "German";
        internal const string PORTOGUESE = "Portuguese";
        internal const string JAPANESE = "Japanese";


    }

}
