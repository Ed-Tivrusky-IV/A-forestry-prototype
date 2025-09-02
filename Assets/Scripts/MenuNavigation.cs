using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
//using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

public class MenuNavigation : MonoBehaviour
{
    public GameObject[] ImageSets;
    private Image[] MenuOptions;
    private Outline[] outlines;
    private int currentImageSetIndex = 0;
    private int currentImageIndex = 0;
    private readonly int imagesPerSet = 4;
    private readonly int imageSetsNum = 3;
    // Start is called before the first frame update
    void Start()
    {
        DisableMenu();
    }

    // Update is called once per frame
    void Update()
    {
        SwitchFocusState();
    }

    private void EnableCurrentImageSet()
    {
        for (int j = 0; j < imageSetsNum; j++)
        {
            if (j == currentImageSetIndex)
            {
                ImageSets[j].SetActive(true);
                //Debug.Log("Activated Image Set " + j);
            }
            else
            {
                ImageSets[j].SetActive(false);
            }
        }
    }

    private void SwitchImageSets()
    {
        EnableCurrentImageSet();
        MenuOptions = new Image[imagesPerSet];
        for (int i = 0; i < imagesPerSet; i++)
        {
            MenuOptions[i] = ImageSets[currentImageSetIndex].transform.GetChild(i).GetComponent<Image>();
        }

        outlines = new Outline[MenuOptions.Length];
        for (int i = 0; i < MenuOptions.Length; i++)
        {
            outlines[i] = MenuOptions[i].GetComponent<Outline>();
        }

        if (++currentImageSetIndex == imageSetsNum)
        {
            currentImageSetIndex = 0;
        }
    }

    private void SwitchFocusState()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentImageIndex--;
            if (currentImageIndex < 0)
            {
                currentImageIndex = MenuOptions.Length - 1;
            }
            ToggleTheOutline(currentImageIndex);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentImageIndex++;
            if (currentImageIndex >= MenuOptions.Length)
            {
                currentImageIndex = 0;
            }
            ToggleTheOutline(currentImageIndex);
        }
    }

    private void ToggleTheOutline(int index)
    {
        foreach (Outline outline in outlines)
        {
            outline.enabled = false;
        }
        outlines[index].enabled = true;
    }

    public void EnableMenu()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
        SwitchImageSets();
        ToggleTheOutline(currentImageIndex);
    }

    public void DisableMenu()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        //SwitchImageSets();
    }
}
