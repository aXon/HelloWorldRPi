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

Public Class DiscoLights
        Inherits SenseHatDemo

        Private Shared ReadOnly Random As Random = New Random()
        Private _currentMode As ColorMode

        Private Enum ColorMode
            SoftRandom
            HardRandom
            Sparkling
            Blocks
            Unicolor
        End Enum

        Public Sub New(ByVal senseHat As ISenseHat)
            MyBase.New(senseHat)
        End Sub

        Public Overrides Sub Run()
            While True
                FillDisplay()
                senseHat.Display.Update()

                If senseHat.Joystick.Update() AndAlso (senseHat.Joystick.EnterKey = KeyState.Pressing) Then
                    SwitchToNextColorMode()
                End If

                Sleep(TimeSpan.FromMilliseconds(50))
            End While
        End Sub

        Private Sub SwitchToNextColorMode()
            _currentMode += 1

            If _currentMode > ColorMode.Unicolor Then
                _currentMode = ColorMode.SoftRandom
            End If

            senseHat.Display.Clear()
        End Sub

        Private Sub FillDisplay()
            Select Case _currentMode
                Case ColorMode.SoftRandom
                    FillDisplaySoftRandom()
                Case ColorMode.HardRandom
                    FillDisplayHardRandom()
                Case ColorMode.Sparkling
                    FillDisplaySparkling()
                Case ColorMode.Blocks
                    FillDisplayBlocks()
                Case ColorMode.Unicolor
                    FillDisplayUnicolor()
                Case Else
                    Throw New ArgumentOutOfRangeException()
            End Select
        End Sub

        Private Sub FillDisplaySoftRandom()
            For y As Integer = 0 To 8 - 1

                For x As Integer = 0 To 8 - 1
                    Dim pixel As Color = Color.FromArgb(255, CByte(Random.[Next](256)), CByte(Random.[Next](256)), CByte(Random.[Next](256)))
                    senseHat.Display.Screen(x, y) = pixel
                Next
            Next
        End Sub

        Private Sub FillDisplayHardRandom()
            For y As Integer = 0 To 8 - 1

                For x As Integer = 0 To 8 - 1
                    Dim pixel As Color = Color.FromArgb(255, CByte((Random.[Next](2) * 255)), CByte((Random.[Next](2) * 255)), CByte((Random.[Next](2) * 255)))
                    senseHat.Display.Screen(x, y) = pixel
                Next
            Next
        End Sub

        Private Sub FillDisplaySparkling()
            Const probabilityForNewSparkle As Double = 0.99
            Const oldSparkleFadeRate As Double = 0.75

            For y As Integer = 0 To 8 - 1

                For x As Integer = 0 To 8 - 1
                    Dim sparkle As Boolean = Random.NextDouble() >= probabilityForNewSparkle
                    Dim pixel As Color

                    If sparkle Then
                        pixel = Colors.White
                    Else
                        Dim lastIntensity As Byte = senseHat.Display.Screen(x, y).R

                        If lastIntensity <= 10 Then
                            pixel = Colors.Black
                        Else
                            Dim newIntensity As Byte = CByte(Math.Round(lastIntensity * oldSparkleFadeRate))
                            pixel = Color.FromArgb(255, newIntensity, newIntensity, newIntensity)
                        End If
                    End If

                    senseHat.Display.Screen(x, y) = pixel
                Next
            Next
        End Sub

        Private Sub FillDisplayBlocks()
            For y As Integer = 0 To 8 - 1 Step 2

                For x As Integer = 0 To 8 - 1 Step 2
                    Dim pixel As Color = Color.FromArgb(255, CByte((Random.[Next](2) * 255)), CByte((Random.[Next](2) * 255)), CByte((Random.[Next](2) * 255)))
                    senseHat.Display.Screen(x, y) = pixel
                    senseHat.Display.Screen(x + 1, y) = pixel
                    senseHat.Display.Screen(x, y + 1) = pixel
                    senseHat.Display.Screen(x + 1, y + 1) = pixel
                Next
            Next
        End Sub

        Private Sub FillDisplayUnicolor()
            senseHat.Display.Fill(Color.FromArgb(255, CByte((Random.[Next](2) * 255)), CByte((Random.[Next](2) * 255)), CByte((Random.[Next](2) * 255))))
        End Sub
    End Class


