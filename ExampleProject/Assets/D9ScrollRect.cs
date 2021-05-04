using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

class D9Position
{
    public Vector2 position;
    public float time;
}

public class D9ScrollRect : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerDownHandler, IInitializePotentialDragHandler
{
    public RectTransform viewPort;
    public RectTransform content;

    public bool onActiveInitPosition = true;

    public bool horizontal = true;
    public bool vertical = true;

    public float speedMultiplier = 80;
    public float speedDecreaseDuration = 30;
    [Range(1,50)] public float scrollStopDuration = 12;

    Vector2 clickPosition;
    Vector2 endPosition;
    Vector2 contentStartPosition;
    Vector2 contentOriginLocalPosition;

    public Vector2 speed { get; private set; }

    bool isTouching = false;

    Canvas parentCanvas;

    List<D9Position> prevPositions = new List<D9Position>();
    const int BACK_FRAME = 3;

    private void OnEnable()
    {
        viewPort.pivot = new Vector2(0.5f, 0.5f);

        if (onActiveInitPosition)
        {
            var contentUpPositionY = (1 - content.pivot.y) * content.rect.height;
            content.localPosition += ((viewPort.localPosition + new Vector3(0, viewPort.rect.height / 2)) - (content.localPosition + new Vector3(0, contentUpPositionY)));
        }

        parentCanvas = GetComponentInParent<Canvas>();

        speed = Vector2.zero;

        time = Time.time;
    }

    private void Start()
    {
        contentOriginLocalPosition = content.transform.localPosition;

        for (int i = 0; i < BACK_FRAME; i++)
        {
            var a = new D9Position
            {
                time = Time.time,
                position = Vector2.zero
            };
            prevPositions.Add(a);
        }
    }

    float time = 0;

    private void Update()
    {
        content.transform.position += (Vector3)speed * Time.deltaTime;

        while (time < Time.time)
        {
            speed -= speed / speedDecreaseDuration;

            if (speed.sqrMagnitude < 0.001f)
                speed = Vector2.zero;

            if (vertical)
            {
                var contentDownPositionY = -(content.pivot.y) * content.rect.height;
                var contentUpPositionY = (1 - content.pivot.y) * content.rect.height;

                if (content.localPosition.y + contentUpPositionY < viewPort.localPosition.y + viewPort.rect.height / 2)
                {
                    var a = ((viewPort.localPosition + new Vector3(0, viewPort.rect.height / 2)) - (content.localPosition + new Vector3(0, contentUpPositionY))) / scrollStopDuration;
                    content.localPosition += new Vector3(0, a.y);
                    speed -= speed / speedDecreaseDuration;
                }

                if (content.localPosition.y + contentDownPositionY > viewPort.localPosition.y - viewPort.rect.height / 2)
                {
                    var a = ((viewPort.localPosition - new Vector3(0, viewPort.rect.height / 2)) - (content.localPosition + new Vector3(0, contentDownPositionY))) / scrollStopDuration;
                    content.localPosition += new Vector3(0, a.y);
                    speed -= speed / speedDecreaseDuration;
                }
            }

            if (horizontal)
            {
                var contentDownPositionX = -(content.pivot.x) * content.rect.width;
                var contentUpPositionX = (1 - content.pivot.x) * content.rect.width;

                if (content.localPosition.x + contentUpPositionX < viewPort.localPosition.x + viewPort.rect.width / 2)
                {
                    var a = ((viewPort.localPosition + new Vector3(viewPort.rect.width / 2, 0)) - (content.localPosition + new Vector3(contentUpPositionX, 0))) / scrollStopDuration;
                    content.localPosition += new Vector3(a.x, 0);
                    speed -= speed / speedDecreaseDuration;
                }

                if (content.localPosition.x + contentDownPositionX > viewPort.localPosition.x - viewPort.rect.width / 2)
                {
                    var a = ((viewPort.localPosition - new Vector3(viewPort.rect.width / 2, 0)) - (content.localPosition + new Vector3(contentDownPositionX, 0))) / scrollStopDuration;
                    content.localPosition += new Vector3(a.x, 0);
                    speed -= speed / speedDecreaseDuration;
                }
            }
            time += 1 / 120f;
        }

        time = Time.time;

        if (isTouching)
        {
            if (parentCanvas.renderMode == RenderMode.ScreenSpaceOverlay)
                content.transform.position = contentStartPosition + ((Vector2)Input.mousePosition - clickPosition);
            else
                content.transform.position = (Vector3)contentStartPosition + (Camera.main.ScreenToWorldPoint(Input.mousePosition) - Camera.main.ScreenToWorldPoint(clickPosition));

            prevPositions[0].position = Input.mousePosition;
            prevPositions[0].time = Time.time;

            for (int i = BACK_FRAME - 1; i > 0; i--)
            {
                prevPositions[i].position = prevPositions[i - 1].position;
                prevPositions[i].time = prevPositions[i - 1].time;
            }

            if (scrollStopDuration == 1)
            {
                if (vertical)
                {
                    var contentDownPositionY = -(content.pivot.y) * content.rect.height;
                    var contentUpPositionY = (1 - content.pivot.y) * content.rect.height;

                    if (content.localPosition.y + contentUpPositionY < viewPort.localPosition.y + viewPort.rect.height / 2)
                    {
                        var a = ((viewPort.localPosition + new Vector3(0, viewPort.rect.height / 2)) - (content.localPosition + new Vector3(0, contentUpPositionY))) / scrollStopDuration;
                        content.localPosition += new Vector3(0, a.y);
                        speed -= speed / speedDecreaseDuration;
                    }

                    if (content.localPosition.y + contentDownPositionY > viewPort.localPosition.y - viewPort.rect.height / 2)
                    {
                        var a = ((viewPort.localPosition - new Vector3(0, viewPort.rect.height / 2)) - (content.localPosition + new Vector3(0, contentDownPositionY))) / scrollStopDuration;
                        content.localPosition += new Vector3(0, a.y);
                        speed -= speed / speedDecreaseDuration;
                    }
                }

                if (horizontal)
                {
                    var contentDownPositionX = -(content.pivot.x) * content.rect.width;
                    var contentUpPositionX = (1 - content.pivot.x) * content.rect.width;

                    if (content.localPosition.x + contentUpPositionX < viewPort.localPosition.x + viewPort.rect.width / 2)
                    {
                        var a = ((viewPort.localPosition + new Vector3(viewPort.rect.width / 2, 0)) - (content.localPosition + new Vector3(contentUpPositionX, 0))) / scrollStopDuration;
                        content.localPosition += new Vector3(a.x, 0);
                        speed -= speed / speedDecreaseDuration;
                    }

                    if (content.localPosition.x + contentDownPositionX > viewPort.localPosition.x - viewPort.rect.width / 2)
                    {
                        var a = ((viewPort.localPosition - new Vector3(viewPort.rect.width / 2, 0)) - (content.localPosition + new Vector3(contentDownPositionX, 0))) / scrollStopDuration;
                        content.localPosition += new Vector3(a.x, 0);
                        speed -= speed / speedDecreaseDuration;
                    }
                }
            }

        }

        if (!horizontal)
            content.transform.localPosition = new Vector3(contentOriginLocalPosition.x, content.transform.localPosition.y);

        if (!vertical)
            content.transform.localPosition = new Vector3(content.transform.localPosition.x, contentOriginLocalPosition.y);

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!horizontal && Math.Abs(eventData.delta.x) > Math.Abs(eventData.delta.y))
            routeToParent = true;
        else if (!vertical && Math.Abs(eventData.delta.x) < Math.Abs(eventData.delta.y))
            routeToParent = true;
        else
            routeToParent = false;

