using Cinemachine;
using Mek.Utilities;
using UnityEngine;

namespace Game.Scripts.Controllers
{
    public class CameraController : SingletonBehaviour<CameraController>
    {
        public Camera MainCamera { get; private set; }
        
        public CinemachineVirtualCamera VCam;
        
        protected override void OnAwake()
        {
            MainCamera = Camera.main;
        }

        public void Follow(Transform t)
        {
            VCam.Follow = t;
        }
    }
}
