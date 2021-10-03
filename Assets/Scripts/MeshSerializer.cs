using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class MeshSerializer : MonoBehaviour
{
    [SerializeField]
    public GameObject inputObject;
    [SerializeField]
    public GameObject outputObject;
    
    public string json_path = "Assets/Resources/Mesh.json";
    
    private ObjectContainer objectContainer;
    string modelString;

    public void SerializeModel()
    {
        objectContainer = new ObjectContainer();
        SerializationRecursion(inputObject.transform, objectContainer);

        modelString = JsonUtility.ToJson(objectContainer);
        objectContainer = null;
        WriteJson();

        Debug.Log("Serialization finished");
    }

    private void SerializationRecursion(Transform root, ObjectContainer container)
    {        
        container.name = root.gameObject.name;
        container.SetTransform(root);
        var meshFilter = root.GetComponent<MeshFilter>();
        
        if (null!= meshFilter)
        {            
            container.SetMesh(meshFilter.sharedMesh);
        }
        if (root.childCount==0)
        {
            return;
        }
        foreach (Transform child in root)
        {
            var container_child = new ObjectContainer();
            container.AddChild(container_child);
            
            SerializationRecursion(child, container_child);
        }

    }

    public void WriteJson()
    {
        StreamWriter writer = new StreamWriter(json_path);
        writer.Write(modelString);
        writer.Flush();
        writer.Close();
    }

    public void ReadJson()
    {
        StreamReader reader = new StreamReader(json_path);
        modelString = reader.ReadToEnd();
        reader.Close();
    }

    public void Reset()
    {

    }

    public void DeserializeMesh()
    {
        ReadJson();
        ObjectContainer container = JsonUtility.FromJson<ObjectContainer>(modelString);
        outputObject = new GameObject();
        DeserializationRecursion(outputObject, container);
        Debug.Log("Deserialization finished");
    }

    private void DeserializationRecursion(GameObject root, ObjectContainer container)
    {
        if(null != container.meshContainer)
        {
            var filter = root.AddComponent<MeshFilter>();
            var renderer = root.AddComponent<MeshRenderer>();
            
            root.name = container.name;
            container.transform.ExtractTransform(root.transform);

            Mesh mesh = new Mesh();

            filter.sharedMesh = mesh;
            mesh.name = container.name;
            container.meshContainer.ExtractMesh(mesh);
            renderer.material = new Material(Shader.Find("Diffuse"));
            
        }

        if (container.objects.Count==0)
        {
            return;
        }

        foreach (ObjectContainer child_container in container.objects)
        {
            var child = new GameObject();
            child.transform.SetParent(root.transform);
            
            DeserializationRecursion(child, child_container);
        }
    }

    private void Update()
    {
        
    }

    private void OnApplicationQuit()
    {
        
    }
}


//
//  Layout for JSON
//
[Serializable]
public class ObjectContainer
{
    public string name;

    public TransformContainer transform;

    public List<ObjectContainer> objects = new List<ObjectContainer>();

    public MeshContainer meshContainer = new MeshContainer();

    public void SetTransform(Transform transform)
    {
        this.transform = new TransformContainer(transform);
    }

    public void AddChild(ObjectContainer child)
    {
        objects.Add(child);
    }

    public void SetMesh(Mesh mesh)
    {
        this.meshContainer.ConvertMesh(mesh);
    }

    [Serializable]
    public class MeshContainer
    {
        public string name;

        public List<VectorContainer> vertices = new List<VectorContainer>();
        public List<VectorContainer> normals = new List<VectorContainer>();
        public List<VectorContainer> tangents = new List<VectorContainer>();
        public List<int> triangles = new List<int>();
        public List<VectorContainer> uv = new List<VectorContainer>();
        public List<VectorContainer> uv2 = new List<VectorContainer>();
        public List<VectorContainer> uv3 = new List<VectorContainer>();
        public List<VectorContainer> uv4 = new List<VectorContainer>();
        public List<VectorContainer> uv5 = new List<VectorContainer>();
        public List<VectorContainer> uv6 = new List<VectorContainer>();
        public List<VectorContainer> uv7 = new List<VectorContainer>();
        public List<VectorContainer> uv8 = new List<VectorContainer>();

        public MeshContainer() { }

