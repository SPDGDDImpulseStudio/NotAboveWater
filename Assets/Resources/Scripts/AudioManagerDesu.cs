using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerDesu : ISingleton<AudioManagerDesu>
{
    #region Attributes

    public List<AudioSource> tracks_BGM;
    public List<AudioSource> tracks_SFX;

    #endregion

    public void PlayBGM(int trackNumber)
    {
        switch (trackNumber)
        {
            case 0:
                tracks_BGM[trackNumber].Play();
                break;
        }
    }

    public void PlaySFX(int trackNumber)
    {
        switch (trackNumber)
        {
            case 0:
                tracks_SFX[trackNumber].Play();
                break;
        }
    }
}
