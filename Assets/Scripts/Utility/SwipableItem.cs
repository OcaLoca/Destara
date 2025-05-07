using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class SwipableItem : MonoBehaviour
{
    public static SwipableItem Singleton { get; private set; }
    public List<GameObject> pagesIndicators = new List<GameObject>();
    public List<GameObject> pages = new List<GameObject>();
    public int currentPage = 0;
    public int maxPages;
    public bool canSwipe;
    public bool isSwiping = false;


    //inside class
    Vector2 firstPressPos;
    Vector2 secondPressPos;
    Vector2 currentSwipe;

    public Color selectedColor, defaultColor;


    public bool isActive = false;

    public void ChangeCurrentClassSelectedNumber(int currentPage)
    {
        this.currentPage = currentPage;
    }

    private void Awake()
    {
        Singleton = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        maxPages = pages.Count;
    }

    // Update is called once per frame
    void Update()
    {
        if (canSwipe)
        {
            Swipe();
        }

    }

    public void Swipe()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //save began touch 2d point
            firstPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
        if (Input.GetMouseButtonUp(0))
        {
            //save ended touch 2d point
            secondPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            //create vector from the two points
            currentSwipe = new Vector2(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);

            //normalize the 2d vector
            currentSwipe.Normalize();


            //swipe left
            if (currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
            {

                if (currentPage < maxPages - 1)
                {
                    pages[currentPage].gameObject.SetActive(false);
                    //pagesIndicators[currentPage].gameObject.GetComponent<Animator>().SetBool("isActive", false);
                    //pagesIndicators[currentPage].GetComponent<Image>().color = defaultColor;
                    currentPage += 1;
                    pages[currentPage].gameObject.SetActive(true);
                    //pagesIndicators[currentPage].GetComponent<Image>().color = selectedColor;
                    //pagesIndicators[currentPage + 1].gameObject.GetComponent<Animator>().SetBool("isActive", true);


                }

            }
            //swipe right
            if (currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
            {

                if (currentPage > 0)
                {
                    pages[currentPage].gameObject.SetActive(false);
                    //pagesIndicators[currentPage].gameObject.GetComponent<Animator>().SetBool("isActive", false);
                    //pagesIndicators[currentPage].GetComponent<Image>().color = defaultColor;
                    currentPage -= 1;
                    pages[currentPage].gameObject.SetActive(true);
                    //pagesIndicators[currentPage].GetComponent<Image>().color = selectedColor;
                    //pagesIndicators[currentPage - 1].gameObject.GetComponent<Animator>().SetBool("isActive", true);

                    //pageTxt.text = (currentPage + 1).ToString() + "/5";
                }

            }
        }
    }

    public void SetPages(int n)
    {

        maxPages = n;

        DisableAllDots();

        if (maxPages <= 1)
        {
            canSwipe = false;
            EnableDots();
        }
        else
        {
            canSwipe = true;
            EnableDots();
        }

        SetStartPage();
    }

    void DisableAllDots()
    {
        foreach (GameObject dot in pagesIndicators)
        {
            dot.SetActive(false);
        }
    }

    void EnableDots()
    {
        for (int i = 0; i < maxPages; i++)
        {
            pagesIndicators[i].SetActive(true);
        }
    }

    void SetStartPage()
    {
        foreach (GameObject page in pages)
        {
            page.SetActive(false);
        }
        foreach (GameObject indicator in pagesIndicators)
        {
            indicator.GetComponent<Image>().color = defaultColor;
        }
        currentPage = 0;
        pages[currentPage].SetActive(true);
        pagesIndicators[currentPage].GetComponent<Image>().color = selectedColor;
    }
}
