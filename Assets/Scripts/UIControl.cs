using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIControl : MonoBehaviour
{
    public GameObject menuCanvas;
    public GameObject flyCanvas;
    public GameObject guideCanvas;
    public GameObject tipCanvas;
    public GameObject gameCanvas;
    private List<Transform> Cubes = new List<Transform>();
    public Transform CubesWrapper;
    private int dropIndex = 0;
    private Vector3 originPosition;
    public Dropdown dropdown;
    public GameObject toggleGroup;
    private bool isMoving;
    private float movingSpeed = 30;
    Transform target;
    public Button urlButton;
    //private string[] tagColors = { "E74C3CFF", "3498DBFF", "2ECC71FF" };
    private void Start()
    {
        gameCanvas.SetActive(false);
        menuCanvas.SetActive(true);
        flyCanvas.SetActive(false);
        guideCanvas.SetActive(false);
        CubesWrapper.gameObject.SetActive(false);
        isMoving = false;
        
    }
    public void OnQuit()
    {
        Application.Quit();
    }
    public void OnMenu()
    {
        CubesWrapper.gameObject.SetActive(false);
        flyCanvas.SetActive(false);
        guideCanvas.SetActive(false);
        gameCanvas.SetActive(false);
        menuCanvas.SetActive(true);
        transform.GetComponent<KeyMove>().enabled = false;

    }
    public void OnFly()
    {
        CubesWrapper.gameObject.SetActive(true);
        gameCanvas.SetActive(true);
        flyCanvas.SetActive(true);
        OnFlyToggleChange();
        guideCanvas.SetActive(false);
        menuCanvas.SetActive(false);
        transform.GetComponent<KeyMove>().enabled = true;
    }
    public void OnGuide()
    {
        CubesWrapper.gameObject.SetActive(false);
        
        gameCanvas.SetActive(true);
        flyCanvas.SetActive(false);
        guideCanvas.SetActive(true);
        menuCanvas.SetActive(false);
        transform.GetComponent<KeyMove>().enabled = false;
        InitDropdown();
    }
   
    public void OnGuideConfirm()
    {
        originPosition = Camera.main.transform.position;
        isMoving = true;
        transform.LookAt(target.position);
    }
    public void OnGuideCancel()
    {
        Camera.main.transform.position = originPosition;
        isMoving = false;
        transform.LookAt(target.position);
    }
    public void OnGuideDrop()
    {
        if(dropdown.value < Cubes.Count)
        {
            target = Cubes[dropdown.value];
            Camera.main.transform.DOMove(target.transform.position - new Vector3(0, -100, 150), 1);
            Camera.main.transform.DORotate(new Vector3(45, 0, 0), 1);
            urlButton.GetComponentInChildren<Text>().text = target.GetComponent<Target>().UrlText;
        }
        isMoving = false;

    }
    public void OnGuideUrl()
    {
        if (target != null && urlButton.GetComponentInChildren<Text>().text != "")
        {
            Application.OpenURL(urlButton.GetComponentInChildren<Text>().text);
        }
    }
    private void InitDropdown()
    {
        dropdown.ClearOptions();
        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
        Toggle activeToggle = toggleGroup.GetComponent<ToggleGroup>().ActiveToggles().First();
        string toggleName = activeToggle.name;
        Cubes.Clear();
        foreach (Transform child in CubesWrapper)
        {
            if (child.CompareTag(toggleName))
            {
                Cubes.Add(child);
                Dropdown.OptionData temoData = new Dropdown.OptionData();
                temoData.text = child.GetComponent<Target>().NameText;
                options.Add(temoData);
            }
            
        }
        dropdown.AddOptions(options);
        OnGuideDrop();
    }
    
    public void OnFlyToggleChange()
    {
        
        if (flyCanvas.activeSelf)
        {
            Toggle activeToggle = toggleGroup.GetComponent<ToggleGroup>().ActiveToggles().First();
            string toggleName = activeToggle.name;
            foreach (Transform cube in CubesWrapper)
            {
                cube.gameObject.SetActive(cube.CompareTag(toggleName));
            }
        }
        if (guideCanvas.activeSelf)
        {
            InitDropdown();
            OnGuideDrop();
        }
        
    }
    private void Update()
    {
        if (isMoving && guideCanvas.activeSelf)
        {
            transform.LookAt(target);
            transform.RotateAround(target.transform.position, Vector3.up, movingSpeed * Time.deltaTime);
        }
        else
        {
            isMoving = false;
        }
    }

}
