using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolTip : MonoBehaviour
{
    [SerializeField]
    private Text header;
    [SerializeField]
    private Text content;
    [SerializeField]
    private LayoutElement layoutElement;
    [SerializeField]
    private int maxCharacter;
    [SerializeField]
    private RectTransform rectTransform;

    public void setText(string contentText, string headerText = "")
    {
        if (headerText == "")
        {
            header.gameObject.SetActive(false);
        }
        else
        {
            header.gameObject.SetActive(true);
            header.text = headerText;
        }
        content.text = contentText;
        int headerLength = header.text.Length;
        int contentLength = content.text.Length;
        if (headerLength > maxCharacter || contentLength > maxCharacter)
        {
            layoutElement.enabled = true;
        }
        else
        {
            layoutElement.enabled = false;
        }
    }

    private void Update()
    {
        Vector2 position = Input.mousePosition;
        float pivotX = position.x / Screen.width;
        float pivotY = position.y / Screen.height;
        rectTransform.pivot = new Vector2(pivotX, pivotY);
        transform.position = position;
    }
}
