using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogicaCalidad : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown;
    public int calidad;
    [SerializeField] private GameObject filtroBorroso; // Imagen UI para simular el efecto feo

    // flags para no spamear warnings
    private bool _warnedNoDropdown, _warnedNoFiltro;

    void Awake()
    {
        // Autowire defensivo: intenta encontrar el TMP_Dropdown en este GO o hijos (incluye inactivos)
        if (!dropdown)
        {
            dropdown = GetComponentInChildren<TMP_Dropdown>(true);
            if (!dropdown && !_warnedNoDropdown)
            {
                _warnedNoDropdown = true;
                Debug.LogWarning("[LogicaCalidad] Falta TMP_Dropdown. Asignalo en el Inspector o dejá el valor de PlayerPrefs.", this);
            }
        }

        if (!filtroBorroso && !_warnedNoFiltro)
        {
            _warnedNoFiltro = true;
            // No forzamos búsqueda agresiva para no enganchar el GO equivocado; solo avisamos.
            Debug.LogWarning("[LogicaCalidad] Falta referencia a 'filtroBorroso'. No se activará el efecto visual de baja calidad.", this);
        }
    }

    void Start()
    {
        // Si no existe la key, PlayerPrefs devuelve el valor por defecto que le pases (2)
        calidad = PlayerPrefs.GetInt("numeroDeCalidad", 2);  // Dato seguro. :contentReference[oaicite:2]{index=2}

        // Si hay dropdown, reflejá el valor guardado; acotá al rango de niveles disponibles
        if (dropdown)
        {
            int maxIndex = Mathf.Max(0, QualitySettings.names.Length - 1); // nombres de niveles de calidad configurados. :contentReference[oaicite:3]{index=3}
            dropdown.value = Mathf.Clamp(calidad, 0, maxIndex);
        }

        AjustarCalidad();
    }

    public void AjustarCalidad()
    {
        // Usa el value del dropdown si existe; si no, cae al int 'calidad'
        int maxIndex = Mathf.Max(0, QualitySettings.names.Length - 1);        // :contentReference[oaicite:4]{index=4}
        int level = dropdown ? dropdown.value : Mathf.Clamp(calidad, 0, maxIndex);

        // Cambiar el nivel de calidad en tiempo de ejecución es soportado por la API
        QualitySettings.SetQualityLevel(level);                                // :contentReference[oaicite:5]{index=5}

        PlayerPrefs.SetInt("numeroDeCalidad", level);
        calidad = level;

        switch (calidad)
        {
            case 0: // LOW
                QualitySettings.antiAliasing = 0;
                Screen.SetResolution(Screen.width / 4, Screen.height / 4, true); // Si la resolución no existe, Unity elige la más cercana. :contentReference[oaicite:6]{index=6}
                QualitySettings.shadows = ShadowQuality.Disable;
                QualitySettings.globalTextureMipmapLimit = 2;
                QualitySettings.lodBias = 0.3f;
                if (filtroBorroso) filtroBorroso.SetActive(true);
                break;

            case 1: // MEDIUM
                QualitySettings.antiAliasing = 2;
                Screen.SetResolution(Screen.width, Screen.height, true);        // Cambiar resolución está soportado; usa la más cercana si no existe. :contentReference[oaicite:7]{index=7}
                QualitySettings.shadows = ShadowQuality.HardOnly;
                QualitySettings.globalTextureMipmapLimit = 1;
                QualitySettings.lodBias = 0.7f;
                if (filtroBorroso) filtroBorroso.SetActive(false);
                break;

            case 2: // ULTRA
            default:
                QualitySettings.antiAliasing = 8;
                Screen.SetResolution(Screen.width * 2, Screen.height * 2, true); // Unity ajusta a la más cercana soportada. :contentReference[oaicite:8]{index=8}
                QualitySettings.shadows = ShadowQuality.All;
                QualitySettings.globalTextureMipmapLimit = 0;
                QualitySettings.lodBias = 1.5f;
                if (filtroBorroso) filtroBorroso.SetActive(false);
                break;
        }
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        if (!dropdown) dropdown = GetComponentInChildren<TMP_Dropdown>(true);   // TMP_Dropdown pertenece a TextMeshPro. :contentReference[oaicite:9]{index=9}
    }
#endif
}