        if (routeToParent)
            DoForParents<IBeginDragHandler>((parent) => { parent.OnBeginDrag(eventData); });

        if (routeToParent)
        {
            isTouching = false;
        }
        else
        {
            isTouching = true;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (routeToParent)
            DoForParents<IEndDragHandler>((parent) => { parent.OnEndDrag(eventData); });

        routeToParent = false;

        if (!isTouching)
            return;

        endPosition = Input.mousePosition;

        if (parentCanvas.renderMode == RenderMode.ScreenSpaceOverlay)
            speed = endPosition - prevPositions[BACK_FRAME - 1].position;
        else
            speed = Camera.main.ScreenToWorldPoint(endPosition) - Camera.main.ScreenToWorldPoint(prevPositions[BACK_FRAME - 1].position);

        var t = ((1 / 60f) / (Time.time - prevPositions[BACK_FRAME - 1].time));

        speed *= speedMultiplier * t;
        isTouching = false;
    }

    public void OnInitializePotentialDrag(PointerEventData eventData)
    {
        speed = Vector2.zero;
        contentStartPosition = content.transform.position;
        clickPosition = Input.mousePosition;

        for (int i = 0; i < BACK_FRAME; i++)
        {
            prevPositions[i].position = Input.mousePosition;
            prevPositions[i].time = Time.time;
        }

        DoForParents<IInitializePotentialDragHandler>((parent) => { parent.OnInitializePotentialDrag(eventData); });
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (routeToParent)
            DoForParents<IDragHandler>((parent) => { parent.OnDrag(eventData); });
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (routeToParent)
            DoForParents<IPointerDownHandler>((parent) => { parent.OnPointerDown(eventData); });
    }

    private bool routeToParent;

    private void DoForParents<T>(Action<T> action) where T : IEventSystemHandler
    {
        Transform parent = transform.parent;
        while (parent != null)
        {
            foreach (var component in parent.GetComponents<Component>())
            {
                if (component is T)
                    action((T)(IEventSystemHandler)component);
            }
            parent = parent.parent;
        }
    }

    public void SetSpeed(Vector2 speed)
    {
        this.speed = speed;
    }
}
