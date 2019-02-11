using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public static AudioManager instance;

    public AudioSource[] sources;

    public AudioSource backgroundMusicSource;

    public int currentBPM;
    private double BeatLength {
        get {
            return 60 / ((double)currentBPM);
        }
    }

    public Song song;

    private double startDSPTime;
    private const double PLAY_OFFSET = 0.4;
    private void Awake() {
        instance = this;
        startDSPTime = AudioSettings.dspTime;
    }

    private double lastRequestedBeat = 0;
    private List<Note> notes = new List<Note>(32);
    private float timeUntilNextRequest = 0;
    private const double REQUEST_INTERVAL = 0.5f;
    void Update () {
        if(timeUntilNextRequest <= 0) {
            notes.Clear();
            double dspTimeFromStart = AudioSettings.dspTime - startDSPTime;
            double dspEndTime = dspTimeFromStart + REQUEST_INTERVAL + 0.1;
            song.GetNotes(lastRequestedBeat + 0.000001, dspEndTime/ BeatLength, ref notes);

            for (int i = 0; i < notes.Count; i++) {
                Note note = notes[i];
                double dspPlayTime = startDSPTime + (note.beat * BeatLength) + PLAY_OFFSET;
                PlayScheduled(note.clip, dspPlayTime, note.volume);
                if(note.beat > lastRequestedBeat) {
                    lastRequestedBeat = note.beat;
                }
            }
            timeUntilNextRequest = (float)REQUEST_INTERVAL;
        } else {
            timeUntilNextRequest -= Time.deltaTime;
        }
	}

    internal void StartBackgroundMusic() {
        double modBeat = lastRequestedBeat % song.length;
        double lastSongStart = lastRequestedBeat - modBeat;
        double nextSongStart = lastSongStart + song.length;
        double musicStartDSP = startDSPTime + (nextSongStart * BeatLength) + PLAY_OFFSET;
        backgroundMusicSource.PlayScheduled(musicStartDSP);
    }

    private int currentAudioSource = 0;
    private void PlayScheduled(AudioClip clip, double time, float volume = 1) {
        AudioSource current = sources[currentAudioSource];
        current.clip = clip;
        current.volume = volume;
        current.PlayScheduled(time);
        currentAudioSource = (currentAudioSource + 1) % sources.Length;
    }
}
