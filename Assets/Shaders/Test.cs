using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Test : MonoBehaviour
{
    public Color[] gradientColors;
    private Mesh _mesh;

    private Vector3[] vertices;

    private Color[] colors;
    
   [Range(0,1F)][SerializeField] private float lerpTime = 0.5f;
    private int index = 0;

    private int colorIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        _mesh = GetComponent<MeshFilter>().mesh;
        vertices = _mesh.vertices;
        _mesh.colors  = new Color[vertices.Length];

    }

    private float time = 0f;

    // Update is called once per frame
    void Update()
    {
        colors = _mesh.colors;

        if (index >= vertices.Length)
        {
            index = 0;
        }
        

        colors[index] = Color.Lerp(gradientColors[colorIndex], colors[index],lerpTime*Time.deltaTime);

        time += Mathf.Lerp(time, 5f, lerpTime * Time.deltaTime);

        if (time > 4.9f)
        {
            colorIndex = Random.Range(0, gradientColors.Length);
            time = 0f;
            index = Random.Range(0, vertices.Length);
            
        }

        _mesh.colors = colors;
        
    }
}
