using UnityEngine;

namespace Audio{
    [System.Serializable]
    public class Sound
    {
        public string Name;
        public AudioClip Clip;
        [Range(0.1f, 3f)] public float Pitch = 1.0f;
        public bool Loop = false;
        [HideInInspector] public AudioSource Source;
    }
}

