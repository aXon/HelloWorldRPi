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
Imports System.Collections.Generic
Imports Windows.UI
Imports Emmellsoft.IoT.Rpi.SenseHat
Imports Emmellsoft.IoT.Rpi.SenseHat.Fonts
Imports Emmellsoft.IoT.Rpi.SenseHat.Fonts.SingleColor


Public Class SingleColorScrollText
        Inherits SenseHatDemo

        Private ReadOnly _scrollText As String

        Private Enum RenderMode
            YellowOnBlue
            BlackOnStaticRainbow
            BlackOnMovingRainbow
            StaticRainbowOnBlack
            MovingRainbowOnBlack
        End Enum

        Private ReadOnly _rainbowColors As Color(,) = New Color(7, 7) {}
        Private _currentMode As RenderMode

    Public Sub New(ByVal senseHat As ISenseHat, ByVal scrollText As String)
        MyBase.New(senseHat)
        _scrollText = scrollText
    End Sub

    Public Overrides Sub Run()
            SenseHat.Display.Reset()
            SenseHat.Display.CopyScreenToColors(_rainbowColors)
            Dim font As SingleColorFont = SingleColorFont.Deserialize(FontBytes)
            Dim characters As IEnumerable(Of SingleColorCharacter) = font.GetChars(_scrollText)
            Dim characterRenderer As SingleColorCharacterRenderer = New SingleColorCharacterRenderer(AddressOf GetCharacterColor)
            Dim textScroller = New TextScroller(Of SingleColorCharacter)(SenseHat.Display, characterRenderer, characters)

            While True

                If Not textScroller.[Step]() Then
                    textScroller.Reset()
                End If

                FillDisplay(textScroller.ScrollPixelOffset)
                textScroller.Render()
                SenseHat.Display.Update()

                If SenseHat.Joystick.Update() AndAlso (SenseHat.Joystick.EnterKey = KeyState.Pressing) Then
                    SwitchToNextScrollMode()
                End If

                Sleep(TimeSpan.FromMilliseconds(50))
            End While
        End Sub

        Private Sub SwitchToNextScrollMode()
            _currentMode += 1

            If _currentMode > RenderMode.MovingRainbowOnBlack Then
                _currentMode = RenderMode.YellowOnBlue
            End If
        End Sub

        Private Sub FillDisplay(ByVal scrollPixelOffset As Integer)
            Select Case _currentMode
                Case RenderMode.YellowOnBlue
                    SenseHat.Display.Fill(Colors.Blue)
                Case RenderMode.BlackOnStaticRainbow
                    SenseHat.Display.Reset()
                Case RenderMode.BlackOnMovingRainbow
                    SenseHat.Display.CopyColorsToScreen(_rainbowColors, -scrollPixelOffset)
                Case RenderMode.StaticRainbowOnBlack
                    SenseHat.Display.Fill(Colors.Black)
                Case RenderMode.MovingRainbowOnBlack
                    SenseHat.Display.Fill(Colors.Black)
                Case Else
                    Throw New ArgumentOutOfRangeException()
            End Select
        End Sub

        Private Function GetCharacterColor(ByVal pixelMap As SingleColorCharacterRendererPixelMap) As Color
            Select Case _currentMode
                Case RenderMode.YellowOnBlue
                    Return Colors.Yellow
                Case RenderMode.BlackOnStaticRainbow
                    Return Colors.Black
                Case RenderMode.BlackOnMovingRainbow
                    Return Colors.Black
                Case RenderMode.StaticRainbowOnBlack
                    Return _rainbowColors(pixelMap.DisplayPixelX, pixelMap.DisplayPixelY)
                Case RenderMode.MovingRainbowOnBlack
                    Return _rainbowColors(pixelMap.CharPixelX, pixelMap.CharPixelY)
                Case Else
                    Throw New ArgumentOutOfRangeException()
            End Select
        End Function

        Private Shared ReadOnly Property FontBytes As IEnumerable(Of Byte)
            Get
                Return New Byte() {&H20, &H0, &H0, &H0, &H0, &H0, &H0, &H0, &HFF, &H0, &H41, &H0, &H0, &H7C, &H7E, &HB, &HB, &H7E, &H7C, &H0, &HFF, &H0, &H42, &H0, &H0, &H7F, &H7F, &H49, &H49, &H7F, &H36, &H0, &HFF, &H0, &H43, &H0, &H0, &H3E, &H7F, &H41, &H41, &H63, &H22, &H0, &HFF, &H0, &H44, &H0, &H0, &H7F, &H7F, &H41, &H63, &H3E, &H1C, &H0, &HFF, &H0, &H45, &H0, &H0, &H7F, &H7F, &H49, &H49, &H41, &H41, &H0, &HFF, &H0, &H46, &H0, &H0, &H7F, &H7F, &H9, &H9, &H1, &H1, &H0, &HFF, &H0, &H47, &H0, &H0, &H3E, &H7F, &H41, &H49, &H7B, &H3A, &H0, &HFF, &H0, &H48, &H0, &H0, &H7F, &H7F, &H8, &H8, &H7F, &H7F, &H0, &HFF, &H0, &H49, &H0, &H0, &H41, &H7F, &H7F, &H41, &H0, &HFF, &H0, &H4A, &H0, &H0, &H20, &H60, &H41, &H7F, &H3F, &H1, &H0, &HFF, &H0, &H4B, &H0, &H0, &H7F, &H7F, &H1C, &H36, &H63, &H41, &H0, &HFF, &H0, &H4C, &H0, &H0, &H7F, &H7F, &H40, &H40, &H40, &H40, &H0, &HFF, &H0, &H4D, &H0, &H0, &H7F, &H7F, &H6, &HC, &H6, &H7F, &H7F, &H0, &HFF, &H0, &H4E, &H0, &H0, &H7F, &H7F, &HE, &H1C, &H7F, &H7F, &H0, &HFF, &H0, &H4F, &H0, &H0, &H3E, &H7F, &H41, &H41, &H7F, &H3E, &H0, &HFF, &H0, &H50, &H0, &H0, &H7F, &H7F, &H9, &H9, &HF, &H6, &H0, &HFF, &H0, &H51, &H0, &H0, &H1E, &H3F, &H21, &H61, &H7F, &H5E, &H0, &HFF, &H0, &H52, &H0, &H0, &H7F, &H7F, &H19, &H39, &H6F, &H46, &H0, &HFF, &H0, &H53, &H0, &H0, &H26, &H6F, &H49, &H49, &H7B, &H32, &H0, &HFF, &H0, &H54, &H0, &H0, &H1, &H1, &H7F, &H7F, &H1, &H1, &H0, &HFF, &H0, &H55, &H0, &H0, &H3F, &H7F, &H40, &H40, &H7F, &H3F, &H0, &HFF, &H0, &H56, &H0, &H0, &H1F, &H3F, &H60, &H60, &H3F, &H1F, &H0, &HFF, &H0, &H57, &H0, &H0, &H7F, &H7F, &H30, &H18, &H30, &H7F, &H7F, &H0, &HFF, &H0, &H58, &H0, &H0, &H63, &H77, &H1C, &H1C, &H77, &H63, &H0, &HFF, &H0, &H59, &H0, &H0, &H7, &HF, &H78, &H78, &HF, &H7, &H0, &HFF, &H0, &H5A, &H0, &H0, &H61, &H71, &H59, &H4D, &H47, &H43, &H0, &HFF, &H0, &HC5, &H0, &H0, &H70, &H7A, &H2D, &H2D, &H7A, &H70, &H0, &HFF, &H0, &HC4, &H0, &H0, &H71, &H79, &H2C, &H2C, &H79, &H71, &H0, &HFF, &H0, &HD6, &H0, &H0, &H39, &H7D, &H44, &H44, &H7D, &H39, &H0, &HFF, &H0, &HC9, &H0, &H0, &H7C, &H7C, &H54, &H56, &H45, &H45, &H0, &HFF, &H0, &HDC, &H0, &H0, &H3D, &H7D, &H40, &H40, &H7D, &H3D, &H0, &HFF, &H0, &H61, &H0, &H20, &H74, &H54, &H54, &H7C, &H78, &H0, &HFF, &H0, &H62, &H0, &H0, &H7F, &H7F, &H48, &H48, &H78, &H30, &H0, &HFF, &H0, &H63, &H0, &H0, &H38, &H7C, &H44, &H44, &H44, &H0, &HFF, &H0, &H64, &H0, &H0, &H38, &H7C, &H44, &H44, &H7F, &H7F, &H0, &HFF, &H0, &H65, &H0, &H0, &H38, &H7C, &H54, &H54, &H5C, &H18, &H0, &HFF, &H0, &H66, &H0, &H0, &H4, &H7E, &H7F, &H5, &H5, &H0, &HFF, &H0, &H67, &H0, &H0, &H98, &HBC, &HA4, &HA4, &HFC, &H7C, &H0, &HFF, &H0, &H68, &H0, &H0, &H7F, &H7F, &H8, &H8, &H78, &H70, &H0, &HFF, &H0, &H69, &H0, &H0, &H48, &H7A, &H7A, &H40, &H0, &HFF, &H0, &H6A, &H0, &H80, &H80, &H80, &HFA, &H7A, &H0, &HFF, &H0, &H6B, &H0, &H0, &H7F, &H7F, &H10, &H38, &H68, &H40, &H0, &HFF, &H0, &H6C, &H0, &H0, &H41, &H7F, &H7F, &H40, &H0, &HFF, &H0, &H6D, &H0, &H0, &H7C, &H7C, &H18, &H38, &H1C, &H7C, &H78, &H0, &HFF, &H0, &H6E, &H0, &H0, &H7C, &H7C, &H4, &H4, &H7C, &H78, &H0, &HFF, &H0, &H6F, &H0, &H0, &H38, &H7C, &H44, &H44, &H7C, &H38, &H0, &HFF, &H0, &H70, &H0, &H0, &HFC, &HFC, &H24, &H24, &H3C, &H18, &H0, &HFF, &H0, &H71, &H0, &H0, &H18, &H3C, &H24, &H24, &HFC, &HFC, &H0, &HFF, &H0, &H72, &H0, &H0, &H7C, &H7C, &H4, &H4, &HC, &H8, &H0, &HFF, &H0, &H73, &H0, &H0, &H48, &H5C, &H54, &H54, &H74, &H24, &H0, &HFF, &H0, &H74, &H0, &H0, &H4, &H4, &H3F, &H7F, &H44, &H44, &H0, &HFF, &H0, &H75, &H0, &H0, &H3C, &H7C, &H40, &H40, &H7C, &H7C, &H0, &HFF, &H0, &H76, &H0, &H0, &H1C, &H3C, &H60, &H60, &H3C, &H1C, &H0, &HFF, &H0, &H77, &H0, &H0, &H1C, &H7C, &H70, &H38, &H70, &H7C, &H1C, &H0, &HFF, &H0, &H78, &H0, &H0, &H44, &H6C, &H38, &H38, &H6C, &H44, &H0, &HFF, &H0, &H79, &H0, &H0, &H9C, &HBC, &HA0, &HE0, &H7C, &H3C, &H0, &HFF, &H0, &H7A, &H0, &H0, &H44, &H64, &H74, &H5C, &H4C, &H44, &H0, &HFF, &H0, &HE5, &H0, &H20, &H74, &H55, &H55, &H7C, &H78, &H0, &HFF, &H0, &HE4, &H0, &H20, &H75, &H54, &H54, &H7D, &H78, &H0, &HFF, &H0, &HF6, &H0, &H0, &H30, &H7A, &H48, &H48, &H7A, &H30, &H0, &HFF, &H0, &HE9, &H0, &H0, &H38, &H7C, &H54, &H56, &H5D, &H19, &H0, &HFF, &H0, &HFC, &H0, &H0, &H3A, &H7A, &H40, &H40, &H7A, &H7A, &H0, &HFF, &H0, &H30, &H0, &H0, &H3E, &H7F, &H49, &H45, &H7F, &H3E, &H0, &HFF, &H0, &H31, &H0, &H0, &H40, &H44, &H7F, &H7F, &H40, &H40, &H0, &HFF, &H0, &H32, &H0, &H0, &H62, &H73, &H51, &H49, &H4F, &H46, &H0, &HFF, &H0, &H33, &H0, &H0, &H22, &H63, &H49, &H49, &H7F, &H36, &H0, &HFF, &H0, &H34, &H0, &H0, &H18, &H18, &H14, &H16, &H7F, &H7F, &H10, &HFF, &H0, &H35, &H0, &H0, &H27, &H67, &H45, &H45, &H7D, &H39, &H0, &HFF, &H0, &H36, &H0, &H0, &H3E, &H7F, &H49, &H49, &H7B, &H32, &H0, &HFF, &H0, &H37, &H0, &H0, &H3, &H3, &H79, &H7D, &H7, &H3, &H0, &HFF, &H0, &H38, &H0, &H0, &H36, &H7F, &H49, &H49, &H7F, &H36, &H0, &HFF, &H0, &H39, &H0, &H0, &H26, &H6F, &H49, &H49, &H7F, &H3E, &H0, &HFF, &H0, &H2E, &H0, &H0, &H60, &H60, &H0, &HFF, &H0, &H2C, &H0, &H0, &H80, &HE0, &H60, &H0, &HFF, &H0, &H3F, &H0, &H0, &H2, &H3, &H51, &H59, &HF, &H6, &H0, &HFF, &H0, &H21, &H0, &H0, &H4F, &H4F, &H0, &HFF, &H0, &H22, &H0, &H0, &H7, &H7, &H0, &H0, &H7, &H7, &H0, &HFF, &H0, &H23, &H0, &H0, &H14, &H7F, &H7F, &H14, &H14, &H7F, &H7F, &H14, &H0, &HFF, &H0, &H24, &H0, &H0, &H24, &H2E, &H6B, &H6B, &H3A, &H12, &H0, &HFF, &H0, &H25, &H0, &H0, &H63, &H33, &H18, &HC, &H66, &H63, &H0, &HFF, &H0, &H26, &H0, &H0, &H32, &H7F, &H4D, &H4D, &H77, &H72, &H50, &H0, &HFF, &H0, &H2D, &H0, &H0, &H8, &H8, &H8, &H8, &H8, &H8, &H0, &HFF, &H0, &H2B, &H0, &H0, &H8, &H8, &H3E, &H3E, &H8, &H8, &H0, &HFF, &H0, &H2A, &H0, &H0, &H8, &H2A, &H3E, &H1C, &H1C, &H3E, &H2A, &H8, &H0, &HFF, &H0, &H3A, &H0, &H0, &H66, &H66, &H0, &HFF, &H0, &H3B, &H0, &H0, &H80, &HE6, &H66, &H0, &HFF, &H0, &H2F, &H0, &H0, &H40, &H60, &H30, &H18, &HC, &H6, &H2, &H0, &HFF, &H0, &H5C, &H0, &H0, &H2, &H6, &HC, &H18, &H30, &H60, &H40, &H0, &HFF, &H0, &H3C, &H0, &H0, &H8, &H1C, &H36, &H63, &H41, &H41, &H0, &HFF, &H0, &H3E, &H0, &H0, &H41, &H41, &H63, &H36, &H1C, &H8, &H0, &HFF, &H0, &H28, &H0, &H0, &H1C, &H3E, &H63, &H41, &H0, &HFF, &H0, &H29, &H0, &H0, &H41, &H63, &H3E, &H1C, &H0, &HFF, &H0, &H27, &H0, &H0, &H4, &H6, &H3, &H1, &H0, &HFF, &H0, &H60, &H0, &H0, &H1, &H3, &H6, &H4, &H0, &HFF, &H0, &H3D, &H0, &H0, &H14, &H14, &H14, &H14, &H14, &H14, &H0}
            End Get
        End Property
    End Class
