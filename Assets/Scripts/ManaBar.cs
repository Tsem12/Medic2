using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ManaBar : MonoBehaviour
{
    [SerializeField] float manaBarTextureSpeed;
    private RawImage barImage;

    private void Awake()
    {
        barImage = transform.Find("bar").GetComponent<RawImage>();
    }

    private void Update()
    {
        Rect rectUv = barImage.uvRect;
        rectUv.x -= manaBarTextureSpeed * Time.deltaTime;
        barImage.uvRect = rectUv;
    }
}
