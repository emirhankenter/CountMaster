using System;
using Mek.Navigation;
using UnityEngine;

namespace Game.Scripts.View
{
    public class EndGamePanelParameters : ViewParams
    {
        public Action OnClaimed;
        
        public EndGamePanelParameters(Action onClaimed) : base(ViewTypes.EndGamePanel)
        {
            OnClaimed = onClaimed;
        }
    }
}
