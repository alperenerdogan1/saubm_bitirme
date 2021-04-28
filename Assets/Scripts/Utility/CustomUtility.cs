using UnityEngine;

public static class CustomUtility
{
    public static void ChangeAlpha(Material material, float alphaVal)
    {
        Color oldColor = material.color;
        material.color = new Color(oldColor.r, oldColor.g, oldColor.b, alphaVal);
    }
    public static void ChangeLocalScale(GameObject gameObject, Vector3 appliedScale)
    {
        Vector3 tempScale = gameObject.transform.localScale;
        tempScale.x = appliedScale.x;
        tempScale.y = appliedScale.y;
        tempScale.z = appliedScale.z;
        gameObject.transform.localScale = tempScale;
    }
}
