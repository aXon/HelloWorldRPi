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
Imports Emmellsoft.IoT.Rpi.SenseHat.Fonts.MultiColor


Public Class MultiColorScrollText
        Inherits SenseHatDemo

        Private ReadOnly _scrollText As String

        Public Sub New(ByVal senseHat As ISenseHat, ByVal scrollText As String)
            MyBase.New(senseHat)
            _scrollText = scrollText
        End Sub

        Public Overrides Sub Run()
            Dim font As MultiColorFont = MultiColorFont.LoadFromImage(New Uri("ms-appx:///Assets/ColorFont.png"), " ABCDEFGHIJKLMNOPQRSTUVWXYZÅÄÖÉÜabcdefghijklmnopqrstuvwxyzåäöéü0123456789.,?!""#$%&-+*:;/\<>()'`=", Color.FromArgb(&HFF, &HFF, &H0, &HFF)).Result
            Dim characters As IEnumerable(Of MultiColorCharacter) = font.GetChars(_scrollText)
        'Dim backgroundColor As Color = Color.FromArgb(&HFF, &H0, &H20, &H0)
        Dim backgroundColor As Color = Colors.Black
        Dim characterRenderer = New MultiColorCharacterRenderer()
            Dim textScroller = New TextScroller(Of MultiColorCharacter)(senseHat.Display, characterRenderer, characters)

            While True

                If Not textScroller.[Step]() Then
                    textScroller.Reset()
                End If

                senseHat.Display.Fill(backgroundColor)
                textScroller.Render()
                senseHat.Display.Update()
            Sleep(TimeSpan.FromMilliseconds(40))
        End While
        End Sub
    End Class

