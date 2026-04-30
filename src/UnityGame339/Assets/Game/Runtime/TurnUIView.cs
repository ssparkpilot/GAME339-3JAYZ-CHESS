using System;
using Game339.Shared.Models;
using Game339.Shared.Services;
using Game339.Shared.Services.Implementation;
using TMPro;
using UnityEngine;


namespace Game.Runtime
{
    public class TurnUIView : ObserverMonoBehaviour
    {
        [SerializeField] private TMP_Text label;
        [SerializeField] private string prefix;


       


        protected override void Subscribe()
        {


            var turnService = ServiceResolver.Resolve<TurnManager>();
            turnService.CurrentTurnNumber.ChangeEvent += OnChange;
        }


        protected override void Unsubscribe()
        {
            var turnService = ServiceResolver.Resolve<TurnManager>();
            turnService.CurrentTurnNumber.ChangeEvent -= OnChange;
        }


        private void OnChange(int value)
        {
            label.text =  ": " + value;
        }
    }
}



