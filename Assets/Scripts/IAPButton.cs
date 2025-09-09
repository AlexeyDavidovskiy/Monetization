using UnityEngine;
using UnityEngine.UI;

public class IAPButton : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            IAPController.Instance?.BuyCoins();
        });
    }
}
