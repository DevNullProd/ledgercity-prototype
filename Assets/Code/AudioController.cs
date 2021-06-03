using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    //**MANAGE Sound & Music*//
    //**ATTACHED TO GameManager.GameObject*//
    /*NOTES:
     * 
     */

    public static AudioController audioCntInst;

    [SerializeField] GameObject CityAmbience = null;

    public AudioSource MusicSource = null;              //Atached to camera
    [SerializeField] AudioClip[] MusicClip = null;

    AudioSource audioSource = null;    //Attached to PLAYER
    [SerializeField] AudioClip[] SoundsEffects = null; 

    public bool SoundEffectsOff = false;
    public bool MusicOff = false;

    void Awake()
    {
        audioCntInst = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = Camera.main.GetComponent<AudioSource>();
        SetCaveAmbience(false);
        PlayMusic(0);
    }

    public void PlayAudio(string audioClip, float volume, float pitch = 1f)
    {
        if (SoundEffectsOff)
            return;

        switch (audioClip)
        {
            case "click":
                audioSource.PlayOneShot(SoundsEffects[0], volume + Random.Range(-0.15f, 0.15f));
                break;

            case "error":
                audioSource.PlayOneShot(SoundsEffects[1], volume + Random.Range(-0.15f, 0.15f));
                break;

            case "lose":
                audioSource.PlayOneShot(SoundsEffects[2], volume + Random.Range(-0.15f, 0.15f));
                break;

            case "win":
                audioSource.PlayOneShot(SoundsEffects[3], volume + Random.Range(-0.15f, 0.15f));
                break;

            case "winbig":
                audioSource.PlayOneShot(SoundsEffects[4], volume + Random.Range(-0.15f, 0.15f));
                break;

            case "score":
                audioSource.PlayOneShot(SoundsEffects[5], volume + Random.Range(-0.15f, 0.15f));
                break;

            case "boo":
                audioSource.PlayOneShot(SoundsEffects[6], volume + Random.Range(-0.15f, 0.15f));
                break;

            case "hammmer":
                audioSource.PlayOneShot(SoundsEffects[7], volume + Random.Range(-0.15f, 0.15f));
                break;

            case "trafo":
                audioSource.PlayOneShot(SoundsEffects[8], volume + Random.Range(-0.15f, 0.15f));
                break;

            case "wood":
                audioSource.PlayOneShot(SoundsEffects[9], volume + Random.Range(-0.05f, 0.05f));
                break;

            case "speed":
                audioSource.PlayOneShot(SoundsEffects[10], volume + Random.Range(-0.05f, 0.05f));
                break;

            case "spin":
                audioSource.PlayOneShot(SoundsEffects[11], volume + Random.Range(-0.01f, 0.1f));
                break;

            case "elevator":
                audioSource.PlayOneShot(SoundsEffects[12], volume + Random.Range(-0.15f, 0.15f));
                break;

            case "slow":
                audioSource.PlayOneShot(SoundsEffects[13], volume + Random.Range(-0.15f, 0.15f));
                break;

            case "pickup":
                audioSource.PlayOneShot(SoundsEffects[14], volume + Random.Range(-0.15f, 0.15f));
                break;

            case "warning":
                audioSource.PlayOneShot(SoundsEffects[15], volume + Random.Range(-0.15f, 0.15f));
                break;

            case "spellstone":
                audioSource.PlayOneShot(SoundsEffects[16], volume + Random.Range(-0.15f, 0.15f));
                break;

            case "spellteleport":
                audioSource.PlayOneShot(SoundsEffects[17], volume + Random.Range(-0.15f, 0.15f));
                break;

            case "thunder":
                audioSource.PlayOneShot(SoundsEffects[0], volume + Random.Range(-0.15f, 0.15f));
                break;

            default:
                Debug.LogError("Non existing audio CLIP");
                break;

        }
    }

    public void PlayClick()
    {
        PlayAudio("click", 0.5f);
    }

    public void PlayMusic(int music)
    {
        if (MusicClip.Length > 0)
        {
            MusicSource.clip = MusicClip[music];
            if (MusicOff == false)
                MusicSource.Play();
        }
    }

    public void Play_Thunder()
    {
        PlayAudio("thunder", 0.5f);
    }

    public void SetCaveAmbience(bool enable_CaveAmbience_Sound)
    {
        if (CityAmbience != null)
        {
            if (enable_CaveAmbience_Sound)
                CityAmbience.SetActive(true);
            else
                CityAmbience.SetActive(false);
        }
    }
}
