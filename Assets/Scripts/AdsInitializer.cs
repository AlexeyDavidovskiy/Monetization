using UnityEngine;
using UnityEngine.Advertisements;

public class AdsInitializer : MonoBehaviour, IUnityAdsInitializationListener
{
    [SerializeField] private string androidGameId;
    [SerializeField] private string iOSGameId;
    [SerializeField] private bool testMode = true;

    private string gameId;

    public void InitializeAds()
    {
#if UNITY_IOS
gameId = iOSGameId;
#elif UNITY_ANDROID
gameId = AndroidGameId;
#elif UNITY_EDITOR
gameId = androidGameId;
#endif

        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(gameId, testMode, this);
        }

    }

    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }
}
