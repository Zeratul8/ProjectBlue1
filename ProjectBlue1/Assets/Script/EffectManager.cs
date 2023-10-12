using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using AllIn1VfxToolkit.Demo.Scripts;
public class EffectManager : SingletonMonoBehaviour<EffectManager>
{
    public enum EffectType
    {
        None = -1,
        Slash,
        SwordAura,
        Max
    }

    [Space, Header("Projectile References")]
    [Space, SerializeField] private GameObject projectileBasePrefab;
    [SerializeField] private GameObject projectileSceneSetupObject;
    [SerializeField] private Transform projectileSpawnPoint;

    [Space, Header("Demo Controller Input")]
    [SerializeField] private KeyCode playEffectKey = KeyCode.Q;


    [SerializeField]
    EffectsDictionary effectsDictionary = new EffectsDictionary();
    private int /*currDemoCollectionIndex, currDemoEffectIndex, */currentEffectPlays;
    //private AllIn1DemoScaleTween currLabelTween, playButtTween, nextButtTween, prevButtTween;
    private float timeSinceEffectPlay;
    private AllIn1TimeControl allIn1TimeControl;

    private void Start()
    {
        projectileSceneSetupObject.SetActive(false);
        //currDemoCollectionIndex = startingCollectionIndex;
        // currDemoEffectIndex = startingEffectIndex;
        allIn1TimeControl = gameObject.GetComponent<AllIn1TimeControl>();
        //SetupAndInstantiateCurrentEffect();
        foreach(All1VfxDemoEffect effect in effectsDictionary.Values)
        {
            SetupAndInstantiateCurrentEffect(effect);
        }
    }

    private void Update()
    {
        //if (currDemoEffect.canBePlayedAgain && Input.GetKeyDown(playEffectKey)) PlayCurrentEffect(true);
    }





    public void SlashSword(bool isAfterSetupAndInstantiateEffect = false)
    {
        PlayCurrentEffect(effectsDictionary[EffectType.Slash], isAfterSetupAndInstantiateEffect);
    }








    void PlayCurrentEffect(All1VfxDemoEffect currEffect, bool isAfterSetupAndInstantiateEffect = false)
    {
        if (!isAfterSetupAndInstantiateEffect && currEffect.onlyOneAtATime) DestroyAllChildren();

        timeSinceEffectPlay = 0f;

        Transform tempTransform = null;
        if (currEffect.isShootProjectile)
        {
            Transform projectileBase = Instantiate(projectileBasePrefab, projectileSpawnPoint.position, Quaternion.identity).transform;
            projectileBase.forward = projectileSpawnPoint.forward;
            projectileBase.parent = transform;
            projectileBase.localRotation = Quaternion.identity;

            tempTransform = Instantiate(currEffect.projectilePrefab, projectileSpawnPoint.position, Quaternion.identity).transform;
            tempTransform.localRotation = Quaternion.identity;
            tempTransform.forward = projectileSpawnPoint.forward;
            tempTransform.parent = projectileBase;
            AllIn1DemoProjectile tempProjectileInstance = projectileBase.GetComponent<AllIn1DemoProjectile>();
            tempProjectileInstance.Initialize(transform, projectileSpawnPoint.forward, currEffect.projectileSpeed, currEffect.impactPrefab, currEffect.scaleMultiplier);
            if (currEffect.doCameraShake) tempProjectileInstance.AddScreenShakeOnImpact(currEffect.projectileImpactShakeAmount);
        }
        else
        {
            tempTransform = Instantiate(currEffect.effectPrefab, transform).transform;
            if (!currEffect.spawnTouchingFloor) tempTransform.localPosition = Vector3.zero;
            tempTransform.localRotation = currEffect.effectPrefab.transform.rotation;
            if (currEffect.canBePlayedAgain && currEffect.randomSpreadRadius > 0f && currentEffectPlays > 0)
            {
                tempTransform.position += new Vector3(Random.Range(-currEffect.randomSpreadRadius, currEffect.randomSpreadRadius), 0f,
                    Random.Range(-currEffect.randomSpreadRadius, currEffect.randomSpreadRadius));
            }
        }
        if (currEffect.muzzleFlashPrefab != null)
        {

            tempTransform = Instantiate(currEffect.muzzleFlashPrefab, projectileSpawnPoint.position, Quaternion.identity).transform;
            tempTransform.localRotation = Quaternion.identity;
            tempTransform.forward = projectileSpawnPoint.forward;
            tempTransform.parent = transform;
            tempTransform.localScale *= currEffect.scaleMultiplier;
        }
        tempTransform.localScale *= currEffect.scaleMultiplier;
        tempTransform.position += currEffect.positionOffset;

        if (!isAfterSetupAndInstantiateEffect && currEffect.doCameraShake) AllIn1Shaker.i.DoCameraShake(currEffect.mainEffectShakeAmount);

        //currentEffectPlays++;
    }

    private void SetupAndInstantiateCurrentEffect(All1VfxDemoEffect currEffect)
    {
        DestroyAllChildren();
        // currentEffectPlays = 0;
        //ComputeValidEffectAndCollectionIndex();
        projectileSceneSetupObject.SetActive(currEffect.isShootProjectile);

        PlayCurrentEffect(currEffect, true);
    }

    /*private void ComputeValidEffectAndCollectionIndex()
    {
        int demoEffectOperation = 0; // 0 means no operation, 1 means assign first collection effect, 2 means last collection effect
        if(currDemoEffectIndex < 0)
        {
            currDemoCollectionIndex--;
            demoEffectOperation = 2;
        }
        else if(currDemoEffectIndex >= effectsToSpawnCollections[currDemoCollectionIndex].demoEffectCollection.Length)
        {
            currDemoCollectionIndex++;
            demoEffectOperation = 1;
        }

        if(currDemoCollectionIndex < 0)
        {
            currDemoCollectionIndex = effectsToSpawnCollections.Length - 1;
            demoEffectOperation = 2;
        }
        else if(currDemoCollectionIndex >= effectsToSpawnCollections.Length)
        {
            currDemoCollectionIndex = 0;
            demoEffectOperation = 1;
        }

        if(demoEffectOperation > 0)
        {
            if(demoEffectOperation == 1) currDemoEffectIndex = 0;
            else if(demoEffectOperation == 2) currDemoEffectIndex = effectsToSpawnCollections[currDemoCollectionIndex].demoEffectCollection.Length - 1;
        }
    }*/

    /*private IEnumerator CurrentEffectLabelTweenEffectCR()
    {
        //This mess of a function prevents the Best Fit on currentEffectLabel to scale in a weird way during 1 frame
        Color startColor = currentEffectLabel.color;
        currLabelTween.ScaleDownTween();
        currentEffectLabel.color = new Color(startColor.r, startColor.g, startColor.b, 0f);
        yield return null;
        currentEffectLabel.color = new Color(startColor.r, startColor.g, startColor.b, 1f);
    }*/

    private void DestroyAllChildren()
    {
        foreach (Transform child in transform) Destroy(child.gameObject);
    }
}