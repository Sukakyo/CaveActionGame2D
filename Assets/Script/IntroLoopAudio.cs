using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// イントロ付きループ BGM を制御するクラスです。
/// </summary>
/// <remarks>
/// WebGL では PlayScheduled で再生するとループしないのでその対応を入れている。
/// WebGL では２つの AudioSource を交互に再生。
/// </remarks>
public class IntroLoopAudio : MonoBehaviour
{
    /// <summary>BGM のイントロ部分の音声データ。</summary>
    [SerializeField] private AudioClip AudioClipIntro;

    /// <summary>BGM のループ部分の音声データ。</summary>
    [SerializeField] private AudioClip AudioClipLoop;

    [SerializeField] private AudioMixerGroup AudioMixerGroup;

    /// <summary>BGM のイントロ部分の AudioSource。</summary>
    private AudioSource _introAudioSource;

    /// <summary>BGM のループ部分の AudioSource。</summary>
    private AudioSource[] _loopAudioSources = new AudioSource[2];

    /// <summary>一時停止中かどうか。</summary>
    private bool _isPause;

    /// <summary>現在の再生するループ部分のインデックス。</summary>
    private int _nowPlayIndex = 0;

    /// <summary>ループ部分に使用する AudioSource の数。</summary>
    private int _loopSourceCount = 0;

    /// <summary>再生中であるかどうか。一時停止、非アクティブの場合は false を返す。</summary>
    private bool IsPlaying
      => (_introAudioSource.isPlaying || _introAudioSource.time > 0)
        || (_loopAudioSources[0].isPlaying || _loopAudioSources[0].time > 0)
        || (_loopAudioSources[1] != null && (_loopAudioSources[1].isPlaying || _loopAudioSources[1].time > 0));

    /// <summary>現在アクティブで再生しているループ側の AudioSource。</summary>
    private AudioSource LoopAudioSourceActive
      => _loopAudioSources[1] != null && _loopAudioSources[1].time > 0 ? _loopAudioSources[1] : _loopAudioSources[0];

    /// <summary>現在の再生時間 (s)。</summary>
    public float time
      => _introAudioSource == null ? 0
        : _introAudioSource.time > 0 ? _introAudioSource.time
        : LoopAudioSourceActive.time > 0 ? AudioClipIntro.length + LoopAudioSourceActive.time
        : 0;


    void Awake()
    {
        _loopSourceCount = 2;   // WebGL でなければ 1 でもよい

        // AudioSource を自身に追加
        _introAudioSource = gameObject.AddComponent<AudioSource>();
        _introAudioSource.outputAudioMixerGroup = AudioMixerGroup;
        _loopAudioSources[0] = gameObject.AddComponent<AudioSource>();
        _loopAudioSources[0].outputAudioMixerGroup = AudioMixerGroup;
        if (_loopSourceCount >= 2)
        {
            _loopAudioSources[1] = gameObject.AddComponent<AudioSource>();
            _loopAudioSources[1].outputAudioMixerGroup = AudioMixerGroup;
        }

        _introAudioSource.clip = AudioClipIntro;
        _introAudioSource.loop = false;
        _introAudioSource.playOnAwake = false;

        _loopAudioSources[0].clip = AudioClipLoop;
        _loopAudioSources[0].loop = _loopSourceCount == 1;
        _loopAudioSources[0].playOnAwake = false;
        if (_loopAudioSources[1] != null)
        {
            _loopAudioSources[1].clip = AudioClipLoop;
            _loopAudioSources[1].loop = false;
            _loopAudioSources[1].playOnAwake = false;
        }
    
    
        
    }


