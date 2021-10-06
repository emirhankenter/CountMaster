using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Game.Scripts.Behaviours;
using Mek.Coroutines;
using Mek.Utilities;
using UnityEngine;

namespace Game.Scripts.Controllers
{
    public enum CameraState
    {
        Running,
        Stair
    }
    public class CameraController : SingletonBehaviour<CameraController>
    {
        public Camera MainCamera { get; private set; }
        public CameraState CameraState { get; private set; }
        
        public CinemachineVirtualCamera VCam;
        public CinemachineVirtualCamera StairCam;
        
        protected override void OnAwake()
        {
            MainCamera = Camera.main;
        }

        public void Follow(Transform t)
        {
            VCam.Follow = t;
        }

        public void ChangeState(CameraState cameraState)
        {
            CameraState = cameraState;
            
            // VCam.gameObject.SetActive(cameraState == CameraState.Running);
            // StairCam.gameObject.SetActive(cameraState == CameraState.Stair);
        }

        public void FollowTriangularStickMen(List<List<StickMan>> triangularStickMen, float delay)
        {
            FollowTriangularStickMenRoutine(triangularStickMen, delay).StartCoroutine();
        }

        private IEnumerator FollowTriangularStickMenRoutine(List<List<StickMan>> triangularStickMen, float delay = 0f)
        {
            var followPoint = VCam.Follow;
            yield return new WaitForSeconds(delay);

            VCam.Follow = followPoint;
            

            var firstStickMan = triangularStickMen.First().First();
            var lastStickMan = triangularStickMen.Last().Last();

            yield return new WaitUntil(() => firstStickMan.IsStopped);
            
            while (!lastStickMan.IsStopped)
            {
                followPoint.transform.localPosition += Vector3.up * 4f * Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }
        }
    }
}
