using UnityEngine;

public class Exercise1 : MonoBehaviour
{
    [SerializeField] private ComputeShader computeShader;
    [SerializeField] private RenderTexture renderTexture;

    private void Start()
    {
        renderTexture.enableRandomWrite = true;
        renderTexture.Create();
        var kernelIndex = computeShader.FindKernel("CSMain");
        computeShader.SetTexture(kernelIndex, "Result", renderTexture);
        computeShader.Dispatch(kernelIndex, renderTexture.width / 8, renderTexture.height / 8, 1);
    }

    private void OnDestroy()
    {
        renderTexture.Release();
    }
}