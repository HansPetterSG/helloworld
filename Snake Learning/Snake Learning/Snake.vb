Public Structure Snake
    Dim Body() As Coordinate
    Dim Direction As Integer
    Dim Network As Brain

    Public Sub Turn(ByVal LeftRight As Integer)
        If LeftRight = 0 Then
            Direction -= 1
            If Direction = -1 Then
                Direction = 3
            End If
        ElseIf LeftRight = 1 Then
            Direction += 1
            If Direction = 4 Then
                Direction = 0
            End If
        End If
    End Sub
End Structure
