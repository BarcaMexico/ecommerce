Imports Microsoft.VisualBasic

Namespace CE

    Public Class clsCostosPapel_CE

        Private _IDArticulo As String
        Public Property IDArticulo() As String
            Get
                Return _IDArticulo
            End Get
            Set(ByVal value As String)
                _IDArticulo = value
            End Set
        End Property


        Private _Valor As String
        Public Property Valor() As String
            Get
                Return _Valor
            End Get
            Set(ByVal value As String)
                _Valor = value
            End Set
        End Property

    End Class

End Namespace

