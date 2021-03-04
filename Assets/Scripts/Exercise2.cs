using UnityEngine;

public class Exercise2 : MonoBehaviour
{
    [SerializeField] private ComputeShader computeShader;
    [SerializeField] private RenderTexture renderTexture;
    [SerializeField] private Texture2D texture;
    [SerializeField] private float left;
    [SerializeField] private float right;
    [SerializeField] private int width, iterations, bailout, height, bottom, top;
    
    private void Start()
    {
        renderTexture.enableRandomWrite = true;
        renderTexture.Create();
        var kernelIndex = computeShader.FindKernel("CSMain");
        computeShader.SetFloat("_Left", left);
        computeShader.SetFloat("_Right", right);
        computeShader.SetFloat("_Top", top);
        computeShader.SetFloat("_Bottom", bottom);
        computeShader.SetInt("_Width", width);
        computeShader.SetInt("_Height", height);
        computeShader.SetInt("_Iterations", iterations);
        computeShader.SetInt("_Bailout", bailout);
        computeShader.SetTexture(kernelIndex, "_MandelbrotSet", renderTexture);
        computeShader.SetTexture(kernelIndex, "_ColorGradient", texture);
        computeShader.Dispatch(kernelIndex, renderTexture.width / 8, renderTexture.height / 8, 1);
    }
 
    private void OnDestroy()
    {
        renderTexture.Release();
    }
    
    private void Reset()
    {
        iterations = 100;
        bailout = 2;
        bottom = -1;
        left = -2.5f;
        right = 1;
        top = 1;
        computeShader = null;
        renderTexture = null;
        texture = null;
    }
}