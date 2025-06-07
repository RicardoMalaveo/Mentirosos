using System.Collections;
using UnityEngine;

public class CardAnimation : MonoBehaviour
{
    public AnimationCurve moveCurve;
    public AnimationCurve rotationCurve;
    public Card cards;

    private bool isAnimating = false;

    public void AnimateCard(Transform startPos, Transform endPos, float duration, Vector3 finalEulerRotation)
    {
            StartCoroutine(AnimateMovement(startPos.position,endPos.position,duration,Quaternion.Euler(finalEulerRotation)));
    }

    private IEnumerator AnimateMovement(Vector3 from, Vector3 to, float duration, Quaternion finalRotation) 
    { 
        isAnimating = true;
        float time = 0f; 
        Quaternion startRotation = transform.rotation;

        while (time < duration)
        {
            float t = time / duration;
            float easedT = moveCurve.Evaluate(t);
            
            float easedRot = rotationCurve.Evaluate(t);
            transform.position = Vector3.Lerp(from, to, easedT);

            transform.rotation = Quaternion.Slerp(startRotation, finalRotation, easedRot);

            time += Time.deltaTime;
            yield return null;
        }
        isAnimating = false;
    }
}
