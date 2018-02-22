using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class GradientCreator : MonoBehaviour {

	public AnimationCurve curve;
	public int width;

	public bool execute;

	// Update is called once per frame
	void Update () 
	{
		if (execute)
		{
			Execute();
			execute = false;
		}	
	}

	public void Execute ()
	{
		Texture2D texture = new Texture2D(width, 1, TextureFormat.ARGB32, false);
		Color[] pixels = texture.GetPixels();

		float minScaleDivision = 1f/width;

		for (int i = 0; i < pixels.Length; i++)
		{
			float samplePosition = minScaleDivision * (i);

			float grayscale = curve.Evaluate(samplePosition);
			 
			Color newColor = FromGrayscale(grayscale);

			pixels[i] = newColor;
		}

		texture.SetPixels(pixels);

		ExportImage(texture);
	}

	public static void ExportImage(Texture2D texture)
	{
		string path = GetImagePath();

		if (!string.IsNullOrEmpty(path))
		{
			byte[] bytes = texture.EncodeToPNG();
			System.IO.File.WriteAllBytes(path, bytes);
			AssetDatabase.Refresh();
		}
	}

	public static string GetImagePath ()
	{
		string path = EditorUtility.SaveFilePanel ("Save Image", Application.dataPath, "Fractal", "png");

		return path;
	}

	public static Color FromGrayscale (float grayscale)
	{
		float clamped = grayscale;

		return new Color(clamped, clamped, clamped, 1);
	}
}
