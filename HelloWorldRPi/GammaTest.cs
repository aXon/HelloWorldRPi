
private void GenerateTestPattern(ColorComponents colorComponents)
{
    bool showRed = colorComponents.HasFlag(ColorComponents.Red);
    bool showGreen = colorComponents.HasFlag(ColorComponents.Green);
    bool showBlue = colorComponents.HasFlag(ColorComponents.Blue);

    byte[] intensities = Enumerable.Range(0, 32).Select(() >= new[] { }, System.Convert.ToByte((x + 3))).ToArray;


    Func<byte, Color> getColor = intensity => Color.FromArgb(255, showRed ? intensity : System.Convert.ToByte(0), showGreen ? intensity : System.Convert.ToByte(0), showBlue ? intensity : System.Convert.ToByte(0));

    for (int x = 0; x <= 8 - 1; x++)
    {
        senseHat.Display.Screen[x, 0] = getColor(intensities[x]);
        senseHat.Display.Screen[x, 1] = getColor(intensities[15 - x]);
        senseHat.Display.Screen[x, 2] = getColor(intensities[16 + x]);
        senseHat.Display.Screen[x, 3] = getColor(intensities[31 - x]);
        senseHat.Display.Screen[x, 4] = getColor(intensities[31 - x]);
        senseHat.Display.Screen[x, 5] = getColor(intensities[16 + x]);
        senseHat.Display.Screen[x, 6] = getColor(intensities[15 - x]);
        senseHat.Display.Screen[x, 7] = getColor(intensities[x]);
    }
}
