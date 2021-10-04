using System.Collections.Generic;
using UnityEngine;

namespace Mek.Audio
{
    public class AudioWorker : MonoBehaviour
    {
        private static GameObject _audioPool;

        private static List<AudioSource> _pool = new List<AudioSource>();

        public void Push(AudioSource audio)
        {
            audio.transform.SetParent(_audioPool.transform, true);
            _pool.Add(audio);
        }

        public AudioSource Pull()
        {
            AudioSource audio;
            if (_pool.Count == 0)
            {
                var go = new GameObject("Audio");
                audio = go.AddComponent<AudioSource>();
            }
            else
            {
                audio = _pool[0];
                _pool.Remove(audio);
            }
            audio.transform.SetParent(null, true);

            return audio;
        }

        private static AudioWorker _instance;

        public static AudioWorker Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<AudioWorker>();
                    if (_instance == null)
                    {
                        var go = new GameObject(nameof(AudioWorker));
                        var audioWorker = go.AddComponent<AudioWorker>();
                        _audioPool = new GameObject("AudioPool");
                        _instance = audioWorker;
                        DontDestroyOnLoad(audioWorker);
                    }
                }

                return _instance;
            }
        }
    }
}