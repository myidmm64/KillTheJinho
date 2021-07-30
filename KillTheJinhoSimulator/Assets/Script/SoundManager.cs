using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource BgAudioSource = null;
    // Start is called before the first frame update
    void Start()
    {
        BgAudioSource.Play();
    }
    public void BgSoundStop()
    {
        BgAudioSource.Pause();
    }
    public void BgSoundPlay()
    {
        BgAudioSource.Play();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void SFXPlay(string sfxName, AudioClip clip)
    {
        GameObject go = new GameObject(sfxName + "Sound");
        AudioSource audioSource = go.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.Play();

        Destroy(go, clip.length);
    }
}
