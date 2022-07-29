using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helper 
{
    private static Matrix4x4 IsoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));

    public static Vector3 ToIso(this Vector3 input) => IsoMatrix.MultiplyPoint3x4(input);

    private static readonly Dictionary<float, WaitForSeconds> WaitDictionary = new Dictionary<float, WaitForSeconds>();
    public static WaitForSeconds GetWait(float time)
    {
        if (WaitDictionary.TryGetValue(time, out var wait)) return wait;

        WaitDictionary[time] = new WaitForSeconds(time);
        return WaitDictionary[time];
    }

    public static int Positive(int number)
    {
        return number < 0 ? number * -1 : number;
    }

    public static Vector3Int CastV3ToInt(this Vector3 oldV3)
    {
        int v3X = (int)oldV3.x;
        int v3Y = (int)oldV3.y;
        int v3Z = (int)oldV3.z;

        Vector3Int newV3 = new Vector3Int(v3X, v3Y, v3Z);

        return newV3;
    }

    public static bool AreTheseTooClose(Vector3 v1, Vector3 v2, float distance)
    {
        if (v1.magnitude - v2.magnitude < distance) return true;
        else return false;
    }

    public static bool ContainBounds(this Bounds bounds, Bounds target)
    {
        return bounds.Contains(target.center);
    }

    public static void ToggleUIElementVisibility(this List<GameObject> list,bool condition)
    {
        foreach(GameObject element in list)
        {
            element.SetActive(condition);
        }
    }

    public static void SetFirstMaterial(this MeshRenderer meshRend, Material newMat)
    {
        var currMats = meshRend.materials;
        currMats[0] = newMat;
        meshRend.materials = currMats;
    }

    public static void SetMaterial(this MeshRenderer meshRend, Material newMat)
    {
        meshRend.material = newMat;
    }

    public static Vector3Int XYToXZInt(Vector2 vec2)
    {
        var vec3 = new Vector3(vec2.x, 0, vec2.y);
        return new Vector3Int((int)vec3.x, (int)vec3.y, (int)vec3.z);
    }

    public static Vector3 XYToXZ(this Vector2 vec2)
    {
        return new Vector3(vec2.x, 0, vec2.y);
    }

    public static Bounds OrthoBounds(this Camera cam)
    {
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float cameraHeight = cam.orthographicSize * 2;
        Bounds bounds = new Bounds(
            cam.transform.position,
            new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
        return bounds;
    }

    static Texture2D _whiteTexture;
    public static Texture2D WhiteTexture
    {
        get
        {
            if (_whiteTexture == null)
            {
                _whiteTexture = new Texture2D(1, 1);
                _whiteTexture.SetPixel(0, 0, Color.white);
                _whiteTexture.Apply();
            }

            return _whiteTexture;
        }
    }

    public static void DrawScreenRect(Rect rect, Color color)
    {
        GUI.color = color;
        GUI.DrawTexture(rect, WhiteTexture);
        GUI.color = Color.white;
    }

    public static void DrawScreenRectBorder(Rect rect, float thickness, Color color)
    {
        // Top
        Helper.DrawScreenRect(new Rect(rect.xMin, rect.yMin, rect.width, thickness), color);
        // Left
        Helper.DrawScreenRect(new Rect(rect.xMin, rect.yMin, thickness, rect.height), color);
        // Right
        Helper.DrawScreenRect(new Rect(rect.xMax - thickness, rect.yMin, thickness, rect.height), color);
        // Bottom
        Helper.DrawScreenRect(new Rect(rect.xMin, rect.yMax - thickness, rect.width, thickness), color);
    }

    public static Rect GetScreenRect(Vector3 screenPosition1, Vector3 screenPosition2)
    {
        // Move origin from bottom left to top left
        screenPosition1.y = Screen.height - screenPosition1.y;
        screenPosition2.y = Screen.height - screenPosition2.y;
        // Calculate corners
        var topLeft = Vector3.Min(screenPosition1, screenPosition2);
        var bottomRight = Vector3.Max(screenPosition1, screenPosition2);
        // Create Rect
        return Rect.MinMaxRect(topLeft.x, topLeft.y, bottomRight.x, bottomRight.y);
    }
}
