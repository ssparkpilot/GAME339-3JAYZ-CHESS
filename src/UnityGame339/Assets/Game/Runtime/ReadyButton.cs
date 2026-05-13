using System;
using System.Collections;
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
        
        private float delayBeforePress = 1f;
        
        private bool isOnCooldown = false;

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
            if (!button.interactable || isOnCooldown)
                return;

            turnManager.AdvancePhase();

            button.interactable = false;
            isOnCooldown = true;

            StartCoroutine(WaitBeforePress());
        }

        private IEnumerator WaitBeforePress()
        {
            yield return new WaitForSeconds(delayBeforePress);

            isOnCooldown = false;

            UpdateButtonState();
        }


        private void OnStateChanged(TurnOwner owner, TurnPhase phase)
        {
            UpdateButtonState();
        }

        private void UpdateButtonState()
        {
            button.interactable =
                !isOnCooldown && // cooldown check
                turnManager.CurrentPhase == TurnPhase.PlayerActing &&
                !turnManager.IsBusy;
        }
    }
}