    void Update()
    {
        // WebGL のためのループ切り替え処理
        if (_loopSourceCount >= 2)
        {
            // 終了する１秒前から次の再生のスケジュールを登録する
            if (_nowPlayIndex == 0 && _loopAudioSources[0].time >= AudioClipLoop.length - 1)
            {
                _loopAudioSources[1].PlayScheduled(AudioSettings.dspTime + (AudioClipLoop.length - _loopAudioSources[0].time));
                _nowPlayIndex = 1;
            }
            else if (_nowPlayIndex == 1 && _loopAudioSources[1].time >= AudioClipLoop.length - 1)
            {
                _loopAudioSources[0].PlayScheduled(AudioSettings.dspTime + (AudioClipLoop.length - _loopAudioSources[1].time));
                _nowPlayIndex = 0;
            }
        }
    }

    public void Play()
    {
        // クリップが設定されていない場合は何もしない
        if (_introAudioSource == null || _loopAudioSources == null) return;

        // Pause 中は isPlaying は false
        // 標準機能だけでは一時停止中か判別不可能
        if (_isPause)
        {
            _introAudioSource.UnPause();
            if (_introAudioSource.isPlaying)
            {
                // イントロ中ならループ開始時間を残り時間で再設定
                _loopAudioSources[0].Stop();
                _loopAudioSources[0].PlayScheduled(AudioSettings.dspTime + AudioClipIntro.length - _introAudioSource.time);
            }
            else
            {
                if (_loopSourceCount >= 2)
                {
                    // WebGL の場合は切り替え処理を実行
                    if (_loopAudioSources[0].time > 0)
                    {
                        _loopAudioSources[0].UnPause();
                        if (_loopAudioSources[0].time >= AudioClipLoop.length - 1)
                        {
                            _loopAudioSources[1].Stop();
                            _loopAudioSources[1].PlayScheduled(AudioSettings.dspTime + (AudioClipLoop.length - _loopAudioSources[0].time));
                            _nowPlayIndex = 1;
                        }
                    }
                    else
                    {
                        _loopAudioSources[1].UnPause();
                        if (_loopAudioSources[1].time >= AudioClipLoop.length - 1)
                        {
                            _loopAudioSources[0].Stop();
                            _loopAudioSources[0].PlayScheduled(AudioSettings.dspTime + (AudioClipLoop.length - _loopAudioSources[0].time));
                            _nowPlayIndex = 0;
                        }
                    }
                }
                else
                {
                    // WebGL 以外は UnPause するだけ
                    _loopAudioSources[0].UnPause();
                }
            }
        }
        else if (IsPlaying == false)
        {
            // 最初から再生
            Stop();
            _introAudioSource.Play();

            // イントロの時間が経過した後に再生できるようにする
            // 設定する時間はゲーム刑か時間での設定となる
            _loopAudioSources[0].PlayScheduled(AudioSettings.dspTime + AudioClipIntro.length);
        }

        _isPause = false;
    }

    /// <summary>BGM を一時停止します。</summary>
    public void Pause()
    {
        if (_introAudioSource == null || _loopAudioSources == null) return;

        _introAudioSource.Pause();
        _loopAudioSources[0].Pause();
        if (_loopAudioSources[1] != null) _loopAudioSources[1].Pause();

        _isPause = true;
    }

    /// <summary>BGM を停止します。</summary>
    public void Stop()
    {
        if (_introAudioSource == null || _loopAudioSources == null) return;

        _introAudioSource.Stop();
        _loopAudioSources[0].Stop();
        if (_loopAudioSources[1] != null) _loopAudioSources[1].Stop();

        _isPause = false;
    }

    public void DontDestroy()
    {
        DontDestroyOnLoad(this);
    }


    public void SetMusic(AudioClip intro,AudioClip loop)
    {
        AudioClipIntro = intro;
        AudioClipLoop = loop;
        _introAudioSource = null;
        _loopAudioSources = new AudioSource[2];
        _isPause = false;
        _nowPlayIndex = 0;
        _loopSourceCount = 0;

        AudioSource[] audioSources = GetComponents<AudioSource>();

        foreach (AudioSource audioSource in audioSources)
        {
            Destroy(audioSource);
        }

        Awake();
    }
}