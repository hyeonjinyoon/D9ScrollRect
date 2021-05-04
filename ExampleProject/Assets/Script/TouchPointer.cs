//using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchPointer : MonoBehaviour
{
    [SerializeField] private Image image;

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            transform.position = Input.mousePosition;
        }

        if (Input.GetMouseButtonDown(0))
        {
            //image.DOKill();
            image.color = new Color(1, 0, 0, 0.6f);
        }

        if (Input.GetMouseButtonUp(0))
        {
            //image.DOKill();
            //image.DOColor(new Color(1, 0, 0, 0), 0.3f);
            image.color = new Color(1, 0, 0, 0);
        }

    }
}
