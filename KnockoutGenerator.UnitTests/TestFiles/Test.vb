Namespace ClassLibrary1

    Public Class Test

        Property Foo As String
        Property Bar As Integer
        Property SystemTime As DateTime

        Private _Prop2 As String = "Empty"
        Property Prop2 As String
            Get
                Return _Prop2
            End Get
            Set(ByVal value As String)
                _Prop2 = value
            End Set
        End Property

        Function getValue() As Global.System.Int32
            Dim UNKOWN As Global.System.Int32
            Return n
        End Function

    End Class

End Namespace