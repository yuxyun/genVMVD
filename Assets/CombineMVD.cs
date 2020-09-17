using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class CombineMVD : MonoBehaviour
{
    public ComputeShader combineMVDShader;
    private ComputeBuffer MVBuffer;
    private ComputeBuffer DepthBuffer;
    private ComputeBuffer MVDBuffer;
    private int kernel;
    private int length = 1024;
    private uint[] MVData;
    private uint[] DepthData;
    private uint[] MVDData;

    void runComputeShader()
    {
        MVData = new uint[length];
        DepthData = new uint[length];
        MVDData = new uint[length];

        for (uint i = 0; i < length; i++)
        {
            MVData[i] = i;
            DepthData[i] = i;
            MVDData[i] = 0;
        }

        kernel = combineMVDShader.FindKernel("CSMain");

        MVBuffer = new ComputeBuffer(MVData.Length, 4);
        MVBuffer.SetData(MVData);
        DepthBuffer = new ComputeBuffer(DepthData.Length, 4);
        DepthBuffer.SetData(DepthData);
        MVDBuffer = new ComputeBuffer(MVDData.Length, 4);
        combineMVDShader.SetBuffer(kernel, "MV", MVBuffer);
        combineMVDShader.SetBuffer(kernel, "Depth", DepthBuffer);
        combineMVDShader.SetBuffer(kernel, "MVD", MVDBuffer);

        combineMVDShader.Dispatch(kernel, length, 1, 1);

        MVDBuffer.GetData(MVDData);

        for (int i = 0; i < length; i++)
        {
            UnityEngine.Debug.Log("runComputeShader MVData[" + i + "]=" + MVData[i] + ", DepthData[" + i + "]=" + DepthData[i] + ", MVDData[" + i + "]" + "=" + MVDData[i]);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        runComputeShader();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
