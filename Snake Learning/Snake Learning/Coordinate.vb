Public Structure Coordinate
    Public Property X As Integer
    Public Property Y As Integer

    Public Sub New(ByVal NewX As Integer, ByVal NewY As Integer)
        X = NewX
        Y = NewY
    End Sub

    Public Overrides Function ToString() As String
        Return "(" + X.ToString + ", " + Y.ToString + ")"
    End Function
End Structure
