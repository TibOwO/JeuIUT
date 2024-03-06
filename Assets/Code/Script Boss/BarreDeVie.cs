using UnityEngine;
using UnityEngine.UI;

namespace MagicPigGames
{
    public class BarreDeVie : MonoBehaviour
    {
        public Slider slider; // Référence au Slider de la barre de vie

        // Assurez-vous que le Slider est correctement initialisé dans l'inspecteur Unity

        // Set le niveau de progression de la barre (0.0 à 1.0)
        public void SetProgress(float progress)
        {
            progress = Mathf.Clamp01(progress); // Assurez-vous que le progrès est entre 0 et 1

            if (slider != null)
            {
                slider.value = progress; // Définir la valeur du Slider en fonction du progrès
            }
            else
            {
                Debug.LogError("Référence au Slider non définie pour BarreDeVie.");
            }
        }
    }
}
