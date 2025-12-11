using Cinemachine;
using StarterAssets;
using TMPro;
using UnityEngine;

/// <summary>
/// 射撃・ズーム・UI 更新など、アクティブな武器の状態と挙動を管理するクラス
/// </summary>
[RequireComponent(typeof(Animator))]
public class ActiveWeapon : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] WeaponSO startingWeaponSO;
    [SerializeField] CinemachineVirtualCamera playerFollowCamera;
    [SerializeField] Camera weaponCamera;
    [SerializeField] GameObject zoomVignette;
    [SerializeField] TMP_Text ammoText;
    #endregion

    #region Private Fields
    private WeaponSO _currentWeaponSO;
    private Animator _animator;
    private StarterAssetsInputs _starterAssetsInputs;
    private FirstPersonController _firstPersonController;
    private Weapon _currentWeapon;
    private GameManager _gameManager;

    private float _timeSinceLastShot = 0f;
    private float _defaultFOV;
    private float _defaultRotationSpeed;
    private int _currentAmmo;
    #endregion

    const string SHOOT_STRING = "Shoot";

    void OnEnable()
    {
        StarterAssetsInputs.OnShootEvent += HandleShoot;
        StarterAssetsInputs.OnZoomEvent += HandleZoom;
    }

    void OnDisable()
    {
        StarterAssetsInputs.OnShootEvent -= HandleShoot;
        StarterAssetsInputs.OnZoomEvent -= HandleZoom;
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    void Start()
    {
        // initialize
        _gameManager = GameManager.Instance;
        _starterAssetsInputs = GetComponentInParent<StarterAssetsInputs>();
        _firstPersonController = GetComponentInParent<FirstPersonController>();

        _defaultFOV = playerFollowCamera.m_Lens.FieldOfView;
        _defaultRotationSpeed = _firstPersonController.RotationSpeed;

        // 初期武器をセットアップする
        SwitchWeapon(startingWeaponSO);
        // AdjustAmmo(_currentWeaponSO.magazineSize);
    }

    void Update()
    {
        _timeSinceLastShot += Time.deltaTime;
    }

    public void AdjustAmmo(int amount)
    {
        _currentAmmo += amount;

        if (_currentAmmo > _currentWeaponSO.MagazineSize) _currentAmmo = _currentWeaponSO.MagazineSize;

        ammoText.text = _currentAmmo.ToString("D2");
    }

    void HandleShoot(bool shoot)
    {
        if (!shoot || _gameManager.MenuActive) return;

        // もし発射間隔を過ぎたら発射
        if (_timeSinceLastShot >= _currentWeaponSO.FireRate && _currentAmmo > 0)
        {
            _currentWeapon.Shoot(_currentWeaponSO);
            _animator.Play(SHOOT_STRING, 0, 0f);
            _timeSinceLastShot = 0f;
            if (!UnlimitedBulletsManager.Instance.UnlimitedBullets) AdjustAmmo(-1);
        }

        if (!_currentWeaponSO.IsAutomatic)
        {
            _starterAssetsInputs.ShootInput(false);
        }
    }

    public void SwitchWeapon(WeaponSO weaponSO)
    {
        if (_currentWeapon) Destroy(_currentWeapon.gameObject);

        Weapon newWeapon = Instantiate(weaponSO.WeaponPrefab, transform).GetComponent<Weapon>();
        _currentWeapon = newWeapon;
        _currentWeaponSO = weaponSO;

        AdjustAmmo(_currentWeaponSO.MagazineSize);
    }

    void HandleZoom(bool zoom)
    {
        if (!_currentWeaponSO.CanZoom || _gameManager.MenuActive) return;

        if (zoom)
        {
            zoomVignette.SetActive(true);
            playerFollowCamera.m_Lens.FieldOfView = _currentWeaponSO.ZoomAmount;
            weaponCamera.fieldOfView = _currentWeaponSO.ZoomAmount;
            _firstPersonController.ChangeRotationSpeed(_currentWeaponSO.ZoomRotationSpeed);
        }
        else
        {
            zoomVignette.SetActive(false);
            playerFollowCamera.m_Lens.FieldOfView = _defaultFOV;
            weaponCamera.fieldOfView = _defaultFOV;
            _firstPersonController.ChangeRotationSpeed(_defaultRotationSpeed);
        }
    }
}
