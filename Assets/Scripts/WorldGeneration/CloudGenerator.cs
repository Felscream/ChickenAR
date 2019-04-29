using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class CloudGenerator : MonoBehaviour
{
    private struct Cloud
    {
        public float Scale;
        public int X;
        public int Y;
    }

    private struct Batch
    {
        public int Length;
        public Cloud[] Clouds;
        public Matrix4x4[] Objects;
    }

    private struct FrameParams
    {
        public float Time;
        public float DeltaTime;
        public int BatchIndex;
    }

    public Mesh CloudMesh;
    public Material CloudMaterial;
    public float CloudSize = 1f;
    public float MaxScale = 1f;
    public float TexScale = 0.1f;
    public float MinNoiseSize = 0.6f;
    public float ScaleSize = 1.5f;
    public int CloudsCount = 100;
    public Vector2 Speed = new Vector2(0.2f, 0.2f);

    private float _deltaTime;
    private const int BatchSize = 1023;

    private Batch[] _batches;
    private Task[] _tasks;

    private TimeScaleManager _time;

    private void Start()
    {
        _time = TimeScaleManager.Instance;
        int count = CloudsCount * CloudsCount;
        
        int batchCount = count / BatchSize + 1;
        
        _batches = new Batch[batchCount];
        _tasks = new Task[batchCount];

        for (int i = 0; i < batchCount; i++)
        {
            int length = Mathf.Min(BatchSize, count - i * BatchSize);
            _batches[i].Length = length;
            _batches[i].Clouds = new Cloud[length];
            _batches[i].Objects = new Matrix4x4[length];
        }
        
        float offset = -CloudsCount * 0.5f;
        for (int cloudY = 0; cloudY < CloudsCount; cloudY++)
        {
            for (int cloudX = 0; cloudX < CloudsCount; cloudX++)
            {
                Cloud cloud = new Cloud
                {
                    Scale = 0,
                    X = cloudX,
                    Y = cloudY,
                };
                
                Vector3 position = new Vector3
                (
                    offset + transform.position.x + cloudX * CloudSize,
                    transform.position.y,
                    offset + transform.position.z + cloudY * CloudSize
                );
                
                int index = cloudY * CloudsCount + cloudX;

                int x = index / BatchSize;
                int y = index % BatchSize;

                _batches[x].Clouds[y] = cloud;
                _batches[x].Objects[y] = Matrix4x4.TRS(position, Quaternion.identity, Vector3.zero);
            }
        }
    }

    private void Update()
    {
        _deltaTime = _time != null ? TimeScaleManager.Delta : Time.deltaTime;
        for (int batchIndex = 0; batchIndex < _batches.Length; batchIndex++)
        {
            FrameParams frameParams = new FrameParams
            {
                BatchIndex = batchIndex,
                DeltaTime = _deltaTime,
                Time = _time != null ? TimeScaleManager.TimeScale * Time.time : Time.time 
            };
            
            _tasks[batchIndex] = Task.Factory.StartNew(UpdateBatch, frameParams);
        }
        
        Task.WaitAll(_tasks);
        
        for (int batchIndex = 0; batchIndex < _batches.Length; batchIndex++)
        {
            Graphics.DrawMeshInstanced(CloudMesh, 0, CloudMaterial, _batches[batchIndex].Objects);
        }
    }

    private void UpdateBatch(object input)
    {
        FrameParams frameParams = (FrameParams)input;
        for (int cloudIndex = 0; cloudIndex < _batches[frameParams.BatchIndex].Length; cloudIndex++)
        {
            int i = frameParams.BatchIndex;
            int j = cloudIndex;
            
            float x = _batches[i].Clouds[j].X * TexScale + frameParams.Time * Speed.x;
            float y = _batches[i].Clouds[j].Y * TexScale + frameParams.Time * Speed.y;
            float noise = Mathf.PerlinNoise(x, y);
            
            int dir = noise > MinNoiseSize ? 1 : -1;
            
            float shift = ScaleSize * frameParams.DeltaTime * Speed.magnitude * dir;
            float scale = _batches[i].Clouds[j].Scale + shift;
            scale = Mathf.Clamp(scale, 0, MaxScale);
            _batches[i].Clouds[j].Scale = scale;
            
            _batches[i].Objects[j].m00 = scale;
            _batches[i].Objects[j].m11 = scale;
            _batches[i].Objects[j].m22 = scale;
        }
    }
}
