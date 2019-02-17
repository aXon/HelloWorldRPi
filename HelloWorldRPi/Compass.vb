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
Imports Windows.Foundation
Imports Windows.UI
Imports Emmellsoft.IoT.Rpi.SenseHat

Public Class Compass
        Inherits SenseHatDemo

    Public Sub New(ByVal senseHat As ISenseHat, Optional byVal setScreenText As Action(Of String) = Nothing)
        MyBase.New(senseHat)
    End Sub

    Public Overrides Sub Run()
            senseHat.Display.Clear()
            senseHat.Display.Update()
            Const halfCircle As Double = Math.PI
            Const fullCircle As Double = Math.PI * 2
            Dim northColor As Color = Colors.Red
            Dim southColor As Color = Colors.White
            Dim centerColor As Color = Colors.DarkBlue
            Dim mainPageUpdateRate As TimeSpan = TimeSpan.FromSeconds(0.5)
            Dim nextMainPageUpdate As DateTime = DateTime.Now.Add(mainPageUpdateRate)

            While True
                senseHat.Sensors.ImuSensor.Update()

                If senseHat.Sensors.Pose.HasValue Then
                    Dim northAngle As Double = senseHat.Sensors.Pose.Value.Z

                    If northAngle < 0 Then
                        northAngle += fullCircle
                    End If

                    northAngle = fullCircle - northAngle
                    Dim southAngle As Double = northAngle + halfCircle
                    Dim northPoint As Point = GetPixelCoordinate(northAngle)
                    Dim southPoint As Point = GetPixelCoordinate(southAngle)
                    senseHat.Display.Clear()
                    senseHat.Display.Screen(CInt(northPoint.X), CInt(northPoint.Y)) = northColor
                    senseHat.Display.Screen(CInt(southPoint.X), CInt(southPoint.Y)) = southColor
                    senseHat.Display.Screen(3, 3) = centerColor
                    senseHat.Display.Screen(4, 3) = centerColor
                    senseHat.Display.Screen(3, 4) = centerColor
                    senseHat.Display.Screen(4, 4) = centerColor
                    senseHat.Display.Update()

                'If (SetScreenText IsNot Nothing) AndAlso nextMainPageUpdate <= DateTime.Now Then
                '    SetScreenText($"{northAngle / fullCircle * 360}")
                '    nextMainPageUpdate = DateTime.Now.Add(mainPageUpdateRate)
                'End If
            End If

                Sleep(TimeSpan.FromMilliseconds(2))
            End While
        End Sub

        Private Shared Function GetPixelCoordinate(ByVal angle As Double) As Point
            Return New Point(Math.Round(Math.Cos(angle) * 3.5 + 3.5), Math.Round(Math.Sin(angle) * 3.5 + 3.5))
        End Function
    End Class

