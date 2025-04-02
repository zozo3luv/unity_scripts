using UnityEngine;
using PandaTitle;

[ExecuteInEditMode]
public class PandaPostProcess : MonoBehaviour
{
	public Material PostProcessMat;

	[Range(0, 1)] public float StepFactor = 0.5F;
    [Range(0, 1)] public float MainAlpha=1F;
	[Range(0, 1)] public float BlurFactor;
	[Range(0, 4)] public float LineUVScale;
	[Range(0, 1.5F)] public float Chromatic;
	[Range(0, 1)] public float Frequency;
	[Range(0, 1)] public float Amplitude;
	[Range(1, 3)] public float VignettePower=1.5F;
	[Range(0, 3)] public float VignetteScale= 1.5F;
	[Range(0, 2)] public float TimeScale = 1F;
	private void Awake()
	{
		if( PostProcessMat == null )
		{
			enabled = false;
		}
		else
		{
			
			PostProcessMat.mainTexture = PostProcessMat.mainTexture;
		}

	}

	//void Start()
	//{
	
	//var BlurFactor = PostProcessMat.GetFloat("_BlurFactorK");


	//}

	//void Update()
	//{
		

	
	//}

	void OnRenderImage( RenderTexture src, RenderTexture dest )
	{
		PostProcessMat.SetFloat("_StepFactorK", StepFactor);

		PostProcessMat.SetFloat("_BlurFactorK", BlurFactor);
		
		PostProcessMat.SetFloat("_LineUVScaleK", LineUVScale);

		PostProcessMat.SetFloat("_MainAlphaK", MainAlpha);

		PostProcessMat.SetFloat("_zhenpinK", Frequency);

		PostProcessMat.SetFloat("_zhenfuK", Amplitude);
		PostProcessMat.SetFloat("_RedBlueFactorK", Chromatic);
		PostProcessMat.SetFloat("_VignettePowerK", VignettePower);
		PostProcessMat.SetFloat("_VignetteScaleK", VignetteScale);

		Time.timeScale = TimeScale;
		Graphics.Blit( src, dest, PostProcessMat );
	}
}
