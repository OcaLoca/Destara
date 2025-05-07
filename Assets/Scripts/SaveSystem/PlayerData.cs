
using StarworkGC.Localization;
using System.Collections.Generic;

namespace Game
{
    [System.Serializable]
    public class PlayerData
    {
        public string playerName;
        public float lifePoints;
        public int level;
        public float exp;
        public int courage;
        public int lucky;
        public int superstition;
        public float constitution;
        public float dexterity;
        public float strength;
        public float intelligence;
        public int currentAP;
        public string classIDToString;
        public List<string> passedPages;
        public string currentPageID;
        public int currentTorch;
        public int maxTorch;
        public int currentPaper;
        public string[] equippedItemsList;
        public PlayerManager.Difficulty Difficulty;
        public List<string> playerInventoryItemsID;
        public List<int> playerInventoryItemsNumber;
        public List<string> abilities;

        public PlayerData()
        {
            equippedItemsList = new string[11]; // Cambiato da 9 a 10
            // Inizializzazione array a null
            for (int i = 0; i < equippedItemsList.Length; i++)
            {
                equippedItemsList[i] = null;
            }

            //stats
            classIDToString = PlayerManager.Singleton.classPlayerTypeToString;
            playerName = PlayerManager.Singleton.playerName;
            lifePoints = PlayerManager.Singleton.lifePoints;
            exp = PlayerManager.Singleton.GetPlayerExp();
            level = PlayerManager.Singleton.GetPlayerLevel;
            currentAP = PlayerManager.Singleton.currentAP;

            courage = PlayerManager.Singleton.courage;
            lucky = PlayerManager.Singleton.lucky;
            superstition = PlayerManager.Singleton.superstition;

            if (PlayerManager.Singleton.constitution != 0) //stranissimo bug da investigare a volte è a 0
            {
                constitution = (int)PlayerManager.Singleton.constitution;
                dexterity = (int)PlayerManager.Singleton.dexterity;
                strength = (int)PlayerManager.Singleton.strength;
                intelligence = (int)PlayerManager.Singleton.inteligence;
            }

            currentPageID = PlayerManager.Singleton.lastPage;
            currentTorch = PlayerManager.Singleton.currentTorch;
            maxTorch = PlayerManager.Singleton.maxTorchCount;

            currentPaper = PlayerManager.Singleton.currentPaper;
            Difficulty = PlayerManager.Singleton.selectedDifficulty;
            passedPages = PlayerManager.Singleton.pagesRead;

            abilities = new List<string>();
            foreach (ScriptableAbility ability in PlayerManager.Singleton.GetPlayerAbilities())
            {
                abilities.Add(ability.localizedID);
            }

            // Aggiungi inventory
            playerInventoryItemsID = new List<string>();
            playerInventoryItemsNumber = new List<int>();

            //inventory = PlayerManager.Singleton.GetAndCovertItemsInventoryForJSON();
            foreach (KeyValuePair<ScriptableItem, int> item in PlayerManager.Singleton.inventory)
            {
                playerInventoryItemsID.Add(item.Key.ID);
                playerInventoryItemsNumber.Add(item.Value);
            }

            // Aggiungi equipaggiamenti NON VA DIZIONARIO
            AddAllEquipmentsToInventory(PlayerManager.Singleton.heavyWeaponList);
            AddAllEquipmentsToInventory(PlayerManager.Singleton.lightWeaponList);
            AddAllEquipmentsToInventory(PlayerManager.Singleton.rangeWeaponList);
            AddAllEquipmentsToInventory(PlayerManager.Singleton.specialWeaponList);
            AddAllEquipmentsToInventory(PlayerManager.Singleton.lightDefenceList);
            AddAllEquipmentsToInventory(PlayerManager.Singleton.balancedDefenceList);
            AddAllEquipmentsToInventory(PlayerManager.Singleton.heavyDefenceList);
            AddAllEquipmentsToInventory(PlayerManager.Singleton.talismansList);
            AddAllEquipmentsToInventory(PlayerManager.Singleton.relicsList);
            AddAllEquipmentsToInventory(PlayerManager.Singleton.gemstonesList);

            // Aggiungi oggetti equipaggiati
            SetEquippedItems();
        }

        private void AddAllEquipmentsToInventory(List<ScriptableItem> itemList)
        {
            foreach (ScriptableItem item in itemList)
            {
                playerInventoryItemsID.Add(item.ID);
            }
        }

        private void SetEquippedItems()
        {
            if (PlayerManager.Singleton.playerWeapon.heavyWeapon != null)
            {
                equippedItemsList[0] = PlayerManager.Singleton.playerWeapon.heavyWeapon.ID;
            }
            if (PlayerManager.Singleton.playerWeapon.lightWeapon != null)
            {
                equippedItemsList[1] = PlayerManager.Singleton.playerWeapon.lightWeapon.ID;
            }
            if (PlayerManager.Singleton.playerWeapon.rangeWeapon != null)
            {
                equippedItemsList[2] = PlayerManager.Singleton.playerWeapon.rangeWeapon.ID;
            }
            if (PlayerManager.Singleton.playerWeapon.specialWeapon != null)
            {
                equippedItemsList[3] = PlayerManager.Singleton.playerWeapon.specialWeapon.ID;
            }
            if (PlayerManager.Singleton.playerEquipment.equippedLightDefence != null)
            {
                equippedItemsList[4] = PlayerManager.Singleton.playerEquipment.equippedLightDefence.ID;
            }
            if (PlayerManager.Singleton.playerEquipment.equippedHeavyDefence != null)
            {
                equippedItemsList[5] = PlayerManager.Singleton.playerEquipment.equippedHeavyDefence.ID;
            }
            if (PlayerManager.Singleton.playerEquipment.equippedBalancedDefence != null)
            {
                equippedItemsList[6] = PlayerManager.Singleton.playerEquipment.equippedBalancedDefence.ID;
            }
            if (PlayerManager.Singleton.playerEquipment.equippedClassDefaultDefence != null)
            {
                equippedItemsList[7] = PlayerManager.Singleton.playerEquipment.equippedClassDefaultDefence.ID;
            }
            if (PlayerManager.Singleton.playerEquipment.equippedGemstone != null)
            {
                equippedItemsList[8] = PlayerManager.Singleton.playerEquipment.equippedGemstone.ID;
            }
            if (PlayerManager.Singleton.playerEquipment.equippedTalisman != null)
            {
                equippedItemsList[9] = PlayerManager.Singleton.playerEquipment.equippedTalisman.ID;
            }
            if (PlayerManager.Singleton.playerEquipment.equippedRelic != null)
            {
                equippedItemsList[10] = PlayerManager.Singleton.playerEquipment.equippedRelic.ID;
            }
        }
    }
}
