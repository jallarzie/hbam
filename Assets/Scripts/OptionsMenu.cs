using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum Speed
{
    Lethargic,
    Slow,
    Normal,
    Fast,
    Extreme
}

public class OptionsMenu : MonoBehaviour {

    [SerializeField]
    private GameObject _credits;

    [SerializeField]
    private GameObject _settings;

    [SerializeField]
    private Slider _volume;

    [SerializeField]
    private Button _speedUp;
    [SerializeField]
    private Button _speedDown;
    [SerializeField]
    private Text _speedText;

    [SerializeField]
    private Button _amountUp;
    [SerializeField]
    private Button _amountDown;
    [SerializeField]
    private Text _amountText;

    [SerializeField]
    private int _minAmount;
    [SerializeField]
    private int _maxAmount;

    private Speed _currentSpeed;
    private int _currentAmount;

    void Start ()
    {
        _volume.value = PlayerPrefs.GetFloat("volume", 1.0f);
        _currentSpeed = (Speed)PlayerPrefs.GetInt("speed", 2);
        _currentAmount = PlayerPrefs.GetInt("amount", _minAmount);

        _speedText.text = _currentSpeed.ToString();
        _amountText.text = _currentAmount.ToString();

        _speedUp.interactable = _currentSpeed != Speed.Extreme;
        _speedDown.interactable = _currentSpeed != Speed.Lethargic;

        _amountDown.interactable = _currentAmount > _minAmount;
        _amountUp.interactable = _currentAmount < _maxAmount;
    }

    public void OnVolumeChanged(float value)
    {
        PlayerPrefs.SetFloat("volume", value);
        PlayerPrefs.Save();
    }

    public void OnSpeedUp()
    {
        _currentSpeed++;

        _speedUp.interactable = _currentSpeed != Speed.Extreme;
        _speedDown.interactable = _currentSpeed != Speed.Lethargic;

        PlayerPrefs.SetInt("speed", (int)_currentSpeed);
        PlayerPrefs.Save();
        _speedText.text = _currentSpeed.ToString();
    }

    public void OnSpeedDown()
    {
        _currentSpeed--;

        _speedUp.interactable = _currentSpeed != Speed.Extreme;
        _speedDown.interactable = _currentSpeed != Speed.Lethargic;

        PlayerPrefs.SetInt("speed", (int)_currentSpeed);
        PlayerPrefs.Save();
        _speedText.text = _currentSpeed.ToString();
    }

    public void OnAmountUp()
    {
        _currentAmount++;

        _amountDown.interactable = _currentAmount > _minAmount;
        _amountUp.interactable = _currentAmount < _maxAmount;

        PlayerPrefs.SetInt("amount", _currentAmount);
        PlayerPrefs.Save();
        _amountText.text = _currentAmount.ToString();
    }

    public void OnAmountDown()
    {
        _currentAmount--;

        _amountDown.interactable = _currentAmount > _minAmount;
        _amountUp.interactable = _currentAmount < _maxAmount;

        PlayerPrefs.SetInt("amount", _currentAmount);
        PlayerPrefs.Save();
        _amountText.text = _currentAmount.ToString();
    }

    public void ShowCredits()
    {
        _settings.SetActive(false);
        _credits.SetActive(true);   
    }

    public void ShowSettings()
    {
        _settings.SetActive(true);
        _credits.SetActive(false);
    }
}
