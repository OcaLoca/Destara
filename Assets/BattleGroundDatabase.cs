using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game
{

    public class BattleGroundDatabase : MonoBehaviour
    {
        [SerializeField]
        List<BattleGroundManager> battleGroundManagers;
        public List<BattleGroundManager> GetBattleGroundsDatabase { get => battleGroundManagers; }
        [SerializeField] GameObject battleGroundContainer;

        private void Awake()
        {
            foreach (BattleGroundManager data in Resources.LoadAll<BattleGroundManager>("BattleGroundPrefab"))
            {
                battleGroundManagers.Add(data);
            }
        }
        public BattleGroundManager LoadBattleGround()
        {
            foreach (BattleGroundManager battleGround in battleGroundManagers)
            {
                foreach(string ID in battleGround.GetGroundID)
                {
                    if(ID == PlayerManager.Singleton.currentPage.pageID)
                    {
                        return battleGround;
                    }
                }
            }
            return null;
        }

        public void SetupGround()
        {
            if (LoadBattleGround() != null)
            {
                foreach (Transform child in battleGroundContainer.transform)
                {
                    Destroy(child.gameObject);
                }
                BattleGroundManager battleGroundManager = Instantiate(LoadBattleGround());
                battleGroundManager.transform.SetParent(battleGroundContainer.transform, false);
                
            }
        }
    }
}
