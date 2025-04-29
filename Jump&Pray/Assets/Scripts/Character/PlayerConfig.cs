public static class PlayerConfig
{
    private static float padSensitivity = 10.0f;
    private static float mouseSensitivity = 10.0f;


    public static float GetPadSensitivity()
    {
        return padSensitivity;
    }

    public static float GetMouseSensitivity()
    {
        return mouseSensitivity;
    }

    public static void SetPadSensitivity(float value)
    {
        padSensitivity = value;
    }

    public static void SetMouseSensitivity(float value)
    {
        mouseSensitivity = value;
    }
}
