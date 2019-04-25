using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Audio
{
    [Serializable]
    class VolumeData
    {
        public float ambiance;
        public float soundEffect;

        public VolumeData(float m, float s)
        {
            ambiance = m;
            soundEffect = s;
        }
    }

    public class AudioManager : MonoBehaviour
    {
        private static AudioManager _instance;

        public float _soundEffectVolume = 1f;
        public float _ambianceVolume = 1f;

        private string _fileName = "ChickenARVolumeData.dat";

        public Sound[] SoundEffects;

        public static AudioManager Instance { get { return _instance; } }

        public static float SoundEffectVolume
        {
            get { return _instance._soundEffectVolume; }
            set {
                _instance._soundEffectVolume = Mathf.Max(Mathf.Min(value, 1f), 0f);
                for(int i = 0; i < _instance.SoundEffects.Length; i++)
                {
                    _instance.SoundEffects[i].Source.volume = _instance._soundEffectVolume;
                }
            }
        }

        public static float AmbianceVolume
        {
            get { return _instance._ambianceVolume;}
            set { _instance._ambianceVolume = Mathf.Max(Mathf.Min(value, 1f), 0f); }
        }

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                DestroyImmediate(gameObject);
            }
        }

        public static void PlaySoundEffect(string name)
        {
            Sound t = Array.Find(_instance.SoundEffects, sound => sound.Name == name);
            if(t == null)
            {
                Debug.LogError("Sound effect " + name + " not found", _instance.gameObject);
                return;
            }
            t.Source.Play();
        }

        private void Save()
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + _fileName, FileMode.OpenOrCreate);
            VolumeData data = new VolumeData(_ambianceVolume, _soundEffectVolume);
            bf.Serialize(file, data);
            file.Close();
        }

        private void Load()
        {
            if (File.Exists(Application.persistentDataPath + "/" + _fileName))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + "/" + _fileName, FileMode.Open);
                VolumeData data = (VolumeData)bf.Deserialize(file);
                file.Close();
                _ambianceVolume = data.ambiance;
                _soundEffectVolume = data.soundEffect;
            }
        }

        private void OnEnable()
        {
            Load();
            foreach (Sound s in SoundEffects)
            {
                s.Source = gameObject.AddComponent<AudioSource>();
                s.Source.clip = s.Clip;
                s.Source.pitch = s.Pitch;
                s.Source.volume = SoundEffectVolume;
                s.Source.spatialBlend = 0.0f;
            }
        }

        private void OnDisable()
        {
            Save();
        }
    }
}

