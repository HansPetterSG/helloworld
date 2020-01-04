Public Class Form1
    Dim NewBox As PictureBox
    Dim BoxArray(30, 30) As PictureBox
    Dim Random As New Random
    Dim MainSnake As Snake
    Dim Lost As Boolean
    Dim Food As Coordinate
    Dim BufferCoordinate As Coordinate
    Dim TickCounter As Integer
    Dim TurnArray(3) As Double
    Dim DistanceBuffer As Integer
    Dim Found As Boolean
    Dim LostCause As Integer
    Dim PositionBuffer(0) As Position
    Dim TimeBuffer As Integer
    Dim TicksSinceFood As Integer
    Dim Record As Integer
    Dim Players(100, 5) As Integer
    Dim SortedPlayers(100) As Integer
    Dim DeadPlayers(50) As Integer
    Dim Generation As Integer
    Dim Average As Integer
    Dim BestScore As Integer

    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        For y = 1 To 30
            For x = 1 To 30
                NewBox = New PictureBox
                NewBox.BorderStyle = BorderStyle.None
                NewBox.Size = New Size(20, 20)
                NewBox.Location = New Point((91 + (x * 22)), (-12 + (y * 22)))
                NewBox.BackColor = Color.White
                Me.Controls.Add(NewBox)
                BoxArray(x, y) = NewBox
                PictureBox1.SendToBack()
            Next
        Next
        For p = 1 To 100
            For i = 1 To 5
                Players(p, i) = Random.Next(-999, 1000)
            Next
        Next
        Generation = 1
    End Sub

    Private Sub MakeFood()
        Found = True
        While Found = True
            Found = False
            Food = New Coordinate(Random.Next(1, 31), Random.Next(1, 31))
            For i = 0 To MainSnake.Body.GetUpperBound(0)
                If Food.X = MainSnake.Body(i).X And Food.Y = MainSnake.Body(i).Y Then
                    Found = True
                End If
            Next
        End While
    End Sub

    Private Sub NewGame()
        ReDim MainSnake.Body(3)
        MainSnake.Body(0) = New Coordinate(Random.Next(6, 26), Random.Next(6, 26))
        MainSnake.Direction = Random.Next(0, 4)
        For i = 1 To MainSnake.Body.GetUpperBound(0)
            If MainSnake.Direction = 0 Then
                MainSnake.Body(i) = New Coordinate(MainSnake.Body(0).X, (MainSnake.Body(0).Y + i))
            ElseIf MainSnake.Direction = 1 Then
                MainSnake.Body(i) = New Coordinate((MainSnake.Body(0).X - i), MainSnake.Body(0).Y)
            ElseIf MainSnake.Direction = 2 Then
                MainSnake.Body(i) = New Coordinate(MainSnake.Body(0).X, (MainSnake.Body(0).Y - i))
            Else
                MainSnake.Body(i) = New Coordinate((MainSnake.Body(0).X + i), MainSnake.Body(0).Y)
            End If
        Next
        MakeFood()
        TickCounter = 0
        Lost = False
        ReDim PositionBuffer(0)
        ReDim PositionBuffer(0).CoordinateList(MainSnake.Body.GetUpperBound(0))
        For i = 0 To PositionBuffer(0).CoordinateList.GetUpperBound(0)
            PositionBuffer(0).CoordinateList(i) = MainSnake.Body(i)
        Next
        TicksSinceFood = 0
    End Sub

    Private Sub NextTick()
        TickCounter += 1
        TicksSinceFood += 1
        BufferCoordinate = MainSnake.Body(MainSnake.Body.GetUpperBound(0))
        For i = 0 To (MainSnake.Body.GetUpperBound(0) - 1)
            MainSnake.Body(MainSnake.Body.GetUpperBound(0) - i) = MainSnake.Body(MainSnake.Body.GetUpperBound(0) - (i + 1))
        Next
        If MainSnake.Direction = 0 Then
            MainSnake.Body(0) = New Coordinate(MainSnake.Body(0).X, (MainSnake.Body(0).Y - 1))
        ElseIf MainSnake.Direction = 1 Then
            MainSnake.Body(0) = New Coordinate((MainSnake.Body(0).X + 1), MainSnake.Body(0).Y)
        ElseIf MainSnake.Direction = 2 Then
            MainSnake.Body(0) = New Coordinate(MainSnake.Body(0).X, (MainSnake.Body(0).Y + 1))
        Else
            MainSnake.Body(0) = New Coordinate((MainSnake.Body(0).X - 1), MainSnake.Body(0).Y)
        End If
        If MainSnake.Body(0).X < 1 Or MainSnake.Body(0).X > 30 Or MainSnake.Body(0).Y < 1 Or MainSnake.Body(0).Y > 30 Then
            Lost = True
            LostCause = 0
        End If
        If Lost = False Then
            For i = 1 To MainSnake.Body.GetUpperBound(0)
                If MainSnake.Body(0).X = MainSnake.Body(i).X And MainSnake.Body(0).Y = MainSnake.Body(i).Y Then
                    Lost = True
                    LostCause = 1
                End If
            Next
        End If
        If Lost = False And (TicksSinceFood Mod 50) = 0 Then
            For i = 0 To PositionBuffer.GetUpperBound(0)
                Lost = True
                For j = 0 To MainSnake.Body.GetUpperBound(0)
                    If Not PositionBuffer(i).CoordinateList(j).ToString = MainSnake.Body(j).ToString Then
                        Lost = False
                    End If
                Next
                If Lost = True Then
                    Exit For
                End If
            Next
            If Lost = True Then
                LostCause = 2
            End If
        End If
        If Lost = False Then
            If MainSnake.Body(0).X = Food.X And MainSnake.Body(0).Y = Food.Y Then
                ReDim Preserve MainSnake.Body(MainSnake.Body.GetUpperBound(0) + 1)
                MainSnake.Body(MainSnake.Body.GetUpperBound(0)) = BufferCoordinate
                MakeFood()
                ReDim PositionBuffer(0)
                ReDim PositionBuffer(0).CoordinateList(MainSnake.Body.GetUpperBound(0))
                For i = 0 To PositionBuffer(0).CoordinateList.GetUpperBound(0)
                    PositionBuffer(0).CoordinateList(i) = MainSnake.Body(i)
                Next
                TicksSinceFood = 0
            Else
                ReDim Preserve PositionBuffer(PositionBuffer.GetUpperBound(0) + 1)
                ReDim PositionBuffer(PositionBuffer.GetUpperBound(0)).CoordinateList(MainSnake.Body.GetUpperBound(0))
                For i = 0 To MainSnake.Body.GetUpperBound(0)
                    PositionBuffer(PositionBuffer.GetUpperBound(0)).CoordinateList(i) = MainSnake.Body(i)
                Next
            End If
            Found = False
            DistanceBuffer = 0
            While Found = False
                DistanceBuffer += 1
                If MainSnake.Direction = 0 Then
                    If (MainSnake.Body(0).X - DistanceBuffer) = 0 Then
                        TurnArray(0) = ((MainSnake.Network.WallValue / 1000) * (30 - DistanceBuffer) * (MainSnake.Network.TurnValue / 1000))
                        Found = True
                    ElseIf Food.Y = MainSnake.Body(0).Y And Food.X = (MainSnake.Body(0).X - DistanceBuffer) Then
                        TurnArray(0) = ((MainSnake.Network.FoodValue / 1000) * (30 - DistanceBuffer) * (MainSnake.Network.TurnValue / 1000))
                        Found = True
                    Else
                        For i = 1 To MainSnake.Body.GetUpperBound(0)
                            If MainSnake.Body(i).Y = MainSnake.Body(0).Y And MainSnake.Body(i).X = (MainSnake.Body(0).X - DistanceBuffer) Then
                                TurnArray(0) = ((MainSnake.Network.SelfValue / 1000) * (30 - DistanceBuffer) * (MainSnake.Network.TurnValue / 1000))
                                Found = True
                                Exit For
                            End If
                        Next
                    End If
                ElseIf MainSnake.Direction = 1 Then
                    If (MainSnake.Body(0).Y - DistanceBuffer) = 0 Then
                        TurnArray(0) = ((MainSnake.Network.WallValue / 1000) * (30 - DistanceBuffer) * (MainSnake.Network.TurnValue / 1000))
                        Found = True
                    ElseIf Food.X = MainSnake.Body(0).X And Food.Y = (MainSnake.Body(0).Y - DistanceBuffer) Then
                        TurnArray(0) = ((MainSnake.Network.FoodValue / 1000) * (30 - DistanceBuffer) * (MainSnake.Network.TurnValue / 1000))
                        Found = True
                    Else
                        For i = 1 To MainSnake.Body.GetUpperBound(0)
                            If MainSnake.Body(i).X = MainSnake.Body(0).X And MainSnake.Body(i).Y = (MainSnake.Body(0).Y - DistanceBuffer) Then
                                TurnArray(0) = ((MainSnake.Network.SelfValue / 1000) * (30 - DistanceBuffer) * (MainSnake.Network.TurnValue / 1000))
                                Found = True
                                Exit For
                            End If
                        Next
                    End If
                ElseIf MainSnake.Direction = 2 Then
                    If (MainSnake.Body(0).X + DistanceBuffer) = 31 Then
                        TurnArray(0) = ((MainSnake.Network.WallValue / 1000) * (30 - DistanceBuffer) * (MainSnake.Network.TurnValue / 1000))
                        Found = True
                    ElseIf Food.Y = MainSnake.Body(0).Y And Food.X = (MainSnake.Body(0).X + DistanceBuffer) Then
                        TurnArray(0) = ((MainSnake.Network.FoodValue / 1000) * (30 - DistanceBuffer) * (MainSnake.Network.TurnValue / 1000))
                        Found = True
                    Else
                        For i = 1 To MainSnake.Body.GetUpperBound(0)
                            If MainSnake.Body(i).Y = MainSnake.Body(0).Y And MainSnake.Body(i).X = (MainSnake.Body(0).X + DistanceBuffer) Then
                                TurnArray(0) = ((MainSnake.Network.SelfValue / 1000) * (30 - DistanceBuffer) * (MainSnake.Network.TurnValue / 1000))
                                Found = True
                                Exit For
                            End If
                        Next
                    End If
                Else
                    If (MainSnake.Body(0).Y + DistanceBuffer) = 31 Then
                        TurnArray(0) = ((MainSnake.Network.WallValue / 1000) * (30 - DistanceBuffer) * (MainSnake.Network.TurnValue / 1000))
                        Found = True
                    ElseIf Food.X = MainSnake.Body(0).X And Food.Y = (MainSnake.Body(0).Y + DistanceBuffer) Then
                        TurnArray(0) = ((MainSnake.Network.FoodValue / 1000) * (30 - DistanceBuffer) * (MainSnake.Network.TurnValue / 1000))
                        Found = True
                    Else
                        For i = 1 To MainSnake.Body.GetUpperBound(0)
                            If MainSnake.Body(i).X = MainSnake.Body(0).X And MainSnake.Body(i).Y = (MainSnake.Body(0).Y + DistanceBuffer) Then
                                TurnArray(0) = ((MainSnake.Network.SelfValue / 1000) * (30 - DistanceBuffer) * (MainSnake.Network.TurnValue / 1000))
                                Found = True
                                Exit For
                            End If
                        Next
                    End If
                End If
            End While
            Found = False
            DistanceBuffer = 0
            While Found = False
                DistanceBuffer += 1
                If MainSnake.Direction = 0 Then
                    If (MainSnake.Body(0).X + DistanceBuffer) = 31 Then
                        TurnArray(2) = ((MainSnake.Network.WallValue / 1000) * (30 - DistanceBuffer) * (MainSnake.Network.TurnValue / 1000))
                        Found = True
                    ElseIf Food.Y = MainSnake.Body(0).Y And Food.X = (MainSnake.Body(0).X + DistanceBuffer) Then
                        TurnArray(2) = ((MainSnake.Network.FoodValue / 1000) * (30 - DistanceBuffer) * (MainSnake.Network.TurnValue / 1000))
                        Found = True
                    Else
                        For i = 1 To MainSnake.Body.GetUpperBound(0)
                            If MainSnake.Body(i).Y = MainSnake.Body(0).Y And MainSnake.Body(i).X = (MainSnake.Body(0).X + DistanceBuffer) Then
                                TurnArray(2) = ((MainSnake.Network.SelfValue / 1000) * (30 - DistanceBuffer) * (MainSnake.Network.TurnValue / 1000))
                                Found = True
                                Exit For
                            End If
                        Next
                    End If
                ElseIf MainSnake.Direction = 1 Then
                    If (MainSnake.Body(0).Y + DistanceBuffer) = 31 Then
                        TurnArray(2) = ((MainSnake.Network.WallValue / 1000) * (30 - DistanceBuffer) * (MainSnake.Network.TurnValue / 1000))
                        Found = True
                    ElseIf Food.X = MainSnake.Body(0).X And Food.Y = (MainSnake.Body(0).Y + DistanceBuffer) Then
                        TurnArray(2) = ((MainSnake.Network.FoodValue / 1000) * (30 - DistanceBuffer) * (MainSnake.Network.TurnValue / 1000))
                        Found = True
                    Else
                        For i = 1 To MainSnake.Body.GetUpperBound(0)
                            If MainSnake.Body(i).X = MainSnake.Body(0).X And MainSnake.Body(i).Y = (MainSnake.Body(0).Y + DistanceBuffer) Then
                                TurnArray(2) = ((MainSnake.Network.SelfValue / 1000) * (30 - DistanceBuffer) * (MainSnake.Network.TurnValue / 1000))
                                Found = True
                                Exit For
                            End If
                        Next
                    End If
                ElseIf MainSnake.Direction = 2 Then
                    If (MainSnake.Body(0).X - DistanceBuffer) = 0 Then
                        TurnArray(2) = ((MainSnake.Network.WallValue / 1000) * (30 - DistanceBuffer) * (MainSnake.Network.TurnValue / 1000))
                        Found = True
                    ElseIf Food.Y = MainSnake.Body(0).Y And Food.X = (MainSnake.Body(0).X - DistanceBuffer) Then
                        TurnArray(2) = ((MainSnake.Network.FoodValue / 1000) * (30 - DistanceBuffer) * (MainSnake.Network.TurnValue / 1000))
                        Found = True
                    Else
                        For i = 1 To MainSnake.Body.GetUpperBound(0)
                            If MainSnake.Body(i).Y = MainSnake.Body(0).Y And MainSnake.Body(i).X = (MainSnake.Body(0).X - DistanceBuffer) Then
                                TurnArray(2) = ((MainSnake.Network.SelfValue / 1000) * (30 - DistanceBuffer) * (MainSnake.Network.TurnValue / 1000))
                                Found = True
                                Exit For
                            End If
                        Next
                    End If
                Else
                    If (MainSnake.Body(0).Y - DistanceBuffer) = 0 Then
                        TurnArray(2) = ((MainSnake.Network.WallValue / 1000) * (30 - DistanceBuffer) * (MainSnake.Network.TurnValue / 1000))
                        Found = True
                    ElseIf Food.X = MainSnake.Body(0).X And Food.Y = (MainSnake.Body(0).Y - DistanceBuffer) Then
                        TurnArray(2) = ((MainSnake.Network.FoodValue / 1000) * (30 - DistanceBuffer) * (MainSnake.Network.TurnValue / 1000))
                        Found = True
                    Else
                        For i = 1 To MainSnake.Body.GetUpperBound(0)
                            If MainSnake.Body(i).X = MainSnake.Body(0).X And MainSnake.Body(i).Y = (MainSnake.Body(0).Y - DistanceBuffer) Then
                                TurnArray(2) = ((MainSnake.Network.SelfValue / 1000) * (30 - DistanceBuffer) * (MainSnake.Network.TurnValue / 1000))
                                Found = True
                                Exit For
                            End If
                        Next
                    End If
                End If
            End While
            Found = False
            DistanceBuffer = 0
            While Found = False
                DistanceBuffer += 1
                If MainSnake.Direction = 0 Then
                    If (MainSnake.Body(0).Y - DistanceBuffer) = 0 Then
                        TurnArray(1) = ((MainSnake.Network.WallValue / 1000) * (30 - DistanceBuffer) * (MainSnake.Network.StraightValue / 1000))
                        Found = True
                    ElseIf Food.X = MainSnake.Body(0).X And Food.Y = (MainSnake.Body(0).Y - DistanceBuffer) Then
                        TurnArray(1) = ((MainSnake.Network.FoodValue / 1000) * (30 - DistanceBuffer) * (MainSnake.Network.StraightValue / 1000))
                        Found = True
                    Else
                        For i = 1 To MainSnake.Body.GetUpperBound(0)
                            If MainSnake.Body(i).X = MainSnake.Body(0).X And MainSnake.Body(i).Y = (MainSnake.Body(0).Y - DistanceBuffer) Then
                                TurnArray(1) = ((MainSnake.Network.SelfValue / 1000) * (30 - DistanceBuffer) * (MainSnake.Network.StraightValue / 1000))
                                Found = True
                                Exit For
                            End If
                        Next
                    End If
                ElseIf MainSnake.Direction = 1 Then
                    If (MainSnake.Body(0).X + DistanceBuffer) = 31 Then
                        TurnArray(1) = ((MainSnake.Network.WallValue / 1000) * (30 - DistanceBuffer) * (MainSnake.Network.StraightValue / 1000))
                        Found = True
                    ElseIf Food.Y = MainSnake.Body(0).Y And Food.X = (MainSnake.Body(0).X + DistanceBuffer) Then
                        TurnArray(1) = ((MainSnake.Network.FoodValue / 1000) * (30 - DistanceBuffer) * (MainSnake.Network.StraightValue / 1000))
                        Found = True
                    Else
                        For i = 1 To MainSnake.Body.GetUpperBound(0)
                            If MainSnake.Body(i).Y = MainSnake.Body(0).Y And MainSnake.Body(i).X = (MainSnake.Body(0).X + DistanceBuffer) Then
                                TurnArray(1) = ((MainSnake.Network.SelfValue / 1000) * (30 - DistanceBuffer) * (MainSnake.Network.StraightValue / 1000))
                                Found = True
                                Exit For
                            End If
                        Next
                    End If
                ElseIf MainSnake.Direction = 2 Then
                    If (MainSnake.Body(0).Y + DistanceBuffer) = 31 Then
                        TurnArray(1) = ((MainSnake.Network.WallValue / 1000) * (30 - DistanceBuffer) * (MainSnake.Network.StraightValue / 1000))
                        Found = True
                    ElseIf Food.X = MainSnake.Body(0).X And Food.Y = (MainSnake.Body(0).Y + DistanceBuffer) Then
                        TurnArray(1) = ((MainSnake.Network.FoodValue / 1000) * (30 - DistanceBuffer) * (MainSnake.Network.StraightValue / 1000))
                        Found = True
                    Else
                        For i = 1 To MainSnake.Body.GetUpperBound(0)
                            If MainSnake.Body(i).X = MainSnake.Body(0).X And MainSnake.Body(i).Y = (MainSnake.Body(0).Y + DistanceBuffer) Then
                                TurnArray(1) = ((MainSnake.Network.SelfValue / 1000) * (30 - DistanceBuffer) * (MainSnake.Network.StraightValue / 1000))
                                Found = True
                                Exit For
                            End If
                        Next
                    End If
                Else
                    If (MainSnake.Body(0).X - DistanceBuffer) = 0 Then
                        TurnArray(1) = ((MainSnake.Network.WallValue / 1000) * (30 - DistanceBuffer) * (MainSnake.Network.StraightValue / 1000))
                        Found = True
                    ElseIf Food.Y = MainSnake.Body(0).Y And Food.X = (MainSnake.Body(0).X - DistanceBuffer) Then
                        TurnArray(1) = ((MainSnake.Network.FoodValue / 1000) * (30 - DistanceBuffer) * (MainSnake.Network.StraightValue / 1000))
                        Found = True
                    Else
                        For i = 1 To MainSnake.Body.GetUpperBound(0)
                            If MainSnake.Body(i).Y = MainSnake.Body(0).Y And MainSnake.Body(i).X = (MainSnake.Body(0).X - DistanceBuffer) Then
                                TurnArray(1) = ((MainSnake.Network.SelfValue / 1000) * (30 - DistanceBuffer) * (MainSnake.Network.StraightValue / 1000))
                                Found = True
                                Exit For
                            End If
                        Next
                    End If
                End If
            End While
            If TurnArray(0) > TurnArray(1) And TurnArray(0) > TurnArray(2) Then
                MainSnake.Turn(0)
            ElseIf TurnArray(2) > TurnArray(0) And TurnArray(2) > TurnArray(1) Then
                MainSnake.Turn(1)
            End If
        End If
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        TimeBuffer = My.Computer.Clock.TickCount
        Timer1.Enabled = False
        For x = 1 To 30
            For y = 1 To 30
                BoxArray(x, y).BackColor = Color.White
            Next
        Next
        Players(0, 0) = -1
        Generation += NumericUpDown1.Value
        For g = 1 To NumericUpDown1.Value
            Record = 0
            For p = 1 To 100
                Players(p, 0) = 0
                MainSnake.Network.FoodValue = Players(p, 1)
                MainSnake.Network.WallValue = Players(p, 2)
                MainSnake.Network.SelfValue = Players(p, 3)
                MainSnake.Network.TurnValue = Players(p, 4)
                MainSnake.Network.StraightValue = Players(p, 5)
                For r = 1 To 100
                    NewGame()
                    While Lost = False
                        NextTick()
                    End While
                    Players(p, 0) += (MainSnake.Body.GetUpperBound(0) - 3)
                    Record = Math.Max(Record, (MainSnake.Body.GetUpperBound(0) - 3))
                Next
            Next
            Average = 0
            For p = 1 To 100
                Average += Players(p, 0)
            Next
            Average = Math.Floor(Average / 100)
            For i = 0 To 100
                SortedPlayers(i) = 0
            Next
            For i = 1 To 100
                For j = 1 To 100
                    If Players(i, 0) > Players(SortedPlayers(j), 0) Then
                        For k = 0 To (100 - j)
                            SortedPlayers(100 - k) = SortedPlayers(99 - k)
                        Next
                        SortedPlayers(j) = i
                        Exit For
                    End If
                Next
            Next
            BestScore = Players(SortedPlayers(1), 0)
            For i = 1 To 50
                For j = 1 To 5
                    Players(SortedPlayers(50 + i), j) = (Players(SortedPlayers(i), j) + (Random.Next(-30, 31)))
                Next
            Next
        Next
        Label1.Text = "Generation " + Generation.ToString + vbNewLine + vbNewLine + "Best Score: " + BestScore.ToString + vbNewLine + "Average: " + Average.ToString + vbNewLine + "Longest: " + Record.ToString
        Label2.Text = (My.Computer.Clock.TickCount - TimeBuffer).ToString
    End Sub

    Private Sub UpdateFrame()
        For x = 1 To 30
            For y = 1 To 30
                BoxArray(x, y).BackColor = Color.White
            Next
        Next
        BoxArray(MainSnake.Body(0).X, MainSnake.Body(0).Y).BackColor = Color.Red
        For i = 1 To MainSnake.Body.GetUpperBound(0)
            BoxArray(MainSnake.Body(i).X, MainSnake.Body(i).Y).BackColor = Color.Yellow
        Next
        BoxArray(Food.X, Food.Y).BackColor = Color.Purple
    End Sub

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click
        For i = 1 To 100
            MainSnake.Network.FoodValue = Players(SortedPlayers(1), 1)
            MainSnake.Network.WallValue = Players(SortedPlayers(1), 2)
            MainSnake.Network.SelfValue = Players(SortedPlayers(1), 3)
            MainSnake.Network.TurnValue = Players(SortedPlayers(1), 4)
            MainSnake.Network.StraightValue = Players(SortedPlayers(1), 5)
            NewGame()
            UpdateFrame()
            Timer1.Enabled = True
        Next
    End Sub

    Private Sub Timer1_Tick(sender As System.Object, e As System.EventArgs) Handles Timer1.Tick
        Timer1.Enabled = False
        NextTick()
        If Lost = False Then
            UpdateFrame()
            Timer1.Enabled = True
        End If
    End Sub
End Class
