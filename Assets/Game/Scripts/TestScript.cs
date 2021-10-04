using System.Collections;
using UnityEngine;
using System;
using Game.Scripts.Models;
using Mek.Extensions;
using Mek.Interfaces;
using Mek.Localization;
using Mek.Remote;
using Mek.Models;
using Mek.Utilities;
using Mek.Coroutines;
using Mek.Navigation;
using Sirenix.OdinInspector;

namespace Game.Scripts
{
    [Serializable]
    public class TestClass : IObservableModel
    {
        public event Action PropertyChanged;
        public int Number;
    }
    public class TestScript : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particleA;
        [SerializeField] private ParticleSystem _particleB;
        [SerializeField] private GameObject _cube;

        private const string CoroutineKey = "TestRoutine";

        private MekLog Log => new MekLog(nameof(TestScript), DebugLevel.Debug);

        private void Start()
        {
            //PlayerData.Prefs[PrefStats.PlayerLevel].Changed += OnPlayerLevelChanged;
        }

        private void OnDestroy()
        {
            //PlayerData.Prefs[PrefStats.PlayerLevel].Changed -= OnPlayerLevelChanged;
        }

        private void OnPlayerLevelChanged()
        {
            Log.Info($"Player Level Changed to: {PlayerData.Instance.PlayerLevel}");
        }


        private IEnumerator TestRoutine()
        {
            Log.Debug("TestRoutineStarted");
            var timer = 3f;
            while (timer >= 0)
            {
                timer -= Time.deltaTime;
                
                // Log.Debug($"Timer: {timer}");

                yield return new WaitForEndOfFrame();
            }

            Log.Debug(CoroutineKey);
        }

        private void OnApplicationQuit()
        {
            //PlayerData.Instance.LastActive = DateTime.Now;
        }

        [Button]
        private void ParticleA()
        {
            _particleA.Spawn(Vector3.zero, Quaternion.identity);
        }
        [Button]
        private void ParticleB()
        {
            _particleB.Spawn(Vector3.zero, Quaternion.identity);
        }

        [Button]
        private void OpenMyPanel()
        {
            Navigation.Panel.Open(ViewTypes.MyPanel);
        }
        [Button]
        private void CloseMyPanel()
        {
            Navigation.Panel.CloseActiveContent();
        }

        [Button]
        private void OpenMyPopup()
        {
            Navigation.Popup.Open(ViewTypes.MyPopup);
        }
        [Button]
        private void CloseMyPopup()
        {
            Navigation.Popup.CloseActiveContent();
        }

        [Button]
        private void Set()
        {
            PlayerData.Instance.MyTestObject = new TestClass(){Number = 123123};
        }

        [Button]
        private void Get()
        {
            var myTest = PlayerData.Instance.MyTestObject;
            Debug.LogError($"{myTest.Number}");
        }

        [Button]
        private void SetLanguage(SystemLanguage lang)
        {
            LocalizationManager.SetLanguage(lang);
        }

        [SerializeField] private TestObject _prefab;
        [Button]
        private void Spawn()
        {
            var testObject = _prefab.Spawn();

            CoroutineController.DoAfterGivenTime(1, () =>
            {
                testObject.Recycle();
            });
        }

        [Button]
        private void StartTestRoutine()
        {
            // if (!CoroutineController.IsCoroutineRunning(CoroutineKey))
            // {
            //     TestRoutine().StartCoroutine(CoroutineKey);
            //     CoroutineKey.OnRoutineWithKeyCompleted(() =>
            //     {
            //         Debug.Log($"RoutineFinished");
            //     });
            // }

            if (!CoroutineController.IsCoroutineRunning(TestRoutine()))
            {
                TestRoutine().StartCoroutine();
            }
        }

        [Button]
        private void RegisterPopup()
        {
            Navigation.Popup.Register(ViewTypes.MyPopup);
            Debug.LogError("Registered");
        }

        [Button]
        private void RemoteTest()
        {
            RemoteConfig.DoAfterInitialized(() =>
            {
                Debug.LogError($"IntKey: {GameConfig.IntRemoteExample.Key}, Value: {(int)GameConfig.IntRemoteExample}");
                Debug.LogError($"StringKey: {GameConfig.StringRemoteExample.Key}, Value: {GameConfig.StringRemoteExample}");
                Debug.LogError($"LongKey: {GameConfig.LongRemoteExample.Key}, Value: {GameConfig.LongRemoteExample}");
                Debug.LogError($"FloatKey: {GameConfig.FloatRemoteExample.Key}, Value: {GameConfig.FloatRemoteExample}");
                
                var test = (TestClass) GameConfig.JsonRemoteExample;
                Debug.LogError($"Json<{nameof(TestClass)}>Key: {GameConfig.JsonRemoteExample.Key}, Value: {test.Number}");
            });
        }

        private class tojsonobj
        {
            public int Value;

            public tojsonobj(int value)
            {
                Value = value;
            }
        }
    }
}