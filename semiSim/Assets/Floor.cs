using System;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class Floor : MonoBehaviour
{
    [System.Serializable]
    private struct Grid
    {
        public int x;
        public int y;
    }
    
    [SerializeField] private Grid gridSize;
    [SerializeField] private GameObject tile;
    [SerializeField] private float width;

    private Grid _grid;
    
    private void Start()
    {
        _grid = this.GetComponent<Grid>();
    }

    private void OnValidate()
    {
        EditorApplication.delayCall += () =>
        {
            foreach (Transform child in this.transform)
            {
                DestroyImmediate(child.gameObject);
            }
        };

        EditorApplication.delayCall += () =>
        {
            for (var i = 0; i < gridSize.x; i++)
            {
                for (var j = 0; j < gridSize.y; j++)
                {
                    var pos = new Vector3(i * width, 0, j  * width)
                              + transform.position
                              + (0.5f * new Vector3(width, 0, width));
                    var ga = Instantiate(tile, pos, Quaternion.identity);
                    ga.transform.SetParent(this.transform);
                    ga.name = $"{i}, {j}";
                }
            }
        };
    }
}
