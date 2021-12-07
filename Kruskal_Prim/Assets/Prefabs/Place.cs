using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Place : MonoBehaviour
{
    private static string input;
    public GameObject go;
    public LineRenderer line;
    public GameObject poid;

    bool placEdges;
    public bool pausetillP=true;
    GameObject temp;
    LineRenderer tempLine;
    Vector3 mousePos;
   
    int NoeudPlaced = 0;
    int EdgePlaced = 0;

    int sc;
    int des;

    Vector3 tempscPose;


    List<GameObject> squares;
    public bool crediyet=false;

    List <(int,int,int, Vector3, Vector3)> edge = new List<(int, int, int,Vector3,Vector3)>();


    // Start is called before the first frame update
    void Start()
    {
        squares = new List<GameObject>();

    }
    private void Update()
    {

        if (!placEdges)
        {
            mousePos = Input.mousePosition;

            if (Input.GetMouseButtonDown(1))
            {

                foreach (GameObject square in squares)
                {
                    BoxCollider2D col = square.GetComponent<BoxCollider2D>();

                    if (col.OverlapPoint(Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10))))
                    {

                        squares.Remove(square);
                        DestroyImmediate(square);
                    }
                }
            }

            //instatiate 

            if (Input.GetMouseButtonDown(0))
            {

                bool isOverUI = mousePos.IsPointOverUIObject();
                print(isOverUI);
                
                if (!isOverUI)
                {
                    GameObject tmpObj = Instantiate(go);
                    squares.Add(tmpObj);

                    tmpObj.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10));

                    var n = tmpObj.GetComponentInChildren<TextMeshPro>();
                    n.text = NoeudPlaced.ToString();

                    NoeudPlaced++;

                }
            }
        }
        else
        {
       

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray;
                RaycastHit hit;

                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                { 
                    //if first press
                    if (!crediyet)
                    {
                         if (hit.collider.tag == "n")
                        {
                            temp = hit.collider.transform.gameObject;

                            tempLine = SetLine(hit.transform.position);

                            //cash src
                            var nameOfN = temp.GetComponentInChildren<TextMeshPro>(); 
                            sc = int.Parse(nameOfN.text);
                           

                            crediyet = true;
                        }
                    }

                    tempLine.SetPosition(1, hit.transform.position);

                    if (hit.collider.tag == "n" && hit.transform.gameObject != temp)
                    {
                        tempLine.SetPosition(1, hit.transform.position);

                        //cash dest
                        var nameOfN = hit.collider.transform.gameObject.GetComponentInChildren<TextMeshPro>();
                        des = int.Parse(nameOfN.text);

                        StartCoroutine(PausetillP());

                        //  Instantiate(poid, dist); 

                    }
                }
            }
        }
    }

    IEnumerator PausetillP()
    {
        yield return new WaitWhile(()=>pausetillP);
      
        //Find Midway 
        var tempx = (tempLine.GetPosition(0).x + tempLine.GetPosition(1).x) / 2;
        var tempy = (tempLine.GetPosition(0).y + tempLine.GetPosition(1).y) / 2;

        var p = Instantiate(poid);
        p.transform.position = new Vector3(tempx, tempy, 10);

        var poidtext = p.GetComponent<TextMeshPro>();
        poidtext.text = input;

        pausetillP = true;
        crediyet = false;

        //cach edges 
        edge.Add((sc,des,int.Parse(poidtext.text), tempLine.GetPosition(0), tempLine.GetPosition(1)));
       
    }


    //draw line 

    LineRenderer SetLine(Vector3 start)
    {
            GameObject myLine = new GameObject();
            myLine.AddComponent<LineRenderer>();
            myLine.transform.position = start;
            LineRenderer lr = myLine.GetComponent<LineRenderer>();
            lr.SetPosition(0, start);
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(Color.red, 0.0f), new GradientColorKey(Color.red, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(0.5f, 0.0f), new GradientAlphaKey(.5f, 1.0f) }
        );
        lr.colorGradient = gradient;
        lr.material = new Material(Shader.Find("Sprites/Default"));

        EdgePlaced++;
        return lr;
    }

    public void PlacEdges()
    {
        placEdges = true;  
    }


    public void ReadStringInput(string s )
    {
        input = s;
        pausetillP = false;

    }

    public void PassData()
    {
        K._instance.Init(NoeudPlaced, EdgePlaced, edge);
    }
}