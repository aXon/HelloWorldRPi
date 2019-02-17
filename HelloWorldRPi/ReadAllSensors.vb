''
'
'  The original C# file was part of Rpi.SenseHat.Demo https://github.com/emmellsoft/RPi.SenseHat
'  Copyright (c) 2017, Mattias Larsson

'  Permission Is hereby granted, free of charge, to any person obtaining a copy of 
'  this software And associated documentation files (the "Software"), to deal in 
'  the Software without restriction, including without limitation the rights to use, 
'  copy, modify, merge, publish, distribute, sublicense, And/Or sell copies of the 
'  Software, And to permit persons to whom the Software Is furnished to do so, 
'  subject to the following conditions:
'
'  The above copyright notice And this permission notice shall be included in all 
'  copies Or substantial portions of the Software.
'
'  THE SOFTWARE Is PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS Or IMPLIED, 
'  INCLUDING BUT Not LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A 
'  PARTICULAR PURPOSE And NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS Or COPYRIGHT 
'  HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES Or OTHER LIABILITY, WHETHER IN AN ACTION 
'  OF CONTRACT, TORT Or OTHERWISE, ARISING FROM, OUT OF Or IN CONNECTION WITH THE 
'  SOFTWARE Or THE USE Or OTHER DEALINGS IN THE SOFTWARE.
'
'  The current version is the Visual Basic equivalent
'  Copyright (c) 2019. Nils Bausch
'


Imports System
Imports System.Diagnostics
Imports System.Text
Imports Emmellsoft.IoT.Rpi.SenseHat


Public NotInheritable Class ReadAllSensors
        Inherits SenseHatDemo

    Public Sub New(ByVal senseHat As ISenseHat, Optional ByVal setScreenText As Action(Of String) = Nothing)
        MyBase.New(senseHat)
    End Sub

    Public Overrides Sub Run()
            Dim mainPageUpdateRate As TimeSpan = TimeSpan.FromSeconds(0.5)
            Dim nextMainPageUpdate As DateTime = DateTime.Now.Add(mainPageUpdateRate)
            Dim stringBuilder = New StringBuilder()

            While True
                Sleep(TimeSpan.FromMilliseconds(50))
                senseHat.Sensors.ImuSensor.Update()
                senseHat.Sensors.PressureSensor.Update()
                senseHat.Sensors.HumiditySensor.Update()
                stringBuilder.Clear()
                stringBuilder.AppendLine($"Gyro: {If(senseHat.Sensors.Gyro?.ToString(False), "N/A")}")
                stringBuilder.AppendLine($"Accel: {If(senseHat.Sensors.Acceleration?.ToString(False), "N/A")}")
                stringBuilder.AppendLine($"Mag: {If(senseHat.Sensors.MagneticField?.ToString(False), "N/A")}")
                stringBuilder.AppendLine($"Pose: {If(senseHat.Sensors.Pose?.ToString(False), "N/A")}")
                stringBuilder.AppendLine($"Press: {If(senseHat.Sensors.Pressure?.ToString(), "N/A")}")
                stringBuilder.AppendLine($"Temp: {If(senseHat.Sensors.Temperature?.ToString(), "N/A")}")
                stringBuilder.AppendLine($"Hum: {If(senseHat.Sensors.Humidity?.ToString(), "N/A")}")

            'If (SetScreenText IsNot Nothing) AndAlso nextMainPageUpdate <= DateTime.Now Then
            '    SetScreenText(stringBuilder.ToString())
            '    nextMainPageUpdate = DateTime.Now.Add(mainPageUpdateRate)
            'End If

            Debug.WriteLine(stringBuilder.ToString())
            End While
        End Sub
    End Class


