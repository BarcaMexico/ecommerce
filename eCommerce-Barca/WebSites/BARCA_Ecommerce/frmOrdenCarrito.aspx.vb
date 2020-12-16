Imports Modulo

Imports System.Xml
Imports System.Globalization
Imports System.IO

Partial Class frmOrdenCarrito
    Inherits System.Web.UI.Page
    Public ws As DIS.DIServer

    Private Respuesta As XmlNode
    Private TablaPolo As String

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim rueba As String = "Prueba"
        
        If Not Page.IsPostBack Then
            TablaPolo = "<table style=' text-align: center;width: 100%; min-width: 600px;border: solid 1px #575656; background: #ffffff'><tr><th>Articulo</th><th>Cantidad</th><th>Precio unitario</th><th>Precio total</th></tr> "

            If IsNothing(Session("carritonumitems")) Then
                Response.Redirect("frmOrden.aspx")
            End If

            Dim tRow As New TableRow()
            Dim tCell As New TableCell()

            ws = New DIS.DIServer

            Dim consultafecha As String = ""

            Dim lstCarrito As New List(Of CE.clsArticulos_CE)
            Dim intCont As Integer = -1

            Dim carritoiva As ArrayList = New ArrayList
            Dim carritoDescuentoEDIT As ArrayList = New ArrayList
            Dim precioYaConDescuento As String = ""
            Dim mystring As String
            Dim Respuesta As XmlNode
            Dim restring As String
            Dim totalxarticulo As Double = 0
            Dim totaliva As Double = 0
            Dim totalivafinal As Double = 0
            Dim fechastring As String = DateTime.Now.ToString("yyyyMMdd")
            Dim subtotal As Double = 0
            Dim sqldato As String
            Dim banderaCar As Boolean = False
            Dim RowPolo As String = ""
            Dim errornumberr As Integer = 0

            Try
                errornumberr += 1
                Session("Sumaivas") = 0
                If IsNothing(Session("lstCarrito")) Then
                    errornumberr += 1
                Else
                    errornumberr += 1

                    lstCarrito = CType(Session("lstCarrito"), List(Of CE.clsArticulos_CE))

                    Try
                        errornumberr += 1
                        If Session("carritoDescuentoEDIT") = Nothing Then
                            banderaCar = True
                            errornumberr += 1
                        Else
                            carritoDescuentoEDIT = CType(Session("carritoDescuentoEDIT"), ArrayList)
                            errornumberr += 1
                        End If

                    Catch ex As Exception
                        errornumberr += 1
                        carritoDescuentoEDIT = CType(Session("carritoDescuentoEDIT"), ArrayList)
                    End Try

                    errornumberr += 1

                    For Each articulo In lstCarrito

                        errornumberr += 1
                        intCont = intCont + 1

                        RowPolo = "<tr>"

                        '-------------------------------------
                        'NOMBRE ARITCULO
                        '-------------------------------------
                        tRow = New TableRow()
                        tCell = New TableCell()
                        tCell.Text = articulo.strCodigoArticulo
                        tRow.Cells.Add(tCell)
                        RowPolo += "<td style='border: solid 1px #575656; background: #ffffff'>" & articulo.strCodigoArticulo & "</td>"

                        errornumberr += 1

                        '-------------------------------------
                        'CANTIDAD
                        '-------------------------------------
                        tCell = New TableCell()
                        tCell.Text = articulo.dbCantidad.ToString '"<input id='row" & intCont & "' type='text' size='4' value='" & articulo.dbCantidad.ToString & "' onchange='myFunction(this.value,id)'>"
                        tCell.HorizontalAlign = HorizontalAlign.Center
                        tRow.Cells.Add(tCell)
                        RowPolo += "<td style='border: solid 1px #575656; background: #ffffff'>" & articulo.dbCantidad.ToString & "</td>"

                        errornumberr += 1

                        '-------------------------------------
                        'CANTIDAD PEDIDO
                        '-------------------------------------
                        tCell = New TableCell()
                        tCell.Text = "<input id='row" & articulo.strCodigoArticulo.ToString.Trim & "' type='text' size='6' value='" & articulo.dbCantidadPedido.ToString & "' onchange='myFunction(this.value,id)'>"
                        tCell.HorizontalAlign = HorizontalAlign.Center
                        tRow.Cells.Add(tCell)
                        'RowPolo += "<td style='border: solid 1px #575656; background: #ffffff'>" & articulo.dbCantidadPedido.ToString & "</td>"

                        errornumberr += 1

                        '-------------------------------------
                        'PRECIO ORIGINAL
                        '-------------------------------------
                        tCell = New TableCell()
                        errornumberr += 1

                        'mystring = "<env:Envelope xmlns:env='http://schemas.xmlsoap.org/soap/envelope/'><env:Header><SessionID>" & Session("Token") & "</SessionID></env:Header><env:Body><dis:GetItemPrice xmlns:dis='http://www.sap.com/SBO/DIS'><CardCode>" & Session("RazCode") & "</CardCode><ItemCode>" & articulo.strCodigoArticulo & "</ItemCode><Quantity>" & articulo.dbCantidad.ToString & "</Quantity><Date>" & fechastring & "</Date></dis:GetItemPrice></env:Body></env:Envelope>"
                        'errornumberr += 1
                        'restring = ws.Interact(Session("Token"), mystring)
                        'errornumberr += 1

                        tCell.Text = articulo.strCurrency & " " & String.Format("{0:N}", articulo.dbPrecio)
                        errornumberr += 1
                        precioYaConDescuento = Convert.ToDouble(articulo.dbPrecio)
                        errornumberr += 1
                        tRow.Cells.Add(tCell)
                        errornumberr += 1
                        RowPolo += "<td style='border: solid 1px #575656; background: #ffffff'>" & tCell.Text & "</td>"
                        errornumberr += 1
                        
                        '-------------------------------------
                        'PRECIO * CANTIDAD = TOTAL DE LINEA
                        '-------------------------------------
                        tCell = New TableCell()
                        totalxarticulo = CDbl(precioYaConDescuento) * articulo.dbCantidad
                        totalxarticulo = Convert.ToDouble(totalxarticulo)
                        tCell.Text = Session("RazMON") & " " & Convert.ToDouble(totalxarticulo.ToString).ToString("N2", CultureInfo.CreateSpecificCulture("en-US"))

                        RowPolo += "<td style='border: solid 1px #575656; background: #ffffff'>" & tCell.Text & "</td></tr>"
                        ''TablaPolo += RowPolo
                        tRow.Cells.Add(tCell)
                        errornumberr += 1

                        'Dim monedaartic As String = ReadXML(restring, "Currency")
                        subtotal = subtotal + totalxarticulo
                        errornumberr += 1

                        '-------------------------------------
                        'CODIGO RESISTENCIA
                        '-------------------------------------
                        tCell = New TableCell()
                        tCell.Text = articulo.strU_CREST_Name.ToString
                        tCell.HorizontalAlign = HorizontalAlign.Left
                        tRow.Cells.Add(tCell)
                        'RowPolo += "<td style='border: solid 1px #575656; background: #ffffff'>" & tCell.Text & "</td>"
                        errornumberr += 1

                        '-------------------------------------
                        'TIPO
                        '-------------------------------------
                        tCell = New TableCell()
                        tCell.Text = articulo.strU_TIPO_Name.ToString
                        tCell.HorizontalAlign = HorizontalAlign.Left
                        tRow.Cells.Add(tCell)
                        'RowPolo += "<td style='border: solid 1px #575656; background: #ffffff'>" & tCell.Text & "</td>"
                        errornumberr += 1

                        '-------------------------------------
                        'LARGO PLIEGO
                        '-------------------------------------
                        tCell = New TableCell()
                        tCell.Text = articulo.dbU_LPLIEGO.ToString
                        tCell.HorizontalAlign = HorizontalAlign.Left
                        tRow.Cells.Add(tCell)
                        'RowPolo += "<td style='border: solid 1px #575656; background: #ffffff'>" & tCell.Text & "</td>"
                        errornumberr += 1

                        '-------------------------------------
                        'ANCHO PLIEGO
                        '-------------------------------------
                        tCell = New TableCell()
                        tCell.Text = articulo.dbU_APLIEGO.ToString
                        tCell.HorizontalAlign = HorizontalAlign.Left
                        tRow.Cells.Add(tCell)
                        'RowPolo += "<td style='border: solid 1px #575656; background: #ffffff'>" & tCell.Text & "</td>"
                        errornumberr += 1

                        '-------------------------------------
                        'AREA
                        '-------------------------------------
                        tCell = New TableCell()
                        tCell.Text = articulo.strU_AREA.ToString
                        tCell.HorizontalAlign = HorizontalAlign.Left
                        tRow.Cells.Add(tCell)
                        'RowPolo += "<td style='border: solid 1px #575656; background: #ffffff'>" & tCell.Text & "</td>"
                        errornumberr += 1

                        TablaPolo += RowPolo

                        sqldato = " SELECT top 20 t2.ItemCode,  t3.rate   FROM   OITM t2  inner join  OSTC t3 on '" & articulo.strImpuesto & "'=t3.Code   where t2.ItemCode ='" & articulo.strCodigoArticulo & "' "
                        Respuesta = ws.ExecuteSQL(Session("Token"), sqldato)
                        totaliva = ReadXML(Respuesta.InnerXml, "rate")
                        errornumberr += 1

                        sqldato = "   select top 1 vatliable from oitm  where ItemCode ='" & articulo.strCodigoArticulo & "' "
                        Respuesta = ws.ExecuteSQL(Session("Token"), sqldato)
                        Dim vatliable As String = ReadXML(Respuesta.InnerXml, "vatliable")
                        If vatliable = "Y" Then
                            carritoiva.Insert(intCont, articulo.strImpuesto)
                        Else
                            totaliva = 0
                            carritoiva.Insert(intCont, "")
                        End If

                        If totaliva > 0 Then
                            Session("Sumaivas") = Session("Sumaivas") + (totalxarticulo * totaliva / 100)
                            mystring = Session("Sumaivas")
                        End If
                        Table1.Rows.Add(tRow)

                    Next

                End If

                Session("ivas") = carritoiva
                tRow = New TableRow()
                For x = 1 To 3
                    tCell = New TableCell()
                    tCell.Text = " "
                    tRow.Cells.Add(tCell)
                Next
                tCell = New TableHeaderCell()
                tCell.Text = "Subtotal"
                tCell.HorizontalAlign = HorizontalAlign.Right
                tRow.Cells.Add(tCell)
                tCell = New TableCell()
                'aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa 

                '-------------------------------------
                'SUBTOTAL
                '-------------------------------------
                tCell.Text = Session("RazMON") & " " & String.Format("{0:N}", Convert.ToDouble(subtotal))
                RowPolo = " <tr><td></td><td></td><th style='border: solid 1px #575656; background: #ffffff'>Subtotoal</th><td style='border: solid 1px #575656; background: #ffffff'>" & tCell.Text & "</td></tr>"
                tRow.Cells.Add(tCell)
                Table1.Rows.Add(tRow)
                tRow = New TableRow()
                For x = 1 To 3
                    tCell = New TableCell()
                    tCell.Text = " "
                    tRow.Cells.Add(tCell)
                Next
                tCell = New TableHeaderCell()
                tCell.Text = "Impuesto"
                tRow.Cells.Add(tCell)
                tCell = New TableCell()

                Session("subtotal") = subtotal

                '-------------------------------------
                'IMPUESTO
                '-------------------------------------
                tCell.Text = Session("RazMON") & " " & String.Format("{0:N}", Session("Sumaivas"))
                RowPolo += " <tr><td></td><td></td><th style='border: solid 1px #575656; background: #ffffff'>Impuesto</th><td style='border: solid 1px #575656; background: #ffffff'>" & tCell.Text & "</td></tr>"
                tRow.Cells.Add(tCell)
                Table1.Rows.Add(tRow)
                tRow = New TableRow()
                For x = 1 To 3
                    tCell = New TableCell()
                    tCell.Text = " "
                    tRow.Cells.Add(tCell)
                Next
                tCell = New TableHeaderCell()
                tCell.Text = "Total"
                tCell.HorizontalAlign = HorizontalAlign.Right
                tRow.Cells.Add(tCell)
                tCell = New TableCell()

                '-------------------------------------
                'TOTAL
                '-------------------------------------
                tCell.Text = Session("RazMON") & " " & String.Format("{0:N}", Convert.ToDouble(subtotal) + Session("Sumaivas"))
                RowPolo += " <tr><td ></td><td></td><th style='border: solid 1px #575656; background: #ffffff'>Total</th><td style='border: solid 1px #575656; background: #ffffff'>" & tCell.Text & "</td></tr>"

                '------------------Tabla Envio Mail-------------------
                TablaPolo += RowPolo
                If Session("usutipo") = "venta" Then
                    TablaPolo = "Socio de Negocio: " & Session("RazName") & "<br><br>" & TablaPolo
                Else
                    TablaPolo = "Socio de Negocio: " & Session("usuName") & "<br><br>" & TablaPolo
                End If
                Session("pedidoTableHTML") = TablaPolo

                tRow.Cells.Add(tCell)
                Table1.Rows.Add(tRow)
                Session("carritoDescuentoEDIT") = carritoDescuentoEDIT
                ClientScript.RegisterStartupScript(Me.[GetType](), "aleasrt", "alert(' " & RowPolo & " '); ", True)

                'Agregamos Datos Generales (Datos de Cabecera)
                Dim oDatosCabecera As New CE.clsDatosCabecera_CE
                If Not IsNothing(Session("oDatosCabecera")) Then
                    Try
                        oDatosCabecera = CType(Session("oDatosCabecera"), CE.clsDatosCabecera_CE)
                        txtsCC_U_PEDIDONO.Value = oDatosCabecera.strU_PEDIDONO
                        txtsCC_U_RECEP.Value = oDatosCabecera.strU_RECEP
                        txtsCC_U_REF.Value = oDatosCabecera.strU_REF
                        txtsArchivo.Value = oDatosCabecera.strArchCotizacion
                        txtsDestino.Value = oDatosCabecera.strDomicilio

                        If IsDate(oDatosCabecera.strFechaEntrega) Then
                            txtsFechaEntrega.Value = CDate(oDatosCabecera.strFechaEntrega).ToString("dd-MM-yyyy")
                        Else
                            txtsFechaEntrega.Value = ""
                        End If

                        If IsDate(oDatosCabecera.strFechaOrdenCompra) Then
                            txtsFechaOrdenCompra.Value = CDate(oDatosCabecera.strFechaOrdenCompra).ToString("dd-MM-yyyy")
                        Else
                            txtsFechaOrdenCompra.Value = ""
                        End If

                    Catch ex As Exception
                    Finally
                        oDatosCabecera = Nothing
                    End Try
                Else
                    Try
                        txtsFechaEntrega.Value = Now.ToString("dd-MM-yyyy")
                        txtsFechaOrdenCompra.Value = Now.ToString("dd-MM-yyyy")
                        Session("oDatosCabecera") = oDatosCabecera
                    Catch ex As Exception
                    Finally
                        oDatosCabecera = Nothing
                    End Try
                End If

                'LLenamos Combo Destino de Datos Generales (Datos de Cabecera)
                Try
                    Dim docDestinos As New XmlDocument()
                    Dim m_nodelist As XmlNodeList
                    Dim m_node As XmlNode
                    Dim strQryDestino As String = ""
                    Try
                        cmbDestino.Items.Clear()
                        strQryDestino = "SELECT T1.[Address], isnull(T1.[Address],'') + ' ' + isnull(T1.[Street],'') + ' ' + isnull(T1.Block,'') + ' ' + isnull(T1.City,'') AS 'Direccion'"
                        strQryDestino = strQryDestino & " FROM OCRD T0  INNER JOIN CRD1 T1 ON T0.[CardCode] = T1.[CardCode]"
                        strQryDestino = strQryDestino & " where T0.CardCode = '" & Session("RazCode") & "' and AdresType = 'S'"
                        Respuesta = ws.ExecuteSQL(Session("Token"), strQryDestino)
                        docDestinos.LoadXml(Respuesta.InnerXml)
                        m_nodelist = docDestinos.GetElementsByTagName("row")
                        If m_nodelist.Count > 0 Then
                            For Each m_node In m_nodelist
                                'Si encontro destino lo agregamos al combo 
                                If m_node.ChildNodes.Item(0).InnerText.ToString <> "" Then
                                    cmbDestino.Items.Add(New ListItem(m_node.ChildNodes.Item(1).InnerText.ToString.Trim,
                                                                      m_node.ChildNodes.Item(0).InnerText.ToString.Trim))
                                End If
                            Next
                        Else
                            cmbDestino.Items.Add("")
                        End If

                        'Seleccionamos dato en el combo
                        If txtsDestino.Value.ToString <> "" Then
                            cmbDestino.ClearSelection()
                            cmbDestino.SelectedValue = txtsDestino.Value.ToString
                        End If
                        
                    Catch ex As Exception
                    Finally
                        docDestinos = Nothing
                    End Try
                Catch ex As Exception
                End Try

                'Agregamos los comentarios
                Try
                    If Not IsNothing(Session("strComentarios")) Then
                        TextArea1.InnerText = Session("strComentarios")
                    Else
                        TextArea1.InnerText = "Repetición"
                    End If
                Catch ex As Exception
                End Try

            Catch ex As Exception
                'Dim ectraoficial As String = ex.Message
                ClientScript.RegisterStartupScript(Me.[GetType](), "aleasrt", "alert(' " & ex.Message & " " & errornumberr & " " & fechastring & "  '); ", True)
            Finally
                ClientScript.RegisterStartupScript(Me.[GetType](), "aleasrt", "alert(' " & errornumberr & " '); ", True)
                lstCarrito = Nothing
            End Try

        End If

        'Guardar Imagen
        'If fupArchivoCot.HasFile Then
        '    Dim strtt As String = fupArchivoCot.FileName
        'End If

    End Sub
     
    Protected Sub secretbutton_ServerClick(sender As Object, e As EventArgs) Handles secretbutton.ServerClick
        Dim dbValor As Double = 0
        Dim confirmValue As String = ""

        Try
            'Actualizan cantidad del articulo
            confirmValue = Request.Form("confirm_value")

            If confirmValue = "Ok" Then

                Dim lstCarrito As New List(Of CE.clsArticulos_CE)
                dbValor = actuacan.Value
                actuacan.Value = dbValor

                If dbValor > 0 Then

                    ws = New DIS.DIServer
                    Try
                        'Dim carrito As ArrayList = New ArrayList
                        'Dim carritocan As ArrayList = New ArrayList
                        'Dim carritoprecio As ArrayList = New ArrayList
                        'Dim carritoitem As ArrayList = New ArrayList
                        'Dim carritoDescuentoEDIT As ArrayList = New ArrayList

                        If IsNothing(Session("lstCarrito")) Then
                        Else
                            lstCarrito = CType(Session("lstCarrito"), List(Of CE.clsArticulos_CE))

                            'carrito = CType(Session("carrito"), ArrayList)
                            'carritocan = CType(Session("carritocan"), ArrayList)
                            'carritoDescuentoEDIT = CType(Session("carritoDescuentoEDIT"), ArrayList)

                            'Dim preciosporarticulo As ArrayList = getItemPriceDiscountByPolo(Session("RazCode"), carrito(actuaid.Value), actuacan.Value, Session("Token"))

                            'carritoDescuentoEDIT(actuaid.Value) = Convert.ToDouble(preciosporarticulo(1))

                            Dim query = lstCarrito.Where(Function(art As CE.clsArticulos_CE) art.strCodigoArticulo = actuaitem.Value)
                            If query.Count > 0 Then
                                query.Single().dbCantidadPedido = dbValor

                                'Traemos la cantidad del articulo seleccionado y actualizar
                                Respuesta = ws.ExecuteSQL(Session("Token"), "EXEC [dbo].[IL_ECOM_OV_ART_CAN] 17,'" & actuaitem.Value & "', '" & query.Single().dbCantidadPedido & "','" & query.Single().intU_PORVENTAS & "'")
                                query.Single().dbCantidad = CDbl(ReadXML(Respuesta.InnerXml, "Quantity"))

                                'Si tiene hijos, actualizamos sus cantidades
                                Dim docRespuesta As New XmlDocument()
                                Dim m_nodelist As XmlNodeList
                                Dim m_node As XmlNode
                                Try
                                    Respuesta = ws.ExecuteSQL(Session("Token"), "EXEC [dbo].[IL_ECOM_OV_ART_HIJOS] 17,'" & actuaitem.Value & "','" & query.Single().dbCantidad & "','" & query.Single().dbCantidadPedido & "'")
                                    docRespuesta.LoadXml(Respuesta.InnerXml)
                                    m_nodelist = docRespuesta.GetElementsByTagName("row")
                                    If m_nodelist.Count > 0 Then
                                        For Each m_node In m_nodelist
                                            'Si encontro articulo hijo, actualizamos su cantidad 
                                            If m_node.ChildNodes.Item(0).InnerText.ToString <> "" Then
                                                Dim query1 = lstCarrito.Where(Function(art As CE.clsArticulos_CE) art.strCodigoArticulo = m_node.ChildNodes.Item(0).InnerText.ToString)
                                                If query1.Count > 0 Then
                                                    query1.Single().dbCantidad = CDbl(m_node.ChildNodes.Item(2).InnerText.ToString)
                                                    query1.Single().dbCantidadPedido = CDbl(m_node.ChildNodes.Item(3).InnerText.ToString)
                                                End If
                                            End If
                                        Next
                                    End If
                                Catch ex As Exception
                                Finally
                                    docRespuesta = Nothing
                                End Try
                                'Fin Hijos

                                Session("lstCarrito") = lstCarrito
                            End If

                            'carritocan(actuaid.Value) = actuacan.Value
                            'Session("carritocan") = carritocan
                            'Session("carritoDescuentoEDIT") = carritoDescuentoEDIT
                            Response.Redirect("frmOrdenCarrito.aspx")
                        End If
                    Catch ex As Exception
                        Response.Redirect("frmOrdenCarrito.aspx")
                    End Try
                Else

                    'en caso de ser cero lo que se pide se elimina del carrito
                    'Dim carrito As ArrayList = New ArrayList
                    'Dim carritocan As ArrayList = New ArrayList
                    'Dim carritoprecio As ArrayList = New ArrayList
                    'Dim carritoitem As ArrayList = New ArrayList
                    'Dim carritoDescuento As ArrayList = New ArrayList
                    'Dim carritoNotaArt As ArrayList = New ArrayList

                    If IsNothing(Session("lstCarrito")) Then
                    Else
                        lstCarrito = CType(Session("lstCarrito"), List(Of CE.clsArticulos_CE))
                        lstCarrito.RemoveAll(Function(art As CE.clsArticulos_CE) art.strCodigoArticulo = actuaitem.Value)
                        Session("lstCarrito") = lstCarrito

                        'carrito = CType(Session("carrito"), ArrayList)
                        'carritocan = CType(Session("carritocan"), ArrayList)
                        'carritoprecio = CType(Session("precio"), ArrayList)
                        'carritoitem = CType(Session("nom"), ArrayList)
                        'carritoDescuento = CType(Session("carritoDescuento"), ArrayList)
                        'carritoNotaArt = CType(Session("carritoNotaArt"), ArrayList)

                        'carrito.RemoveAt(actuaid.Value)
                        'carritocan.RemoveAt(actuaid.Value)
                        'carritoprecio.RemoveAt(actuaid.Value)
                        'carritoitem.RemoveAt(actuaid.Value)
                        'carritoDescuento.RemoveAt(actuaid.Value)
                        'carritoNotaArt.RemoveAt(actuaid.Value)

                        'Session("carritonumitems") = CInt(Session("carritonumitems")) - 1
                        'Session("carrito") = carrito
                        'Session("carritocan") = carritocan
                        'Session("precio") = carritoprecio
                        'Session("nom") = carritoitem
                        'Session("carritoDescuento") = carritoDescuento
                        'Session("carritoNotaArt") = carritoNotaArt

                        If lstCarrito.Count = 0 Then
                            Limpiarcarro_ServerClick("", Nothing)
                        End If
                        Response.Redirect("frmOrdenCarrito.aspx")
                    End If
                End If
            Else
                ClientScript.RegisterStartupScript(Me.[GetType](), "aleasrt", " document.location.href='frmOrdenCarrito';", True)
            End If

        Catch ex As Exception
            Response.Redirect("frmOrdenCarrito.aspx")
        End Try
    End Sub

    Protected Sub Limpiarcarro_ServerClick(sender As Object, e As EventArgs) Handles Limpiarcarro.ServerClick
        Session("lstCarrito") = Nothing
        Session("oDatosCabecera") = Nothing
        Session("oDatosCabeceraCal") = Nothing
        Session("strComentarios") = Nothing

        Session("carrito") = Nothing
        Session("carritocan") = Nothing
        Session("precio") = Nothing
        Session("nom") = Nothing
        Session("carritonumitems") = Nothing
        Session("ivas") = Nothing
        Session("carritoDescuento") = Nothing
        Session("carritoNotaArt") = Nothing
        Session("carritoDescuentoEDIT") = Nothing

        Response.Redirect("frmOrden.aspx")
    End Sub

    Protected Sub regresar_ServerClick(sender As Object, e As EventArgs) Handles regresar.ServerClick
        Response.Redirect("frmOrden.aspx")
    End Sub

    Protected Sub agregaorden()
        Dim answer As Date = Today 'Today.AddDays(15)
        Dim fechastring = answer.ToString("yyyyMMdd")
        Dim subtotal As Double = 0
        Dim orden As String = ""
        Dim intLineNum As Integer = 0

        Dim oCalculo As CN.clsCalculoBarca_CN
        Dim lstCarrito As List(Of CE.clsArticulos_CE)
        Dim oDatosCabecera As CE.clsDatosCabecera_CE
        Dim oDatosCabeceraCal As CE.clsDatosCabecera_Calculados_CE
        Dim strOrden As StringBuilder
        Dim strPath As String = ""
        Dim Respuesta As System.Xml.XmlNode

        Try
            If IsNothing(Session("lstCarrito")) Then
            Else
                oCalculo = New CN.clsCalculoBarca_CN
                If oCalculo.CheckSalesOrder() Then

                    strOrden = New StringBuilder
                    ws = New DIS.DIServer
                    lstCarrito = New List(Of CE.clsArticulos_CE)
                    oDatosCabecera = New CE.clsDatosCabecera_CE
                    oDatosCabeceraCal = New CE.clsDatosCabecera_Calculados_CE

                    lstCarrito = CType(Session("lstCarrito"), List(Of CE.clsArticulos_CE))
                    oDatosCabecera = CType(Session("oDatosCabecera"), CE.clsDatosCabecera_CE)
                    oDatosCabeceraCal = CType(Session("oDatosCabeceraCal"), CE.clsDatosCabecera_Calculados_CE)

                    If Session("tipodocventas") = "pedido" Then

                        strOrden.Append("<BOM xmlns='http://www.sap.com/SBO/DIS'><BO><AdmInfo><Object>oOrders</Object>")
                        strOrden.Append("</AdmInfo><Documents><row>")

                        'Fecha Entrega
                        Try
                            If oDatosCabecera.strFechaEntrega.ToString <> "" Then
                                If IsDate(oDatosCabecera.strFechaEntrega) Then
                                    strOrden.Append("<DocDueDate>" & CDate(oDatosCabecera.strFechaEntrega).ToString("yyyyMMdd") & "</DocDueDate>")
                                End If
                                If IsDate(oDatosCabecera.strFechaOrdenCompra) Then
                                    strOrden.Append("<U_Fechaentrega>" & CDate(oDatosCabecera.strFechaOrdenCompra).ToString("yyyyMMdd") & "</U_Fechaentrega>")
                                End If
                            Else
                                strOrden.Append("<DocDueDate>" & fechastring & "</DocDueDate>")
                                strOrden.Append("<U_Fechaentrega>" & fechastring & "</U_Fechaentrega>")
                            End If
                        Catch ex As Exception
                            strOrden.Append("<DocDueDate>" & fechastring & "</DocDueDate>")
                            strOrden.Append("<U_Fechaentrega>" & fechastring & "</U_Fechaentrega>")
                        End Try
                        
                        strOrden.Append("<CardCode>" & Session("RazCode") & "</CardCode>")
                        strOrden.Append("<Comments>" & TextArea1.InnerText & "</Comments>")
                        strOrden.Append("<SalesPersonCode>" & Session("usuCode") & "</SalesPersonCode>")

                        'Datos Cabecera
                        If oDatosCabecera.strDomicilio <> "" Then
                            strOrden.Append("<ShipToCode>" & oDatosCabecera.strDomicilio & "</ShipToCode>")
                        End If
                        strOrden.Append("<U_PEDIDONO>" & oDatosCabecera.strU_PEDIDONO & "</U_PEDIDONO>")
                        strOrden.Append("<U_DRECEP>" & oDatosCabecera.strU_RECEP & "</U_DRECEP>")
                        strOrden.Append("<U_Ref>" & oDatosCabecera.strU_REF & "</U_Ref>")

                        If oDatosCabecera.strArchCotizacion <> "" Then
                            Respuesta = ws.ExecuteSQL(Session("Token"), "select top 1 isnull(AttachPath,'') as 'AttachPath' from OADP")
                            strPath = ReadXML(Respuesta.InnerXml, "AttachPath")
                            strPath = [String].Format("{0}{1}", strPath, oDatosCabecera.strArchCotizacion)
                            strOrden.Append("<U_COTIZA1>" & strPath & "</U_COTIZA1>")
                        End If

                        'Datos Cabecera Calculados
                        strOrden.Append("<U_ATJUEGO>" & oDatosCabeceraCal.dbU_ATJUEGO.ToString & "</U_ATJUEGO>")
                        strOrden.Append("<U_PTJUEGO>" & oDatosCabeceraCal.dbU_PTJUEGO.ToString & "</U_PTJUEGO>")
                        strOrden.Append("<U_PNJUEGO>" & oDatosCabeceraCal.dbU_PNJUEGO.ToString & "</U_PNJUEGO>")
                        strOrden.Append("<U_TM2>" & oDatosCabeceraCal.dbU_TM2.ToString & "</U_TM2>")
                        strOrden.Append("<U_TKG>" & oDatosCabeceraCal.dbU_TKG.ToString & "</U_TKG>")
                        strOrden.Append("<U_TRIM>" & oDatosCabeceraCal.dbU_TRIM.ToString & "</U_TRIM>")
                        strOrden.Append("<U_DESCUENTO>" & oDatosCabeceraCal.dbU_DESCUENTO.ToString & "</U_DESCUENTO>")
                        strOrden.Append("<U_COSTESP>" & oDatosCabeceraCal.dbU_COSTESP.ToString & "</U_COSTESP>")
                        strOrden.Append("<U_PORCOMISION>" & oDatosCabeceraCal.dbU_PORCOMISION.ToString & "</U_PORCOMISION>")
                        strOrden.Append("<U_COMXMILLAR>" & oDatosCabeceraCal.dbU_COMXMILLAR.ToString & "</U_COMXMILLAR>")
                        strOrden.Append("<U_COMISION>" & oDatosCabeceraCal.dbU_COMISION.ToString & "</U_COMISION>")
                        strOrden.Append("<U_DREAL>" & oDatosCabeceraCal.dbU_DREAL.ToString & "</U_DREAL>")
                        strOrden.Append("<U_PK>" & oDatosCabeceraCal.dbU_PK.ToString & "</U_PK>")
                        strOrden.Append("<U_PKR>" & oDatosCabeceraCal.dbU_PKR.ToString & "</U_PKR>")
                        strOrden.Append("<U_COFERTA>" & oDatosCabeceraCal.dbU_COFERTA.ToString & "</U_COFERTA>")

                        'HRS 17-08-2017 Guardar informacion en el Pedido
                        strOrden.Append("<U_CtoPapel>" & oDatosCabeceraCal.dbU_CtoPapel.ToString & "</U_CtoPapel>")
                        strOrden.Append("<U_CtopSC>" & oDatosCabeceraCal.dbU_CtopSC.ToString & "</U_CtopSC>")
                        strOrden.Append("<U_CtopCC>" & oDatosCabeceraCal.dbU_CtopCC.ToString & "</U_CtopCC>")
                        strOrden.Append("<U_Costo_Flete>" & oDatosCabeceraCal.dbU_Costo_Flete.ToString & "</U_Costo_Flete>")

                        strOrden.Append("</row></Documents>")
                        strOrden.Append("<Document_Lines>")

                        'orden = "" &
                        '    "<BOM xmlns='http://www.sap.com/SBO/DIS'><BO><AdmInfo><Object>oOrders</Object>" &
                        '    "</AdmInfo><Documents><row> <DocDueDate>" & fechastring & "</DocDueDate><Comments>" & TextArea1.InnerText & "</Comments> <CardCode>" & Session("RazCode") & "</CardCode></row>" &
                        '    "</Documents><Document_Lines>"

                        For Each articulo In lstCarrito
                            If articulo.strCodigoArticulo.ToString <> "" Then
                                intLineNum = intLineNum + 1

                                strOrden.Append("<row><LineNum>" & intLineNum & "</LineNum>")
                                strOrden.Append("<ItemCode>" & articulo.strCodigoArticulo.ToString & "</ItemCode>")
                                strOrden.Append("<Quantity>" & articulo.dbCantidad.ToString & "</Quantity>")
                                strOrden.Append("<UnitPrice>" & articulo.dbPrecio.ToString & "</UnitPrice>")
                                strOrden.Append("<TaxCode>" & articulo.strImpuesto.ToString & "</TaxCode>")
                                strOrden.Append("<Currency>" & articulo.strCurrency.ToString & "</Currency>")
                                'strOrden.Append("<WarehouseCode>" & articulo.strAlmacen.ToString & "</WarehouseCode>")

                                'Campos Definidos
                                strOrden.Append("<U_CANTIDAD>" & articulo.dbCantidadPedido.ToString & "</U_CANTIDAD>")
                                strOrden.Append("<U_L>" & articulo.dbU_L.ToString & "</U_L>")
                                strOrden.Append("<U_A>" & articulo.dbU_A.ToString & "</U_A>")
                                strOrden.Append("<U_F>" & articulo.dbU_F.ToString & "</U_F>")
                                strOrden.Append("<U_ESPSUP>" & articulo.dbU_ESPSUP.ToString & "</U_ESPSUP>")
                                strOrden.Append("<U_ESPINF>" & articulo.dbU_ESPINF.ToString & "</U_ESPINF>")
                                strOrden.Append("<U_CREST>" & articulo.strU_CREST.ToString & "</U_CREST>")
                                strOrden.Append("<U_TIPO>" & articulo.strU_TIPO.ToString & "</U_TIPO>")
                                strOrden.Append("<U_PRECIONET>" & articulo.dbU_PRECIONET.ToString & "</U_PRECIONET>")
                                strOrden.Append("<U_NORANU>" & articulo.dbU_NORANU.ToString & "</U_NORANU>")
                                strOrden.Append("<U_LPLIEGO>" & articulo.dbU_LPLIEGO.ToString & "</U_LPLIEGO>")
                                strOrden.Append("<U_APLIEGO>" & articulo.dbU_APLIEGO.ToString & "</U_APLIEGO>")
                                strOrden.Append("<U_CLRIMPR>" & articulo.strU_CLRIMPR.ToString & "</U_CLRIMPR>")
                                strOrden.Append("<U_PORVENTAS>" & articulo.intU_PORVENTAS.ToString & "</U_PORVENTAS>")
                                strOrden.Append("<U_CIERRE>" & articulo.strU_CIERRE.ToString & "</U_CIERRE>")
                                strOrden.Append("<U_TF>" & articulo.strU_TF.ToString & "</U_TF>")
                                strOrden.Append("<U_AREA>" & articulo.strU_AREA.ToString & "</U_AREA>")
                                strOrden.Append("<U_PESO>" & articulo.strU_PESO.ToString & "</U_PESO>")
                                strOrden.Append("<U_PJUEGO >" & articulo.intU_PJUEGO.ToString & "</U_PJUEGO >")
                                strOrden.Append("<U_PIEZASPATADO>" & articulo.intU_PIEZASPATADO.ToString & "</U_PIEZASPATADO>")
                                strOrden.Append("<U_VALESPECIAL>" & articulo.dbU_VALESPECIAL.ToString & "</U_VALESPECIAL>")
                                strOrden.Append("<U_PIEZASTARIMA>" & articulo.intU_PIEZASTARIMA.ToString & "</U_PIEZASTARIMA>")

                                'HRS 07-Julio-2017
                                strOrden.Append("<U_IL_CE_Michelman>" & articulo.dbU_IL_CE_Michelman.ToString & "</U_IL_CE_Michelman>")
                                strOrden.Append("<U_IL_CE_MichelmanAC>" & articulo.dbU_IL_CE_MichelmanAC.ToString & "</U_IL_CE_MichelmanAC>")
                                strOrden.Append("<U_IL_CE_OpenSesame>" & articulo.dbU_IL_CE_OpenSesame.ToString & "</U_IL_CE_OpenSesame>")
                                strOrden.Append("<U_IL_CE_Pegado>" & articulo.dbU_IL_CE_Pegado.ToString & "</U_IL_CE_Pegado>")
                                strOrden.Append("<U_IL_CE_Grapado>" & articulo.dbU_IL_CE_Grapado.ToString & "</U_IL_CE_Grapado>")
                                strOrden.Append("<U_IL_CE_Tarima>" & articulo.dbU_IL_CE_Tarima.ToString & "</U_IL_CE_Tarima>")
                                strOrden.Append("<U_IL_CE_WaterPaper>" & articulo.dbU_IL_CE_WaterPaper.ToString & "</U_IL_CE_WaterPaper>")
                                strOrden.Append("<U_IL_CE_WaterPaperAC>" & articulo.dbU_IL_CE_WaterPaperAC.ToString & "</U_IL_CE_WaterPaperAC>")
                                strOrden.Append("<U_IL_CE_Emulsion>" & articulo.dbU_IL_CE_Emulsion.ToString & "</U_IL_CE_Emulsion>")
                                strOrden.Append("<U_IL_CE_Strinking>" & articulo.dbU_IL_CE_Strinking.ToString & "</U_IL_CE_Strinking>")
                                strOrden.Append("<U_IL_CE_Cera>" & articulo.dbU_IL_CE_Cera.ToString & "</U_IL_CE_Cera>")
                                strOrden.Append("<U_IL_CE_MaqDesvarbe>" & articulo.dbU_IL_CE_MaqDesvarbe.ToString & "</U_IL_CE_MaqDesvarbe>")
                                strOrden.Append("<U_IL_CE_TarimaEsp>" & articulo.dbU_IL_CE_TarimaEsp.ToString & "</U_IL_CE_TarimaEsp>")
                                strOrden.Append("<U_IL_CE_MaqPegado>" & articulo.dbU_IL_CE_MaqPegado.ToString & "</U_IL_CE_MaqPegado>")
                                strOrden.Append("<U_IL_CE_Desvarbe>" & articulo.dbU_IL_CE_Desvarbe.ToString & "</U_IL_CE_Desvarbe>")
                                strOrden.Append("<U_IL_CE_OtrosCost>" & articulo.dbU_IL_CE_OtrosCost.ToString & "</U_IL_CE_OtrosCost>")
                                strOrden.Append("<U_IL_CE_NotaAlsea>" & articulo.dbU_IL_CE_NotaAlsea.ToString & "</U_IL_CE_NotaAlsea>")

                                'HRS 17-08-2017 Guardar informacion en el Pedido
                                strOrden.Append("<U_Costopapelr>" & articulo.dbU_Costopapelr.ToString & "</U_Costopapelr>")

                                strOrden.Append("</row>")
                            End If
                        Next

                        'For i As Integer = 0 To carrito.Count - 1
                        '    orden = orden + "" &
                        '        "<FreeText>" & carritoNotaArt(i) & "</FreeText>"
                        'Next

                        strOrden.Append("</Document_Lines></BO></BOM>")

                    Else
                        orden = "" &
                          "<BOM xmlns='http://www.sap.com/SBO/DIS'><BO><AdmInfo><Object>oQuotations</Object>" &
                          "</AdmInfo><Documents><row> <DocDueDate>" & fechastring & "</DocDueDate><Comments>" & TextArea1.InnerText & "</Comments> <CardCode>" & Session("RazCode") & "</CardCode></row>" &
                          "</Documents><Document_Lines>"
                        'For i As Integer = 0 To carrito.Count - 1
                        '    orden = orden + "" &
                        '        "<row><LineNum>" & (i + 1) & "</LineNum><ItemCode>" & carrito(i) & "</ItemCode><Quantity>" & carritocan(i) & "</Quantity><UnitPrice>" & carritoprecio(i) & "</UnitPrice><TaxCode>" & carritoiva(i) & "</TaxCode><FreeText>" & carritoNotaArt(i) & "</FreeText><WarehouseCode>" & Session("AlmacenUsu") & "</WarehouseCode></row>"

                        'Next
                        orden = orden + "</Document_Lines></BO></BOM>"

                    End If

                    Respuesta = ws.AddObject(Session("Token"), strOrden.ToString, "AddOrder")

                    Dim errorr As String = ReadXML(Respuesta.InnerXml, "env:Text")
                    If errorr = "" Then
                        Dim strDocumentoCreado As String = ReadXML(Respuesta.InnerXml, "RetKey")

                        'Calculos posteriores a la generacion del documento
                        If Session("tipodocventas") = "pedido" Then
                            If strDocumentoCreado <> "" Then
                                If IsNumeric(strDocumentoCreado) Then
                                    Try
                                        'HRS 17-08-2017 Proceso comentado ya no necesario con los cambios
                                        ''oCalculo.FormDataEvent_FORM_DATA_ADD_Pedido("17", strDocumentoCreado)
                                    Catch ex As Exception
                                    End Try
                                End If
                            End If
                        End If

                        If CheckMail.Checked Then
                            EnvioMailPedido()
                        End If

                        Try
                            orden = ""
                            If Session("tipodocventas") = "pedido" Then
                                'Obtenemos DocNum de documento creado
                                orden = "select docnum from ordr where docentry = " & strDocumentoCreado
                                Respuesta = ws.ExecuteSQL(Session("Token"), orden)
                                orden = ReadXML(Respuesta.InnerXml, "docnum")
                            End If
                            Dim mensajediserver As String = " <env:Envelope xmlns:env='http://schemas.xmlsoap.org/soap/envelope/'>   <env:Header>    <SessionID>" & Session("Token") & "</SessionID>   </env:Header>   <env:Body>    <dis:SendMessage xmlns:dis='http://www.sap.com/SBO/DIS'>     <Service>MessagesService</Service>      <Message>       <Subject>Nuevo Documento " & Session("tipodocventas") & ", Socio: " & Session("RazCode") & "</Subject>       <Text>Orden de venta " & orden & "</Text>       <RecipientCollection>        <Recipient>         <UserCode>" & Session("usuAlertas") & "</UserCode>         <SendInternal>tYES</SendInternal>        </Recipient>       </RecipientCollection>              </Message>     </dis:SendMessage>   </env:Body>  </env:Envelope>"
                            '" & TextArea1.Value & "','" & Session("RazCode") & "','Admin','" & id & "','" & DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") & "')"
                            ws.Interact(Session("Token"), mensajediserver)
                        Catch ex As Exception

                        End Try

                        Session("lstCarrito") = Nothing
                        Session("oDatosCabecera") = Nothing
                        Session("oDatosCabeceraCal") = Nothing

                        Session("carrito") = Nothing
                        Session("carritocan") = Nothing
                        Session("precio") = Nothing
                        Session("nom") = Nothing
                        Session("carritonumitems") = Nothing
                        Session("ivas") = Nothing
                        Session("carritoDescuentoEDIT") = Nothing
                        Session("carritoNotaArt") = Nothing

                        'EnvioMailPedido("leopoldo.delatorre@interlatin.com.mx", "leopoldo.delatorre@interlatin.com.mx")

                        ClientScript.RegisterStartupScript(Me.[GetType](), "aleasrt", "alert('Documento " & Session("tipodocventas") & " creado. ');document.location.href='frmOrden';", True)
                    Else
                        errorr = Replace(errorr, "'", "")

                        Me.Page.ClientScript.RegisterStartupScript(Me.GetType(), "aleasrt", "alert('" & errorr & "');document.location.href='frmOrdenCarrito';", True)

                    End If
                    System.Diagnostics.Debug.Write(ReadXML(Respuesta.InnerXml, "env:Text") & vbCrLf)
                Else
                    Me.Page.ClientScript.RegisterStartupScript(Me.GetType(), "aleasrt", "alert('" & oCalculo.strNumeroError & " " & oCalculo.strDetalleError & "');document.location.href='frmOrdenCarrito';", True)
                End If 'fin If oCalculo.CheckSalesOrder()

            End If 'fin If IsNothing(Session("lstCarrito")) 
        Catch ex As Exception
            System.Diagnostics.Debug.Write(ex.Message & vbCrLf)
        Finally
            strOrden = Nothing
            ws = Nothing
            oCalculo = Nothing
            lstCarrito = Nothing
            oDatosCabecera = Nothing
            oDatosCabeceraCal = Nothing
        End Try
    End Sub

    Protected Sub OnConfirm(sender As Object, e As EventArgs)
        Dim confirmValue As String = Request.Form("confirm_value")
        If confirmValue = "Ok" Then 
            agregaorden()
            'ClientScript.RegisterStartupScript(Me.[GetType](), "alert", "alert('You clicked YES!')", True)
        Else
            ClientScript.RegisterStartupScript(Me.[GetType](), "aleasrt", " document.location.href='frmOrdenCarrito';", True)
        End If
    End Sub

    Protected Sub Mailer()
        '   <table style="width:100%"  border="1">
        '  <tr>
        '    <td>Jill</td>
        '    <td>Smith</td>
        '    <td>50</td>
        '  </tr> 
        '  <tr>
        '    <td> </td>
        '    <th>Impuesto </th>
        '    <td>94</td>
        '  </tr> 
        '</table> 
    End Sub

    'Protected Sub cambioPorcentaje_ServerClick(sender As Object, e As EventArgs) Handles cambioPorcentaje.ServerClick
    '    Try
    '        'actualizan porcentaje de descuento
    '        Dim valor As Double = Convert.ToDouble(actuacan.Value)

    '        actuacan.Value = valor
    '        If valor >= 0 Then
    '            Try

    '                Dim carritoDescuentoEDIT As ArrayList = New ArrayList
    '                Dim carrito As ArrayList = New ArrayList
    '                Dim carritocan As ArrayList = New ArrayList
    '                carritoDescuentoEDIT = CType(Session("carritoDescuentoEDIT"), ArrayList)
    '                carrito = CType(Session("carrito"), ArrayList)
    '                carritocan = CType(Session("carritocan"), ArrayList)

    '                Dim preciosporarticulo As ArrayList = getItemPriceDiscountByPolo(Session("RazCode"), carrito(actuaid.Value), carritocan(actuaid.Value), Session("Token"))

    '                If Convert.ToDouble(preciosporarticulo(1)) > Convert.ToDouble(actuacan.Value) Then
    '                    carritoDescuentoEDIT(actuaid.Value) = Convert.ToDouble(actuacan.Value)
    '                Else
    '                    carritoDescuentoEDIT(actuaid.Value) = Convert.ToDouble(preciosporarticulo(1))

    '                End If

    '                Session("carritoDescuentoEDIT") = carritoDescuentoEDIT
    '                ClientScript.RegisterStartupScript(Me.[GetType](), "aleasrt", " document.location.href='frmOrdenCarrito';", True)

    '            Catch ex As Exception
    '                ClientScript.RegisterStartupScript(Me.[GetType](), "aleasrt", " document.location.href='frmOrdenCarrito';", True)
    '            End Try


    '        Else
    '            ClientScript.RegisterStartupScript(Me.[GetType](), "aleasrt", " document.location.href='frmOrdenCarrito';", True)

    '        End If

    '    Catch ex As Exception
    '        ClientScript.RegisterStartupScript(Me.[GetType](), "aleasrt", " document.location.href='frmOrdenCarrito';", True)
    '    End Try
    'End Sub

    Protected Sub cambioNota_ServerClick(sender As Object, e As EventArgs) Handles cambioNota.ServerClick
        Try
            Session("strComentarios") = TextArea1.InnerText
        Catch ex As Exception
        End Try
        Response.Redirect("frmOrdenCarrito.aspx")
    End Sub

    Private Sub EnvioMailPedido()
        If IsNothing(Session("MailDestino")) Then
            Exit Sub
        End If
        If Session("MailDestino") = "" Then
            Exit Sub
        End If

        Try
            Dim oMsg As CDO.Message = New CDO.Message()
            Dim iConfg As CDO.Configuration
            Dim oFields As ADODB.Fields
            Dim oField As ADODB.Field
            iConfg = oMsg.Configuration
            oFields = iConfg.Fields

            'Canal de comunicacion
            oField = oFields("http://schemas.microsoft.com/cdo/configuration/sendusing")
            oField.Value = CDO.CdoSendUsing.cdoSendUsingPort
            '1: cdoSendUsingPickup 2: cdoSendUsingPort

            'IP del servidor SMTP
            oField = oFields("http://schemas.microsoft.com/cdo/configuration/smtpserver")
            oField.Value = Session("smtp")

            'Tiempo de espera
            oField = oFields("http://schemas.microsoft.com/cdo/configuration/smtpconnectiontimeout")
            oField.Value = 20 'Segs

            'Puerto del SMTP
            oField = oFields("http://schemas.microsoft.com/cdo/configuration/smtpserverport")
            oField.Value = Session("puerto")  '585 TCP/IP

            'Tipo de autenticacion
            oField = oFields("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate")
            oField.Value = CDO.CdoProtocolsAuthentication.cdoBasic

            'Nombre de usuario
            oField = oFields("http://schemas.microsoft.com/cdo/configuration/sendusername")
            oField.Value = Session("username")

            'Contraseña
            oField = oFields("http://schemas.microsoft.com/cdo/configuration/sendpassword")
            oField.Value = Session("password")

            'Utiliza SSL
            oField = oFields("http://schemas.microsoft.com/cdo/configuration/smtpusessl")
            If Session("ssl") = "SI" Then
                oField.Value = True
            Else
                oField.Value = False
            End If
            'Actualizamos los datos de la cuenta SMTP
            oFields.Update()
            oMsg.Configuration = iConfg
            oMsg.Subject = "Orden de venta - " & Now
            '1) Solo texto
            oMsg.HTMLBody = "Orden de venta - " & Now
            '2) Correo en HTML
            'oMsg.HTMLBody = "<body></body>" 
            'Cuentas de Email
            oMsg.ReplyTo = Session("MailDestino")
            oMsg.To = Session("MailDestino")
            oMsg.From = Session("acount")

            Dim dir = System.IO.Path.GetTempPath()
            Dim file__1 = Path.Combine(dir, Guid.NewGuid().ToString() + ".xls")
            Directory.CreateDirectory(dir)
            File.WriteAllText(file__1, Session("pedidoTableHTML"))
            Dim _count = File.ReadAllText(file__1)
            'Adjuntos
            Dim Attchs As CDO.IBodyPart
            Attchs = oMsg.AddAttachment(file__1)
            'Attchs = oMsg.AddAttachment(adjPDF)
            'Attchs = oMsg.AddAttachment("c:\Windows\Factura.pdf")

            'Enviamos el correo
            oMsg.Send()

            'Limpiamos las variables
            oMsg = Nothing
            iConfg = Nothing
            oFields = Nothing
            oField = Nothing
            File.Delete(file__1)
            ' Return "Envio Correcto"
        Catch ex As Exception

            'Return ex.Message
        End Try

    End Sub

    Protected Sub guardaDatosGen_ServerClick(sender As Object, e As EventArgs) Handles guardaDatosGen.ServerClick
        Dim oDatosCabecera As New CE.clsDatosCabecera_CE
        Try
            oDatosCabecera = CType(Session("oDatosCabecera"), CE.clsDatosCabecera_CE)
            oDatosCabecera.strU_PEDIDONO = txtsCC_U_PEDIDONO.Value.ToString.Trim
            oDatosCabecera.strU_RECEP = txtsCC_U_RECEP.Value.ToString.Trim
            oDatosCabecera.strU_REF = txtsCC_U_REF.Value.ToString.Trim
            oDatosCabecera.strDomicilio = txtsDestino.Value.ToString.Trim
            oDatosCabecera.strFechaEntrega = txtsFechaEntrega.Value.ToString
            oDatosCabecera.strFechaOrdenCompra = txtsFechaOrdenCompra.Value.ToString
            Session("oDatosCabecera") = oDatosCabecera
        Catch ex As Exception
        Finally
            oDatosCabecera = Nothing
        End Try
        Response.Redirect("frmOrdenCarrito.aspx")
    End Sub

    Protected Sub cargarImagen_Click(sender As Object, e As EventArgs) Handles cargarImagen.Click
        Dim oDatosCabecera As New CE.clsDatosCabecera_CE
        Dim squery As String
        Dim strPath As String
        Dim archivo As String

        Try
            If fileUpload1.HasFile Then
                ws = New DIS.DIServer
                squery = "select top 1 isnull(AttachPath,'') as 'AttachPath' from OADP"
                Respuesta = ws.ExecuteSQL(Session("Token"), squery)
                strPath = ReadXML(Respuesta.InnerXml, "AttachPath")

                If Directory.Exists(strPath) Then
                    archivo = [String].Format("{0}{1}", strPath, fileUpload1.FileName)

                    ' Verificar que el archivo no exista
                    If Not File.Exists(archivo) Then
                        fileUpload1.PostedFile.SaveAs(archivo)
                    End If

                    'Guardar Archivo Seleccionado
                    Try
                        oDatosCabecera = CType(Session("oDatosCabecera"), CE.clsDatosCabecera_CE)
                        oDatosCabecera.strArchCotizacion = fileUpload1.FileName
                        Session("oDatosCabecera") = oDatosCabecera
                    Catch ex As Exception
                    Finally
                        oDatosCabecera = Nothing
                    End Try

                End If
            End If
        Catch ex As Exception
            Me.Page.ClientScript.RegisterStartupScript(Me.GetType(), "aleasrt", "alert('" & ex.Message & "');document.location.href='frmOrdenCarrito';", True)
        Finally
            ws = Nothing
        End Try
        Response.Redirect("frmOrdenCarrito.aspx")
    End Sub

End Class
