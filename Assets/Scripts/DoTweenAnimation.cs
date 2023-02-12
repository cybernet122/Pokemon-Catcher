using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class DoTweenAnimation : MonoBehaviour
{
    Vector3 startVec;
    Sequence sequence;

    void Start()
    {
        startVec = transform.localPosition;
        if(GetComponent<VerticalLayoutGroup>())
        GetComponent<VerticalLayoutGroup>().enabled = true;
        int rng = Random.Range(1,4);
        Invoke("StartAnimation"+ rng,0.1f);
    }

    private void StartAnimation1()
    {
        sequence = DOTween.Sequence();
        transform.localPosition = new Vector2(startVec.x + 20, startVec.y - 15);
        sequence.Append(transform.DOLocalMove(new Vector2(startVec.x + 20, startVec.y - 15), 0f).SetEase(Ease.Linear));
        sequence.Append(transform.DOLocalMove(new Vector2(startVec.x - 20, startVec.y + 10), 0.25f).SetEase(Ease.Linear));
        sequence.Append(transform.DOLocalMove(new Vector2(startVec.x - 20, startVec.y - 15), 0.25f).SetEase(Ease.Linear));
        sequence.Append(transform.DOLocalMove(new Vector2(startVec.x + 20, startVec.y + 10), 0.25f).SetEase(Ease.Linear));
        sequence.Append(transform.DOLocalMove(new Vector2(startVec.x + 20, startVec.y - 15), 0.25f).SetEase(Ease.Linear));
        sequence.SetLoops(1000);
    }

    private void StartAnimation2()
    {
        sequence = DOTween.Sequence();
        transform.localPosition = new Vector2(startVec.x, startVec.y + 10);
        sequence.Append(transform.DOLocalMove(new Vector2(startVec.x, startVec.y + 10), 0f).SetEase(Ease.Linear));
        sequence.Append(transform.DOLocalMove(new Vector2(startVec.x - 20, startVec.y - 15), 0.25f).SetEase(Ease.Linear));
        sequence.Append(transform.DOLocalMove(new Vector2(startVec.x + 20, startVec.y + 10), 0.25f).SetEase(Ease.Linear));
        sequence.Append(transform.DOLocalMove(new Vector2(startVec.x + 20, startVec.y - 15), 0.25f).SetEase(Ease.Linear));
        sequence.Append(transform.DOLocalMove(new Vector2(startVec.x, startVec.y + 10), 0.25f).SetEase(Ease.Linear));
        sequence.SetLoops(1000);
    }

    private void StartAnimation3()
    {
        sequence = DOTween.Sequence();
        transform.localPosition = new Vector2(startVec.x - 20, startVec.y - 15);
        sequence.Append(transform.DOLocalMove(new Vector2(startVec.x - 20, startVec.y - 15), 0f).SetEase(Ease.Linear));
        sequence.Append(transform.DOLocalMove(new Vector2(startVec.x + 20, startVec.y + 10), 0.25f).SetEase(Ease.Linear));
        sequence.Append(transform.DOLocalMove(new Vector2(startVec.x + 20, startVec.y - 15), 0.25f).SetEase(Ease.Linear));
        sequence.Append(transform.DOLocalMove(new Vector2(startVec.x - 20, startVec.y + 10), 0.25f).SetEase(Ease.Linear));
        sequence.Append(transform.DOLocalMove(new Vector2(startVec.x - 20, startVec.y - 15), 0.25f).SetEase(Ease.Linear));
        sequence.SetLoops(1000);
    }

    private void OnEnable()
    {
        if (sequence != null)
        {
            int rng = Random.Range(1, 4);
            Invoke("StartAnimation" + rng, 0.1f);
        }
    }

    private void OnDisable()
    {
        StopAnimation();
    }

    public void StopAnimation()
    {
        if (sequence != null)
        {
            sequence.Pause();
            sequence.Kill();
        }
    }
}
