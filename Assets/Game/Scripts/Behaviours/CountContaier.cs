using Game.Scripts.Enums;
using Game.Scripts.Models;
using TMPro;
using UnityEngine;

namespace Game.Scripts.Behaviours
{
    public class CountContaier : MonoBehaviour
    {
        [SerializeField] private Renderer _renderer;
        [SerializeField] private TextMeshPro _stickManCountText;

        public void Initialize(TeamSide teamSide)
        {
            var color = teamSide == TeamSide.Friendly
                ? GameConfig.StickManColorFriendly
                : GameConfig.StickManColorEnemy;

            _renderer.material.color = color;
        }
        
        public void UpdateCount(int count)
        {
            _stickManCountText.SetText(count.ToString());

            gameObject.SetActive(count > 0);
            _stickManCountText.gameObject.SetActive(count > 0);;
        }
    }
}
