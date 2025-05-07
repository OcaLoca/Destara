using StarworkGC.Localization;
using StarworkGC.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Game.ScriptableAbility;
using static System.Net.Mime.MediaTypeNames;
//using static UnityEditor.Progress;

namespace Game
{
    [CreateAssetMenu(fileName = "ConsumableItem", menuName = "Item/Consumable", order = 0)]
    public class ConsumableItem : ScriptableItem
    {
        [SerializeField] bool onlyInFight;
        public float maxStackable;

        [SerializeField] bool temporary = false;
        [SerializeField] float timeIfTemporary;
        [SerializeField] float numberOfTurnIfTemporary;
        [Tooltip("L'oggetto che serve per sbloccare il tasto")]
        [SerializeField] public BuffType defaultBuff;
        [Tooltip("L'ogg")]
        [SerializeField] float defaulBuffAmount;
        [SerializeField] BuffType[] buffType;
        [SerializeField] float[] buffAmount;
        [SerializeField] public TargetType tartgetType;
        [SerializeField] public float targetQuantity;
        [SerializeField] bool hasASingleBuffes;
        public ItemBuff[] itemBuffes;

        public enum TargetType
        {
            Player,
            Allies,
            Enemies
        }
        public enum BuffType
        {
            Health,
            AP,
            Stamina,
            Experience,
            Strenght,
            Dexterity,
            Constitution,
            Inteligence,
            Courage,
            Superstition,
            Lucky,
            HealPoison,
            HealBurn,
            Save
        }

        public struct ItemBuff
        {
            public float buff;
            public BuffType BuffType;
        }

        private void OnEnable()
        {
            itemBuffes = new ItemBuff[buffAmount.Length];
            //itemType = ItemType.Consumable;
            int i = 0;

            if (buffAmount.Length != buffType.Length)
            {
                Debug.LogErrorFormat("Attenzione non hai assegnato un numero equivalente di tipi e quantità tra i buff dell'oggetto {0}", ID);
            }
            else
            {
                foreach (float buff in buffAmount)
                {
                    itemBuffes[i].buff = buff;
                    itemBuffes[i].BuffType = buffType[i];
                    i++;
                }
            }
        }

        public static bool playerCanTriggerObject = true;
        public override void TriggerItem()
        {
            Debug.Log("Uso oggetto " + itemNameLocalization);
            string txtAlarm = string.Empty;

            //if (!hasASingleBuffes)
            //{
            //    TriggerMultipleBuffItemWhileReading();
            //}
            if (defaultBuff == BuffType.AP)
            {
                PlayerManager.Singleton.UpdatePlayerManagerAbilityPoints((int)defaulBuffAmount);
            }
            else
            {
                PlayerManager.Singleton.UpdateLifePoints(defaulBuffAmount, 0);
                if (temporary) { PlayerManager.Singleton.UpdateLifePoints(defaulBuffAmount, timeIfTemporary); }
            }

            if (onlyInFight)
            {
                txtAlarm = Localization.Get(LocalizationIDDatabase.DONT_USE_OBJECT_HERE);
                GameApplication.Singleton.view.InventoryView.ShowPanelCantUseObj(txtAlarm);
                ObjectCantBeUsed(txtAlarm);
                return;
            }

            if (playerCanTriggerObject)
            {
                PlayerManager.Singleton.RemoveItemFromPlayerInventory(this, 1);
                return;
            }
            txtAlarm = Localization.Get(LocalizationIDDatabase.DONT_USE_OBJECT);
            ObjectCantBeUsed(txtAlarm);
        }


        public void ObjectCantBeUsed(string txt)
        {
            playerCanTriggerObject = true;
            GameApplication.Singleton.view.InventoryView.ShowPanelCantUseObj(txt);
        }

