using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmartMVC;
using UnityEngine.UI;

namespace Game
{
    public class CharacterCreationController : Controller<GameApplication>
    {
        void OnEnable()
        {
            AddEventListenerToApp(MVCEvents.CHANGE_SELECTED_CLASS, OnChangeClassSelection);
        }

        void OnDisable()
        {
            RemoveEventListenerFromApp(MVCEvents.CHANGE_SELECTED_CLASS, OnChangeClassSelection);
        }

        void OnChangeClassSelection(params object[] data)
        {
            int selectedClass = (int)data[0];
            app.view.CharacterCreationView.ChangeSelectedClass(selectedClass);
        }
    }
}