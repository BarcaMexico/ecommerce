Imports Microsoft.VisualBasic

Namespace CE

    Public Class clsDatosCabecera_CE

        Private _strU_PEDIDONO As String
        Private _strU_RECEP As String
        Private _strU_REF As String
        Private _strDomicilio As String
        Private _strArchCotizacion As String
        Private _strFechaEntrega As String
        Private _strFechaOrdenCompra As String

        Public Property strU_PEDIDONO() As String
            Get
                Return _strU_PEDIDONO
            End Get
            Set(ByVal value As String)
                _strU_PEDIDONO = value
            End Set
        End Property

        Public Property strU_RECEP() As String
            Get
                Return _strU_RECEP
            End Get
            Set(ByVal value As String)
                _strU_RECEP = value
            End Set
        End Property

        Public Property strU_REF() As String
            Get
                Return _strU_REF
            End Get
            Set(ByVal value As String)
                _strU_REF = value
            End Set
        End Property

        Public Property strDomicilio() As String
            Get
                Return _strDomicilio
            End Get
            Set(ByVal value As String)
                _strDomicilio = value
            End Set
        End Property

        Public Property strArchCotizacion() As String
            Get
                Return _strArchCotizacion
            End Get
            Set(ByVal value As String)
                _strArchCotizacion = value
            End Set
        End Property

        Public Property strFechaEntrega() As String
            Get
                Return _strFechaEntrega
            End Get
            Set(ByVal value As String)
                _strFechaEntrega = value
            End Set
        End Property

        Public Property strFechaOrdenCompra() As String
            Get
                Return _strFechaOrdenCompra
            End Get
            Set(ByVal value As String)
                _strFechaOrdenCompra = value
            End Set
        End Property

    End Class

End Namespace

