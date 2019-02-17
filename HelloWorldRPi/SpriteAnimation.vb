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
Imports Emmellsoft.IoT.Rpi.SenseHat.Sprites

Public Class SpriteAnimation
        Inherits SenseHatDemo

        Public Sub New(ByVal senseHat As ISenseHat)
            MyBase.New(senseHat)
        End Sub

        Public Overrides Sub Run()
        Dim imageUri = New Uri("ms-appx:///Assets/MiniMario.png") ' need to replace it?
        Dim pixels As Color(,) = PixelSupport.GetPixels(imageUri).Result
            Dim spriteMap = New SpriteMap(pixels)
            Dim animationIndex As Integer = 0
            Dim sprite As Sprite = spriteMap.GetSprite(animationIndex)
            Dim frameDuration As TimeSpan = TimeSpan.FromMilliseconds(70)
            Dim nextAnimationUpdateTime As DateTime = DateTime.Now.Add(frameDuration)
            Dim spriteX As Integer = 0
            Dim spriteY As Integer = 0
            Dim direction As DisplayDirection = DisplayDirection.Deg0
            Dim flipHorizontal As Boolean = False
            Dim flipVertical As Boolean = False

            While True
                Dim redrawSprite As Boolean = False

                If DateTime.Now >= nextAnimationUpdateTime Then
                    nextAnimationUpdateTime = DateTime.Now.Add(frameDuration)
                    redrawSprite = True
                    animationIndex += 1

                    If animationIndex >= 6 Then
                        animationIndex = 0
                    End If

                    sprite = spriteMap.GetSprite(animationIndex)
                End If

                If senseHat.Joystick.Update() Then
                    UpdatePosition(spriteX, spriteY)
                    UpdateDrawingDirection(direction, flipHorizontal, flipVertical)
                    redrawSprite = True
                End If

                If redrawSprite Then
                    senseHat.Display.Clear()
                    sprite.Draw(senseHat.Display, spriteX, spriteY, True, direction, flipHorizontal, flipVertical)
                    senseHat.Display.Update()
                End If

                Sleep(TimeSpan.FromMilliseconds(2))
            End While
        End Sub

        Private Sub UpdatePosition(ByRef spriteX As Integer, ByRef spriteY As Integer)
            If senseHat.Joystick.LeftKey = KeyState.Pressed Then

                If spriteX > -8 Then
                    spriteX -= 1
                End If
            ElseIf senseHat.Joystick.RightKey = KeyState.Pressed Then

                If spriteX < 8 Then
                    spriteX += 1
                End If
            End If

            If senseHat.Joystick.UpKey = KeyState.Pressed Then

                If spriteY > -8 Then
                    spriteY -= 1
                End If
            ElseIf senseHat.Joystick.DownKey = KeyState.Pressed Then

                If spriteY < 8 Then
                    spriteY += 1
                End If
            End If
        End Sub

        Private Sub UpdateDrawingDirection(ByRef direction As DisplayDirection, ByRef flipHorizontal As Boolean, ByRef flipVertical As Boolean)
            If senseHat.Joystick.EnterKey = KeyState.Pressed Then
                direction += 1

                If direction > DisplayDirection.Deg270 Then
                    direction = DisplayDirection.Deg0
                    flipHorizontal = Not flipHorizontal

                    If Not flipHorizontal Then
                        flipVertical = Not flipVertical
                    End If
                End If
            End If
        End Sub
    End Class

