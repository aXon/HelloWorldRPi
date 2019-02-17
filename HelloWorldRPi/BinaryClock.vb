''
'
'  The original C# file was part of Rpi.SenseHat.Demo https://github.com/emmellsoft/RPi.SenseHat
'  Copyright (c) 2017, Mattias Larsson
'
'
' The binary clock demo Is developed by Mark Muller using
' the RPi.SenseHat Windows IoT class library for the Raspberry Pi 
'
'
'  The current version is the Visual Basic equivalent
'  Copyright (c) 2019. Nils Bausch
'

Imports Emmellsoft.IoT.Rpi.SenseHat
Imports System
Imports Windows.UI


Public Class BinaryClock
        Inherits SenseHatDemo

        Private ReadOnly _activeBitColor As Color = Colors.Red
        Private ReadOnly _inctiveBitColor As Color = Colors.DimGray

        Public Sub New(ByVal senseHat As ISenseHat, Optional ByVal setScreenText As Action(Of String) = Nothing)
            MyBase.New(senseHat)
        End Sub

        Public Overrides Sub Run()
            While True
                senseHat.Display.Clear()
                senseHat.Display.Screen(0, 0) = _activeBitColor
                Dim now As DateTime = DateTime.Now
                DrawBinary(0, now.Hour)
                DrawBinary(3, now.Minute)
                DrawBinary(6, now.Second)
                senseHat.Display.Update()
                'SetScreenText?.Invoke(now.ToString("HH':'mm':'ss"))
                Sleep(TimeSpan.FromMilliseconds(200))
            End While
        End Sub

        Private Sub DrawBinary(ByVal x As Integer, ByVal value As Integer)
            For y As Integer = 7 To 0
                Dim bitColor As Color = If((value Mod 2 = 1), _activeBitColor, _inctiveBitColor)
                senseHat.Display.Screen(x, y) = bitColor
                senseHat.Display.Screen(x + 1, y) = bitColor

                value = value >> 1

            Next
        End Sub
    End Class


