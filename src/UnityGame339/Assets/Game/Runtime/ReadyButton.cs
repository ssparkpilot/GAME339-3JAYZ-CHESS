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

        private TurnManager turnManager;

        protected override void Subscribe()
        {
            turnManager = ServiceResolver.Resolve<TurnManager>();

            button.onClick.AddListener(OnClick);

            // listen for phase changes
            turnManager.OnTurnStateChanged += OnStateChanged;

            UpdateButtonState();
        }

        protected override void Unsubscribe()
        {
            button.onClick.RemoveListener(OnClick);

            if (turnManager != null)
                turnManager.OnTurnStateChanged -= OnStateChanged;
        }

        private void OnClick()
        {
            if (!button.interactable)
                return; // extra safety

            turnManager.AdvancePhase();

            // immediately disable so player can't spam
            button.interactable = false;
        }

        private void OnStateChanged(TurnOwner owner, TurnPhase phase)
        {
            UpdateButtonState();
        }

        private void UpdateButtonState()
        {
            button.interactable =
                turnManager.CurrentPhase == TurnPhase.PlayerActing &&
                !turnManager.IsBusy;
        }
    }
}



