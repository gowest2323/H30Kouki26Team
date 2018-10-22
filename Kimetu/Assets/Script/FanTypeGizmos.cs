using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanTypeGizmos : MonoBehaviour
{
    private static readonly int TRIANGLE_COUNT = 12;

    /// <summary>
    /// 扇型ギズモを描く
    /// </summary>
    /// <param name="target">ターゲット</param>
    /// <param name="angle">角度</param>
    /// <param name="length">半径の長さ</param>
    /// <param name="color">色</param>
    public static void DrawFanGizmos(GameObject target, float angle, float length, Color color)
    {
        if(target == null)
        {
            return;
        }

        Gizmos.color = color;

        Transform transform = target.transform;
        Vector3 pos = transform.position + Vector3.up * 0.01f; // 0.01fは地面と高さだと見づらいので調整用。
        Quaternion rot = transform.rotation;
        Vector3 scale = Vector3.one * length;

        if(angle > 0.0f)
        {
            Mesh fanMesh = CreateFanMesh(angle, TRIANGLE_COUNT);
            Gizmos.DrawMesh(fanMesh,
                            transform.position + Vector3.up * 0.05f,
                            transform.rotation,
                            Vector3.one * length);
        }
    }

    /// <summary>
    /// 扇型のメッシュを作る
    /// </summary>
    /// <param name="angle"></param>
    /// <param name="triangleCount"></param>
    /// <returns></returns>
    private static Mesh CreateFanMesh(float angle, int triangleCount)
    {
        var mesh = new Mesh();

        var vertices = CreateFanVertices(angle, triangleCount);
        var triangleIndexes = new List<int>(triangleCount * 3);

        for (int i = 0; i < triangleCount; ++i)
        {
            triangleIndexes.Add(0);
            triangleIndexes.Add(i + 1);
            triangleIndexes.Add(i + 2);
        }

        mesh.vertices = vertices;
        mesh.triangles = triangleIndexes.ToArray();

        mesh.RecalculateNormals();

        return mesh;
    }

    private static Vector3[] CreateFanVertices(float angle, int triangleCount)
    {
        if (angle <= 0.0f)
        {
            throw new System.ArgumentException(string.Format("角度がおかしい！ angle={0}", angle));
        }

        if (triangleCount <= 0)
        {
            throw new System.ArgumentException(string.Format("数がおかしい！ triangleCount={0}", triangleCount));
        }

        angle = Mathf.Min(angle, 360.0f);

        var vertices = new List<Vector3>(triangleCount + 2);

        // 始点
        vertices.Add(Vector3.zero);

        // Mathf.Sin()とMathf.Cos()で使用するのは角度ではなくラジアンなので変換しておく。
        float radian = angle * Mathf.Deg2Rad;
        float startRad = -radian / 2;
        float incRad = radian / triangleCount;

        for (int i = 0; i < triangleCount + 1; ++i)
        {
            float currentRad = startRad + (incRad * i);

            Vector3 vertex = new Vector3(Mathf.Sin(currentRad), 0.0f, Mathf.Cos(currentRad));
            vertices.Add(vertex);
        }

        return vertices.ToArray();
    }

}
