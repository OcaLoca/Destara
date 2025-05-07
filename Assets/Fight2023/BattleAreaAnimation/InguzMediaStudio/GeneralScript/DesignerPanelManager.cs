/* ---------------------------------------------- 
 * Copyright � Mobybit
 * ----------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace Game
{
    public class DesignerPanelManager : MonoBehaviour
    {
        public static DesignerPanelManager Singleton { get; set; }

        private void Awake()
        {
            Singleton = this;
        }

        [Header("Weapons")]
        public Weapon SpadaComune;
        public Weapon ArcoComune;
        public Weapon MazzaComune;
        public Weapon AsciaDaViaggio;
        public Weapon OssoDiOrso;
        public Weapon ArcoLiane;

        [Header("Ability")]
        public ScriptableAbility LupoNero;
        public ScriptableAbility Trappola;
        public ScriptableAbility Fiuto;
        public ScriptableAbility Trucida;
        public ScriptableAbility ToccoNero;
        public ScriptableAbility Energimente;
        public ScriptableAbility Curaverde;
        public ScriptableAbility ArteMagia;



        [Header("Button")]
        [SerializeField] Button btnFightViandante;
        [SerializeField] Button btnChangeSpadaComuneDamage;
        [SerializeField] Button btnChangeArcoComuneDamage;
        [SerializeField] Button btnChangeArcoLianeDamage;
        [SerializeField] Button btnChangeAsciaDaViaggioDamage;
        [SerializeField] Button btnChangeOssoDiOrsoDamage;
        [SerializeField] Button btnChangeMazzaComuneDamage;
        [SerializeField] Button btnConfirmEnemyChangeStats;

        [Header("Input")]
        public TMP_InputField inputWeaponDamageAmount;

        public TMP_InputField inputEnemyConstitutionAmount;
        public TMP_InputField inputEnemyBaseAttackDamageAmount;

        private void OnEnable()
        {
            List<Button> myButtons = new List<Button> { btnChangeArcoComuneDamage, btnChangeSpadaComuneDamage, btnChangeArcoLianeDamage, btnChangeAsciaDaViaggioDamage, btnChangeMazzaComuneDamage, btnChangeOssoDiOrsoDamage, btnChangeSpadaComuneDamage };

            foreach (var item in myButtons)
            {
                item.onClick.RemoveAllListeners();
                item.onClick.AddListener(delegate { ChangeWeaponDamage(item.name); });
            }

            btnConfirmEnemyChangeStats.onClick.RemoveAllListeners();
            btnConfirmEnemyChangeStats.onClick.AddListener(delegate { ChangeEnemyStats(); });
        }

        void ChangeWeaponDamage(string objName)
        {
            switch (objName)
            {
                case "SpadaComune":
                    SpadaComune.weaponConstDamage = float.Parse(inputWeaponDamageAmount.text);
                    break;
                case "ArcoComune":
                    ArcoComune.weaponConstDamage = float.Parse(inputWeaponDamageAmount.text);
                    break;
                case "MazzaComune":
                    MazzaComune.weaponConstDamage = float.Parse(inputWeaponDamageAmount.text);
                    break;
                case "AsciaDaViaggio":
                    AsciaDaViaggio.weaponConstDamage = float.Parse(inputWeaponDamageAmount.text);
                    break;
                case "OssoDiOrso":
                    OssoDiOrso.weaponConstDamage = float.Parse(inputWeaponDamageAmount.text);
                    break;
                case "ArcoDiLiane":
                    ArcoLiane.weaponConstDamage = float.Parse(inputWeaponDamageAmount.text);
                    break;
                default:
                    break;
            }
        }

        void ChangeEnemyStats()
        {
            foreach (ScriptableEnemy enemy in ScriptableObjectsDatabase.Singleton.enemiesDatabase)
            {
                if (inputEnemyConstitutionAmount.text != string.Empty)
                {
                    enemy.constitution = float.Parse(inputEnemyConstitutionAmount.text);
                }
                if (inputEnemyBaseAttackDamageAmount.text != string.Empty)
                {
                    enemy.baseAttack = float.Parse(inputEnemyBaseAttackDamageAmount.text);
                }
            }
        }

        void ChangePage(string pageID)
        {
            //if (PlayerManager.Singleton.isMale)
            //{
                foreach (ScriptablePage scriptableChapter in PagesMaleDatabase.Singleton.chaptersDatabase)
                {
                    if (scriptableChapter.pageID == pageID)
                    {
                        GameApplication.Singleton.view.BookView.ShowPage(scriptableChapter);
                    }
                }
            //}
            //else
            //{
            //    foreach (ScriptablePage scriptableChapter in PagesFemaleDatabase.Singleton.chaptersDatabase)
            //    {
            //        if (scriptableChapter.pageID == pageID)
            //        {
            //            GameApplication.Singleton.view.BookView.ShowPage(scriptableChapter);
            //        }
            //    }
            //}
        }

    }

}
