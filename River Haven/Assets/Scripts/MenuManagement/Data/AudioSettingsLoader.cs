using UnityEngine;
using MenuManagement.Data; // Assuming this is where your DataManager is located
using UnityEngine.Audio;

public class AudioSettingsLoader : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer; // Assign your AudioMixer in the Inspector

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

        // Load the data from the DataManager
        dataManager.Load();

        // Apply the volume settings from the saved data
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

    // Helper function to set the volume on the AudioMixer
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
