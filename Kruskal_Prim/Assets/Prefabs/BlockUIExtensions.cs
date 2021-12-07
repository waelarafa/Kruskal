using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public static class BlockUIExtensions
{
    public static bool IsPointOverUIObject(this Vector3 pos)
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("here");
            return true;
        }

        PointerEventData eventPosition = new PointerEventData(EventSystem.current)
        {
            position = new Vector3(pos.x, pos.y, pos.z)
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventPosition, results);
        Debug.Log("here0");
        return results.Count > 0;
    }
}