using UnityEngine;
using UnityEngine.UI; // Si estás usando UI con botones

public class MenuNavigator : MonoBehaviour
{
    public Button[] menuButtons; // Lista de botones en el menú
    private int currentSelection = 0; // Índice de la opción seleccionada

    public float navigationDelay = 0.2f; // Retardo entre cambios de selección
    private float lastNavigationTime = 0f; // Para prevenir cambios de selección rápidos

    private void Start()
    {
        // Asegúrate de que haya al menos un botón en el menú
        if (menuButtons.Length > 0)
        {
            // Resalta la primera opción
            menuButtons[currentSelection].Select();
        }
    }

    private void Update()
    {
        // Verifica si ha pasado suficiente tiempo para cambiar la selección
        if (Time.time - lastNavigationTime > navigationDelay)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow)) // Navegar hacia arriba
            {
                NavigateUp();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow)) // Navegar hacia abajo
            {
                NavigateDown();
            }
            else if (Input.GetKeyDown(KeyCode.Return)) // Confirmar selección
            {
                SelectOption();
            }
        }
    }

    private void NavigateUp()
    {
        currentSelection = Mathf.Max(currentSelection - 1, 0); // Asegura que no se salga del rango
        UpdateSelection();
        lastNavigationTime = Time.time; // Reinicia el temporizador de navegación
    }

    private void NavigateDown()
    {
        currentSelection = Mathf.Min(currentSelection + 1, menuButtons.Length - 1); // Asegura que no se salga del rango
        UpdateSelection();
        lastNavigationTime = Time.time; // Reinicia el temporizador de navegación
    }

    private void UpdateSelection()
    {
        // Resalta el botón correspondiente a la opción seleccionada
        menuButtons[currentSelection].Select();
    }

    private void SelectOption()
    {
        // Aquí puedes agregar lo que suceda al seleccionar una opción
        menuButtons[currentSelection].onClick.Invoke(); // Llama al evento de click del botón seleccionado
    }
}
