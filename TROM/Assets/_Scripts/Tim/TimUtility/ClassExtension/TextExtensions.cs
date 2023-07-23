using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TextExtensions
{
    public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition,
        Quaternion localRotation,float characterSize, int fontSize,
        Color color, TextAnchor textAnchor,TextAlignment textAlignment, int sortingOrder)
    {
        GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;
        transform.localRotation = localRotation;
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.alignment = textAlignment;
        textMesh.text = text;
        textMesh.characterSize = characterSize;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        return textMesh;
    }

    public static TextMesh CreateWorldTextDefault(string text, Transform parent = null,
        Vector3 localPosition = default,Quaternion localRotation = default,
        float characterSize = 0.1f, int fontSize = 40,
        Color? color = null, TextAnchor textAnchor = TextAnchor.MiddleCenter,
        TextAlignment textAlignment = TextAlignment.Center, int sortingOrder = 0)
    {
        if (color == null) color = Color.white;
        return CreateWorldText(parent, text, localPosition, localRotation, characterSize, fontSize, 
                (Color)color, textAnchor, textAlignment, sortingOrder);
    }
}
