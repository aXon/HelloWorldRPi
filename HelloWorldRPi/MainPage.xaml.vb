Imports System
Imports Emmellsoft.IoT.Rpi.SenseHat
Imports Emmellsoft.IoT.Rpi.SenseHat.Fonts.SingleColor
Imports Windows.Devices.Gpio
Imports Windows.UI


' The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

''' <summary>
''' An empty page that can be used on its own or navigated to within a Frame.
''' </summary>
Public NotInheritable Class MainPage
    Inherits Page

    Private hat As ISenseHat

    Private Async Sub MainPage_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        ' initialize the SenseHat object
        hat = Await SenseHatFactory.GetSenseHat.ConfigureAwait(False)
        Dim tinyFont As New TinyFont ' used for the display
        Dim display As ISenseHatDisplay = hat.Display ' SenseHat display object
        ' infinite loop
        While True
            ' force the SenseHat to read a new value from it's temp sensor
            hat.Sensors.HumiditySensor.Update()
            ' see is value is available, if not wait
            If (hat.Sensors.Temperature.HasValue) Then
                ' convert reading to integer
                Dim temperature As Integer = CInt(Math.Round(hat.Sensors.Temperature.Value))
                Dim Text As String = temperature.ToString()
                ' the display can only show 2 digits, thus show stars is greater
                If (Text.Length > 2) Then
                    Text = "**"
                End If
                display.Clear()
                tinyFont.Write(display, Text, Colors.White)
                display.Update()
                ' Sleep for a bit; temperatures change slowly
                Await Task.Delay(TimeSpan.FromSeconds(0.5))
            Else
                'Rapid update until there is a temperature reading
                Await Task.Delay(TimeSpan.FromSeconds(0.5))
            End If
        End While
    End Sub
End Class
