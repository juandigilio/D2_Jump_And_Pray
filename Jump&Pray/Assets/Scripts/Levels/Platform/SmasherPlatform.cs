using UnityEngine;
using System.Collections;

public class SmasherPlatform : MonoBehaviour
{
    [SerializeField] private GameObject leftBlock, rightBlock;
    [SerializeField] private float blockDistance = 2f;
    [SerializeField] private float timeBeforeSmash = 1f;
    [SerializeField] private float smashSpeed = 5f;
    [SerializeField] private float stayClosedTime = 1f;
    [SerializeField] private float shakeIntensity = 0.1f;
    [SerializeField] private float shakeFrequency = 20f;

    private Vector3 leftStartPos, rightStartPos;
    private Vector3 leftTargetPos, rightTargetPos;
    private bool isSmashing = false;
    private bool playerDetected = false;

    private void Start()
    {
        SetupBlocks();
    }

    private void SetupBlocks()
    {
        Vector3 size = GetComponent<Collider>().bounds.size;
        Vector3 center = GetComponent<Collider>().bounds.center;

        float y = center.y;
        float z = center.z;

        leftStartPos = new Vector3(center.x - size.x / 2 - blockDistance, y, z);
        rightStartPos = new Vector3(center.x + size.x / 2 + blockDistance, y, z);

        Vector3 offset = Vector3.right * (blockDistance - 0.2f);
        leftTargetPos = center;
        rightTargetPos = center;

        leftBlock.transform.position = leftStartPos;
        rightBlock.transform.position = rightStartPos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isSmashing || playerDetected || !other.CompareTag("Player")) return;

        Debug.Log("Player detected by SmasherPlatform");
        Debug.Log(isSmashing);
        playerDetected = true;
        isSmashing = true;
        StartCoroutine(SmashSequence());
    }

    private IEnumerator SmashSequence()
    {
        float elapsed = 0f;
        Vector3 lStart = leftBlock.transform.position;
        Vector3 rStart = rightBlock.transform.position;

        while (elapsed < timeBeforeSmash)
        {
            float shakeOffset = Mathf.Sin(Time.time * shakeFrequency) * shakeIntensity;

            leftBlock.transform.position = lStart + Vector3.up * shakeOffset;
            rightBlock.transform.position = rStart + Vector3.up * shakeOffset;

            elapsed += Time.deltaTime;

            yield return null;
        }

        leftBlock.transform.position = lStart;
        rightBlock.transform.position = rStart;

        StartCoroutine(MoveBlock(leftBlock.transform, leftTargetPos, false));
        StartCoroutine(MoveBlock(rightBlock.transform, rightTargetPos, false));

        yield return new WaitForSeconds(stayClosedTime);

        StartCoroutine(MoveBlock(leftBlock.transform, leftStartPos, true));
        StartCoroutine(MoveBlock(rightBlock.transform, rightStartPos, true));

        yield return null; 

        //isSmashing = false;
        playerDetected = false;
    }

    private IEnumerator MoveBlock(Transform block, Vector3 targetPos, bool isEnding)
    {
        while (Vector3.Distance(block.position, targetPos) > 0.01f)
        {
            block.position = Vector3.MoveTowards(block.position, targetPos, smashSpeed * Time.deltaTime);
            yield return null;
        }

        if (isEnding)
        {
            isSmashing = false;
        }
    }
}

