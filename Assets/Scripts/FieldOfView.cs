using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    private Mesh mesh;
    private float fov;
    private Vector3 origin;
    private float startingAngle;

    private void Start()
    {
        //na starcie tworzy siê mesh
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        //mo¿na zmieniaæ fov (ale nie mam obrotu fova razem z graczem bo mi sie obraca³, ale nie aktualizowa³a siê kolizja obiektów(??)
        fov = 360f;
        origin = Vector3.zero;
    }

    //¿eby mesh siê aktualizowa³ w trakcie ruchu 
    private void LateUpdate()
    {
        //iloœæ promieni mesh, im wiêcej tym bardziej okr¹g³y
        int rayCount = 720;
        float angle = startingAngle;
        float angleIncrease = fov / rayCount;
        //zasiêg FoVa
        float viewDistance = 10f;

        Vector3[] vertices = new Vector3[rayCount + 1 + 1];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];

        vertices[0] = origin;

        int vertexIndex = 1;
        int triangleIndex = 0;
        for (int i = 0; i <= rayCount; i++)
        {
            Vector3 vertex;                                                                                //layer mask obiektu, który ma blokowac FoV
            RaycastHit2D raycastHit2D = Physics2D.Raycast(origin, GetVectorFromAngle(angle), viewDistance, layerMask);

            //je¿eli mesh uderza w collider
            if (raycastHit2D.collider == null) //no hit
            {
                vertex = origin + GetVectorFromAngle(angle) * viewDistance;
            }
            else //hit
            {
                vertex = raycastHit2D.point;
            }

            vertices[vertexIndex] = vertex;

            if (i > 0)
            {
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;

                triangleIndex += 3;
            }

            vertexIndex++;
            angle -= angleIncrease;
        }

        //ustawia mesh pod obliczone wartoœci
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        //jak wychodzi³em kawa³ek poza origin point to mi znika³ fov wiêc rekalkuluje granice sceny
        mesh.RecalculateBounds();
    }

    public void SetOrigin(Vector3 origin)
    {
        this.origin = origin;
    }

   //funkcje z neta
    public static Vector3 GetVectorFromAngle(float angle)
    {
        float angleRad = angle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }

}