        public void ConvertMesh(UnityEngine.Mesh mesh)
        {
            name = mesh.name;           

            for (int i = 0; i < mesh.vertices.Length; i++)
            {
                vertices.Add(new VectorContainer(mesh.vertices[i]));
                normals.Add(new VectorContainer(mesh.normals[i]));
                tangents.Add(new VectorContainer(mesh.tangents[i]));
            }

            foreach (int index in mesh.triangles)
            {
                triangles.Add(index);
            }

            foreach (Vector2 _uv in mesh.uv)
            {
                uv.Add(new VectorContainer(_uv));
            }

            foreach (Vector2 _uv in mesh.uv2)
            {
                uv2.Add(new VectorContainer(_uv));
            }

            foreach (Vector2 _uv in mesh.uv3)
            {
                uv3.Add(new VectorContainer(_uv));
            }

            foreach (Vector2 _uv in mesh.uv4)
            {
                uv4.Add(new VectorContainer(_uv));
            }

            foreach (Vector2 _uv in mesh.uv5)
            {
                uv5.Add(new VectorContainer(_uv));
            }

            foreach (Vector2 _uv in mesh.uv6)
            {
                uv6.Add(new VectorContainer(_uv));
            }

            foreach (Vector2 _uv in mesh.uv7)
            {
                uv7.Add(new VectorContainer(_uv));
            }

            foreach (Vector2 _uv in mesh.uv8)
            {
                uv8.Add(new VectorContainer(_uv));
            }
        }

        public void ExtractMesh(Mesh mesh)
        {
            List<Vector3> vertices = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();
            List<Vector4> tangents = new List<Vector4>();
            List<int> triangles = new List<int>();
            List<Vector2> uv = new List<Vector2>();
            List<Vector2> uv2 = new List<Vector2>();
            List<Vector2> uv3 = new List<Vector2>();
            List<Vector2> uv4 = new List<Vector2>();
            List<Vector2> uv5 = new List<Vector2>();
            List<Vector2> uv6 = new List<Vector2>();
            List<Vector2> uv7 = new List<Vector2>();
            List<Vector2> uv8 = new List<Vector2>();

            for (int i = 0; i < this.vertices.Count; i++)
            {
                var vertex = this.vertices[i];
                var normal = this.normals[i];
                var tangent = this.tangents[i];
                vertices.Add(new Vector3(vertex.x, vertex.y, vertex.z));
                normals.Add(new Vector3(normal.x, normal.y, normal.z));
                tangents.Add(new Vector4(tangent.x, normal.y, normal.z));
            }

            foreach (int index in this.triangles)
            {
                triangles.Add(index);
            }

            foreach (VectorContainer _uv in this.uv)
            {
                uv.Add(new Vector2(_uv.x, _uv.y));
            }

            foreach (VectorContainer _uv in this.uv2)
            {
                uv2.Add(new Vector2(_uv.x, _uv.y));
            }

            foreach (VectorContainer _uv in this.uv3)
            {
                uv3.Add(new Vector2(_uv.x, _uv.y));
            }

            foreach (VectorContainer _uv in this.uv4)
            {
                uv4.Add(new Vector2(_uv.x, _uv.y));
            }

            foreach (VectorContainer _uv in this.uv5)
            {
                uv5.Add(new Vector2(_uv.x, _uv.y));
            }

            foreach (VectorContainer _uv in this.uv6)
            {
                uv6.Add(new Vector2(_uv.x, _uv.y));
            }

            foreach (VectorContainer _uv in this.uv7)
            {
                uv7.Add(new Vector2(_uv.x, _uv.y));
            }

            foreach (VectorContainer _uv in this.uv8)
            {
                uv8.Add(new Vector2(_uv.x, _uv.y));
            }
            
            mesh.vertices = vertices.ToArray();
            mesh.normals = normals.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.tangents = tangents.ToArray();

            mesh.uv = uv.ToArray();
            mesh.uv2 = uv2.ToArray();
            mesh.uv3 = uv3.ToArray();
            mesh.uv4 = uv4.ToArray();
            mesh.uv5 = uv5.ToArray();
            mesh.uv6 = uv6.ToArray();
            mesh.uv7 = uv7.ToArray();
            mesh.uv8 = uv8.ToArray();
        }
    }

    [Serializable]
    public class TransformContainer
    {
        public VectorContainer translation;
        public VectorContainer rotation;
        public VectorContainer scale;

        public TransformContainer(Transform transform)
        {
            translation = new VectorContainer(transform.localPosition);
            rotation = new VectorContainer(transform.localRotation);
            scale = new VectorContainer(transform.localScale);
        }
        public void ExtractTransform(Transform transform)
        {
            transform.localPosition = new Vector3(translation.x, translation.y, translation.z);
            transform.localRotation = new Quaternion(rotation.x, rotation.y, rotation.z, rotation.w);
            transform.localScale = new Vector3(scale.x, scale.y, scale.z);            
        }
    }

    [Serializable]
    public class VectorContainer
    {
        public float x, y, z, w;

        public VectorContainer(Vector4 vector) { x = vector.x; y = vector.y; z = vector.z; w = vector.w; }
        public VectorContainer(Vector3 vector) { x = vector.x; y = vector.y; z = vector.z; w = 1f; }
        public VectorContainer(Vector2 vector) { x = vector.x; y = vector.y; z = 1f; w = 1f; }
        public VectorContainer(Quaternion rotation) { x = rotation.x; y = rotation.y; z = rotation.z; w = rotation.w; }

        public void SwitchHands()
        {
            float z = this.z;
            this.z = y;
            y = z;
        }
    }
}






