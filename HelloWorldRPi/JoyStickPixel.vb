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

Public Class JoystickPixel
        Inherits SenseHatDemo

        Private ReadOnly _colors As Color() = {Colors.Red, Colors.Green, Colors.Blue, Colors.Cyan, Colors.Magenta, Colors.Yellow, Colors.White}
        Private _lastPressingEnter As Boolean
        Private _colorIndex As Integer

    Public Sub New(ByVal senseHat As ISenseHat, Optional ByVal setScreenText As Action(Of String) = Nothing)
        MyBase.New(senseHat)
    End Sub

    Public Overrides Sub Run()
            Dim x As Integer = 3
            Dim y As Integer = 3
            senseHat.Display.Clear()

            While True

                If senseHat.Joystick.Update() Then
                    UpdatePosition(x, y)
                    senseHat.Display.Clear()
                    senseHat.Display.Screen(x, y) = _colors(_colorIndex)
                    senseHat.Display.Update()
                'SetScreenText?.Invoke($"{x}, {y}")
            End If

                Sleep(TimeSpan.FromMilliseconds(2))
            End While
        End Sub

        Private Sub UpdatePosition(ByRef x As Integer, ByRef y As Integer)
            If senseHat.Joystick.LeftKey = KeyState.Pressed Then

                If x > 0 Then
                    x -= 1
                End If
            ElseIf senseHat.Joystick.RightKey = KeyState.Pressed Then

                If x < 7 Then
                    x += 1
                End If
            End If

            If senseHat.Joystick.UpKey = KeyState.Pressed Then

                If y > 0 Then
                    y -= 1
                End If
            ElseIf senseHat.Joystick.DownKey = KeyState.Pressed Then

                If y < 7 Then
                    y += 1
                End If
            End If

            Dim currentPressingEnter As Boolean = senseHat.Joystick.EnterKey = KeyState.Pressing

            If _lastPressingEnter <> currentPressingEnter Then
                _lastPressingEnter = currentPressingEnter

                If currentPressingEnter Then
                    _colorIndex += 1

                    If _colorIndex >= _colors.Length Then
                        _colorIndex = 0
                    End If
                End If
            End If
        End Sub
    End Class

