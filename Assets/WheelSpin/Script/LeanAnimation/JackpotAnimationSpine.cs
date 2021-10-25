using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;

public class JackpotAnimationSpine : MonoBehaviour
{
    [SerializeField] SkeletonDataAsset dateAsset;
    [SerializeField] GameObject spinePrefab;
    [SerializeField] Transform parentCoin;
    void InstantiateGameObject()
    {
        GameObject gb = Instantiate(spinePrefab);
        gb.layer =1;
        SkeletonAnimation skeleton = gb.GetComponent<SkeletonAnimation>();
        skeleton.skeletonDataAsset = dateAsset;
        skeleton.loop = true;
        int y = Random.Range(1, 8);
        skeleton.AnimationName = "coins_rain_var"+y;
        skeleton.timeScale = .5f;
        gb.transform.SetParent(parentCoin);
        float x = Random.Range(.2f, .4f);
        gb.transform.localScale = new Vector3(x, x);
        gb.transform.localPosition = new Vector3(Random.Range(-10f, 10f), 0, 0);
        Destroy(gb, 8f);
    }
    private void OnDisable()
    {
        CancelInvoke("InstantiateGameObject");
    }
    private void OnEnable()
    {
        InvokeRepeating("InstantiateGameObject", 0, .1f);
    }
}
