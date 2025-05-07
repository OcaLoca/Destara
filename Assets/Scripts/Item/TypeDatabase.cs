/* ---------------------------------------------- 
 * Copyright � Mobybit
 * ----------------------------------------------
 */

using StarworkGC.Localization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class TypeDatabase : MonoBehaviour
    {
        public static DefenseType defenseType;
        public enum DefenseType
        {
            Light,
            Avarage,
            Heavy,
            Special,
            Null
        }
        public static AttackType attackType;
        public enum AttackType
        {
            Light,
            Ranged,
            Heavy,
            Special
        }

        public static float IsEffectiveOrNot(TypeDatabase.AttackType attackType,TypeDatabase.DefenseType defenseType, float damage, ref string effectiveLog, bool ShowEnemyShield = false)
        {
            if(damage <= 0) { return 0; }
            BattleView bV = GameApplication.Singleton.view.BattleView;

            switch (defenseType)
            {
                case TypeDatabase.DefenseType.Light:
                    switch (attackType)
                    {
                        case AttackType.Heavy:
                            damage = damage * 1.5f;
                            effectiveLog = Localization.Get(LocalizationIDDatabase.EFFECTIVE_LOG);
                            SlowMotionEffect.Instance.EnableSlowMotion(0.5f, 1f);
                            ShakeBattleground.Instance.ShakeObject(damage);
                            if (ShowEnemyShield) { bV.ShowHiddenEnemyShield(IconsDatabase.Singleton.GetArmorSpriteByDefenceType(DefenseType.Light)); }
                            break;
                        case AttackType.Ranged:
                            damage = damage * 0.5f;
                            effectiveLog = Localization.Get(LocalizationIDDatabase.NOT_EFFECTIVE_LOG);
                            break;
                        default:
                            effectiveLog = Localization.Get(LocalizationIDDatabase.EMPTY_HIT_LOG);
                            break;
                    }

                    break;
                case TypeDatabase.DefenseType.Avarage:
                    switch (attackType)
                    {
                        case AttackType.Light:
                            damage = damage * 1.5f;
                            effectiveLog = Localization.Get(LocalizationIDDatabase.EFFECTIVE_LOG);
                            SlowMotionEffect.Instance.EnableSlowMotion(0.5f, 1f);
                            ShakeBattleground.Instance.ShakeObject(damage);
                            if (ShowEnemyShield) { bV.ShowHiddenEnemyShield(IconsDatabase.Singleton.GetArmorSpriteByDefenceType(DefenseType.Avarage)); }
                            break;
                        case AttackType.Heavy:
                            damage = damage * 0.5f;
                            effectiveLog = Localization.Get(LocalizationIDDatabase.NOT_EFFECTIVE_LOG);
                            break;
                        default:
                            effectiveLog = Localization.Get(LocalizationIDDatabase.EMPTY_HIT_LOG);
                            break;
                    }
                    break;
                case DefenseType.Heavy:
                    switch (attackType)
                    {
                        case AttackType.Light:
                            damage = damage * 0.5f;
                            effectiveLog = Localization.Get(LocalizationIDDatabase.NOT_EFFECTIVE_LOG);
                            break;
                        case AttackType.Ranged:
                            damage = damage * 1.5f;
                            effectiveLog = Localization.Get(LocalizationIDDatabase.EFFECTIVE_LOG);
                            SlowMotionEffect.Instance.EnableSlowMotion(0.5f, 1f);
                            ShakeBattleground.Instance.ShakeObject(damage);
                            if (ShowEnemyShield) { bV.ShowHiddenEnemyShield(IconsDatabase.Singleton.GetArmorSpriteByDefenceType(DefenseType.Heavy)); }
                            break;
                        default:
                            effectiveLog = Localization.Get(LocalizationIDDatabase.EMPTY_HIT_LOG);
                            break;
                    }
                    break;
                default:
                    effectiveLog = Localization.Get(LocalizationIDDatabase.EMPTY_HIT_LOG);
                    break;
            }
            return damage;
        }
    }
}
