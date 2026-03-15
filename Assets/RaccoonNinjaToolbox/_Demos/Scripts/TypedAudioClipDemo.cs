using RaccoonNinjaToolbox.Scripts.ScriptableObjects;
using TMPro;
using UnityEngine;

namespace RaccoonNinjaToolbox._Demo.Scripts
{
    public class TypedAudioClipDemo : MonoBehaviour
    {
        [SerializeField] private TypedAudioClip typedAudioClip;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private TextMeshProUGUI clipsPlayedText;
        [SerializeField] private TextMeshProUGUI clipsCompletedText;

        private int _clipsPlayed;
        private int _clipsCompleted;

        private void Start()
        {
            UpdateUI();
        }

        public void PlayClip()
        {
            _clipsPlayed++;
            typedAudioClip.Play(audioSource, OnClipFinished);
            UpdateUI();
        }

        private void OnClipFinished()
        {
            _clipsCompleted++;
            UpdateUI();
        }

        private void UpdateUI()
        {
            clipsPlayedText.TrySetText($"Audio clips played: {_clipsPlayed}");
            clipsCompletedText.TrySetText($"Audio clips completed: {_clipsCompleted}");
        }
    }
}
