using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class RewardedAdsButton : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] private Button showAdButton;
    [SerializeField] private string androidAdUnitId = "Rewarded_Android";
    [SerializeField] private string iOSAd = "Rewarded_iOS";

    string adUnitId = null;

    private void Awake()
    {
#if UNITY_IOS
adUnitId = iOSAdUnitId;
#elif UNITY_ANDROID
adUnitId = androidAdUnitId;
#endif
        showAdButton.interactable = false;
    }

    public void LoadAd()
    {
        Debug.Log("Loading Ad:" + adUnitId);
        Advertisement.Load(adUnitId, this);
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log("Ad Loaded:" + placementId);

        if (placementId.Equals(adUnitId))
        {
            showAdButton.onClick.AddListener(ShowAd);
            showAdButton.interactable = true;
        }
    }

    public void ShowAd()
    {
        showAdButton.interactable = false;
        Advertisement.Show(adUnitId, this);
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if (placementId.Equals(adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            Debug.Log("Unity Ads Rewarded Ad Completed");
        }
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit {adUnitId}: {error.ToString()} - {message}");
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
    }
    public void OnUnityAdsShowClick(string placementId) { }

    public void OnUnityAdsShowStart(string placementId) { }
}
