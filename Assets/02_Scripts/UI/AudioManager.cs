using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public enum BgmType { Lobby, StageA, StageB }
    public enum Sfx { Dead, Hit }

    [Header("브금(BGM) 3종")]
    [SerializeField] private AudioClip lobbyBgm;
    [SerializeField] private AudioClip stageBgmA;
    [SerializeField] private AudioClip stageBgmB;
    [Range(0.0f, 1.0f)][SerializeField] private float bgmVolume = 1.0f;

    private AudioSource bgmPlayer;

    [Header("효과(SFX)")]
    [SerializeField] private AudioClip[] sfxClips;
    [Range(0.0f, 1.0f)][SerializeField] private float sfxVolume = 1.0f;
    [SerializeField] private int channels;

    AudioSource[] sfxPlayers;
    int channelIndex;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        Init();
    }

    void Init()
    {
        var bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.SetParent(transform, false);

        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;


        channels = Mathf.Max(1, channels);
        var sfxObject = new GameObject("sfxPlayer");
        sfxObject.transform.SetParent(transform, false);

        sfxPlayers = new AudioSource[channels];
        for (int i = 0; i < sfxPlayers.Length; i++)
        {
            var sfxP = sfxObject.AddComponent<AudioSource>();
            sfxP.playOnAwake = false;
            sfxP.volume = sfxVolume;
            sfxP.spatialBlend = 0;
            sfxPlayers[i] = sfxP;
        }
    }
    public void PlayBgm(BgmType type)
    {
        if (bgmPlayer == null) return;

        AudioClip target = null;
        switch (type)
        {
            case (BgmType.Lobby): target = lobbyBgm; break;
            case (BgmType.StageA): target = stageBgmA; break;
            case (BgmType.StageB): target = stageBgmB; break;
        }

        if (target == null) return;

        if (bgmPlayer.clip == target && bgmPlayer.isPlaying) return;

        bgmPlayer.Stop();
        bgmPlayer.clip = target;
        bgmPlayer.Play();
    }

    public void PlaySfx(Sfx sfx)
    {
        if (sfxPlayers == null || sfxPlayers.Length == 0) return;
        if (sfxClips == null || sfxClips.Length == 0) return;

        int clipIndex = 0;

        switch (sfx)
        {
            case Sfx.Dead:
                clipIndex = 0; // Dead
                break;

            case Sfx.Hit:
                // Hit0(1) / Hit1(2) 있으면 랜덤
                if (sfxClips.Length >= 3 && sfxClips[2] != null)
                    clipIndex = UnityEngine.Random.Range(1, 2); // 1 or 2
                else
                    clipIndex = 1;
                break;
        }

        if (clipIndex < 0 || clipIndex >= sfxClips.Length || sfxClips[clipIndex] == null) return;

        for (int i = 0; i < sfxPlayers.Length; i++)
        {
            int idx = (i + channelIndex) % sfxPlayers.Length;
            if (sfxPlayers[idx].isPlaying) continue;

            channelIndex = (idx + 1) % sfxPlayers.Length;
            sfxPlayers[idx].clip = sfxClips[clipIndex];
            sfxPlayers[idx].Play();
            return;
        }

        int fallback = channelIndex % sfxPlayers.Length;
        channelIndex = (fallback + 1) % sfxPlayers.Length;

        sfxPlayers[fallback].Stop();
        sfxPlayers[fallback].clip = sfxClips[clipIndex];
        sfxPlayers[fallback].Play();
    }
    public void PlayRandomStageBgm()
    {
        // 0 또는 1
        var pick = UnityEngine.Random.Range(0, 2);
        PlayBgm(pick == 0 ? BgmType.StageA : BgmType.StageB);
    }
}
