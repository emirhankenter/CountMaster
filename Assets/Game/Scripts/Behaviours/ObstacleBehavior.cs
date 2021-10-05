using System;
using UnityEngine;

namespace Game.Scripts.Behaviours
{
    public class ObstacleBehavior : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            // if (!other.gameObject.CompareTag("StickMan")) return;
            //
            // var stickMan = other.gameObject.GetComponent<StickMan>();
            // if (!stickMan)
            // {
            //     Debug.LogError("Something's wrong");
            //     return;
            // }
            // Hit?.Invoke(stickMan);
        }
    }
}
