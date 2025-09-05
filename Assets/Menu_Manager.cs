using UnityEngine;
using UnityEngine.UI; // Si est�s usando UI con botones

public class MenuNavigator : MonoBehaviour
{
    public Button[] menuButtons; // Lista de botones en el men�
    private int currentSelection = 0; // �ndice de la opci�n seleccionada

    public float navigationDelay = 0.2f; // Retardo entre cambios de selecci�n
    private float lastNavigationTime = 0f; // Para prevenir cambios de selecci�n r�pidos

    private void Start()
    {
        // Aseg�rate de que haya al menos un bot�n en el men�
        if (menuButtons.Length > 0)
        {
            // Resalta la primera opci�n
            menuButtons[currentSelection].Select();
        }
    }

    private void Update()
    {
        // Verifica si ha pasado suficiente tiempo para cambiar la selecci�n
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
            else if (Input.GetKeyDown(KeyCode.Return)) // Confirmar selecci�n
            {
                SelectOption();
            }
        }
    }

    private void NavigateUp()
    {
        currentSelection = Mathf.Max(currentSelection - 1, 0); // Asegura que no se salga del rango
        UpdateSelection();
        lastNavigationTime = Time.time; // Reinicia el temporizador de navegaci�n
    }

    private void NavigateDown()
    {
        currentSelection = Mathf.Min(currentSelection + 1, menuButtons.Length - 1); // Asegura que no se salga del rango
        UpdateSelection();
        lastNavigationTime = Time.time; // Reinicia el temporizador de navegaci�n
    }

    private void UpdateSelection()
    {
        // Resalta el bot�n correspondiente a la opci�n seleccionada
        menuButtons[currentSelection].Select();
    }

    private void SelectOption()
    {
        // Aqu� puedes agregar lo que suceda al seleccionar una opci�n
        menuButtons[currentSelection].onClick.Invoke(); // Llama al evento de click del bot�n seleccionado
    }
}
