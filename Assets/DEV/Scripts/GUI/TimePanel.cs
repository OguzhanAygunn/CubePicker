using Cysharp.Threading.Tasks;
using DG.Tweening;
using EVERY;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimePanel : MonoBehaviour
{
    public static TimePanel instance;

    [Title("Main")]
    [SerializeField] bool active;
    [SerializeField] CanvasGroup group;
    [SerializeField] List<TimeInfo> infos;
    [SerializeField] float duration;

    [Space(7)]

    [Title("Time")]
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] DefaultValuesHandler textDefaults;
    [SerializeField] int time;

    [Space(7)]

    [Title("Text Anim")]
    [SerializeField] float textAnimScaleMultiplier;
    [SerializeField] float textAnimDuration;
    [SerializeField] Color textAnimColor;

    [Space(7)]
    [Title("Events")]
    [SerializeField] bool eventsActive = true;
    [SerializeField] List<TimeEventInfo> events; 
    private void Awake()
    {
        instance = (!instance) ? this : instance;
        SetVisibility(active: false, force: true, delay: 0f).Forget();
    }


    public async UniTaskVoid PlayTime(GamePhase type, float delay = 0)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(delay));

        TimeInfo info = GetInfo(type);
        time = info.time;
        TextUpdate();


        if (!active)
        {
            SetVisibility(active: true).Forget();
            await UniTask.Delay(TimeSpan.FromSeconds(duration));
        }

        while (time > 0)
        {
            if (GameManager.instance.gameState != GameState.Go)
            {
                SetVisibility(active: false).Forget();
                return;
            }
                

            await UniTask.Delay(TimeSpan.FromSeconds(1));
            time--;
            time = Mathf.Clamp(time, 0, int.MaxValue);
            TextUpdate();
        }


        SetVisibility(active: false, force: false, delay: 1f).Forget();
        GamePhaseManager.instance.SetPhase(GamePhase.Fight, 0.3f).Forget();
    }

    public TimeInfo GetInfo(GamePhase type)
    {
        return infos.Find(info => info.type == type);
    }

    public void TextUpdate()
    {

        int minute = time / 60;
        int second = time % 60;

        string str = "";
        str += minute > 0 ? minute.ToString() : "0" + minute.ToString();
        str += ":";

        str += second >= 10 ? second.ToString() : "0" + second.ToString();
        text.text = str;

        PlayTextAnim();
        EventPlayController();
    }

    public void PlayTextAnim()
    {
        if (time > 10)
            return;

        Transform textTrs = text.transform;
        AnimationCurve curve = CurveManager.GetCurve("smooth");

        Vector3 startScale = textDefaults.localScale * 0.8f;
        Vector3 endScale = textDefaults.localScale;

        Color startColor = textAnimColor;
        Color endColor = Color.white;

        text.color = startColor;
        text.DOColor(endColor, 0.5f).SetDelay(0.2f);

        textTrs.localScale = startScale;
        textTrs.DOScale(endScale, 0.7f).SetEase(Ease.OutElastic);

    }

    public async UniTaskVoid SetVisibility(bool active, bool force = false, float delay = 0)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(delay));

        this.active = active;

        float _duration = force ? 0 : duration;
        float targetAlpha = active ? 1f : 0f;

        group.DOFade(targetAlpha, _duration);
    }

    public void EventPlayController()
    {
        TimeEventInfo eventInfo = events.Find(_event => _event.triggerTime == time);

        if (eventInfo == null)
            return;

        eventInfo.events.ForEach(e => e.PlayEvent().Forget());
    }
}


[System.Serializable]
public class TimeInfo
{
    public GamePhase type;
    public int time;
}


[System.Serializable]
public class TimeEventInfo
{
    public int triggerTime;
    public List<EventInfo> events;
}