        void TriggerMultipleBuffItemWhileReading()
        {
            foreach (ItemBuff buff in itemBuffes)
            {
                Debug.Log("Tipo buff " + buff.BuffType.ToString());

                if (buff.BuffType == BuffType.Constitution)
                {
                    PlayerManager.Singleton.UpdateCostitution(buff.buff);

                    if (temporary) { PlayerManager.Singleton.UpdateCostitution(-buff.buff, timeIfTemporary); }
                }
                else if (buff.BuffType == BuffType.Strenght)
                {
                    PlayerManager.Singleton.UpdateStrenght(buff.buff);
                    if (temporary) { PlayerManager.Singleton.UpdateStrenght(-buff.buff, timeIfTemporary); }
                }
                else if (buff.BuffType == BuffType.Inteligence)
                {
                    PlayerManager.Singleton.UpdateIntelligence(buff.buff);
                    if (temporary) { PlayerManager.Singleton.UpdateIntelligence(-buff.buff, timeIfTemporary); }
                }
                else if (buff.BuffType == BuffType.Dexterity)
                {
                    PlayerManager.Singleton.UpdateDexterity((int)buff.buff);
                    if (temporary) { PlayerManager.Singleton.UpdateDexterity(-buff.buff, timeIfTemporary); }
                }
                else if (buff.BuffType == BuffType.Superstition)
                {
                    PlayerManager.Singleton.UpdateSuperstition((int)buff.buff);
                    if (temporary) { PlayerManager.Singleton.UpdateSuperstition(-(int)buff.buff, timeIfTemporary); }
                }
                else if (buff.BuffType == BuffType.Lucky)
                {
                    PlayerManager.Singleton.UpdateLucky((int)buff.buff);
                    if (temporary) { PlayerManager.Singleton.UpdateLucky(-(int)buff.buff, timeIfTemporary); }
                }
                else if (buff.BuffType == BuffType.Courage)
                {
                    PlayerManager.Singleton.UpdateCourage((int)buff.buff);
                    if (temporary) { PlayerManager.Singleton.UpdateCourage(-(int)buff.buff, timeIfTemporary); }
                }
                else if (buff.BuffType == BuffType.HealBurn)
                {
                    //rimuovi stato bruciato
                }
                else if (buff.BuffType == BuffType.HealPoison)
                {
                    //rimuovi stato avvelenato
                }
                else if (buff.BuffType == BuffType.Experience)
                {
                    PlayerManager.Singleton.UpdateExperience(buff.buff, 0);
                    if (temporary) { PlayerManager.Singleton.UpdateExperience(-buff.buff, timeIfTemporary); }  //if (temporary) { PlayerManager.Singleton.UpdateExperience(timeIfTemporary, -buff.buff); }
                }
                else if (buff.BuffType == BuffType.Save)
                {
                    // SaveSystem.SavePlayer(SaveType.Hard);
                }
                else if (buff.BuffType == BuffType.AP)
                {
                    PlayerManager.Singleton.UpdatePlayerManagerAbilityPoints((int)buff.buff);
                }
                else
                {
                    PlayerManager.Singleton.UpdateLifePoints(buff.buff, 0);
                    if (temporary) { PlayerManager.Singleton.UpdateLifePoints(-buff.buff, timeIfTemporary); }
                }
            }
        }

        public override BattleController.Unit TriggerItemInBattle(ref BattleController.Unit unit, ConsumableItem item)
        {
            BattlePlayerManager.effectObjectLog = string.Empty;
            if (defaultBuff == BuffType.Health)
            {
                float starterHp = unit.hp;
                unit.RecoverLife(item.defaulBuffAmount);
                float recoveredHp = unit.hp - starterHp;
                BattlePlayerManager.effectObjectLog = string.Format(Localization.Get(LocalizationIDDatabase.PLAYERRECOVERXHP), unit.name, recoveredHp);
                BattleController.PlayerRecoverLife = true;
            }
            else if (defaultBuff == BuffType.AP)
            {
                float starterAP = unit.abilityPoints;
                unit.RecoverAbilityPointsInBattle((int)item.defaulBuffAmount);
                float recoveredAP = unit.abilityPoints - starterAP;
                BattlePlayerManager.effectObjectLog = string.Format(Localization.Get(LocalizationIDDatabase.PLAYERRECOVERXAP), unit.name, recoveredAP);
            }
            else if (defaultBuff == BuffType.Stamina)
            {
                float starterStamina = unit.stamina;
                unit.RecoverStamina(item.defaulBuffAmount);
                float recoveredStamina = unit.stamina - starterStamina;
                BattleController.PlayerRecoverStamina = true;
                BattleController.StaminaRecoveredWithObj = item.defaulBuffAmount;
                BattlePlayerManager.effectObjectLog = string.Format(Localization.Get(LocalizationIDDatabase.RECOVER_X_STAMINA), unit.name, recoveredStamina);
            }

            // unit.BuffDebuffStats(item.buffAmount[0], ScriptableAbility.Buff.hp);
            // Qua faremo DebuffStatsWithObj e passo il tipo di oggetto e non di abilità
            return unit;
        }
    }
}
