using System.Collections.Generic;
using Mek.Coroutines;
using UnityEngine;

namespace Mek.Audio
{
    public static class AudioController
    {
        private static List<AudioClip> _clips = new List<AudioClip>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clip">Audio</param>
        /// <param name="volume">Volume</param>
        /// <param name="position">World space Position of the audio</param>
        /// <param name="in3DSpace">Will audio be played in 3D space?</param>
        /// <param name="minDistance">MaxDistance</param>
        /// <param name="maxDistance">MinDistance</param>
        public static void Play(AudioClip clip, float volume = 1f, Vector3 position = default, bool in3DSpace = false, float minDistance = 0f, float maxDistance = 100f)
        {
            AudioSource audio = AudioWorker.Instance.Pull();
            audio.clip = clip;
            audio.gameObject.name = clip.name;
            audio.transform.position = position;
            audio.spatialBlend = in3DSpace ? 1 : 0;
            audio.minDistance = minDistance;
            audio.maxDistance = maxDistance;
            audio.volume = volume;
            audio.Play();
            _clips.Add(clip);
            
            CoroutineController.DoAfterGivenTime(clip.length, () =>
            {
                AudioWorker.Instance.Push(audio);
                _clips.Remove(clip);
            });
        }

        public static bool IsPlaying(AudioClip clip)
        {
            return _clips.Contains(clip);
        }
    }
}
