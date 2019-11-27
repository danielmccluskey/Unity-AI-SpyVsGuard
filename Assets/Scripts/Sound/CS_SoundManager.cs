//==============================================================================================================================
// Project: The Life of Others
// File: CS_SoundManager.cs
// Author: Daniel McCluskey
// Date Created: 18/04/18
// Brief: This is the script that contains ease of access functions for loading and playing sounds..
// Last Edited by: Daniel McCluskey
//==============================================================================================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_SoundManager : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("The Current radio track that is playing")]
    [SerializeField] private int iCurrentRadioTrack;//The current Radio track being played

    [Tooltip("The Volume to set the tracks to")]
    [Range(0.0f, 1.0f)]
    [SerializeField] private float fVolume;//Volume to set the tracks to whilst playing.

    [Tooltip("The GameObject where you wish the sounds to play from")]
    [SerializeField] private GameObject SpeakerLocation;//To reference the Speakers Position.

    private int iAmountOfRadios;

    //List of Event names
    [Header("FMOD Events to play")]
    [FMODUnity.EventRef]
    public string[] SoundTracksToPlay;//List of Event names to play.

    private List<FMOD.Studio.EventInstance> EventInstanceList = new List<FMOD.Studio.EventInstance>();//FMOD Events

    [Header("Activated Radios")]
    public List<bool> bRadiosActivated = new List<bool>();

    private void Start()
    {
        bRadiosActivated.Clear();
        iAmountOfRadios = SoundTracksToPlay.Length;
        InitialiseRadioTracks();
        SwitchRadioTracks(iCurrentRadioTrack);//Switch to the given radio track
    }

    private void Update()
    {
    }

    // @brief	Function to Initialise the EventInstances for FMOD and set their positions.
    private void InitialiseRadioTracks()
    {
        for (int i = 0; i < SoundTracksToPlay.Length; i++)
        {
            FMOD.Studio.EventInstance TempEventInstance = FMODUnity.RuntimeManager.CreateInstance(SoundTracksToPlay[i]);
            EventInstanceList.Add(TempEventInstance);
            bRadiosActivated.Add(false);
            EventInstanceList[i].set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(SpeakerLocation));//Move the sound location to the Radio speakers
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(EventInstanceList[i], SpeakerLocation.transform, SpeakerLocation.GetComponent<Rigidbody>());//Attach the instance to the speakers, so sound follows when moved.
        }
    }

    // @brief	Function to restart playing all of the tracks from the beginning.
    private void RestartAllTracks()
    {
        for (int i = 0; i < SoundTracksToPlay.Length; i++)
        {
            EventInstanceList[i].start();
        }
    }

    // @brief	Function that increments "iCurrentRadioTrack" to toggle between dfifferent radio tracks.
    private void NextRadioTrack()
    {
        iCurrentRadioTrack++;
        if (iCurrentRadioTrack >= iAmountOfRadios)
        {
            iCurrentRadioTrack = 0;
        }
    }

    // @brief	Function to mute all radio tracks and then turn on a specific one.
    // @param	int a_iRadioTrack = the radio track to turn on.
    public void SwitchRadioTracks(int a_iRadioTrack)
    {
        MuteAllTracks();//Mute every track and deactivate them
        EventInstanceList[a_iRadioTrack].setVolume(fVolume);
        bRadiosActivated[a_iRadioTrack] = true;
    }

    // @brief	Function to Mute all tracks and deactivate them
    public void MuteAllTracks()
    {
        for (int i = 0; i < SoundTracksToPlay.Length; i++)
        {
            EventInstanceList[i].setVolume(0.0f);
            bRadiosActivated[i] = false;
        }
    }

    private void OnDestroy()
    {
        for (int i = 0; i < SoundTracksToPlay.Length; i++)
        {
            EventInstanceList[i].release();
        }
    }

    public void PlayTrack(int a_iIndex)
    {
        SwitchRadioTracks(0);
    }

    // @brief	Function to play a sound and attach it to a game object so it will move with it.
    // @brief	Looped Sounds will play forever with no way to stop it.
    // @param	Vector3 a_v3Position = XYZ Vector3 to play the sound at.
    // @param	string a_sSoundEventName = Sound to Play
    // @example Play3DSoundAtPos(new Vector3(0.0f,0.0f,0.0f), "PlayerShoot");
    public static void PlaySoundOnObject(GameObject a_goGameObject, string a_sSoundEventName)
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached(("event:/" + a_sSoundEventName), a_goGameObject);
    }

    // @brief	Function to play a sound and attach it to a transform so it will move with it.
    // @brief	Looped Sounds will play forever with no way to stop it.
    // @param	Vector3 a_v3Position = XYZ Vector3 to play the sound at.
    // @param	string a_sSoundEventName = Sound to Play
    // @example Play3DSoundAtPos(new Vector3(0.0f,0.0f,0.0f), "PlayerShoot");
    public static void PlaySoundOnObject(Transform a_Position, string a_sSoundEventName)
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached(("event:/" + a_sSoundEventName), a_Position.gameObject);
    }

    // @brief	Function to play a sound and attach it to a transform so it will move with it.
    // @brief   USed for when [FMODUnity.EventRef] is used
    // @brief	Looped Sounds will play forever with no way to stop it.
    // @param	Vector3 a_v3Position = XYZ Vector3 to play the sound at.
    // @param	string a_sSoundEventName = Sound to Play
    // @example Play3DSoundAtPos(new Vector3(0.0f,0.0f,0.0f), "PlayerShoot");
    public static void PlaySoundOnObjectWER(Transform a_Position, string a_sSoundEventName)
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached((a_sSoundEventName), a_Position.gameObject);
    }

    // @brief	Function to play a 3D sound event at a given Vector3 Position
    // @brief	Looped Sounds will play forever with no way to stop it.
    // @param	Vector3 a_v3Position = XYZ Vector3 to play the sound at.
    // @param	string a_sSoundEventName = Sound to Play
    // @example Play3DSoundAtPos(new Vector3(0.0f,0.0f,0.0f), "PlayerShoot");
    public static void PlaySoundAtPos(Vector3 a_v3Position, string a_sSoundEventName)
    {
        FMODUnity.RuntimeManager.PlayOneShot(("event:/" + a_sSoundEventName), a_v3Position);
    }

    // @brief	Function to play a 3D sound event at a given GameObject's Position
    // @brief	Looped Sounds will play forever with no way to stop it.
    // @param	GameObject a_goGameObject = GameObject to play the sound at.
    // @param	string a_sSoundEventName = Sound to Play
    // @example Play3DSoundAtPos(gameObject, "PlayerShoot");
    public static void PlaySoundAtPos(GameObject a_goGameObject, string a_sSoundEventName)
    {
        FMODUnity.RuntimeManager.PlayOneShot(("event:/" + a_sSoundEventName), a_goGameObject.transform.position);
    }
}