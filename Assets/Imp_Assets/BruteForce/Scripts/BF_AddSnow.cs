using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BF_AddSnow : MonoBehaviour
{
    public Material snowMaterial;
    public float angle = 80f;
    public bool isAuto;
    public float intersectionOffset = 0.25f;
    public bool useIntersection;
    public bool useUpdatedRotation;

    Mesh originalMesh;
    MeshFilter meshFilter;
    Mesh newMesh;
    GameObject newGO;
    float yIntersection;
    Quaternion ySlope = Quaternion.identity;
    float zNormal;
    Vector3 normalHit = Vector3.zero;
    float oldyIntersection = -1f;

    int[] oldTri;
    Vector3[] oldVert;
    Vector3[] oldNorm;
    Vector3[] oldNormWorld;
    Vector2[] oldUV;
    Color[] oldCol;

    List<int> triangles = new List<int>();
    List<Vector3> vertexs = new List<Vector3>();
    List<Vector2> uvs = new List<Vector2>();
    List<Color> cols = new List<Color>();

    void Start()
    {
        CheckValues();
        BuildInitialGeometry();
    }

    void Update()
    {
        if (useIntersection)
        {
            CheckIntersection();
        }

        if (useUpdatedRotation)
        {
            UpdateVertexColor();
        }
    }


    void CheckValues()
    {
        meshFilter = gameObject.GetComponent<MeshFilter>();
        originalMesh = meshFilter.mesh;
        oldTri = originalMesh.triangles;
        oldVert = originalMesh.vertices;
        oldNorm = originalMesh.normals;
        oldNormWorld = oldNorm;

        if (isAuto)
        {
            var k = 0;
            foreach (var norm in oldNorm)
            {
                oldNormWorld[k] = this.transform.localToWorldMatrix.MultiplyVector(norm).normalized;
                k++;
            }
            oldCol = new Color[oldVert.Length];
        }
        else
        {
            oldCol = originalMesh.colors;
        }
        oldUV = originalMesh.uv;
    }

    void CheckIntersection()
    {
        var layerMask = 1 << 0 | 1 << 4;
        RaycastHit hit;
        RaycastHit hitIfHit;

        if (Physics.Raycast(transform.position + Vector3.up*5f, Vector3.down, out hit, 200, layerMask))
        {
            if (hit.transform != this.transform)
            {
                yIntersection = hit.point.y + intersectionOffset;
                //Vector3 tangent = Vector3.ProjectOnPlane(Vector3.down, hit.normal).normalized;

                ySlope = Quaternion.LookRotation(hit.normal, Vector3.forward);

                zNormal = ((hit.normal.normalized.z) + 1f) / 2f;
                normalHit = hit.normal.normalized;

                if (yIntersection != oldyIntersection)
                {
                    UpdateSlopeColor();
                }
                oldyIntersection = yIntersection;
            }
            else
            {
                if (Physics.Raycast(hit.point + Vector3.up * -0.05f, Vector3.down, out hitIfHit, 200, layerMask))
                {
                    if (hitIfHit.transform != this.transform)
                    {
                        yIntersection = hitIfHit.point.y + intersectionOffset;
                        //Vector3 tangent = Vector3.ProjectOnPlane(Vector3.down, hitIfHit.normal).normalized;

                        ySlope =  Quaternion.LookRotation(hitIfHit.normal, Vector3.forward);
                        zNormal = ((hitIfHit.normal.normalized.z) + 1f) / 2f;
                        normalHit = hitIfHit.normal.normalized;

                        if (yIntersection != oldyIntersection)
                        {
                            UpdateSlopeColor();
                        }
                        oldyIntersection = yIntersection;
                    }
                }
            }
        }
    }

    void ClearGeometry()
    {
        triangles.Clear();
        triangles.TrimExcess();
        vertexs.Clear();
        vertexs.TrimExcess();
        uvs.Clear();
        uvs.TrimExcess();
        cols.Clear();
        cols.TrimExcess();
    }

    void BuildInitialGeometry()
    {
        if (meshFilter == null)
        {
            meshFilter = gameObject.GetComponent<MeshFilter>();
        }
        newMesh = new Mesh();
        newGO = new GameObject();
        var mF = newGO.AddComponent<MeshFilter>();
        var mR = newGO.AddComponent<MeshRenderer>();
        mF.mesh = newMesh;
        mR.material = snowMaterial;

        snowMaterial.SetFloat("_ISADD", 1);
        snowMaterial.EnableKeyword("IS_ADD");
        if (useIntersection)
        {
            if (snowMaterial.GetFloat("_USEINTER") == 0)
            {
                snowMaterial.SetFloat("_USEINTER", 1);
                snowMaterial.EnableKeyword("USE_INTER");
            }

        }
        else
        {
            if (snowMaterial.GetFloat("_USEINTER") == 1)
            {
                snowMaterial.SetFloat("_USEINTER", 0);
                snowMaterial.DisableKeyword("USE_INTER");
            }
        }




        newGO.transform.parent = this.transform;
        newGO.transform.localPosition = Vector3.zero;
        newGO.transform.localScale = Vector3.one;
        newGO.transform.localRotation = Quaternion.identity;

        var indexNewV = 0;
        foreach (var v in oldVert)
        {
            vertexs.Add(v + new Vector3(0,0,0));
            uvs.Add(oldUV[indexNewV]);

            indexNewV++;
        }
        indexNewV = 0;
        foreach (var innt in oldTri)
        {
            triangles.Add(oldTri[indexNewV]);

            indexNewV++;
        }

        if (isAuto)
        {
            var j = 0;
            foreach (var norm in oldNormWorld)
            {
                if(j>= oldCol.Length)
                {
                    break;
                }
                oldCol[j] = Color.red;
                var theAngle = Vector3.Angle(Vector3.up, norm);
                if (theAngle < (angle+10f))
                {
                    var lerpedColor = Color.Lerp(Color.white, Color.red, Mathf.Max(0f,theAngle- angle/2f) / (angle / 2f));
                    oldCol[j] = lerpedColor;
                }
                j++;
            }
        }

        cols = oldCol.ToList();
        newMesh.vertices = vertexs.ToArray();
        newMesh.triangles = triangles.ToArray();
        newMesh.uv = uvs.ToArray();
        newMesh.colors = cols.ToArray();

        newMesh.normals = originalMesh.normals;

        RecalculateNormalsSeamless(newMesh);
        newMesh.RecalculateBounds();
        newMesh.Optimize();
    }

    void UpdateVertexColor()
    {
        var updatedColors = newMesh.colors;
        var newNormWorld = newMesh.normals;

        if (isAuto)
        {
            var k = 0;
            foreach (var norm in newMesh.normals)
            {
                newNormWorld[k] = this.transform.localToWorldMatrix.MultiplyVector(norm).normalized;
                k++;
            }
        }


        if (isAuto)
        {
            var j = 0;
            foreach (var norm in newNormWorld)
            {
                if (j >= updatedColors.Length)
                {
                    break;
                }

                var theAngle = Vector3.Angle(Vector3.up, norm);
                if (theAngle < (angle + 10f))
                {
                    var lerpedColor = Color.Lerp(new Color(1,1,1, updatedColors[j].a), new Color(1, 0, 0, updatedColors[j].a), Mathf.Max(0f, theAngle - angle / 2f) / (angle / 2f));
                    updatedColors[j].r = lerpedColor.r;
                    updatedColors[j].g = lerpedColor.g;
                    updatedColors[j].b = lerpedColor.b;
                    updatedColors[j].a = lerpedColor.a;
                }
                j++;
            }
        }
        newMesh.colors = updatedColors;
    }

    void UpdateSlopeColor()
    {
        // This is not perfect for now but gets the job done... //
        var j = 0;

        var updatedColors = newMesh.colors;
        var updatedUV4 = new Vector2[updatedColors.Count()];
        var updatedUV5 = new Vector2[updatedColors.Count()];
        var updatedUV6 = new Vector2[updatedColors.Count()];
        var updatedUV7 = new Vector2[updatedColors.Count()];
        foreach (var norm in newMesh.colors)
        {
            updatedColors[j].a = yIntersection;
            updatedUV4[j] = new Vector2(normalHit.x, normalHit.y);
            updatedUV5[j] = new Vector2(zNormal, normalHit.z);
            updatedUV6[j] = new Vector2(ySlope.x, ySlope.y);
            updatedUV7[j] = new Vector2(ySlope.z, ySlope.w);
            j++;
        }

        newMesh.colors = updatedColors;
        newMesh.uv4 = updatedUV4;
        newMesh.uv5 = updatedUV5;
        newMesh.uv6 = updatedUV6;
        newMesh.uv7 = updatedUV7;
    }

    void RecalculateNormalsSeamless(Mesh mesh)
    {
        var trianglesOriginal = mesh.triangles;
        var triangles = trianglesOriginal.ToArray();

        var vertices = mesh.vertices;

        var mergeIndices = new Dictionary<int, int>();

        for (var i = 0; i < vertices.Length; i++)
        {
            var vertexHash = vertices[i].GetHashCode();

            if (mergeIndices.TryGetValue(vertexHash, out var index))
            {
                for (var j = 0; j < triangles.Length; j++)
                    if (triangles[j] == i)
                        triangles[j] = index;
            }
            else
                mergeIndices.Add(vertexHash, i);
        }

        mesh.triangles = triangles;

        var normals = new Vector3[vertices.Length];

        mesh.RecalculateNormals();
        var newNormals = mesh.normals;

        for (var i = 0; i < vertices.Length; i++)
            if (mergeIndices.TryGetValue(vertices[i].GetHashCode(), out var index))
                normals[i] = newNormals[index];

        mesh.triangles = trianglesOriginal;
        mesh.normals = normals;
    }
}
