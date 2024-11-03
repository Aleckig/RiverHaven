using UnityEngine;
using MenuManagement.Data; 
using UnityEngine.Audio;

public class AudioSettingsLoader : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer; 

    private DataManager dataManager;

    private void Start()
    {
        // Find the DataManager in the scene
        dataManager = FindObjectOfType<DataManager>();
        if (dataManager == null)
        {
            Debug.LogError("AudioSettingsLoader: DataManager not found in the scene!");
            return;
        }

        dataManager.Load();

        ApplyVolumeSettings();
    }

    // Applies the saved volume settings to the AudioMixer
    private void ApplyVolumeSettings()
    {
        SetVolume("Master", dataManager.MasterVolume);
        SetVolume("SFX", dataManager.SfxVolume);
        SetVolume("Music", dataManager.MusicVolume);

        Debug.Log("Audio settings loaded and applied at scene start.");
    }

    
    private void SetVolume(string parameterName, float volume)
    {
        if (audioMixer != null)
        {
            float dbVolume = volume > 0 ? Mathf.Log10(volume) * 20 : -80f; // Convert linear value to dB
            audioMixer.SetFloat(parameterName, dbVolume);
        }
        else
        {
            Debug.LogError("AudioSettingsLoader: AudioMixer is not assigned.");
        }
    }
}
