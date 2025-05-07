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
using System.Linq;
using UnityEngine;
using SmartMVC;

namespace Game
{
    ///// <summary>
    ///// /DEPRECATA CON BATTLECONTROLLER CAUSA BUG 
    ///// </summary>
    //public class BattleManager : Controller<GameApplication>
    //{
    //    public List<Unit> enemies = new List<Unit>();
    //    List<Unit> playerAndAllies = new List<Unit>();
    //    List<Unit> allUnits = new List<Unit>();

    //    int turnNumber = 1;

    //    public enum BattleTurn
    //    {
    //        Start,
    //        PlayerTurn,
    //        EnemyTurn,
    //    }

    //    public struct Unit
    //    {
    //        public string name;
    //        public float constitution;
    //        public float dexterity;
    //        public float strenght;
    //        public float inteligence;
    //        public List<ScriptableAbility> ability;
    //        public bool haventAttack;


    //        public Unit(string name, float constitution, float dexterity, float strenght, float inteligence, List<ScriptableAbility> ability, bool canAttack)
    //        {
    //            this.name = name;
    //            this.constitution = constitution;
    //            this.dexterity = dexterity;
    //            this.strenght = strenght;
    //            this.inteligence = inteligence;
    //            this.ability = ability;
    //            this.haventAttack = canAttack;
    //        }

    //    }

    //    private void OnEnable()
    //    {
    //        List<string> ciao = new List<string>();
    //       LoadEnemyByID();
    //       //LoadPlayerFighter();
    //    }
        

    //    private void Update()
    //    {
    //        //SortByDexterity();
    //        //PlayerManager.Singleton.CheckLifePoints();
    //      /// foreach (Unit unit in allUnits)
    //      /// {
    //      ///     Debug.LogError(unit.dexterity);
    //      /// }
    //    }

    //    void SortByDexterity()
    //    {
    //        allUnits.OrderBy(u => u.dexterity).ToList();
    //    }

    //    public List<string> GetMobID()
    //    {
    //        List<string> unitsID = new List<string>();

    //        foreach (ScriptableEnemy unit in PlayerManager.Singleton.currentPage.mobID)
    //        {
    //            unitsID.Add(unit.ID);
    //        }
    //        return unitsID;
    //    }

    //    public void LoadEnemyByID()
    //    {
    //        foreach (ScriptableEnemy u in GameApplication.Singleton.model.ScriptableObjectsDatabase.enemiesDatabase)
    //        {
    //            if (GetMobID().Contains(u.ID))
    //            {
    //                Unit unit = new Unit(u.name, u.constitution, u.dexterity, u.strength, u.inteligence, u.ability, u.canAttack);
    //                enemies.Add(unit);
    //                AddToAllUnitList(unit);
    //            }
    //        }
    //    }

    //    public List<string> LoadPlayerAlly()
    //    {
    //        if(PlayerManager.Singleton.currentPage.allyID.Count > 0)
    //        {
    //            List<string> unitsID = new List<string>();

    //            foreach (ScriptableEnemy ally in PlayerManager.Singleton.currentPage.allyID)
    //            {
    //                unitsID.Add(ally.ID);
    //            }
    //            return unitsID;
    //        }
    //        return null;

    //    }


    //    public void LoadPlayerFighter()
    //    {
    //        Unit classPlayer = new Unit(PlayerManager.Singleton.playerName, PlayerManager.Singleton.constitution, PlayerManager.Singleton.dexterity,
    //                                   PlayerManager.Singleton.strength, PlayerManager.Singleton.inteligence, null, false);
    //        playerAndAllies.Add(classPlayer);
    //        AddToAllUnitList(classPlayer);
    //    }

    //    public void AddToAllUnitList(Unit unit)
    //    {
    //        allUnits.Add(unit);
    //    }

    //    public void LoadBattleView()
    //    {

    //    }

    //    List<Unit> allInsert = new List<Unit>();
    //    List<bool> allAttack = new List<bool>();
    //    int unitsInScene;
        
    //    public bool IsTurnFinished() //ogni volta che attacca richiamo la funzione
    //    {
    //        unitsInScene = allUnits.Count;

    //        foreach (Unit unit in allUnits)
    //        {
    //            if (unit.haventAttack) { return false; }

    //            if(!unit.haventAttack)
    //            {
    //                allAttack.Add(unit.haventAttack);
    //            }
    //        }

    //        if(allAttack.Count == unitsInScene)
    //        {
    //            return true;
    //        }
    //        return false;
    //    }

    //}

}