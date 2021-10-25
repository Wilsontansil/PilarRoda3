using UnityEngine;

public class AnimationStart : MonoBehaviour
{
    [SerializeField] GameObject logoGame;

    private void Awake()
    {
        logoGame.transform.localScale = Vector3.zero;
    }
    private void Start()
    {
        LeanTween.scale(logoGame, Vector2.one, 1f).setEase(LeanTweenType.easeInOutElastic);
    }
}
