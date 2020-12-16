Imports Microsoft.VisualBasic

Namespace CE

    Public Class clsPreciosEspeciales_CE

        Private _strPropiedad As String
        Public Property strPropiedad() As String
            Get
                Return _strPropiedad
            End Get
            Set(ByVal value As String)
                _strPropiedad = value
            End Set
        End Property

        Private _dbPrecio As Double
        Public Property dbPrecio() As Double
            Get
                Return _dbPrecio
            End Get
            Set(ByVal value As Double)
                _dbPrecio = value
            End Set
        End Property

        Public Sub New(ByVal Propiedad As String, ByVal Precio As Double)
            MyBase.New()
            _strPropiedad = Propiedad
            _dbPrecio = Precio
        End Sub

    End Class

End Namespace

