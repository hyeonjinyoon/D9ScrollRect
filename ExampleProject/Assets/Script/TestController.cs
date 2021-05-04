using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestController : MonoBehaviour
{
    [SerializeField] private RectTransform _group1;
    [SerializeField] private RectTransform _group2;

    [SerializeField] private RectTransform _content1;
    [SerializeField] private RectTransform _content2;

    [SerializeField] private TestBlock _testBlockPrefab;

    private void Start()
    {
        Application.targetFrameRate = 60;

        for (int i = 1; i < 20; i++)
        {
            var inst1 = Instantiate(_testBlockPrefab, _content1);
            inst1.text.text = $"{i}";

            var inst2 = Instantiate(_testBlockPrefab, _content2);
            inst2.text.text = $"{i}";
        }
    }

    public void OnClickGroup1()
    {
        _group1.gameObject.SetActive(true);
        _group2.gameObject.SetActive(false);
    }

    public void OnClickGroup2()
    {
        _group1.gameObject.SetActive(false);
        _group2.gameObject.SetActive(true);
    }
}
