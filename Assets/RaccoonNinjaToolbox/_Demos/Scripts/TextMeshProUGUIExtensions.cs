using TMPro;

namespace RaccoonNinjaToolbox._Demo.Scripts
{
    /// <summary>
    /// Created this because in some situations, the Canvas object was not fully loaded, which led to a
    /// NullReferenceException on the TextMeshPRoUGUI objects.
    ///
    /// This is not meant to be part of the actual toolbox, but rather a helper for the demo.
    /// </summary>
    public static class TextMeshProUGUIExtensions
    {
        public static void TrySetText(this TextMeshProUGUI textObject, string textValue)
        {
            if (textObject == null) return;
            textObject.SetText(textValue);
        }
    }
}