Imports Microsoft.VisualBasic

Namespace CE

    Public Class clsDatosCabecera_Calculados_CE

        Private _dbU_ATJUEGO As Double
        Private _dbU_PTJUEGO As Double
        Private _dbU_PNJUEGO As Double
        Private _dbU_TM2 As Double
        Private _dbU_TKG As Double
        Private _dbU_TRIM As Double
        Private _dbU_DESCUENTO As Double
        Private _dbU_COSTESP As Double
        Private _dbU_PORCOMISION As Double
        Private _dbU_COMXMILLAR As Double
        Private _dbU_COMISION As Double
        Private _dbU_DREAL As Double
        Private _dbU_PK As Double
        Private _dbU_PKR As Double
        Private _dbU_COFERTA As Double

        'HRS 17-08-2017 IL_FLETES
        Private _dbU_Costo_Flete As Double
        'HRS 17-08-2017 IL_Costos_Cab_Papel  
        Private _dbU_CtoPapel As Double
        'HRS 17-08-2017 IL_Costos_Cab_PSC 
        Private _dbU_CtopSC As Double
        'HRS 17-08-2017 IL_Costos_Cab_PCC
        Private _dbU_CtopCC As Double

        Public Property dbU_ATJUEGO() As Double
            Get
                Return _dbU_ATJUEGO
            End Get
            Set(ByVal value As Double)
                _dbU_ATJUEGO = value
            End Set
        End Property

        Public Property dbU_PTJUEGO() As Double
            Get
                Return _dbU_PTJUEGO
            End Get
            Set(ByVal value As Double)
                _dbU_PTJUEGO = value
            End Set
        End Property

        Public Property dbU_PNJUEGO() As Double
            Get
                Return _dbU_PNJUEGO
            End Get
            Set(ByVal value As Double)
                _dbU_PNJUEGO = value
            End Set
        End Property

        Public Property dbU_TM2() As Double
            Get
                Return _dbU_TM2
            End Get
            Set(ByVal value As Double)
                _dbU_TM2 = value
            End Set
        End Property

        Public Property dbU_TKG() As Double
            Get
                Return _dbU_TKG
            End Get
            Set(ByVal value As Double)
                _dbU_TKG = value
            End Set
        End Property

        Public Property dbU_TRIM() As Double
            Get
                Return _dbU_TRIM
            End Get
            Set(ByVal value As Double)
                _dbU_TRIM = value
            End Set
        End Property

        Public Property dbU_DESCUENTO() As Double
            Get
                Return _dbU_DESCUENTO
            End Get
            Set(ByVal value As Double)
                _dbU_DESCUENTO = value
            End Set
        End Property

        Public Property dbU_COSTESP() As Double
            Get
                Return _dbU_COSTESP
            End Get
            Set(ByVal value As Double)
                _dbU_COSTESP = value
            End Set
        End Property

        Public Property dbU_PORCOMISION() As Double
            Get
                Return _dbU_PORCOMISION
            End Get
            Set(ByVal value As Double)
                _dbU_PORCOMISION = value
            End Set
        End Property

        Public Property dbU_COMXMILLAR() As Double
            Get
                Return _dbU_COMXMILLAR
            End Get
            Set(ByVal value As Double)
                _dbU_COMXMILLAR = value
            End Set
        End Property

        Public Property dbU_COMISION() As Double
            Get
                Return _dbU_COMISION
            End Get
            Set(ByVal value As Double)
                _dbU_COMISION = value
            End Set
        End Property

        Public Property dbU_DREAL() As Double
            Get
                Return _dbU_DREAL
            End Get
            Set(ByVal value As Double)
                _dbU_DREAL = value
            End Set
        End Property

        Public Property dbU_PK() As Double
            Get
                Return _dbU_PK
            End Get
            Set(ByVal value As Double)
                _dbU_PK = value
            End Set
        End Property

        Public Property dbU_PKR() As Double
            Get
                Return _dbU_PKR
            End Get
            Set(ByVal value As Double)
                _dbU_PKR = value
            End Set
        End Property

        Public Property dbU_COFERTA() As Double
            Get
                Return _dbU_COFERTA
            End Get
            Set(ByVal value As Double)
                _dbU_COFERTA = value
            End Set
        End Property

        'HRS 17-08-2017 IL_FLETES
        Public Property dbU_Costo_Flete() As Double
            Get
                Return _dbU_Costo_Flete
            End Get
            Set(ByVal value As Double)
                _dbU_Costo_Flete = value
            End Set
        End Property

        'HRS 17-08-2017 IL_Costos_Cab_Papel
        Public Property dbU_CtoPapel() As Double
            Get
                Return _dbU_CtoPapel
            End Get
            Set(ByVal value As Double)
                _dbU_CtoPapel = value
            End Set
        End Property

        'HRS 17-08-2017 IL_Costos_Cab_PSC 
        Public Property dbU_CtopSC() As Double
            Get
                Return _dbU_CtopSC
            End Get
            Set(ByVal value As Double)
                _dbU_CtopSC = value
            End Set
        End Property

        'HRS 17-08-2017 IL_Costos_Cab_PCC
        Public Property dbU_CtopCC() As Double
            Get
                Return _dbU_CtopCC
            End Get
            Set(ByVal value As Double)
                _dbU_CtopCC = value
            End Set
        End Property

    End Class

End Namespace
