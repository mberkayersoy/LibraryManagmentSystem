using UnityEngine;
using DG.Tweening;
public class CollectibleMovement : MonoBehaviour
{

    private void OnDisable()
    {
        transform.DOKill();
        transform.rotation = Quaternion.identity;
    }

    private void OnEnable()
    {
        Rotate();
    }
    private void Rotate()
    {
        transform.DORotate(new Vector3(0f, 360f, 0f), 1f, RotateMode.LocalAxisAdd).
            SetEase(Ease.Linear).
            SetLoops(-1);
    }

    private void OnDestroy()
    {
        transform.DOKill();
    }
}
