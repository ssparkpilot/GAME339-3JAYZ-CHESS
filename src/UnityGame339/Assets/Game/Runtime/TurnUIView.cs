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

        protected override void Subscribe()
        {
            var turnService = ServiceResolver.Resolve<TurnManager>();
            turnService.OnTurnStateChanged += OnStateChanged;
            
            // initial update
            OnStateChanged(turnService.CurrentOwner, turnService.CurrentPhase);
        }

        protected override void Unsubscribe()
        {
            var turnService = ServiceResolver.Resolve<TurnManager>();
            turnService.OnTurnStateChanged -= OnStateChanged;
        }

        private void OnStateChanged(TurnOwner owner, TurnPhase phase)
        {
            label.text = GetDisplayText(owner, phase);
        }

        private string GetDisplayText(TurnOwner owner, TurnPhase phase)
        {
            return owner switch
            {
                TurnOwner.Player => $"Player - {FormatPhase(phase)}",
                TurnOwner.Enemy => $"Enemy - {FormatPhase(phase)}",
                _ => "Unknown"
            };
        }

        private string FormatPhase(TurnPhase phase)
        {
            return phase switch
            {
                TurnPhase.PlayerTurnStart => "Start",
                TurnPhase.PlayerActing => "Acting",
                TurnPhase.PlayerTurnEnd => "Ending",
                TurnPhase.EnemyTurnStart => "Start",
                TurnPhase.EnemyMoving => "Moving",
                TurnPhase.EnemyTurnEnd => "Ending",
                _ => phase.ToString()
            };
        }
    }
}