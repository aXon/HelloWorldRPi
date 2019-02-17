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
Imports Windows.UI
Imports Emmellsoft.IoT.Rpi.SenseHat
Imports RichardsTech.Sensors


Public NotInheritable Class GravityBlob
        Inherits SenseHatDemo

    Public Sub New(ByVal senseHat As ISenseHat, Optional ByVal setScreenText As Action(Of String) = Nothing)
        MyBase.New(senseHat)
    End Sub

    Public Overrides Sub Run()
            Dim mainPageUpdateRate As TimeSpan = TimeSpan.FromSeconds(0.5)
            Dim nextMainPageUpdate As DateTime = DateTime.Now.Add(mainPageUpdateRate)

            While True
                Sleep(TimeSpan.FromMilliseconds(50))

                If Not senseHat.Sensors.ImuSensor.Update() Then
                    Continue While
                End If

                If Not senseHat.Sensors.Acceleration.HasValue Then
                    Continue While
                End If

                Dim colors As Color(,) = CreateGravityBlobScreen(senseHat.Sensors.Acceleration.Value)
                senseHat.Display.CopyColorsToScreen(colors)
                senseHat.Display.Update()

            'If (SetScreenText IsNot Nothing) AndAlso nextMainPageUpdate <= DateTime.Now Then
            'SetScreenText($"{senseHat.Sensors.Acceleration.Value.X}, {senseHat.Sensors.Acceleration.Value.Y}, {senseHat.Sensors.Acceleration.Value.Z}")
            'nextMainPageUpdate = DateTime.Now.Add(mainPageUpdateRate)
            'End If
        End While
        End Sub

        Private Shared Function CreateGravityBlobScreen(ByVal vector As Vector3) As Color(,)
            Dim x0 As Double = (vector.X + 1) * 5.5 - 2
            Dim y0 As Double = (vector.Y + 1) * 5.5 - 2
            Dim distScale As Double = 4
            Dim colors = New Color(7, 7) {}
            Dim isUpsideDown As Boolean = vector.Z < 0

            For y As Integer = 0 To 8 - 1

                For x As Integer = 0 To 8 - 1
                    Dim dx As Double = x0 - x
                    Dim dy As Double = y0 - y
                    Dim dist As Double = Math.Sqrt(dx * dx + dy * dy) / distScale

                    If dist > 1 Then
                        dist = 1
                    End If

                    Dim colorIntensity As Integer = CInt(Math.Round(255 * (1 - dist)))

                    If colorIntensity > 255 Then
                        colorIntensity = 255
                    End If

                    colors(x, y) = If(isUpsideDown, Color.FromArgb(255, CByte(colorIntensity), 0, 0), Color.FromArgb(255, 0, CByte(colorIntensity), 0))
                Next
            Next

            Return colors
        End Function
    End Class


