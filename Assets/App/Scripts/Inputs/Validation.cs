using UnityEngine;
using TMPro;

namespace DavsInputValidation
{
    public static class Validation 
    {
        public static void ValidateField(TMP_Text input, int fieldSize, Color invalidColor, Color validColor)
        {
            Debug.Log("field size :"+ input.text.Length);
            if(input.text.Length!=fieldSize)
            {
                input.color = invalidColor;
            }
            else
            {
                input.color = validColor;
            }
        }
    }
}



