using System;
using System.Collections.Generic;
using UnityEngine;

public class Song : ScriptableObject {
    public double length;
    public Track[] tracks;

    public void GetNotes(double startBeat, double endBeat, ref List<Note> notes) {
        double startBeatMod = startBeat % length;
        double endBeatMod = endBeat % length;
        double startBeatOffset = startBeat - startBeatMod;
        double endBeatOffset = endBeat - endBeatMod;
        if (startBeatMod > endBeatMod) {
            _GetNotes(startBeatMod, length, startBeatOffset, ref notes);
            _GetNotes(0, endBeatMod, endBeatOffset, ref notes);
        } else {
            if(startBeatOffset != endBeatOffset) {
                Debug.LogError("the offsets were different! "  + startBeatOffset + " end: " + endBeatOffset);
            }
            _GetNotes(startBeatMod, endBeatMod, startBeatOffset, ref notes);
        }
    }

    private void _GetNotes(double startBeat, double endBeat, double offset, ref List<Note> notes) {
        for (int i = 0; i < tracks.Length; i++) {
            Track track = tracks[i];
            for (int j = 0; j < track.beats.Length; j++) {
                double _beat = track.beats[j];
                if(_beat >= startBeat && _beat <= endBeat) {
                    notes.Add(new Note() {
                        clip = track.clip,
                        beat = _beat + offset
                    });
                }
            }
        }
    }
}

public struct Note {
    public AudioClip clip;
    public double beat;
}

[Serializable]
public class Track {
    public AudioClip clip;
    public double[] beats;
}

