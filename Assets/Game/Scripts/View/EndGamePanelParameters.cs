using System;
using Mek.Navigation;
using UnityEngine;

namespace Game.Scripts.View
{
    public class EndGamePanelParameters : ViewParams
    {
        public Action OnClaimed;
        public int Reward;
        
        public EndGamePanelParameters(int reward, Action onClaimed) : base(ViewTypes.EndGamePanel)
        {
            Reward = reward;
            OnClaimed = onClaimed;
        }
    }
}
