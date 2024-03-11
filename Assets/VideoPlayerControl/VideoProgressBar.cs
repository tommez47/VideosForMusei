using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoProgressBar : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    [SerializeField]
    private VideoPlayer videoPlayer;
    public GameObject time;

    private Slider progress;

    string sec;

    private void Awake()
    {
        progress = GetComponent<Slider>();
    }

    private void Update()
    {
        if (videoPlayer.frameCount > 0)
            progress.value = (float)videoPlayer.frame / (float)videoPlayer.frameCount;

        sec=videoPlayer.time.ToString();
       
        TimeSpan result = TimeSpan.FromMinutes(double.Parse(sec));
        string fromTimeString = result.ToString("hh':'mm");

        time.GetComponent<Text>().text = fromTimeString;

    }

    public void OnDrag(PointerEventData eventData)
    {
        TrySkip(eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {

        TrySkip(eventData);
    }

    private void TrySkip(PointerEventData eventData)
    {

        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            progress.GetComponent<RectTransform>(), eventData.position, null, out localPoint))
        {
            float pct = Mathf.InverseLerp(progress.GetComponent<RectTransform>().rect.xMin,
                progress.GetComponent<RectTransform>().rect.xMax, localPoint.x);
            SkipToPercent(pct);
        }
    }

    private void SkipToPercent(float pct)
    {
        var frame = videoPlayer.frameCount * pct;
        videoPlayer.frame = (long)frame;
    }
}
