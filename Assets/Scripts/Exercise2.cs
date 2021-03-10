using System;
using UnityEngine;

public class Exercise2 : MonoBehaviour
{
    // declare fields (see the Reset method for the required fields)
    [SerializeField] private ComputeShader computeShader;
    [SerializeField] private RenderTexture renderTexture;
    [SerializeField] private Texture2D texture;
    [SerializeField] private float left, right;
    [SerializeField] private float top, bottom;
    [SerializeField] private int iterations, bailout;

    // declare variables
    private int width, height, kernelIndex;
    private RectTransform rt;
    private float zoomfactor;
    
    private void Start()
    {
        // Define variables
        rt = GetComponent<RectTransform>();
        zoomfactor = (float) 0.1;

        renderTexture.enableRandomWrite = true;
        renderTexture.Create();

        kernelIndex = computeShader.FindKernel("CSMain");

        // Set shader variables (ints and floats)
        computeShader.SetFloat("_Left", left);
        computeShader.SetFloat("_Right", right);
        computeShader.SetFloat("_Top", top);
        computeShader.SetFloat("_Bottom", bottom);
        computeShader.SetInt("_Iterations", iterations);
        computeShader.SetInt("_Bailout", bailout);

        width = renderTexture.width;
        height = renderTexture.height;
        computeShader.SetInt("_Width", width);
        computeShader.SetInt("_Height", height);

        computeShader.SetTexture(kernelIndex, "_MandelbrotSet", renderTexture);
        computeShader.SetTexture(kernelIndex, "_ColorGradient", texture);
        computeShader.Dispatch(kernelIndex, renderTexture.width / 8, renderTexture.height / 8, 1);
    }

    private void Update()
    {
        // zoom in with left click
        if (Input.GetMouseButtonDown(0))
        {
            // Compute mouse offset and move mousepoint to middle of screen
            moveToCenter();

            // Zoom in
            left = left + Math.Abs(right-left) * zoomfactor;
            right = right - Math.Abs(right-left) * zoomfactor;
            top = top - Math.Abs(top-bottom) * zoomfactor;
            bottom = bottom + Math.Abs(top-bottom) * zoomfactor;
            computeShader.Dispatch(kernelIndex, renderTexture.width / 8, renderTexture.height / 8, 1);
        }
        // zoom out with right click
        if (Input.GetMouseButtonDown(1))
        {
            // Compute mouse offset and move mousepoint to middle of screen
            moveToCenter();

            // Zoom out
            left = left - Math.Abs(right-left) * zoomfactor;
            right = right + Math.Abs(right-left) * zoomfactor;
            top = top + Math.Abs(top-bottom) * zoomfactor;
            bottom = bottom - Math.Abs(top-bottom) * zoomfactor;
        }
        // Update shader variables and recompute shader
        setShaderVariables();
        computeShader.Dispatch(kernelIndex, renderTexture.width / 8, renderTexture.height / 8, 1);
    }

    // Function to update the center point of the screen
    // It move the point at the mouse position to the center
    private void moveToCenter() {
        // Get mouse position
        Vector3 click = Input.mousePosition;
        // Correct mouse position, such that the center is (0, 0)
        Vector3 half_screen_size = new Vector3(rt.rect.width/2, rt.rect.height/2, 0);
        click = click - half_screen_size;
        // Compute relative width and high offset
        float width_offset = (click.x / half_screen_size.x);
        float height_offset = (click.y / half_screen_size.y);
        // Compute absolute width and high offset, based on shader variables
        width_offset = width_offset * Math.Abs(right-left)/2;
        height_offset = height_offset * Math.Abs(top-bottom)/2;

        // Move mousepoint to middle of screen
        left = left + width_offset;
        right = right + width_offset;
        top = top - height_offset;
        bottom = bottom - height_offset;
    }

    // Function to update the left, right, top, and bottom value of the shader
    private void setShaderVariables()
    {
        computeShader.SetFloat("_Left", left);
        computeShader.SetFloat("_Right", right);
        computeShader.SetFloat("_Top", top);
        computeShader.SetFloat("_Bottom", bottom);
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