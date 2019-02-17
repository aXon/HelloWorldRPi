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
Imports Emmellsoft.IoT.Rpi.SenseHat.Fonts.SingleColor


Public Class WriteTemperature
        Inherits SenseHatDemo

    Public Sub New(ByVal senseHat As ISenseHat)
        MyBase.New(senseHat)
    End Sub

    Private Enum TemperatureUnit
            Celcius
            Fahrenheit
            Kelvin
        End Enum

        Public Overrides Sub Run()
            Dim tinyFont = New TinyFont()
            Dim display As ISenseHatDisplay = SenseHat.Display
            Dim unit As TemperatureUnit = TemperatureUnit.Celcius
            Dim unitText As String = GetUnitText(unit)

            While True
                SenseHat.Sensors.HumiditySensor.Update()

                If SenseHat.Sensors.Temperature.HasValue Then
                    Dim temperatureValue As Double = ConvertTemperatureValue(unit, SenseHat.Sensors.Temperature.Value)
                    Dim temperature As Integer = CInt(Math.Round(temperatureValue))
                    Dim text As String = temperature.ToString()

                    If text.Length > 2 Then
                        text = "**"
                    End If

                    display.Clear()
                    tinyFont.Write(display, text, Colors.White)
                    display.Update()
                'MainPage?.SetScreenText($"{temperatureValue} {unitText}")
                Sleep(TimeSpan.FromSeconds(5))
                Else
                    Sleep(TimeSpan.FromSeconds(0.5))
                End If
            End While
        End Sub

        Private Shared Function ConvertTemperatureValue(ByVal unit As TemperatureUnit, ByVal temperatureInCelcius As Double) As Double
            Select Case unit
                Case TemperatureUnit.Celcius
                    Return temperatureInCelcius
                Case TemperatureUnit.Fahrenheit
                    Return temperatureInCelcius * 9 / 5 + 32
                Case TemperatureUnit.Kelvin
                    Return temperatureInCelcius + 273.15
                Case Else
                    Throw New ArgumentOutOfRangeException()
            End Select
        End Function

        Private Shared Function GetUnitText(ByVal unit As TemperatureUnit) As String
            Select Case unit
                Case TemperatureUnit.Celcius
                    Return "°C"
                Case TemperatureUnit.Fahrenheit
                    Return "°F"
                Case TemperatureUnit.Kelvin
                    Return "K"
                Case Else
                    Throw New ArgumentOutOfRangeException()
            End Select
        End Function
    End Class


