using UnityEngine;
using UnityEngine.UI;

public class MPTWeightButton : MonoBehaviour
{
    public Text shapeName;
    public InputField inputField;

    public void SetName(string newName)
    {
        shapeName.text = newName;
    }

    public void SetWeight(float weight)
    {
        inputField.text = "" + weight;
    }

    public float GetWeight()
    {
        float res = float.Parse(inputField.text);
        return res;
    }
}
