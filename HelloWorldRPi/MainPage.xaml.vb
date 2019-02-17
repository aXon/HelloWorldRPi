Imports System
Imports Emmellsoft.IoT.Rpi.SenseHat
Imports Emmellsoft.IoT.Rpi.SenseHat.Fonts.SingleColor
Imports Windows.Devices.Gpio
Imports Windows.UI
Imports System.Threading
Imports Windows.Networking.Connectivity
Imports System.Net
Imports System.Net.Sockets


' The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

''' <summary>
''' An empty page that can be used on its own or navigated to within a Frame.
''' </summary>
Public NotInheritable Class MainPage
    Inherits Page

    Private hat As ISenseHat

    Private currentDemo As Integer = 0
    Private Const demoMax As Integer = 1

    Private Async Sub MainPage_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        ' initialize the SenseHat object
        hat = Await SenseHatFactory.GetSenseHat.ConfigureAwait(False)
        'Dim newDemo As SenseHatDemo = New JoystickPixel(hat)
        'Dim newDemo As SenseHatDemo = New MultiColorScrollText(hat, "Blueberry")
        Dim newDemo As SenseHatDemo = New MultiColorScrollText(hat, GetIpAddress.ToString)
        newDemo.Run()

    End Sub

    Public Function GetIpAddress() As IPAddress
        Dim hosts = NetworkInformation.GetHostNames()

        For Each host In hosts
            Dim addr As IPAddress
            If Not IPAddress.TryParse(host.DisplayName, addr) Then Continue For
            If addr.AddressFamily <> AddressFamily.InterNetwork Then Continue For
            Return addr
        Next

        Return Nothing
    End Function


End Class
