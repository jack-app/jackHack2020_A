using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(AudioSource))]
public class DragEventHandle : MonoBehaviour
{
    private Cloud draging = null;
    private Vector3 beforepoint = Vector3.zero;
    public CloudManager manager;
    public Camera main_camera;
    [SerializeField]
    AudioSource audioSource;//雲タッチのSE流すsource
    [SerializeField]
    AudioClip audioClip;//雲タッチのSE

    private void Awake()
    {
        var trigger = GetComponent<EventTrigger>();

        EventTrigger.Entry entry1 = new EventTrigger.Entry();
        entry1.eventID = EventTriggerType.BeginDrag;
        entry1.callback.AddListener((x) => OnBeginDrag(x));
        trigger.triggers.Add(entry1);

        EventTrigger.Entry entry2 = new EventTrigger.Entry();
        entry2.eventID = EventTriggerType.Drag;
        entry2.callback.AddListener((x) => OnDrag(x));
        trigger.triggers.Add(entry2);

        EventTrigger.Entry entry3 = new EventTrigger.Entry();
        entry3.eventID = EventTriggerType.EndDrag;
        entry3.callback.AddListener((x) => OnEndDrag(x));
        trigger.triggers.Add(entry3);
    }

    public void OnBeginDrag(BaseEventData eventData)
    {
        audioSource.PlayOneShot(audioClip, 1.0f);
        var ev = (PointerEventData)eventData;
        var point = main_camera.ScreenToWorldPoint(ev.position);
        var touched = manager.clouds.Where(cloud => !double.IsNaN(cloud.IsTouched(point + new Vector3(0,0,10))));
        if(touched.Count() != 0)
        {
            draging = touched.Aggregate((min, next) => min.IsTouched(point + new Vector3(0, 0, 10)) < next.IsTouched(point + new Vector3(0, 0, 10)) ? min : next);
            draging.isTouching = true;
            beforepoint = point;
        }
    }

    public void OnDrag(BaseEventData eventData)
    {
        var ev = (PointerEventData)eventData;
        if (draging != null)
        {
            var point = main_camera.ScreenToWorldPoint(ev.position);
            draging.gameObject.transform.Translate(point - beforepoint);
            beforepoint = point;
        }
    }

    public void OnEndDrag(BaseEventData eventData)
    {
        audioSource.PlayOneShot(audioClip, 1.0f);
        if (draging != null)
        {
            draging.isTouching = false;
            draging.Touched();
        }
        draging = null;
        beforepoint = Vector3.zero;
    }
}
