Imports Microsoft.VisualBasic
Imports System.Xml
Imports System.Globalization
Imports System.IO

Namespace CN

    Public Class clsCalculoBarca_CN
        Inherits System.Web.UI.Page

        Public strNumeroError As String = ""
        Public strDetalleError As String = ""
        Private ws As DIS.DIServer
        Private lstCarrito As List(Of CE.clsArticulos_CE)
        Private oArticuloUno As CE.clsArticulos_CE
        Private oDatosCab_Calculados As CE.clsDatosCabecera_Calculados_CE
        Private oDatosCabecera As CE.clsDatosCabecera_CE
        Private Respuesta As XmlNode
        Private Respuesta2 As XmlNode
        Private docXML As New XmlDocument()
        Private m_nodelist As XmlNodeList

        Public Function CheckSalesOrder() As Boolean
            Dim blnResultado As Boolean = False
            Dim m_node As XmlNode
            Dim strTmp As String = "" 'Uso general
            Dim strTmp2 As String = "" 'Uso general

            Dim cantSAPTotal As Double = 0
            Dim iTem As String = ""
            Dim sItem As String = ""
            Dim squery As String = ""
            Dim counterPrincipal As Integer
            Dim millares As Double
            Dim piezasJuego As Double
            Dim precioVenta As Double
            Dim nPiezas As Double
            Dim nPrecioEsp As Double
            Dim ClienteDocumento As String = ""
            Dim areaTotal As Double
            Dim area As Double
            Dim pesoTotal As Double
            Dim peso As Double
            Dim precioNetoTotal As Double
            Dim nPrecioTrim As Double = 0
            Dim totalM2 As Double
            Dim totalKG As Double
            Dim descuento As Double
            Dim Desct_Otorgado As Double
            Dim porComision As Double
            Dim memo As String = ""
            Dim slpName As String = ""
            Dim descDir As Double = 0
            Dim precioDescuento As Double = 0
            Dim comisionPorMillar As Double = 0
            Dim comision As Double = 0
            Dim descuentoReal As Double = 0
            Dim pkr As Double = 0
            Dim pk As Double = 0

            'HRS 17-08-2017
            Dim dbCostoFlete As Double = 0
            Dim dbCostoPapelr As Double = 0
            Dim dbCtoPapel As Double = 0
            Dim dbCtopSC As Double = 0
            Dim dbCtopCC As Double = 0
            Dim intObjType As Integer = 0
            Dim dbPrecioPrimerRegistro As Double = 0
            Dim dbDocRate As Double = 1.0

            'HRS 17-08-2017 
            If Session("tipodocventas") = "pedido" Then
                intObjType = 17
            Else
                intObjType = 23
            End If

            'El campo U_TRIM de las lineas no se captura por lo tanto siempre es 0 en los calculos 
            Dim dbLocal_U_TRIM As Double = 0

            Try
                'El calculo siempre se hace, no se valida el campo "U_CALACT"
                'El campo U_TRIM de las lineas no se captura por lo tanto siempre es 0 en los calculos 

                If IsNothing(Session("lstCarrito")) Then
                    strNumeroError = "1"
                    strDetalleError = "No tiene articulos el carrito"
                Else
                    ws = New DIS.DIServer
                    lstCarrito = New List(Of CE.clsArticulos_CE)
                    oArticuloUno = New CE.clsArticulos_CE
                    oDatosCab_Calculados = New CE.clsDatosCabecera_Calculados_CE
                    oDatosCabecera = New CE.clsDatosCabecera_CE

                    lstCarrito = CType(Session("lstCarrito"), List(Of CE.clsArticulos_CE))

                    'HRS 17-08-2017 
                    If Not IsNothing(Session("oDatosCabecera")) Then
                        oDatosCabecera = CType(Session("oDatosCabecera"), CE.clsDatosCabecera_CE)
                    End If

                    Dim query1 = lstCarrito.OrderBy(Function(art As CE.clsArticulos_CE) art.intID).Take(1)
                    If query1.Count > 0 Then
                        oArticuloUno = query1.ToList(0)
                    End If

                    cantSAPTotal = oArticuloUno.dbCantidadPedido * 1000
                    iTem = oArticuloUno.strCodigoArticulo

                    For Each articulo In lstCarrito
                        If articulo.strCodigoArticulo <> "" Then
                            sItem = articulo.strCodigoArticulo

                            Try
                                squery = "select itmsgrpcod,itemcode,U_TF,QryGroup64, QryGroup62 from oitm  " & _
                                "where itemcode = '" & Trim(sItem) & "' " & _
                                ""
                                Respuesta = ws.ExecuteSQL(Session("Token"), squery)
                                docXML.LoadXml(Respuesta.InnerXml)
                                m_nodelist = docXML.GetElementsByTagName("row")
                                If m_nodelist.Count > 0 Then
                                    For Each m_node In m_nodelist
                                        'Si encontro datos
                                        If m_node.ChildNodes.Item(0).InnerText.ToString <> "" Then

                                            If m_node.ChildNodes.Item(0).InnerText.ToString = 105 And m_node.ChildNodes.Item(2).InnerText.ToString = "" Then
                                                strNumeroError = "2"
                                                strDetalleError = "El artículo" & sItem & " No tiene capturada su tarjeta de fabricación"
                                                Return False
                                            End If

                                            ' valida que solo existe un artículo principal "QryGroup64"
                                            If m_node.ChildNodes.Item(3).InnerText.ToString = "Y" Then
                                                counterPrincipal += 1
                                                If counterPrincipal > 1 Then
                                                    strNumeroError = "3"
                                                    strDetalleError = "Sólo puede haber un artículo de tipo Principal en la Orden de Venta."
                                                    Return False
                                                End If
                                            End If

                                            ' valida "QryGroup62"
                                            If m_node.ChildNodes.Item(4).InnerText.ToString = "Y" Then
                                                strNumeroError = "4"
                                                strDetalleError = "El artículo se encuentra cancelado."
                                                Return False
                                            End If

                                        End If
                                    Next
                                End If
                            Catch ex As Exception
                                strNumeroError = "-100"
                                strDetalleError = "Error " & squery & " - " & ex.Message
                                Return False
                            End Try

                            millares += articulo.dbCantidadPedido
                            If millares = 0 Then
                                millares += articulo.dbCantidad
                            End If

                            'Calculamos ATJUEGO
                            If articulo.intU_PJUEGO.ToString = "" Or articulo.intU_PJUEGO = 0 Then
                                piezasJuego = 1
                            Else
                                piezasJuego = CDbl(articulo.intU_PJUEGO)
                            End If

                            If articulo.dbPrecio.ToString = "" Or articulo.dbPrecio = 0 Then
                                precioVenta += 0
                            Else
                                'Valida dolares Item("17")
                                If Mid(articulo.strCurrency, 1, 1) = "U" Then
                                    precioVenta += articulo.dbPrecio * CDbl(Session("usdrate")) '* CDec(oMatrix.Columns.Item("16").Cells.Item(iAux).Specific.Value())
                                Else
                                    precioVenta += articulo.dbPrecio
                                    'precioVenta += oMatrix.Columns.Item("17").Cells.Item(iAux).Specific.Value()
                                End If
                            End If

                            'Revisamos especialidades ("U_PIEZASTARIMA")
                            If articulo.intU_PIEZASTARIMA.ToString <> "" Then
                                nPiezas = articulo.intU_PIEZASTARIMA
                            End If

                            'nPrecioEsp += ChangePrices(sItem, oMatrix.Columns.Item("U_VALESPECIAL").Cells.Item(iAux).Specific.Value(), nPiezas, oMatrix.Columns.Item("11").Cells.Item(iAux).Specific.Value(), oForm.Items.Item("4").Specific.value)
                            nPrecioEsp += ChangePrices(sItem, articulo.dbU_VALESPECIAL.ToString, nPiezas, articulo.dbCantidad, Session("RazCode"))

                            ClienteDocumento = Session("RazCode")

                            '"U_TRIM" AQUI Este dato se debe validar con el cliente
                            If dbLocal_U_TRIM > 0 Then
                                'nPrecioTrim += TrimPrice(sItem, oMatrix.Columns.Item("U_TRIM").Cells.Item(iAux).Specific.Value, oMatrix.Columns.Item("U_LPLIEGO").Cells.Item(iAux).Specific.Value, _
                                '                         oMatrix.Columns.Item("U_CREST").Cells.Item(iAux).Specific.Value)
                            End If

                            If articulo.strU_AREA = "" Then
                                areaTotal += 0
                                area = 0
                            Else
                                areaTotal += CDbl(articulo.strU_AREA) * piezasJuego
                                area = CDbl(articulo.strU_AREA)
                            End If

                            If articulo.strU_PESO = "" Then
                                pesoTotal += 0
                                peso = 0
                            Else
                                pesoTotal += CDbl(articulo.strU_PESO) * piezasJuego
                                peso = CDbl(articulo.strU_PESO)
                            End If

                            If articulo.dbU_PRECIONET.ToString = "" Or articulo.dbU_PRECIONET = 0 Then
                                precioNetoTotal += 0
                            Else
                                precioNetoTotal += CDbl(articulo.dbU_PRECIONET.ToString) * piezasJuego
                            End If

                            'HRS 17-08-2017 IL_Costos_Lin_Papel 
                            'HRS 18-12-2017 Se agrega el paramentro Fecha de Entrega
                            Dim strFechaEntregaLocal As String
                            If oDatosCabecera.strFechaEntrega Is Nothing Then
                                strFechaEntregaLocal = DateTime.Now.ToString("yyyyMMdd")
                            Else
                                strFechaEntregaLocal = DateTime.Parse(oDatosCabecera.strFechaEntrega).ToString("yyyyMMdd")
                            End If

                            Try
                                If intObjType > 0 Then
                                    dbCostoPapelr = 0
                                    squery = "EXEC IL_Costos_Lin_Papel " & intObjType & ",'" & Trim(sItem) & "','" & Trim(articulo.strU_CREST) & "','" & strFechaEntregaLocal & "'"
                                    Respuesta = ws.ExecuteSQL(Session("Token"), squery)
                                    docXML.LoadXml(Respuesta.InnerXml)
                                    m_nodelist = docXML.GetElementsByTagName("row")
                                    If m_nodelist.Count > 0 Then
                                        For Each m_node In m_nodelist
                                            'Si encontro datos
                                            If m_node.ChildNodes.Item(0).InnerText.ToString <> "" Then
                                                'Si es numerico
                                                If IsNumeric(m_node.ChildNodes.Item(0).InnerText.ToString) Then
                                                    dbCostoPapelr = CDbl(m_node.ChildNodes.Item(0).InnerText.ToString)
                                                End If

                                                articulo.dbU_Costopapelr = Math.Round(dbCostoPapelr, 4)
                                            End If
                                        Next
                                    End If
                                End If
                            Catch ex As Exception
                                strNumeroError = "-100"
                                strDetalleError = "Error " & squery & " - " & ex.Message
                                Return False
                            End Try
                            'FIN HRS 17-08-2017 IL_Costos_Lin_Papel

                            'HRS 17-08-2017 IL_Costos_Cab_Papel  
                            Try
                                If intObjType > 0 Then
                                    'SUM(U_Costopapelr)
                                    dbCtoPapel += Math.Round(dbCostoPapelr, 4)
                                End If
                            Catch ex As Exception
                            End Try
                            'FIN HRS 17-08-2017 IL_Costos_Cab_Papel  

                            'HRS 17-08-2017 Guardar Datos Carrito
                            Session("lstCarrito") = lstCarrito

                        End If 'fin If articulo.strCodigoArticulo <> ""
                    Next 'fin For Each articulo In lstCarrito

                    precioNetoTotal += nPrecioTrim
                    totalM2 += areaTotal * (cantSAPTotal)
                    totalKG += pesoTotal * (cantSAPTotal)

                    oDatosCab_Calculados.dbU_ATJUEGO = areaTotal
                    oDatosCab_Calculados.dbU_PTJUEGO = pesoTotal
                    oDatosCab_Calculados.dbU_PNJUEGO = precioNetoTotal.ToString("########.00")
                    oDatosCab_Calculados.dbU_TM2 = totalM2
                    oDatosCab_Calculados.dbU_TKG = totalKG
                    oDatosCab_Calculados.dbU_TRIM = nPrecioTrim.ToString("########.00")

                    If precioNetoTotal = 0 Then
                        descuento = 0
                    Else
                        descuento = 100 - Math.Round((precioVenta - nPrecioEsp) * 100 / precioNetoTotal, 2)
                        'descuento = (precioNetoTotal - precioVenta) / precioNetoTotal
                    End If

                    oDatosCab_Calculados.dbU_DESCUENTO = descuento
                    Desct_Otorgado = descuento
                    oDatosCab_Calculados.dbU_COSTESP = nPrecioEsp

                    'HRS 23-04-2018 Se sube el codigo
                    ' ------Cambio de Campo descuento a U_CTOPPSC en query U_Comision Inicia Stores Procedures---------
                    'HRS 17-08-2017 IL_FLETES
                    Try
                        If intObjType > 0 Then
                            dbCostoFlete = 0
                            'Obtenemos datos de domicilio
                            strTmp = ""
                            strTmp2 = ""
                            If oDatosCabecera.strDomicilio = "" Then
                                squery = "select top 1 [State],[City] from CRD1 with(nolock) where [CardCode] = '" & Session("RazCode") & "' and [AdresType] = 'S'"
                            Else
                                squery = "select top 1 [State],[City] from CRD1 with(nolock) where [CardCode] = '" & Session("RazCode") & "' and [Address] = '" & oDatosCabecera.strDomicilio.Trim & "' and [AdresType] = 'S'"
                            End If
                            Respuesta = ws.ExecuteSQL(Session("Token"), squery)
                            strTmp = ReadXML(Respuesta.InnerXml, "State")
                            strTmp2 = ReadXML(Respuesta.InnerXml, "City")
                            'Traer costo del flete
                            squery = "EXEC IL_Fletes " & intObjType & "," & oDatosCab_Calculados.dbU_PTJUEGO & ",'" & Session("RazCode") & "','" & strTmp & "','" & strTmp2 & "'"
                            Respuesta = ws.ExecuteSQL(Session("Token"), squery)
                            docXML.LoadXml(Respuesta.InnerXml)
                            m_nodelist = docXML.GetElementsByTagName("row")
                            If m_nodelist.Count > 0 Then
                                For Each m_node In m_nodelist
                                    'Si encontro datos
                                    If m_node.ChildNodes.Item(0).InnerText.ToString <> "" Then
                                        'Si es numerico
                                        If IsNumeric(m_node.ChildNodes.Item(0).InnerText.ToString) Then
                                            dbCostoFlete = CDbl(m_node.ChildNodes.Item(0).InnerText.ToString)
                                        End If
                                    End If
                                Next
                            End If
                            oDatosCab_Calculados.dbU_Costo_Flete = dbCostoFlete
                        End If
                    Catch ex As Exception
                        strNumeroError = "-100"
                        strDetalleError = "Error " & squery & " - " & ex.Message
                        Return False
                    End Try

                    'HRS 17-08-2017 IL_Costos_Cab_Papel  
                    Try
                        If intObjType > 0 Then
                            oDatosCab_Calculados.dbU_CtoPapel = dbCtoPapel
                        End If
                    Catch ex As Exception
                    End Try

                    'HRS 17-08-2017 IL_Costos_Cab_PSC
                    Try
                        If intObjType > 0 Then
                            dbCtopSC = 0
                            dbPrecioPrimerRegistro = 0
                            dbPrecioPrimerRegistro = oArticuloUno.dbPrecio

                            'Obtenemos el valor de DocRate
                            dbDocRate = 1.0
                            strTmp = ""
                            squery = "select isnull(case T0.[Currency]" & _
                                     " when '##' then 1.000000" & _
                                     " when '$' then 1.000000" & _
                                     " else (select top 1 isnull(T1.Rate,1.000000)" & _
                                     " from ORTT T1 with(nolock) where T1.RateDate = CONVERT(VARCHAR(10), GETDATE(), 112) and T1.[Currency] = T0.[Currency])" & _
                                     " end, 1.000000) DocRate" & _
                                     " from OCRD T0 with(nolock)" & _
                                     " where T0.[CardCode] = '" & Session("RazCode") & "' and T0.CardType = 'C'"

                            Respuesta = ws.ExecuteSQL(Session("Token"), squery)
                            strTmp = ReadXML(Respuesta.InnerXml, "DocRate")
                            If strTmp <> "" Then
                                If IsNumeric(strTmp) Then
                                    dbDocRate = CDbl(strTmp)
                                End If
                            End If

                            squery = "EXEC IL_Costos_Cab_PSC " & intObjType & _
                                                           "," & dbDocRate & _
                                                           "," & oDatosCab_Calculados.dbU_CtoPapel & _
                                                           "," & oDatosCab_Calculados.dbU_COSTESP & _
                                                           "," & oDatosCab_Calculados.dbU_Costo_Flete & _
                                                           "," & dbPrecioPrimerRegistro & ""

                            Respuesta = ws.ExecuteSQL(Session("Token"), squery)
                            docXML.LoadXml(Respuesta.InnerXml)
                            m_nodelist = docXML.GetElementsByTagName("row")
                            If m_nodelist.Count > 0 Then
                                For Each m_node In m_nodelist
                                    'Si encontro datos
                                    If m_node.ChildNodes.Item(0).InnerText.ToString <> "" Then
                                        'Si es numerico
                                        If IsNumeric(m_node.ChildNodes.Item(0).InnerText.ToString) Then
                                            dbCtopSC = CDbl(m_node.ChildNodes.Item(0).InnerText.ToString)
                                        End If
                                    End If
                                Next
                            End If
                            oDatosCab_Calculados.dbU_CtopSC = dbCtopSC
                        End If
                    Catch ex As Exception
                    End Try
                    'HRS FIN 23-04-2018 Se sube el codigo

                    'Trim(oForm.Items.Item("20").Specific.Value)
                    squery = "select memo,slpName from oslp where slpcode = '" & Session("usuCode") & "'"
                    porComision = 0
                    strTmp = ""

                    Respuesta = ws.ExecuteSQL(Session("Token"), squery)
                    memo = ReadXML(Respuesta.InnerXml, "memo").ToUpper
                    slpName = ReadXML(Respuesta.InnerXml, "slpName").ToUpper

                    If memo.Contains("DIRECTO") Then
                        squery = "select U_COMISION from [@com_ag_directos] where cast(code as decimal(18,2)) &lt;= " & descuento & " and cast(name as decimal(18,2)) &gt;= " & descuento & " "
                        Respuesta = ws.ExecuteSQL(Session("Token"), squery)
                        strTmp = ReadXML(Respuesta.InnerXml, "U_COMISION")
                        If strTmp = "" Then
                            porComision = 0
                        Else
                            If IsNumeric(strTmp) Then
                                porComision = CDbl(strTmp)
                            End If
                        End If
                    Else
                        'HRS 10-04-2018 
                        'Cambio de tabla [@com_comisionista] a tabla [@com_comisionista_P]
                        'HRS 23-04-2018
                        'Cambio "descuento" por "dbCtopSC"
                        squery = "select U_COMISION from [@com_comisionista_P] with(nolock) where cast(code as decimal(18,2)) &lt;= " & Math.Round(dbCtopSC, 2) & " and cast(name as decimal(18,2)) &gt;= " & Math.Round(dbCtopSC, 2) & " "
                        Respuesta = ws.ExecuteSQL(Session("Token"), squery)
                        strTmp = ReadXML(Respuesta.InnerXml, "U_COMISION")
                        If strTmp = "" Then
                            porComision = 0
                        Else
                            If IsNumeric(strTmp) Then
                                porComision = ReadXML(Respuesta.InnerXml, "U_COMISION")
                            End If
                        End If
                    End If

                    'valida comisión mínima
                    strTmp = ""
                    squery = "select isnull(U_PORCOM,0) as 'U_PORCOM' from OCRD where CardCode = '" & Session("RazCode") & "'"
                    Respuesta = ws.ExecuteSQL(Session("Token"), squery)
                    strTmp = ReadXML(Respuesta.InnerXml, "U_PORCOM")

                    If porComision < CDbl(strTmp) And CDbl(strTmp) <> 0 Then
                        porComision = CDbl(strTmp)
                    End If

                    oDatosCab_Calculados.dbU_PORCOMISION = porComision

                    strTmp = ""
                    squery = "select Code,isnull(U_ExpTipo,'0') as 'U_ExpTipo',U_CodigoCliente,U_ComiMin from [@BXP_XCPCNS] where Name = '" & Session("usuCode") & "' "
                    Respuesta = ws.ExecuteSQL(Session("Token"), squery)
                    strTmp = ReadXML(Respuesta.InnerXml, "Code")
                    If strTmp <> "" Then
                        strTmp = ReadXML(Respuesta.InnerXml, "U_ExpTipo")
                        If strTmp = "1" Then 'Comision 0
                            porComision = 0
                            oDatosCab_Calculados.dbU_PORCOMISION = porComision
                        Else 'Comision Especial
                            squery = "SELECT U_DESCR, U_DESCG, U_DESCD FROM [@BXP_RSTNC]  " & _
                                    "where Code = '" & Trim(oArticuloUno.strU_CREST) & "' "
                            Respuesta2 = ws.ExecuteSQL(Session("Token"), squery)
                            strTmp = ReadXML(Respuesta2.InnerXml, "U_DESCD")
                            If strTmp <> "" Then
                                descDir = CDbl(strTmp)
                            End If
                            precioDescuento = Math.Round(precioNetoTotal - (precioNetoTotal * descDir / 100), 2)

                            squery = "select U_COMISION from [@com_especiales] where Name= '" & iTem & "'"
                            Respuesta2 = ws.ExecuteSQL(Session("Token"), squery) ' modificacion carlos
                            strTmp = ReadXML(Respuesta2.InnerXml, "U_COMISION")

                            If Math.Round((precioVenta - nPrecioEsp) - precioDescuento, 2) < 0 Or strTmp <> "" Then
                                squery = "select U_COMISION from [@com_especiales] where Name= '" & iTem & "'"
                                Respuesta2 = ws.ExecuteSQL(Session("Token"), squery)
                                strTmp = ReadXML(Respuesta2.InnerXml, "U_COMISION")

                                If strTmp <> "" Then
                                    porComision = CDbl(strTmp)
                                Else
                                    porComision = 0 'No encontro el articulo en COMISIONES ESPECIALES
                                End If

                                strTmp = ReadXML(Respuesta.InnerXml, "U_ComiMin")
                                If strTmp <> "" And strTmp.ToString <> "0" Then
                                    porComision = CDbl(strTmp)
                                End If
                                comisionPorMillar = Math.Round((porComision * precioVenta) / 100, 2)
                            Else
                                comisionPorMillar = Math.Round((precioVenta - nPrecioEsp) - precioDescuento, 2)
                                porComision = Math.Round((comisionPorMillar / precioVenta) * 100, 2)
                            End If

                            oDatosCab_Calculados.dbU_PORCOMISION = porComision

                            strTmp = ReadXML(Respuesta.InnerXml, "U_CodigoCliente")
                            If ClienteDocumento = strTmp Then
                                oDatosCab_Calculados.dbU_PORCOMISION = 0
                                comisionPorMillar = 0
                            End If

                        End If
                    Else
                        comisionPorMillar = Math.Round((precioVenta * porComision) / 100, 2)
                    End If

                    oDatosCab_Calculados.dbU_COMXMILLAR = comisionPorMillar
                    comision = (comisionPorMillar * (cantSAPTotal / 1000))
                    oDatosCab_Calculados.dbU_COMISION = comision
                    descuentoReal = Math.Round(100 - ((precioVenta - comisionPorMillar - nPrecioEsp) * 100 / precioNetoTotal), 2)
                    oDatosCab_Calculados.dbU_DREAL = descuentoReal
                    pk = Math.Round(precioVenta / 1000 / pesoTotal, 2)
                    oDatosCab_Calculados.dbU_PK = pk

                    'HRS 13-03-2019
                    'Se resta el flete al PKR
                    ''pkr = Math.Round(((precioVenta - comisionPorMillar - nPrecioEsp) / 1000) / pesoTotal, 2)
                    pkr = (Math.Round(((precioVenta - comisionPorMillar - nPrecioEsp - dbCostoFlete) / 1000) / pesoTotal, 2))
                    oDatosCab_Calculados.dbU_PKR = pkr

                    strTmp = ""
                    squery = "select U_COSTO 'Costo_X_kg' from [@BA_COSTOS] where Code='" & Trim(oArticuloUno.strU_CREST) & "'"
                    Respuesta = ws.ExecuteSQL(Session("Token"), squery)
                    strTmp = ReadXML(Respuesta.InnerXml, "Costo_X_kg")
                    If strTmp = "" Then
                        strTmp = "0"
                    End If

                    Try
                        oDatosCab_Calculados.dbU_COFERTA = (((oDatosCab_Calculados.dbU_TKG * CDbl(strTmp)) + nPrecioEsp) / CDbl(Session("subtotal"))) * 100
                    Catch ex As Exception
                        oDatosCab_Calculados.dbU_COFERTA = 0
                    End Try

                    ' ''HRS 17-08-2017 IL_FLETES
                    ''Try
                    ''    If intObjType > 0 Then
                    ''        dbCostoFlete = 0
                    ''        'Obtenemos datos de domicilio
                    ''        strTmp = ""
                    ''        strTmp2 = ""
                    ''        If oDatosCabecera.strDomicilio = "" Then
                    ''            squery = "select top 1 [State],[City] from CRD1 with(nolock) where [CardCode] = '" & Session("RazCode") & "' and [AdresType] = 'S'"
                    ''        Else
                    ''            squery = "select top 1 [State],[City] from CRD1 with(nolock) where [CardCode] = '" & Session("RazCode") & "' and [Address] = '" & oDatosCabecera.strDomicilio.Trim & "' and [AdresType] = 'S'"
                    ''        End If
                    ''        Respuesta = ws.ExecuteSQL(Session("Token"), squery)
                    ''        strTmp = ReadXML(Respuesta.InnerXml, "State")
                    ''        strTmp2 = ReadXML(Respuesta.InnerXml, "City")
                    ''        'Traer costo del flete
                    ''        squery = "EXEC IL_Fletes " & intObjType & "," & oDatosCab_Calculados.dbU_PTJUEGO & ",'" & Session("RazCode") & "','" & strTmp & "','" & strTmp2 & "'"
                    ''        Respuesta = ws.ExecuteSQL(Session("Token"), squery)
                    ''        docXML.LoadXml(Respuesta.InnerXml)
                    ''        m_nodelist = docXML.GetElementsByTagName("row")
                    ''        If m_nodelist.Count > 0 Then
                    ''            For Each m_node In m_nodelist
                    ''                'Si encontro datos
                    ''                If m_node.ChildNodes.Item(0).InnerText.ToString <> "" Then
                    ''                    'Si es numerico
                    ''                    If IsNumeric(m_node.ChildNodes.Item(0).InnerText.ToString) Then
                    ''                        dbCostoFlete = CDbl(m_node.ChildNodes.Item(0).InnerText.ToString)
                    ''                    End If
                    ''                End If
                    ''            Next
                    ''        End If
                    ''        oDatosCab_Calculados.dbU_Costo_Flete = dbCostoFlete
                    ''    End If
                    ''Catch ex As Exception
                    ''    strNumeroError = "-100"
                    ''    strDetalleError = "Error " & squery & " - " & ex.Message
                    ''    Return False
                    ''End Try

                    ' ''HRS 17-08-2017 IL_Costos_Cab_Papel  
                    ''Try
                    ''    If intObjType > 0 Then
                    ''        oDatosCab_Calculados.dbU_CtoPapel = dbCtoPapel
                    ''    End If
                    ''Catch ex As Exception
                    ''End Try

                    ' ''HRS 17-08-2017 IL_Costos_Cab_PSC
                    ''Try
                    ''    If intObjType > 0 Then
                    ''        dbCtopSC = 0
                    ''        dbPrecioPrimerRegistro = 0
                    ''        dbPrecioPrimerRegistro = oArticuloUno.dbPrecio

                    ''        'Obtenemos el valor de DocRate
                    ''        dbDocRate = 1.0
                    ''        strTmp = ""
                    ''        squery = "select isnull(case T0.[Currency]" & _
                    ''                 " when '##' then 1.000000" & _
                    ''                 " when '$' then 1.000000" & _
                    ''                 " else (select top 1 isnull(T1.Rate,1.000000)" & _
                    ''                 " from ORTT T1 with(nolock) where T1.RateDate = CONVERT(VARCHAR(10), GETDATE(), 112) and T1.[Currency] = T0.[Currency])" & _
                    ''                 " end, 1.000000) DocRate" & _
                    ''                 " from OCRD T0 with(nolock)" & _
                    ''                 " where T0.[CardCode] = '" & Session("RazCode") & "' and T0.CardType = 'C'"

                    ''        Respuesta = ws.ExecuteSQL(Session("Token"), squery)
                    ''        strTmp = ReadXML(Respuesta.InnerXml, "DocRate")
                    ''        If strTmp <> "" Then
                    ''            If IsNumeric(strTmp) Then
                    ''                dbDocRate = CDbl(strTmp)
                    ''            End If
                    ''        End If

                    ''        squery = "EXEC IL_Costos_Cab_PSC " & intObjType & _
                    ''                                       "," & dbDocRate & _
                    ''                                       "," & oDatosCab_Calculados.dbU_CtoPapel & _
                    ''                                       "," & oDatosCab_Calculados.dbU_COSTESP & _
                    ''                                       "," & oDatosCab_Calculados.dbU_Costo_Flete & _
                    ''                                       "," & dbPrecioPrimerRegistro & ""

                    ''        Respuesta = ws.ExecuteSQL(Session("Token"), squery)
                    ''        docXML.LoadXml(Respuesta.InnerXml)
                    ''        m_nodelist = docXML.GetElementsByTagName("row")
                    ''        If m_nodelist.Count > 0 Then
                    ''            For Each m_node In m_nodelist
                    ''                'Si encontro datos
                    ''                If m_node.ChildNodes.Item(0).InnerText.ToString <> "" Then
                    ''                    'Si es numerico
                    ''                    If IsNumeric(m_node.ChildNodes.Item(0).InnerText.ToString) Then
                    ''                        dbCtopSC = CDbl(m_node.ChildNodes.Item(0).InnerText.ToString)
                    ''                    End If
                    ''                End If
                    ''            Next
                    ''        End If
                    ''        oDatosCab_Calculados.dbU_CtopSC = dbCtopSC
                    ''    End If
                    ''Catch ex As Exception
                    ''End Try

                    'HRS 17-08-2017 IL_Costos_Cab_PCC
                    Try
                        If intObjType > 0 Then
                            dbCtopCC = 0

                            squery = "EXEC IL_Costos_Cab_PCC " & intObjType & _
                                                           "," & dbDocRate & _
                                                           "," & oDatosCab_Calculados.dbU_CtoPapel & _
                                                           "," & oDatosCab_Calculados.dbU_COSTESP & _
                                                           "," & oDatosCab_Calculados.dbU_COMXMILLAR & _
                                                           "," & oDatosCab_Calculados.dbU_Costo_Flete & _
                                                           "," & dbPrecioPrimerRegistro & ""

                            Respuesta = ws.ExecuteSQL(Session("Token"), squery)
                            docXML.LoadXml(Respuesta.InnerXml)
                            m_nodelist = docXML.GetElementsByTagName("row")
                            If m_nodelist.Count > 0 Then
                                For Each m_node In m_nodelist
                                    'Si encontro datos
                                    If m_node.ChildNodes.Item(0).InnerText.ToString <> "" Then
                                        'Si es numerico
                                        If IsNumeric(m_node.ChildNodes.Item(0).InnerText.ToString) Then
                                            dbCtopCC = CDbl(m_node.ChildNodes.Item(0).InnerText.ToString)
                                        End If
                                    End If
                                Next
                            End If

                            oDatosCab_Calculados.dbU_CtopCC = dbCtopCC
                        End If
                    Catch ex As Exception
                    End Try

                    'Guardar Datos Calculados
                    Session("oDatosCabeceraCal") = oDatosCab_Calculados
                    Return True

                End If 'fin If IsNothing(Session("lstCarrito")) 

            Catch ex As Exception
                strNumeroError = "-500"
                strDetalleError = "Error CheckSalesOrder() " & ex.Message
            Finally
                lstCarrito = Nothing
                oArticuloUno = Nothing
                oDatosCab_Calculados = Nothing
                oDatosCabecera = Nothing
                ws = Nothing
            End Try

            Return blnResultado
        End Function

        Private Function ReadXML(Xml As String, NodeName As String) As String
            Dim reader As System.Xml.XmlTextReader = New System.Xml.XmlTextReader(New System.IO.StringReader(Xml))
            Dim valor As String
            valor = ""
            Do While (reader.Read())
                If reader.NodeType = System.Xml.XmlNodeType.Element Then
                    If reader.Name.ToUpper = NodeName.ToUpper Then
                        valor = reader.ReadElementContentAsString
                        Exit Do
                    End If
                End If
            Loop
            reader.Close()
            reader.Dispose()
            Return valor
        End Function

        Private Function ChangePrices(ByVal sItem As String, ByVal nValEsp As Double, ByVal nPiezas As Double, ByVal Cantidad As Double, ByVal Cliente As String) As Double
            Dim dbRespuesta As Double = 0
            Dim strTmp As String = "" 'Uso general
            Dim mlstCarrito As List(Of CE.clsArticulos_CE)
            Dim moArticulo As CE.clsArticulos_CE

            Dim nPrecioEsp As Double = 0
            Dim squery As String = ""
            Dim nImporte As Double = 0
            Dim nPrecio As Double = 0

            'HRS 07-Julio-2017
            Dim lstPreciosEspeciales As New List(Of CE.clsPreciosEspeciales_CE)
            Dim strPropiedad As String = ""

            Try
                'Traemos los datos del articulo
                mlstCarrito = New List(Of CE.clsArticulos_CE)
                mlstCarrito = CType(Session("lstCarrito"), List(Of CE.clsArticulos_CE))

                Dim query = mlstCarrito.Where(Function(art As CE.clsArticulos_CE) art.strCodigoArticulo = sItem)
                moArticulo = query.ToList(0)

                'valida tabla de precios especialidades
                squery = "select * from [@BXP_APRECIO]"

                Respuesta = ws.ExecuteSQL(Session("Token"), squery)
                docXML.LoadXml(Respuesta.InnerXml)
                m_nodelist = docXML.GetElementsByTagName("row")
                If m_nodelist.Count > 0 Then
                    For Each m_node In m_nodelist
                        'Si encontro datos
                        If m_node.ChildNodes.Item(0).InnerText.ToString <> "" Then

                            'U_PROPIEDAD
                            strTmp = m_node.ChildNodes.Item(4).InnerText.ToString

                            'HRS 07-Julio-2017
                            strPropiedad = strTmp

                            'Traemos valor de la propiedad del articulo
                            squery = "SELECT QryGroup" & CInt(strTmp).ToString & " as 'Propiedad' FROM OITM WHERE ItemCode = '" & sItem & "' "
                            Respuesta2 = ws.ExecuteSQL(Session("Token"), squery)
                            strTmp = ReadXML(Respuesta2.InnerXml, "Propiedad")

                            'If oItem.Properties(oRecordSet.Fields.Item("U_PROPIEDAD").Value) = BoYesNoEnum.tYES Then
                            If strTmp = "Y" Then
                                '"U_AJUSTE"
                                nImporte = CDbl(m_node.ChildNodes.Item(3).InnerText.ToString)

                                'U_ESQUEMA
                                Select Case CInt(m_node.ChildNodes.Item(2).InnerText.ToString)
                                    Case 0 'AREA: U_AREA >= U_VALMIN
                                        If CDbl(moArticulo.strU_AREA) >= CDbl(m_node.ChildNodes.Item(5).InnerText.ToString) And CDbl(moArticulo.strU_AREA) <= CDbl(m_node.ChildNodes.Item(6).InnerText.ToString) Then
                                            nPrecioEsp += nImporte * CDbl(moArticulo.strU_AREA) * 1000

                                            'HRS 07-Julio-2017
                                            AgregarListaPreciosEsp(strPropiedad,
                                                                   nImporte * CDbl(moArticulo.strU_AREA) * 1000,
                                                                   lstPreciosEspeciales)

                                        End If
                                    Case 1 'LARGO PLIEGO: U_LPLIEGO  >= U_VALMIN
                                        If moArticulo.dbU_LPLIEGO >= CDbl(m_node.ChildNodes.Item(5).InnerText.ToString) And moArticulo.dbU_LPLIEGO <= CDbl(m_node.ChildNodes.Item(6).InnerText.ToString) Then

                                            'HRS 23-Octubre-2017 Cambio solicitado Oscar (Se comento lo anterior)
                                            nPrecioEsp += (nImporte * (CDbl(moArticulo.dbU_LPLIEGO) / 100)) * 1000

                                            'HRS 07-Julio-2017
                                            AgregarListaPreciosEsp(strPropiedad,
                                                                   (nImporte * (CDbl(moArticulo.dbU_LPLIEGO) / 100)) * 1000,
                                                                   lstPreciosEspeciales)

                                            'nPrecioEsp += nImporte * CDbl(moArticulo.dbU_LPLIEGO) * 1000
                                            'AgregarListaPreciosEsp(strPropiedad,
                                            '                       nImporte * CDbl(moArticulo.dbU_LPLIEGO) * 1000,
                                            '                       lstPreciosEspeciales)

                                        End If
                                    Case 2 'ANCHO PLIEGO: U_APLIEGO >= U_VALMIN
                                        If moArticulo.dbU_APLIEGO >= CDbl(m_node.ChildNodes.Item(5).InnerText.ToString) And moArticulo.dbU_APLIEGO <= CDbl(m_node.ChildNodes.Item(6).InnerText.ToString) Then
                                            nPrecioEsp += nImporte * CDbl(moArticulo.dbU_APLIEGO) * 1000
                                        End If
                                    Case 3 'FONDO: U_F >= U_VALMIN
                                        If moArticulo.dbU_F >= CDbl(m_node.ChildNodes.Item(5).InnerText.ToString) And moArticulo.dbU_F <= CDbl(m_node.ChildNodes.Item(6).InnerText.ToString) Then
                                            nPrecioEsp += nImporte * CDbl(moArticulo.dbU_F) * 1000
                                        End If
                                    Case 4 'PEGADO /GRAPADO: U_VALESPECIAL >= U_VALMIN
                                        If moArticulo.dbU_VALESPECIAL >= CDbl(m_node.ChildNodes.Item(5).InnerText.ToString) And moArticulo.dbU_VALESPECIAL <= CDbl(m_node.ChildNodes.Item(6).InnerText.ToString) Then
                                            nPrecioEsp += nImporte * nValEsp * 1000

                                            'HRS 07-Julio-2017
                                            AgregarListaPreciosEsp(strPropiedad,
                                                                   nImporte * nValEsp * 1000,
                                                                   lstPreciosEspeciales)
                                        End If
                                    Case 5 ' tarimas
                                        If nPiezas > 0 Then
                                            nPrecioEsp += nImporte * Math.Round((1000 / nPiezas), 0)

                                            'HRS 07-Julio-2017
                                            AgregarListaPreciosEsp(strPropiedad,
                                                                   nImporte * Math.Round((1000 / nPiezas), 0),
                                                                   lstPreciosEspeciales)
                                        End If
                                    Case 6 ' tarimas especiales
                                        ' operacion de tarima especial
                                        ''nPiezas son las pzas x tarima 
                                        squery = "select U_CostoTarima from [@TARIMAS_ESPECIALES] where U_cliente='" & Cliente & "'  and code='" & sItem & "'"
                                        Respuesta2 = ws.ExecuteSQL(Session("Token"), squery)
                                        strTmp = ReadXML(Respuesta2.InnerXml, "U_CostoTarima")

                                        Dim calculo As Decimal
                                        If strTmp <> "" Then
                                            'calculo = Math.Round(((Cantidad * 1000) / nPiezas), 0) * oRecordSet2.Fields.Item("U_CostoTarima").Value
                                            '---- cambio en la formula a peticion de oscar
                                            calculo = Math.Round((1000 / nPiezas), 4) * CDbl(strTmp)
                                            If nPiezas > 0 Then
                                                nPrecioEsp += Math.Round(calculo, 3)

                                                'HRS 07-Julio-2017
                                                AgregarListaPreciosEsp(strPropiedad,
                                                                       Math.Round(calculo, 3),
                                                                       lstPreciosEspeciales)
                                            End If
                                        End If
                                    Case 7 ' DESVARBE
                                        nPrecioEsp += nImporte
                                        'HRS 07-Julio-2017
                                        AgregarListaPreciosEsp(strPropiedad,
                                                               nImporte,
                                                               lstPreciosEspeciales)

                                End Select

                            End If
                        End If
                    Next
                End If

                'recorrer la tabla de costos_pedidos
                squery = "select Name, U_Importe from [@costos_pedidos] where Name='" & sItem & "'"
                Respuesta = ws.ExecuteSQL(Session("Token"), squery)
                docXML.LoadXml(Respuesta.InnerXml)
                m_nodelist = docXML.GetElementsByTagName("row")
                If m_nodelist.Count > 0 Then
                    For Each m_node In m_nodelist
                        'Si encontro datos
                        If m_node.ChildNodes.Item(0).InnerText.ToString <> "" Then

                            nPrecioEsp += CDbl(m_node.ChildNodes.Item(1).InnerText.ToString)
                        End If
                    Next
                End If

                'HRS 07-Julio-2017 Precios independientes
                squery = "select Name, U_Importe from [@NOTAS_ALSEA] where Name='" & sItem & "'"
                Respuesta = ws.ExecuteSQL(Session("Token"), squery)
                docXML.LoadXml(Respuesta.InnerXml)
                m_nodelist = docXML.GetElementsByTagName("row")
                If m_nodelist.Count > 0 Then
                    For Each m_node In m_nodelist
                        'Si encontro datos
                        If m_node.ChildNodes.Item(0).InnerText.ToString <> "" Then
                            nPrecioEsp += CDbl(m_node.ChildNodes.Item(1).InnerText.ToString)
                        End If
                    Next
                End If

                'HRS 07-Julio-2017 Precios independientes
                PreciosIndepenientes(sItem, lstPreciosEspeciales)

            Catch ex As Exception
                strNumeroError = "-200"
                strDetalleError = "Error " & squery & " - " & ex.Message
            Finally
                lstPreciosEspeciales = Nothing
            End Try

            dbRespuesta = nPrecioEsp

            Return dbRespuesta
        End Function

        Private Sub AgregarListaPreciosEsp(ByVal strPropiedad As String,
                                           ByVal dbPrecio As Double,
                                           ByRef lstPreciosEspeciales As List(Of CE.clsPreciosEspeciales_CE))
            'HRS 07-Julio-2017 
            Dim lstTmp As New List(Of CE.clsPreciosEspeciales_CE)
            Try
                lstTmp = lstPreciosEspeciales.Where(Function(dato As CE.clsPreciosEspeciales_CE) dato.strPropiedad = strPropiedad).ToList()
                If lstTmp.Count > 0 Then
                    lstTmp.Single().dbPrecio = lstTmp.Single().dbPrecio + dbPrecio
                Else
                    lstPreciosEspeciales.Add(New CE.clsPreciosEspeciales_CE(strPropiedad, dbPrecio))
                End If
            Catch ex As Exception
            Finally
                lstTmp = Nothing
            End Try
        End Sub

        Private Sub PreciosIndepenientes(ByVal strArticulo As String,
                                         ByRef lstPreciosEspeciales As List(Of CE.clsPreciosEspeciales_CE))
            'HRS 07-Julio-2017 Precios independientes
            Dim dbPrecioLista As Double = 0
            Dim lstTmp As New List(Of CE.clsPreciosEspeciales_CE)
            Dim lstCarrito1 = New List(Of CE.clsArticulos_CE)

            Try
                'Obtenemos carrito actual
                lstCarrito1 = CType(Session("lstCarrito"), List(Of CE.clsArticulos_CE))

                For Each articulo In lstCarrito1
                    If articulo.strCodigoArticulo = strArticulo Then

                        'Michelman - Propiedad 18 + 19
                        lstTmp = lstPreciosEspeciales.Where(Function(dato As CE.clsPreciosEspeciales_CE) dato.strPropiedad = "18" Or dato.strPropiedad = "19").ToList()
                        dbPrecioLista = lstTmp.Sum(Function(d As CE.clsPreciosEspeciales_CE) d.dbPrecio)
                        articulo.dbU_IL_CE_Michelman = dbPrecioLista

                        'Michelman AC - Propiedad 41
                        lstTmp.Clear()
                        lstTmp = lstPreciosEspeciales.Where(Function(dato As CE.clsPreciosEspeciales_CE) dato.strPropiedad = "41").ToList()
                        dbPrecioLista = lstTmp.Sum(Function(d As CE.clsPreciosEspeciales_CE) d.dbPrecio)
                        articulo.dbU_IL_CE_MichelmanAC = dbPrecioLista

                        'OpenSesame - Propiedad 20
                        lstTmp.Clear()
                        lstTmp = lstPreciosEspeciales.Where(Function(dato As CE.clsPreciosEspeciales_CE) dato.strPropiedad = "20").ToList()
                        dbPrecioLista = lstTmp.Sum(Function(d As CE.clsPreciosEspeciales_CE) d.dbPrecio)
                        articulo.dbU_IL_CE_OpenSesame = dbPrecioLista

                        'Pegado - Propiedad 30
                        lstTmp.Clear()
                        lstTmp = lstPreciosEspeciales.Where(Function(dato As CE.clsPreciosEspeciales_CE) dato.strPropiedad = "30").ToList()
                        dbPrecioLista = lstTmp.Sum(Function(d As CE.clsPreciosEspeciales_CE) d.dbPrecio)
                        articulo.dbU_IL_CE_Pegado = dbPrecioLista

                        'Grapado - Propiedad 31
                        lstTmp.Clear()
                        lstTmp = lstPreciosEspeciales.Where(Function(dato As CE.clsPreciosEspeciales_CE) dato.strPropiedad = "31").ToList()
                        dbPrecioLista = lstTmp.Sum(Function(d As CE.clsPreciosEspeciales_CE) d.dbPrecio)
                        articulo.dbU_IL_CE_Grapado = dbPrecioLista

                        'Tarima - Propiedad 32
                        lstTmp.Clear()
                        lstTmp = lstPreciosEspeciales.Where(Function(dato As CE.clsPreciosEspeciales_CE) dato.strPropiedad = "32").ToList()
                        dbPrecioLista = lstTmp.Sum(Function(d As CE.clsPreciosEspeciales_CE) d.dbPrecio)
                        articulo.dbU_IL_CE_Tarima = dbPrecioLista

                        'WaterPaper - Propiedad 33 y 39
                        lstTmp.Clear()
                        lstTmp = lstPreciosEspeciales.Where(Function(dato As CE.clsPreciosEspeciales_CE) dato.strPropiedad = "33" Or dato.strPropiedad = "39").ToList()
                        dbPrecioLista = lstTmp.Sum(Function(d As CE.clsPreciosEspeciales_CE) d.dbPrecio)
                        articulo.dbU_IL_CE_WaterPaper = dbPrecioLista

                        'WaterPaper AC - Propiedad 40
                        lstTmp.Clear()
                        lstTmp = lstPreciosEspeciales.Where(Function(dato As CE.clsPreciosEspeciales_CE) dato.strPropiedad = "40").ToList()
                        dbPrecioLista = lstTmp.Sum(Function(d As CE.clsPreciosEspeciales_CE) d.dbPrecio)
                        articulo.dbU_IL_CE_WaterPaperAC = dbPrecioLista

                        'Emulsion - Propiedad 34
                        lstTmp.Clear()
                        lstTmp = lstPreciosEspeciales.Where(Function(dato As CE.clsPreciosEspeciales_CE) dato.strPropiedad = "34").ToList()
                        dbPrecioLista = lstTmp.Sum(Function(d As CE.clsPreciosEspeciales_CE) d.dbPrecio)
                        articulo.dbU_IL_CE_Emulsion = dbPrecioLista

                        'Strinking - Propiedad 35
                        lstTmp.Clear()
                        lstTmp = lstPreciosEspeciales.Where(Function(dato As CE.clsPreciosEspeciales_CE) dato.strPropiedad = "35").ToList()
                        dbPrecioLista = lstTmp.Sum(Function(d As CE.clsPreciosEspeciales_CE) d.dbPrecio)
                        articulo.dbU_IL_CE_Strinking = dbPrecioLista

                        'Cera - Propiedad 36
                        lstTmp.Clear()
                        lstTmp = lstPreciosEspeciales.Where(Function(dato As CE.clsPreciosEspeciales_CE) dato.strPropiedad = "36").ToList()
                        dbPrecioLista = lstTmp.Sum(Function(d As CE.clsPreciosEspeciales_CE) d.dbPrecio)
                        articulo.dbU_IL_CE_Cera = dbPrecioLista

                        'Maq Desvarbe - Propiedad 43
                        lstTmp.Clear()
                        lstTmp = lstPreciosEspeciales.Where(Function(dato As CE.clsPreciosEspeciales_CE) dato.strPropiedad = "43").ToList()
                        dbPrecioLista = lstTmp.Sum(Function(d As CE.clsPreciosEspeciales_CE) d.dbPrecio)
                        articulo.dbU_IL_CE_MaqDesvarbe = dbPrecioLista

                        'Tarima Especial - Propiedad 45
                        lstTmp.Clear()
                        lstTmp = lstPreciosEspeciales.Where(Function(dato As CE.clsPreciosEspeciales_CE) dato.strPropiedad = "45").ToList()
                        dbPrecioLista = lstTmp.Sum(Function(d As CE.clsPreciosEspeciales_CE) d.dbPrecio)
                        articulo.dbU_IL_CE_TarimaEsp = dbPrecioLista

                        'Maq Pegado - Propiedad 28
                        lstTmp.Clear()
                        lstTmp = lstPreciosEspeciales.Where(Function(dato As CE.clsPreciosEspeciales_CE) dato.strPropiedad = "28").ToList()
                        dbPrecioLista = lstTmp.Sum(Function(d As CE.clsPreciosEspeciales_CE) d.dbPrecio)
                        articulo.dbU_IL_CE_MaqPegado = dbPrecioLista

                        'Desvarbe - Propiedad 42
                        lstTmp.Clear()
                        lstTmp = lstPreciosEspeciales.Where(Function(dato As CE.clsPreciosEspeciales_CE) dato.strPropiedad = "42").ToList()
                        dbPrecioLista = lstTmp.Sum(Function(d As CE.clsPreciosEspeciales_CE) d.dbPrecio)
                        articulo.dbU_IL_CE_Desvarbe = dbPrecioLista

                        'Costos Pedidos - Propiedad OC
                        lstTmp.Clear()
                        lstTmp = lstPreciosEspeciales.Where(Function(dato As CE.clsPreciosEspeciales_CE) dato.strPropiedad = "OC").ToList()
                        dbPrecioLista = lstTmp.Sum(Function(d As CE.clsPreciosEspeciales_CE) d.dbPrecio)
                        articulo.dbU_IL_CE_OtrosCost = dbPrecioLista

                        'Nota Alsea - Propiedad NA
                        lstTmp.Clear()
                        lstTmp = lstPreciosEspeciales.Where(Function(dato As CE.clsPreciosEspeciales_CE) dato.strPropiedad = "NA").ToList()
                        dbPrecioLista = lstTmp.Sum(Function(d As CE.clsPreciosEspeciales_CE) d.dbPrecio)
                        articulo.dbU_IL_CE_NotaAlsea = dbPrecioLista

                        'Guardar Datos Carrito
                        Session("lstCarrito") = lstCarrito1
                        Exit For
                    End If
                Next
            Catch ex As Exception
            Finally
                lstTmp = Nothing
                lstCarrito1 = Nothing
            End Try
        End Sub

        Public Function FormDataEvent_FORM_DATA_ADD_Pedido(ByVal strObjectType As String,
                                                           ByVal strDocEntry As String) As Boolean
            Dim blnResultado As Boolean = False
            ''Dim oclsCostosPapel As CE.clsCostosPapel_CE
            ''Dim oListCostosPapel As List(Of CE.clsCostosPapel_CE)
            ''Dim m_node As XmlNode
            ''Dim strTmp As String = "" 'Uso general
            ''Dim strTmp2 As String = "" 'Uso general
            ''Dim squery As String = ""
            ''Dim dbU_Costopapelr As Double = 0
            ''Dim dbU_CtoPapel As Double = 0
            ''Dim dbU_CtopSC As Double = 0
            ''Dim dbU_CtopCC As Double = 0
            ''Dim dbU_Costo_Flete As Double = 0

            ''Try
            ''    ws = New DIS.DIServer
            ''    oListCostosPapel = New List(Of CE.clsCostosPapel_CE)

            ''    'HRS 07-Julio-2017
            ''    Try
            ''        squery = "EXEC IL_Fletes " & strDocEntry & "," & strObjectType
            ''        Respuesta = ws.ExecuteSQL(Session("Token"), squery)
            ''        strTmp = ReadXML(Respuesta.InnerXml, "Valor")
            ''        dbU_Costo_Flete = CDbl(strTmp)

            ''        squery = "UPDATE [ORDR] SET [U_Costo_Flete] = " & dbU_Costo_Flete & " WHERE [DocEntry] = " & strDocEntry
            ''        Respuesta = ws.ExecuteSQL(Session("Token"), squery)
            ''    Catch ex As Exception
            ''        strNumeroError = "-100"
            ''        strDetalleError = "Error " & squery & " - " & ex.Message
            ''        Return False
            ''    End Try

            ''    Try
            ''        squery = "EXEC IL_Costos_Lin_Papel " & strDocEntry & "," & strObjectType
            ''        Respuesta = ws.ExecuteSQL(Session("Token"), squery)
            ''        docXML.LoadXml(Respuesta.InnerXml)

            ''        m_nodelist = docXML.GetElementsByTagName("row")
            ''        If m_nodelist.Count > 0 Then
            ''            For Each m_node In m_nodelist
            ''                'Si encontro datos
            ''                If m_node.ChildNodes.Item(0).InnerText.ToString <> "" Then

            ''                    oclsCostosPapel = New CE.clsCostosPapel_CE
            ''                    oclsCostosPapel.IDArticulo = m_node.ChildNodes.Item(0).InnerText.ToString 'LineNum
            ''                    oclsCostosPapel.Valor = m_node.ChildNodes.Item(1).InnerText.ToString 'Valor
            ''                    oListCostosPapel.Add(oclsCostosPapel)

            ''                End If
            ''            Next
            ''        End If
            ''    Catch ex As Exception
            ''        strNumeroError = "-100"
            ''        strDetalleError = "Error " & squery & " - " & ex.Message
            ''        blnResultado = False
            ''    End Try

            ''    'Actualizamos el campo U_Costopapelr de las lineas del pedido, donde 
            ''    'De los LineNum de la lista oListCostosPapel
            ''    Try
            ''        squery = "select LineNum from RDR1 where DocEntry = " & strDocEntry
            ''        Respuesta = ws.ExecuteSQL(Session("Token"), squery)
            ''        docXML.LoadXml(Respuesta.InnerXml)
            ''        m_nodelist = docXML.GetElementsByTagName("row")
            ''        If m_nodelist.Count > 0 Then
            ''            For Each m_node In m_nodelist
            ''                'Si encontro datos
            ''                If m_node.ChildNodes.Item(0).InnerText.ToString <> "" Then
            ''                    Dim oQuery As System.Collections.Generic.IEnumerable(Of CE.clsCostosPapel_CE) = From obj As CE.clsCostosPapel_CE In oListCostosPapel
            ''                                                                                                    Where obj.IDArticulo = m_node.ChildNodes.Item(0).InnerText.ToString
            ''                                                                                                    Select obj
            ''                    If oQuery.Any Then
            ''                        dbU_Costopapelr = Math.Round(Convert.ToDouble(oQuery.Single.Valor.ToString), 4)

            ''                        squery = "UPDATE [RDR1] SET [U_Costopapelr] = " & dbU_Costopapelr & " WHERE [DocEntry] = " & strDocEntry & " and [LineNum] = '" & m_node.ChildNodes.Item(0).InnerText.ToString & "'"
            ''                        Respuesta = ws.ExecuteSQL(Session("Token"), squery)
            ''                    End If
            ''                End If
            ''            Next
            ''        End If
            ''    Catch ex As Exception
            ''        strNumeroError = "-101"
            ''        strDetalleError = "Error " & squery & " - " & ex.Message
            ''        Return False
            ''    End Try

            ''    Try
            ''        squery = "EXEC IL_Costos_Cab_Papel " & strDocEntry & "," & strObjectType
            ''        Respuesta = ws.ExecuteSQL(Session("Token"), squery)
            ''        strTmp = ReadXML(Respuesta.InnerXml, "Valor")
            ''        dbU_CtoPapel = CDbl(strTmp)

            ''        squery = "UPDATE [ORDR] SET [U_CtoPapel] = " & dbU_CtoPapel & " WHERE [DocEntry] = " & strDocEntry
            ''        Respuesta = ws.ExecuteSQL(Session("Token"), squery)
            ''    Catch ex As Exception
            ''        strNumeroError = "-100"
            ''        strDetalleError = "Error " & squery & " - " & ex.Message
            ''        Return False
            ''    End Try

            ''    Try
            ''        squery = "EXEC IL_Costos_Cab_PSC " & strDocEntry & "," & strObjectType
            ''        Respuesta = ws.ExecuteSQL(Session("Token"), squery)
            ''        strTmp = ReadXML(Respuesta.InnerXml, "Valor")
            ''        dbU_CtopSC = CDbl(strTmp)

            ''        squery = "UPDATE [ORDR] SET [U_CtopSC] = " & dbU_CtopSC & " WHERE [DocEntry] = " & strDocEntry
            ''        Respuesta = ws.ExecuteSQL(Session("Token"), squery)
            ''    Catch ex As Exception
            ''        strNumeroError = "-100"
            ''        strDetalleError = "Error " & squery & " - " & ex.Message
            ''        Return False
            ''    End Try

            ''    Try
            ''        squery = "EXEC IL_Costos_Cab_PCC " & strDocEntry & "," & strObjectType
            ''        Respuesta = ws.ExecuteSQL(Session("Token"), squery)
            ''        strTmp = ReadXML(Respuesta.InnerXml, "Valor")
            ''        dbU_CtopCC = CDbl(strTmp)

            ''        squery = "UPDATE [ORDR] SET [U_CtopCC] = " & dbU_CtopCC & " WHERE [DocEntry] = " & strDocEntry
            ''        Respuesta = ws.ExecuteSQL(Session("Token"), squery)
            ''    Catch ex As Exception
            ''        strNumeroError = "-100"
            ''        strDetalleError = "Error " & squery & " - " & ex.Message
            ''        Return False
            ''    End Try

            ''    blnResultado = True
            ''Catch ex As Exception
            ''    blnResultado = False
            ''Finally
            ''    oListCostosPapel = Nothing
            ''    ws = Nothing
            ''End Try

            Return blnResultado
        End Function

    End Class

End Namespace

