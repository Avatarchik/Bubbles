using UnityEngine;
using UnityEditor;

public class CreateAssetBundles
{
	[MenuItem ("Assets/Build AssetBundles")]
	static void BuildAllAssetBundles ()
	{
		BuildPipeline.BuildAssetBundles (Application.dataPath,BuildAssetBundleOptions.CompleteAssets,BuildTarget.StandaloneWindows);
	}
}