using UnityEngine;

namespace PuzzleTest
{
    [CreateAssetMenu(menuName = "PuzzleTest/New Mask")]
    public class MaskSO : ScriptableObject
    {
        public TextAsset maskJson;
        public Texture2D[] masks;


        public Texture2D GetTextureByImg(string imgName)
        {
            imgName = imgName.Replace(".png", "");

            foreach (Texture2D mask in masks)
            {
                if (mask.name == imgName)
                {
                    return mask;
                }
            }

            return null;
        }
    }
}
