using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCard : MonoBehaviour
{
    [SerializeField] private SceneController sceneController;
    [SerializeField] private GameObject cardBack;
    [SerializeField] private AudioSource audio;

    private int _id;

    public int Id
    {
        get { return _id; }
    }

    private void Awake()
    {
        audio = GetComponent<AudioSource>();
    }

    public void OnMouseDown()
    {
        if (cardBack.activeSelf && sceneController.canReveal)
        {
            audio.Play();
            cardBack.SetActive(false);
            sceneController.RevealCard(this);
        }
    }

    public void Unreveal()
    {
        cardBack.SetActive(true);
    }

    public void ChangeSprite(int id, Sprite image)
    {
        _id = id;
        GetComponent<SpriteRenderer>().sprite = image;
    }

}
