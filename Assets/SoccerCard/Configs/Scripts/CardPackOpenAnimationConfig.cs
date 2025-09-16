using UnityEngine;

[CreateAssetMenu(fileName = "CardPackOpenAnimationConfig", menuName = "Configs/CardPackOpenAnimationConfig")]
public class CardPackOpenAnimationConfig : ScriptableObject
{
    public int flipCount = 8;
    public float flipDuration = 0.3f;
    public float flipSpeedup = 0.7f;
    public float explodeScale = 2.5f;
    public float explodeDuration = 0.3f;
    public float shakeMagnitude = 20f;
    public Color explodeColor = Color.yellow;
    public float fadeDuration = 0.2f;
}