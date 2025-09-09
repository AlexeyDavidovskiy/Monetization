using System;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPController : MonoBehaviour, IStoreListener
{
    public static IAPController Instance { get; private set; }

    private IStoreController storeController;
    private IExtensionProvider extensionProvider;

    public const string COIN_PACK_100 = "coin_pack_100";

    private void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); return; }
    }

    private void Start()
    {
        if (!IsInitialized())
        {
            InitializePurchasing();
        }
    }

    private bool IsInitialized()
    {
        return storeController != null && extensionProvider != null;
    }

    public void InitializePurchasing()
    {
        if (IsInitialized()) return;

        var module = StandardPurchasingModule.Instance();
        module.useFakeStoreUIMode = FakeStoreUIMode.StandardUser;

        var builder = ConfigurationBuilder.Instance(module);
        builder.AddProduct(COIN_PACK_100, ProductType.Consumable);

        UnityPurchasing.Initialize(this, builder);
    }

    public void BuyCoins()
    {
        if (!IsInitialized())
        {
            Debug.LogWarning("IAP не инициализирован.");
            return;
        }

        var product = storeController.products.WithID(COIN_PACK_100);
        if (product != null && product.availableToPurchase)
        {
            Debug.Log($"Покупка продукта: {product.definition.id}");
            storeController.InitiatePurchase(product);
        }
        else
        {
            Debug.LogError("Продукт недоступен для покупки.");
        }
    }

    private void GrantCoins(int amount)
    {
        int coins = PlayerPrefs.GetInt("coins", 0);
        coins += amount;
        PlayerPrefs.SetInt("coins", coins);
        PlayerPrefs.Save();
        Debug.Log($"Вы получили {amount} монет. Всего: {coins}");
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("IAP инициализирован");
        storeController = controller;
        extensionProvider = extensions;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.LogError($"IAP инициализация не удалась (старый метод): {error}");
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.LogError($"IAP инициализация не удалась: {error}, сообщение: {message}");
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        if (String.Equals(args.purchasedProduct.definition.id, COIN_PACK_100, StringComparison.Ordinal))
        {
            GrantCoins(100);
        }
        else
        {
            Debug.LogWarning($"Неизвестный продукт: {args.purchasedProduct.definition.id}");
        }

        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.LogError($"Покупка не удалась: {product.definition.id}, причина: {failureReason}");
    }
}