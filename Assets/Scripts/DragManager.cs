using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragManager : MonoBehaviour
{
    public bool isClick;
    public bool isTxt;
    public int DragObjValue;
    public LineRenderer line;

    private void Awake()
    {
        line = GetComponent<LineRenderer>();
        line.startWidth = 10;
        line.endWidth = 10;
    }
}
