/* ---------------------------------------------- 
 * Copyright � Mobybit
 * ----------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class EscapeRoomDatabase : MonoBehaviour
    {

        [SerializeField]
        List<EscapeRoomManager> escapeRoomManager;
        public List<EscapeRoomManager> GetEscapeRoomDatabase { get => escapeRoomManager; }
        [SerializeField] GameObject escapeRoomContainer;

        //public BattleGroundManager battleGround;

        private void Awake()
        {
            foreach (EscapeRoomManager data in Resources.LoadAll<EscapeRoomManager>("EscapeRoomScene"))
            {
                escapeRoomManager.Add(data);
            }
        }
        public EscapeRoomManager LoadEscapeRoom()
        {
            foreach (EscapeRoomManager escapeRoom in escapeRoomManager)
            {
                foreach (string ID in escapeRoom.GetRoomID)
                {
                    if (ID == PlayerManager.Singleton.currentPage.pageID)
                    {
                        return escapeRoom;
                    }
                }

            }
            return null;
        }

        public void SetupEscapeRoom()
        {
            if (LoadEscapeRoom() != null)
            {
                foreach (Transform child in escapeRoomContainer.transform)
                {
                    Destroy(child.gameObject);
                }
                EscapeRoomManager escapeRoomManager = Instantiate(LoadEscapeRoom());
                escapeRoomManager.transform.SetParent(escapeRoomContainer.transform, false);

            }
        }







    }

}
