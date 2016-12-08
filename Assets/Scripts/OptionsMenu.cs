using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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
	private Text _soundOn;
	[SerializeField]
	private Text _soundOff;
	[SerializeField]
	private Color _soundColorOn;
	[SerializeField]
	private Color _soundColorOff;

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

	[SerializeField]
	private GameObject splash1;
	[SerializeField]
	private GameObject splash2;

    private Speed _currentSpeed;
    private int _currentAmount;
	private bool _currentSound;

    void Start ()
    {
        _currentSpeed = (Speed)PlayerPrefs.GetInt("speed", 2);
        _currentAmount = PlayerPrefs.GetInt("amount", _minAmount);
        _currentSound = PlayerPrefs.GetInt("sound", 1) == 1;

        _speedText.text = _currentSpeed.ToString();
        _amountText.text = _currentAmount.ToString();

        _speedUp.interactable = _currentSpeed != Speed.Extreme;
        _speedDown.interactable = _currentSpeed != Speed.Lethargic;

        _amountDown.interactable = _currentAmount > _minAmount;
        _amountUp.interactable = _currentAmount < _maxAmount;

        _soundOn.color = _currentSound ? _soundColorOn : _soundColorOff;
        _soundOff.color = !_currentSound ? _soundColorOn : _soundColorOff;
    }

	public void ShowSplash1()
	{
		splash1.SetActive(true);
		_settings.SetActive (false);
	}

	public void ShowSplash2()
	{
		splash1.SetActive(false);
		splash2.SetActive(true);
	}

    public void OnVolumeChanged(bool value)
    {
        _currentSound = value;
        _soundOn.color = _currentSound ? _soundColorOn : _soundColorOff;
        _soundOff.color = !_currentSound ? _soundColorOn : _soundColorOff;

        SoundManager.instance.SetSound(_currentSound);
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
		splash1.SetActive(false);
		splash2.SetActive(false);
    }

	public void LoadScene(string loadedScene){
		SceneManager.LoadScene (loadedScene, LoadSceneMode.Single);
	}
}
