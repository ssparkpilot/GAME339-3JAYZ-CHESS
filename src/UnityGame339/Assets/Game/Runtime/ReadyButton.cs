using System;
using Game339.Shared.Models;
using Game339.Shared.Services;
using Game339.Shared.Services.Implementation;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Game.Runtime
{
    public class ReadyButton : ObserverMonoBehaviour
    {
        [SerializeField] private Button button;



        protected override void Subscribe()
        {
                button.onClick.AddListener(OnClick);
        }


        protected override void Unsubscribe()
        {
                           button.onClick.RemoveListener(OnClick);

        }


        private void OnClick()
        {
            ServiceResolver.Resolve<TurnManager>().AdvancePhase();
        }
    }
}



