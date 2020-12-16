Imports Microsoft.VisualBasic

Namespace CE

    Public Class clsArticulos_CE

        Private _intID As Integer
        Private _strCodigoArticulo As String
        Private _strNombreArticulo As String
        Private _dbCantidad As Double
        Private _dbPrecio As Double
        Private _dbDescuento As Double
        Private _strAlmacen As String
        Private _strImpuesto As String
        Private _strNotaArticulo As String

        Private _dbCantidadPedido As Double 'Campo U_Cantidad
        
        Private _strCurrency As String '--Moneda Precio Unitario --BF: BA_Ultimoprecio_Oferta
        Private _dbU_L As Double '--Largo --BF: Bx_Cotizacion Largo
        Private _dbU_A As Double 'Ancho --BF: Bx_Cotizacion Ancho
        Private _dbU_F As Double 'Fondo --BF: Bx_Cotizacion Fondo
        Private _dbU_ESPSUP As Double 'ESP.SUP --Bx_Cotizacion Esp Superior
        Private _dbU_ESPINF As Double 'ESP.INF --Bx_Cotizacion Esp Inferior
        Private _strU_CREST As String 'Codigo Resistencia --BF: Bx_Cotizacion Codigo Resistencia
        Private _strU_TIPO As String 'Tipo --BF: Bx_Cotizacion Tipo
        Private _dbU_PRECIONET As Double 'Precio Neto --Bx_Cotizacion Precio neto
        Private _dbU_NORANU As Double 'NO. RANURAS INT CAJA --Bx_Cotizacion No Ranuras
        Private _dbU_LPLIEGO As Double 'LARGO PLIEGO --Bx_Cotizacion Largo pliego
        Private _dbU_APLIEGO As Double 'ANCHO PLIEGO --Bx_Cotizacion Ancho pliego
        Private _strU_CLRIMPR As String 'COLOR IMPRESION --Bx_Cotizacion Color impresion
        Private _intU_PORVENTAS As Integer 'PORCENTAJE VENTAS --Bx_Porcentaje_Ventas
        Private _strU_CIERRE As String 'CIERRE --Bx_Cotizacion Cierre
        Private _strU_TF As String 'TARJETA DE FABRICACIÓN --Bx_Cotizacion Tarjeta fabricante
        Private _strU_AREA As String 'AREA --Bx_Cotizacion Area
        Private _strU_PESO As String 'PESO --Bx_Cotizacion Peso
        Private _intU_PJUEGO As Integer 'Piezas por juego --Ba_Cotizacion Piezas
        Private _intU_PIEZASPATADO As Integer 'PIEZAS POR ATADO --Ba_Piezasatado
        Private _dbU_VALESPECIAL As Double 'PEGADO/GRABADO --BA_VALESPECIAL
        Private _intU_PIEZASTARIMA As Integer 'PIEZAS TARIMA --BA_PIEZASTAR

        Private _strU_CREST_Name As String 'Codigo Resistencia --BF: Bx_Cotizacion Codigo Resistencia
        Private _strU_TIPO_Name As String 'Tipo --BF: Bx_Cotizacion Tipo

        'HRS 07-Julio-2017 Precios independientes
        Private _dbU_IL_CE_Michelman As Double
        Private _dbU_IL_CE_MichelmanAC As Double
        Private _dbU_IL_CE_OpenSesame As Double
        Private _dbU_IL_CE_Pegado As Double
        Private _dbU_IL_CE_Grapado As Double
        Private _dbU_IL_CE_Tarima As Double
        Private _dbU_IL_CE_WaterPaper As Double
        Private _dbU_IL_CE_WaterPaperAC As Double
        Private _dbU_IL_CE_Emulsion As Double
        Private _dbU_IL_CE_Strinking As Double
        Private _dbU_IL_CE_Cera As Double
        Private _dbU_IL_CE_MaqDesvarbe As Double
        Private _dbU_IL_CE_TarimaEsp As Double
        Private _dbU_IL_CE_MaqPegado As Double
        Private _dbU_IL_CE_Desvarbe As Double
        Private _dbU_IL_CE_OtrosCost As Double
        Private _dbU_IL_CE_NotaAlsea As Double

        'HRS 17-08-2017
        Private _dbU_Costopapelr As Double
        
        Public Property intID() As Integer
            Get
                Return _intID
            End Get
            Set(ByVal value As Integer)
                _intID = value
            End Set
        End Property

        Public Property strCodigoArticulo() As String
            Get
                Return _strCodigoArticulo
            End Get
            Set(ByVal value As String)
                _strCodigoArticulo = value
            End Set
        End Property

        Public Property strNombreArticulo() As String
            Get
                Return _strNombreArticulo
            End Get
            Set(ByVal value As String)
                _strNombreArticulo = value
            End Set
        End Property

        Public Property dbCantidad() As Double
            Get
                Return _dbCantidad
            End Get
            Set(ByVal value As Double)
                _dbCantidad = value
            End Set
        End Property

        Public Property dbPrecio() As Double
            Get
                Return _dbPrecio
            End Get
            Set(ByVal value As Double)
                _dbPrecio = value
            End Set
        End Property

        Public Property dbDescuento() As Double
            Get
                Return _dbDescuento
            End Get
            Set(ByVal value As Double)
                _dbDescuento = value
            End Set
        End Property

        Public Property strAlmacen() As String
            Get
                Return _strAlmacen
            End Get
            Set(ByVal value As String)
                _strAlmacen = value
            End Set
        End Property

        Public Property strImpuesto() As String
            Get
                Return _strImpuesto
            End Get
            Set(ByVal value As String)
                _strImpuesto = value
            End Set
        End Property

        Public Property strNotaArticulo() As String
            Get
                Return _strNotaArticulo
            End Get
            Set(ByVal value As String)
                _strNotaArticulo = value
            End Set
        End Property

        Public Property dbCantidadPedido() As Double
            Get
                Return _dbCantidadPedido
            End Get
            Set(ByVal value As Double)
                _dbCantidadPedido = value
            End Set
        End Property

        Public Property strCurrency() As String
            Get
                Return _strCurrency
            End Get
            Set(ByVal value As String)
                _strCurrency = value
            End Set
        End Property

        Public Property dbU_L() As Double
            Get
                Return _dbU_L
            End Get
            Set(ByVal value As Double)
                _dbU_L = value
            End Set
        End Property

        Public Property dbU_A() As Double
            Get
                Return _dbU_A
            End Get
            Set(ByVal value As Double)
                _dbU_A = value
            End Set
        End Property

        Public Property dbU_F() As Double
            Get
                Return _dbU_F
            End Get
            Set(ByVal value As Double)
                _dbU_F = value
            End Set
        End Property

        Public Property dbU_ESPSUP() As Double
            Get
                Return _dbU_ESPSUP
            End Get
            Set(ByVal value As Double)
                _dbU_ESPSUP = value
            End Set
        End Property

        Public Property dbU_ESPINF() As Double
            Get
                Return _dbU_ESPINF
            End Get
            Set(ByVal value As Double)
                _dbU_ESPINF = value
            End Set
        End Property

        Public Property strU_CREST() As String
            Get
                Return _strU_CREST
            End Get
            Set(ByVal value As String)
                _strU_CREST = value
            End Set
        End Property

        Public Property strU_TIPO() As String
            Get
                Return _strU_TIPO
            End Get
            Set(ByVal value As String)
                _strU_TIPO = value
            End Set
        End Property

        Public Property dbU_PRECIONET() As Double
            Get
                Return _dbU_PRECIONET
            End Get
            Set(ByVal value As Double)
                _dbU_PRECIONET = value
            End Set
        End Property

        Public Property dbU_NORANU() As Double
            Get
                Return _dbU_NORANU
            End Get
            Set(ByVal value As Double)
                _dbU_NORANU = value
            End Set
        End Property

        Public Property dbU_LPLIEGO() As Double
            Get
                Return _dbU_LPLIEGO
            End Get
            Set(ByVal value As Double)
                _dbU_LPLIEGO = value
            End Set
        End Property

        Public Property dbU_APLIEGO() As Double
            Get
                Return _dbU_APLIEGO
            End Get
            Set(ByVal value As Double)
                _dbU_APLIEGO = value
            End Set
        End Property

        Public Property strU_CLRIMPR() As String
            Get
                Return _strU_CLRIMPR
            End Get
            Set(ByVal value As String)
                _strU_CLRIMPR = value
            End Set
        End Property

        Public Property intU_PORVENTAS() As Integer
            Get
                Return _intU_PORVENTAS
            End Get
            Set(ByVal value As Integer)
                _intU_PORVENTAS = value
            End Set
        End Property

        Public Property strU_CIERRE() As String
            Get
                Return _strU_CIERRE
            End Get
            Set(ByVal value As String)
                _strU_CIERRE = value
            End Set
        End Property

        Public Property strU_TF() As String
            Get
                Return _strU_TF
            End Get
            Set(ByVal value As String)
                _strU_TF = value
            End Set
        End Property

        Public Property strU_AREA() As String
            Get
                Return _strU_AREA
            End Get
            Set(ByVal value As String)
                _strU_AREA = value
            End Set
        End Property

        Public Property strU_PESO() As String
            Get
                Return _strU_PESO
            End Get
            Set(ByVal value As String)
                _strU_PESO = value
            End Set
        End Property

        Public Property intU_PJUEGO() As Integer
            Get
                Return _intU_PJUEGO
            End Get
            Set(ByVal value As Integer)
                _intU_PJUEGO = value
            End Set
        End Property

        Public Property intU_PIEZASPATADO() As Integer
            Get
                Return _intU_PIEZASPATADO
            End Get
            Set(ByVal value As Integer)
                _intU_PIEZASPATADO = value
            End Set
        End Property

        Public Property dbU_VALESPECIAL() As Double
            Get
                Return _dbU_VALESPECIAL
            End Get
            Set(ByVal value As Double)
                _dbU_VALESPECIAL = value
            End Set
        End Property

        Public Property intU_PIEZASTARIMA() As Integer
            Get
                Return _intU_PIEZASTARIMA
            End Get
            Set(ByVal value As Integer)
                _intU_PIEZASTARIMA = value
            End Set
        End Property

        Public Property strU_CREST_Name() As String
            Get
                Return _strU_CREST_Name
            End Get
            Set(ByVal value As String)
                _strU_CREST_Name = value
            End Set
        End Property

        Public Property strU_TIPO_Name() As String
            Get
                Return _strU_TIPO_Name
            End Get
            Set(ByVal value As String)
                _strU_TIPO_Name = value
            End Set
        End Property

        'HRS 07-Julio-2017 Precios independientes
        Public Property dbU_IL_CE_Michelman() As Double
            Get
                Return _dbU_IL_CE_Michelman
            End Get
            Set(ByVal value As Double)
                _dbU_IL_CE_Michelman = value
            End Set
        End Property

        Public Property dbU_IL_CE_MichelmanAC() As Double
            Get
                Return _dbU_IL_CE_MichelmanAC
            End Get
            Set(ByVal value As Double)
                _dbU_IL_CE_MichelmanAC = value
            End Set
        End Property

        Public Property dbU_IL_CE_OpenSesame() As Double
            Get
                Return _dbU_IL_CE_OpenSesame
            End Get
            Set(ByVal value As Double)
                _dbU_IL_CE_OpenSesame = value
            End Set
        End Property

        Public Property dbU_IL_CE_Pegado() As Double
            Get
                Return _dbU_IL_CE_Pegado
            End Get
            Set(ByVal value As Double)
                _dbU_IL_CE_Pegado = value
            End Set
        End Property

        Public Property dbU_IL_CE_Grapado() As Double
            Get
                Return _dbU_IL_CE_Grapado
            End Get
            Set(ByVal value As Double)
                _dbU_IL_CE_Grapado = value
            End Set
        End Property

        Public Property dbU_IL_CE_Tarima() As Double
            Get
                Return _dbU_IL_CE_Tarima
            End Get
            Set(ByVal value As Double)
                _dbU_IL_CE_Tarima = value
            End Set
        End Property

        Public Property dbU_IL_CE_WaterPaper() As Double
            Get
                Return _dbU_IL_CE_WaterPaper
            End Get
            Set(ByVal value As Double)
                _dbU_IL_CE_WaterPaper = value
            End Set
        End Property

        Public Property dbU_IL_CE_WaterPaperAC() As Double
            Get
                Return _dbU_IL_CE_WaterPaperAC
            End Get
            Set(ByVal value As Double)
                _dbU_IL_CE_WaterPaperAC = value
            End Set
        End Property

        Public Property dbU_IL_CE_Emulsion() As Double
            Get
                Return _dbU_IL_CE_Emulsion
            End Get
            Set(ByVal value As Double)
                _dbU_IL_CE_Emulsion = value
            End Set
        End Property

        Public Property dbU_IL_CE_Strinking() As Double
            Get
                Return _dbU_IL_CE_Strinking
            End Get
            Set(ByVal value As Double)
                _dbU_IL_CE_Strinking = value
            End Set
        End Property

        Public Property dbU_IL_CE_Cera() As Double
            Get
                Return _dbU_IL_CE_Cera
            End Get
            Set(ByVal value As Double)
                _dbU_IL_CE_Cera = value
            End Set
        End Property

        Public Property dbU_IL_CE_MaqDesvarbe() As Double
            Get
                Return _dbU_IL_CE_MaqDesvarbe
            End Get
            Set(ByVal value As Double)
                _dbU_IL_CE_MaqDesvarbe = value
            End Set
        End Property

        Public Property dbU_IL_CE_TarimaEsp() As Double
            Get
                Return _dbU_IL_CE_TarimaEsp
            End Get
            Set(ByVal value As Double)
                _dbU_IL_CE_TarimaEsp = value
            End Set
        End Property

        Public Property dbU_IL_CE_MaqPegado() As Double
            Get
                Return _dbU_IL_CE_MaqPegado
            End Get
            Set(ByVal value As Double)
                _dbU_IL_CE_MaqPegado = value
            End Set
        End Property

        Public Property dbU_IL_CE_Desvarbe() As Double
            Get
                Return _dbU_IL_CE_Desvarbe
            End Get
            Set(ByVal value As Double)
                _dbU_IL_CE_Desvarbe = value
            End Set
        End Property

        Public Property dbU_IL_CE_OtrosCost() As Double
            Get
                Return _dbU_IL_CE_OtrosCost
            End Get
            Set(ByVal value As Double)
                _dbU_IL_CE_OtrosCost = value
            End Set
        End Property

        Public Property dbU_IL_CE_NotaAlsea() As Double
            Get
                Return _dbU_IL_CE_NotaAlsea
            End Get
            Set(ByVal value As Double)
                _dbU_IL_CE_NotaAlsea = value
            End Set
        End Property

        'HRS 17-08-2017
        Public Property dbU_Costopapelr() As Double
            Get
                Return _dbU_Costopapelr
            End Get
            Set(ByVal value As Double)
                _dbU_Costopapelr = value
            End Set
        End Property

    End Class

End Namespace

