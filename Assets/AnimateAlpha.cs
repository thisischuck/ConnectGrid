using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class AnimateAlpha : MonoBehaviour
{
    public float minAlpha;
    public float maxAlpha;
    public float speed;

    Image image;
    float dir = 1;


    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        float a = image.color.a * 255;

        if (a <= minAlpha)
            dir = 1;
        else if (a > maxAlpha)
            dir = -1;

        a += dir * speed * Time.deltaTime;
        image.color = new Color(image.color.r, image.color.g, image.color.b, a / 255);
    }
}
