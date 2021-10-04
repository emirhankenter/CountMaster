using Mek.Models.Stats;
using System;
using System.Collections.Generic;
using Mek.Models;
using UnityEngine;

namespace Game.Scripts.Models
{
    public class PlayerData : MekPlayerData
    {
        public static PlayerData Instance { get; } = new PlayerData();
        
        public static readonly Dictionary<string, BaseStat> Prefs = new Dictionary<string, BaseStat> {

            // MekPlayerData
            { MekPrefKeys.LastActive, new DateStat(DateTime.UtcNow) },

            // Game Specific PLayerData
            { PrefKeys.PlayerLevel, new IntStat(0, Int32.MaxValue, 1) },
            { PrefKeys.MusicEnabled, new BoolStat(true) },
            { PrefKeys.SoundFXEnabled, new BoolStat(true) },
            { PrefKeys.MyTest, new ObjectStat<TestClass>(JsonUtility.ToJson(new TestClass(){Number = 100}))}
        };

        public int PlayerLevel
        {
            get => PrefsManager.GetInt(PrefKeys.PlayerLevel);
            set => PrefsManager.SetInt(PrefKeys.PlayerLevel, value);
        }

        public bool MusicEnabled
        {
            get => PrefsManager.GetBool(PrefKeys.MusicEnabled);
            set => PrefsManager.SetBool(PrefKeys.MusicEnabled, value);
        }

        public bool SoundFXEnabled
        {
            get => PrefsManager.GetBool(PrefKeys.SoundFXEnabled);
            set => PrefsManager.SetBool(PrefKeys.SoundFXEnabled, value);
        }

        public TestClass MyTestObject
        {
            get => PrefsManager.GetObject<TestClass>(PrefKeys.MyTest);
            set => PrefsManager.SetObject(PrefKeys.MyTest, value);
        }
    }
}