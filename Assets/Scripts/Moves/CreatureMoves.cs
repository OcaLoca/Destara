using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game
{

    public class CreatureMoves : ScriptableObject
    {
        public string ID;
        public MoveType moveTypes;
        public ElementalBuff elementalBuffs;
        public float damage;
        public float arcaneDamageBuffDebuff;
        
        public void SetRealAndFantasyDefence()
        {
            if (PlayerManager.Singleton.superstition > 50)
            {
                damage += arcaneDamageBuffDebuff;   //quando si va in sup alta calcola i danni 
            }
        }

        //public float IsEffectiveOrNot(CreatureEnemy creatureEnemy)   //controllo se la mossa scelta è efficace
        //{
        //        foreach (EquipmentType defenceTypeOfPlayer in creatureEnemy.choiceMove.moveTypes.effectiveAgainstEquipments)
        //        {
        //            if (defenceTypeOfPlayer == PlayerManager.Singleton.totalArmor.equipmentType) { return 1.2f; }
        //        }
        //        foreach (EquipmentType defenceTypeOfPlayer in creatureEnemy.choiceMove.moveTypes.ineffectiveAgainstEquipments)
        //        {
        //            if (defenceTypeOfPlayer == PlayerManager.Singleton.totalArmor.equipmentType) { return 0.6f; }
        //        }
        //        foreach (EquipmentType defenceTypeOfPlayer in creatureEnemy.choiceMove.moveTypes.cantAttackEquipments)
        //        {
        //            if (defenceTypeOfPlayer == PlayerManager.Singleton.totalArmor.equipmentType) { return 0f; }
        //        }
        //        return 1f;
        //}

        //public bool [] ElementIsEffectiveOrNot(CreatureEnemy creatureEnemy)  //controllo se l'elemento della mossa ha effetto o no
        //{
        //    int arrayLenght = PlayerManager.Singleton.elementalBuffs.Length;
        //    bool[] elementalIsEffectAgainstPlayerEquipment = new bool[arrayLenght];

        //    for (int i = 0; i < arrayLenght; i++)
        //    {
        //        foreach (ElementalBuff elementalBuff in creatureEnemy.choiceMove.elementalBuffs.effectiveAgainstElements)
        //        {
        //            if (elementalBuff == PlayerManager.Singleton.elementalBuffs[i])
        //            {
        //                elementalIsEffectAgainstPlayerEquipment[i] = true;
        //            }
        //            elementalIsEffectAgainstPlayerEquipment[i] = false;
        //        }
        //    }
        //    return elementalIsEffectAgainstPlayerEquipment;
        //}

    }
}
