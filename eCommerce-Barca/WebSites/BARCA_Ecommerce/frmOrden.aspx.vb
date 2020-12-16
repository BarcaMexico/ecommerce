Imports Modulo

Imports System.Xml
Imports System.Globalization
Imports System.IO
Imports System.Data

Partial Class frmOrden
    Inherits System.Web.UI.Page
    Public ws As DIS.DIServer

    Private pattern As String = " "
    Private elements() As String
    Private Respuesta As XmlNode

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Session("validFor") = "N" Then
            ClientScript.RegisterStartupScript(Me.[GetType](), "aleasrt", "alert('Socio de negocio inactivo ');document.location.href='frmInicio'; ", True)
        End If

        Session.Timeout = 20
        actualizacarritochiquito()

        If Not Page.IsPostBack Then
            DropDownList1.Visible = False
            ws = New DIS.DIServer
            Try
                txtwharehouse.Value = Session("AlmacenUsu")
                Session("busqueda") = ""
                Session("pagIni") = 1
                'buscarFinale()
            Catch ex As Exception
            Finally
                ws.Dispose()
                ws = Nothing
            End Try
        End If
    End Sub

    Protected Sub btnbuscar_ServerClick(sender As Object, e As EventArgs) Handles btnbuscar.ServerClick
        Session("busqueda") = barrabusqueda.Value
        Session("pagIni") = 1
        buscarFinale()
    End Sub

    Public Sub buscarFinale()
        Dim tRow As New TableRow()
        Dim tCell As New TableCell()
        ws = New DIS.DIServer

        Try
            Respuesta = ws.ExecuteSQL(Session("Token"), "select TaxCode from crd1 where CardCode = '" & Session("RazCode") & "' and TaxCode &lt;&gt; '' order by TaxCode asc")
            Session("TaxCode") = ReadXML(Respuesta.InnerXml, "TaxCode")

            Respuesta = ws.ExecuteSQL(Session("Token"), "select Rate from ORTT where RateDate = CONVERT (date, GETDATE()) ")
            Session("usdrate") = ReadXML(Respuesta.InnerXml, "rate")

            If Session("usdrate") > 0 Then

                Dim today As Date = DateTime.Now
                Dim fechastring = today.ToString("yyyyMMdd")
                Dim encontrados As Integer = 0
                Dim Respuesta As XmlNode
                Dim wharehose As String = txtwharehouse.Value
                Dim sqldato As String = ""

                sqldato = "select top 1 U_arttype as tipo,U_artcan as cantidad from [Ecom].[dbo].[@IL_CONFIG_E] "
                Respuesta = ws.ExecuteSQL(Session("Token"), sqldato)
                Session("configarttype") = ReadXML(Respuesta.InnerXml, "tipo")
                Session("configartcan") = ReadXML(Respuesta.InnerXml, "cantidad")

                sqldato = "SELECT COUNT(1) AS 'disp'"
                sqldato = sqldato & " FROM OITM T0 WHERE T0.[U_CLIENTE] = '" & Session("RazCode") & "'"
                sqldato = sqldato & " AND T0.[frozenFor] &lt;&gt; 'Y' AND T0.[QryGroup62] = 'N'  AND T0.[itmsgrpcod]='105'"
                sqldato = sqldato & " AND (T0.[ItemCode]  like '%" & Session("busqueda").ToString.Trim & "%' OR T0.[ItemName] like '%" & Session("busqueda").ToString.Trim & "%' "
                sqldato = sqldato & " OR T0.[U_TF] like '%" & Session("busqueda").ToString.Trim & "%')"

                Respuesta = ws.ExecuteSQL(Session("Token"), sqldato.ToString)

                Dim totalrows As Integer = ReadXML(Respuesta.InnerXml, "disp")
                Dim twoDarray(CInt(totalrows), 10) As String

                'paginacion ***************************************
                Dim n As Integer
                Dim contapag As Integer = 0
                Dim inic As Integer
                Dim finic As Integer
                Dim configcant As Integer = Session("configartcan")
                If totalrows > configcant Then
                    Dim nacho As Integer = Math.Ceiling(totalrows / Session("configartcan"))

                    PaginationDiv.InnerHtml = " <ul class='pagination  pagination-lg'>"

                    If nacho > 1 Then
                        If nacho > 3 Then
                            'checa inicio
                            If (Session("pagIni") - 1) <= 1 Then
                                inic = 1
                            ElseIf (Session("pagIni")) = nacho Then
                                inic = (Session("pagIni") - 2)
                            Else
                                inic = (Session("pagIni") - 1)
                            End If
                            'checa final
                            If (Session("pagIni") + 1) >= nacho Then
                                finic = nacho
                            ElseIf (Session("pagIni")) = 1 Then
                                finic = (Session("pagIni") + 2)
                            Else
                                finic = (Session("pagIni") + 1)
                            End If
                            If Session("pagIni") > 1 Then
                                PaginationDiv.InnerHtml = PaginationDiv.InnerHtml + " <li style='cursor: pointer; ' class='' id='row" & (Session("pagIni") - 1) & "'       onclick='discre( id)'><a><i class='fa fa-arrow-left'></i></a></li>"
                            End If

                            If inic > 2 Then

                                PaginationDiv.InnerHtml = PaginationDiv.InnerHtml + "<li style='cursor: pointer; '  class='' id='row1'       onclick='discre( id)'><a>1</a></li><li><a>...</a></li>"

                            End If

                            For n = inic To finic
                                If n = Session("pagIni") Then
                                    PaginationDiv.InnerHtml = PaginationDiv.InnerHtml + "<li style='cursor: pointer; '  class='active' id='row" & n & "'       onclick='discre( id)'><a>" & n & "</a></li>"
                                Else
                                    PaginationDiv.InnerHtml = PaginationDiv.InnerHtml + "<li style='cursor: pointer; '  class='' id='row" & n & "'       onclick='discre( id)'><a>" & n & "</a></li>"
                                End If
                            Next
                            If (Session("pagIni") + 2) <= nacho Then
                                PaginationDiv.InnerHtml = PaginationDiv.InnerHtml + "<li><a>...</a></li><li style='cursor: pointer; '  class='' id='row" & nacho & "'       onclick='discre( id)'><a>" & nacho & "</a></li>"
                            End If
                            If Session("pagIni") < nacho Then
                                PaginationDiv.InnerHtml = PaginationDiv.InnerHtml + "<li style='cursor: pointer; '  class='' id='row" & (Session("pagIni") + 1) & "'       onclick='discre( id)'><a><i class='fa fa-arrow-right'></i></a></li>"
                            End If
                        Else
                            For n = 1 To nacho
                                If n = Session("pagIni") Then
                                    PaginationDiv.InnerHtml = PaginationDiv.InnerHtml + "<li style='cursor: pointer; '  class='active' id='row" & n & "'       onclick='discre( id)'><a>" & n & "</a></li>"
                                Else
                                    PaginationDiv.InnerHtml = PaginationDiv.InnerHtml + "<li style='cursor: pointer; '  class='' id='row" & n & "'       onclick='discre( id)'><a>" & n & "</a></li>"
                                End If
                            Next
                        End If
                        PaginationDiv.InnerHtml = PaginationDiv.InnerHtml + " </ul>"
                    Else
                        PaginationDiv.InnerHtml = ""
                    End If
                Else
                    PaginationDiv.InnerHtml = ""
                End If

                sqldato = "SELECT TOP 200 CONVERT(int,ROW_NUMBER() OVER (ORDER BY T0.ItemCode ASC)) AS 'RowNum', T0.ItemCode, T0.ItemName, T0.[U_TF] as 'Tarjeta' "
                sqldato = sqldato & " FROM OITM T0 WHERE T0.[U_CLIENTE] = '" & Session("RazCode") & "'"
                sqldato = sqldato & " AND T0.[frozenFor] &lt;&gt; 'Y' AND T0.[QryGroup62] = 'N'  AND T0.[itmsgrpcod]='105'"
                sqldato = sqldato & " AND (T0.[ItemCode]  like '%" & Session("busqueda").ToString.Trim & "%' OR T0.[ItemName] like '%" & Session("busqueda").ToString.Trim & "%' "
                sqldato = sqldato & " OR T0.[U_TF] like '%" & Session("busqueda").ToString.Trim & "%')"

                'grid table
                '**********************************************************************************************
                Dim dt As New DataTable()
                Dim dr As DataRow = Nothing
                ' dt.Columns.AddRange(New DataColumn(4) {New DataColumn("col1", GetType(String)), New DataColumn("col2", GetType(String)), New DataColumn("col3", GetType(String)), New DataColumn("col4", GetType(String)), New DataColumn("col5", GetType(String))})

                dt.Columns.Add(New DataColumn("Col1", GetType(String)))
                dt.Columns.Add(New DataColumn("Col2", GetType(String)))
                dt.Columns.Add(New DataColumn("codigo", GetType(String)))
                dt.Columns.Add(New DataColumn("nombre", GetType(String)))
                dt.Columns.Add(New DataColumn("tarjetafab", GetType(String)))
                
                ViewState("Customers") = dt

                Me.BindGrid()

                Respuesta = ws.ExecuteSQL(Session("Token"), sqldato)
                Dim doc2 As New XmlDocument()
                doc2.LoadXml(Respuesta.InnerXml)
                System.Diagnostics.Debug.Write(Respuesta.InnerXml & vbCrLf)
                Dim root2 As XmlNode = doc2.FirstChild
                Dim rootin As XmlNode
                root2 = root2.LastChild

                If root2.HasChildNodes Then
                    Dim s As Integer
                    For s = 0 To root2.ChildNodes.Count - 1
                        rootin = root2.ChildNodes(s)
                        If rootin.HasChildNodes Then
                            Dim x As Integer
                            tRow = New TableRow()

                            Dim informacion As New ArrayList
                            For x = 0 To rootin.ChildNodes.Count - 1
                                Select Case rootin.ChildNodes(x).Name
                                    Case "RowNum"
                                        If CInt(rootin.ChildNodes(x).InnerText) > (Session("configartcan") * Session("pagIni")) Then
                                            x = rootin.ChildNodes.Count
                                            Exit Select
                                        End If
                                        If Session("pagIni") <> 1 Then
                                            If CInt(rootin.ChildNodes(x).InnerText) < (Session("configartcan") * (Session("pagIni") - 1)) Then
                                                x = rootin.ChildNodes.Count
                                                Exit Select
                                            End If
                                        End If
                                    Case "ItemCode"
                                        If rootin.ChildNodes(x).InnerText = "" Then
                                            ClientScript.RegisterStartupScript(Me.[GetType](), "aleasrt", "alert(' La busqueda no arrojo resultados ');", True)
                                            Return
                                        Else
                                            twoDarray(s, x) = rootin.ChildNodes(x).InnerText
                                            informacion.Add(rootin.ChildNodes(x).InnerText)
                                        End If
                                        'Case "disp"
                                        '    twoDarray(s, x) = Convert.ToInt64(Math.Floor(Convert.ToDouble(rootin.ChildNodes(x).InnerText)))
                                        '    informacion.Add(rootin.ChildNodes(x).InnerText)
                                    Case "ItemName"
                                        twoDarray(s, x) = rootin.ChildNodes(x).InnerText
                                        informacion.Add(rootin.ChildNodes(x).InnerText)
                                        'Case "iva"
                                        '    twoDarray(s, x + 1) = Session("TaxCode")
                                        'Case "imag"
                                        '    If rootin.ChildNodes(x).InnerText = "" Then
                                        '        twoDarray(s, x + 2) = "data:image/png;base64,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"
                                        '    Else
                                        '        twoDarray(s, x + 2) = Session("ftp") + "imagenes/" + rootin.ChildNodes(x).InnerText
                                        '    End If

                                        'Case "InvntryUom"
                                        '    twoDarray(s, x + 2) = rootin.ChildNodes(x).InnerText
                                    Case "Tarjeta"
                                        twoDarray(s, x) = rootin.ChildNodes(x).InnerText
                                        informacion.Add(rootin.ChildNodes(x).InnerText)
                                End Select
                            Next x
                        End If
                    Next s
                End If

                Dim divs As String = ""
                For v = 0 To twoDarray.GetLength(0) - 1
                    If twoDarray(v, 1) = Nothing Or twoDarray(v, 5) = "null" Then
                    Else
                        encontrados = encontrados + 1

                        'Dim mystring As String = "<env:Envelope xmlns:env='http://schemas.xmlsoap.org/soap/envelope/'><env:Header><SessionID>" & Session("Token") & "</SessionID></env:Header><env:Body><dis:GetItemPrice xmlns:dis='http://www.sap.com/SBO/DIS'><CardCode>" & Session("RazCode") & "</CardCode><ItemCode>" & twoDarray(v, 1) & "</ItemCode><Quantity>1</Quantity><Date>" & fechastring & "</Date></dis:GetItemPrice></env:Body></env:Envelope>"
                        'Dim restring As String = ws.Interact(Session("Token"), mystring)

                        If Session("configarttype") = "stock" Then
                            'twoDarray(v, 4) = ReadXML(restring, "Currency") + " " + String.Format("{0:N}", Convert.ToDouble(ReadXML(restring, "Price")))
                            dt = DirectCast(ViewState("Customers"), DataTable)
                            dr = dt.NewRow()
                            dr("Col1") = ""
                            dr("Col2") = "1"
                            dr("codigo") = twoDarray(v, 1)
                            'dr("nombre") = twoDarray(v, 3)
                            'dr("tarjetafab") = twoDarray(v, 4)
                            dr("nombre") = twoDarray(v, 2)
                            dr("tarjetafab") = twoDarray(v, 3)
                            dt.Rows.Add(dr)

                            ViewState("Customers") = dt
                            Me.BindGrid()

                        Else
                            'twoDarray(v, 4) = ReadXML(restring, "Currency") + " " + String.Format("{0:N}", Convert.ToDouble(ReadXML(restring, "Price")))
                            dt = DirectCast(ViewState("Customers"), DataTable)
                            dr = dt.NewRow()
                            dr("Col1") = ""
                            dr("Col2") = "1"
                            dr("codigo") = twoDarray(v, 1)
                            dr("nombre") = twoDarray(v, 2)
                            dr("tarjetafab") = twoDarray(v, 3)
                            dt.Rows.Add(dr)

                            ViewState("Customers") = dt
                            Me.BindGrid()

                        End If
                    End If
                Next v
                divs = "<div class='row ' ><div class='col-xs-12 col-sm-6  col-md-6 col-lg-6 busqueda ' style=''>    <div><font class='busquedatexto' style=''>Búsqueda  </font><font class='busquedatextovalor'  style=''>" & Session("busqueda") & "</font></div>                 </div>                  <div class='col-xs-12 col-sm-6  col-md-6 col-lg-6 '> " &
                                   "<div class='productoencontrado'>" & totalrows & " Productos Encontrados</div>                 </div>             </div> " + divs

                articuloslista.InnerHtml = divs
            Else
                ClientScript.RegisterStartupScript(Me.[GetType](), "aleasrt", "alert(' No se a definido el tipo de cambio del día. Contacte al Administrador ');document.location.href='frmMensajes'; ", True)
            End If

        Catch ex As Exception
            Dim errrrorr As String = ex.Message
            ClientScript.RegisterStartupScript(Me.[GetType](), "aleasrt", "alert('" & errrrorr & "');  ", True)
        Finally
            tRow.Dispose()
            tCell.Dispose()
            ws.Dispose()
            ws = Nothing
        End Try
    End Sub

    Protected Sub BindGrid()
        mygrid.DataSource = DirectCast(ViewState("Customers"), DataTable)
        mygrid.DataBind()
    End Sub

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

    Public Sub agregar(cantidad As String, itemm As String, nombr As String, ByVal tarjeta As String)
        Dim lstCarrito As New List(Of CE.clsArticulos_CE)
        Dim objArticulo As CE.clsArticulos_CE
        Dim bandera As Boolean = False
        Dim mpContentPlaceHolder As ContentPlaceHolder
        Dim mpdiv As HtmlGenericControl

        ws = New DIS.DIServer
        objArticulo = New CE.clsArticulos_CE

        Try
            'Traemos los datos del articulo seleccionado
            Respuesta = ws.ExecuteSQL(Session("Token"), "EXEC [dbo].[IL_ECOM_OV_ART] 17,'" & itemm & "'")

            'Llenado de Objeto Articulo
            objArticulo.strCodigoArticulo = itemm
            objArticulo.strNombreArticulo = nombr
            objArticulo.dbCantidad = 0 'Se calcula cantidad posteriormente
            objArticulo.dbPrecio = ReadXML(Respuesta.InnerXml, "Price")
            objArticulo.dbDescuento = 0
            objArticulo.strAlmacen = Session("AlmacenUsu")
            objArticulo.strImpuesto = Session("TaxCode")
            objArticulo.strNotaArticulo = ""

            objArticulo.dbCantidadPedido = CDbl(cantidad)

            objArticulo.strCurrency = ReadXML(Respuesta.InnerXml, "Currency")
            objArticulo.dbU_L = ReadXML(Respuesta.InnerXml, "U_L")
            objArticulo.dbU_A = ReadXML(Respuesta.InnerXml, "U_A")
            objArticulo.dbU_F = ReadXML(Respuesta.InnerXml, "U_F")
            objArticulo.dbU_ESPSUP = ReadXML(Respuesta.InnerXml, "U_ESPSUP")
            objArticulo.dbU_ESPINF = ReadXML(Respuesta.InnerXml, "U_ESPINF")
            objArticulo.strU_CREST = ReadXML(Respuesta.InnerXml, "U_CREST")
            objArticulo.strU_CREST_Name = ReadXML(Respuesta.InnerXml, "U_CREST_NAME")
            objArticulo.strU_TIPO = ReadXML(Respuesta.InnerXml, "U_TIPO")
            objArticulo.strU_TIPO_Name = ReadXML(Respuesta.InnerXml, "U_TIPO_NAME")
            objArticulo.dbU_PRECIONET = ReadXML(Respuesta.InnerXml, "U_PRECIONET")
            objArticulo.dbU_NORANU = ReadXML(Respuesta.InnerXml, "U_NORANU")
            objArticulo.dbU_LPLIEGO = ReadXML(Respuesta.InnerXml, "U_LPLIEGO")
            objArticulo.dbU_APLIEGO = ReadXML(Respuesta.InnerXml, "U_APLIEGO")
            objArticulo.strU_CLRIMPR = ReadXML(Respuesta.InnerXml, "U_CLRIMPR")
            objArticulo.intU_PORVENTAS = ReadXML(Respuesta.InnerXml, "U_PORVENTAS")
            objArticulo.strU_CIERRE = ReadXML(Respuesta.InnerXml, "U_CIERRE")
            objArticulo.strU_TF = ReadXML(Respuesta.InnerXml, "U_TF")
            objArticulo.strU_AREA = ReadXML(Respuesta.InnerXml, "U_AREA")
            objArticulo.strU_PESO = ReadXML(Respuesta.InnerXml, "U_PESO")
            objArticulo.intU_PJUEGO = ReadXML(Respuesta.InnerXml, "U_PJUEGO")
            objArticulo.intU_PIEZASPATADO = ReadXML(Respuesta.InnerXml, "U_PIEZASPATADO")
            objArticulo.dbU_VALESPECIAL = ReadXML(Respuesta.InnerXml, "U_VALESPECIAL")
            objArticulo.intU_PIEZASTARIMA = ReadXML(Respuesta.InnerXml, "U_PIEZASTARIMA")
        Catch ex As Exception
            ClientScript.RegisterStartupScript(Me.[GetType](), "aleasrt", "alert(' " & ex.Message & " '); ", True)
        End Try

        Try
            bandera = False

            If IsNothing(Session("lstCarrito")) Then
                'Siguiete ID
                objArticulo.intID = 1

                'Traemos la cantidad
                Respuesta = ws.ExecuteSQL(Session("Token"), "EXEC [dbo].[IL_ECOM_OV_ART_CAN] 17,'" & itemm & "', '" & objArticulo.dbCantidadPedido & "','" & objArticulo.intU_PORVENTAS & "'")
                objArticulo.dbCantidad = CDbl(ReadXML(Respuesta.InnerXml, "Quantity"))

                lstCarrito.Add(objArticulo)
                Session("lstCarrito") = lstCarrito
            Else
                lstCarrito = CType(Session("lstCarrito"), List(Of CE.clsArticulos_CE))
                Dim query = lstCarrito.Where(Function(art As CE.clsArticulos_CE) art.strCodigoArticulo = itemm)
                If query.Count > 0 Then
                    query.Single().dbCantidadPedido = query.Single().dbCantidadPedido + CDbl(cantidad)

                    'Traemos la cantidad
                    Respuesta = ws.ExecuteSQL(Session("Token"), "EXEC [dbo].[IL_ECOM_OV_ART_CAN] 17,'" & itemm & "', '" & query.Single().dbCantidadPedido & "','" & query.Single().intU_PORVENTAS & "'")
                    query.Single().dbCantidad = CDbl(ReadXML(Respuesta.InnerXml, "Quantity"))
                    bandera = True
                End If

                If bandera = False Then
                    'Siguiete ID
                    Dim query1 = lstCarrito.OrderByDescending(Function(art As CE.clsArticulos_CE) art.intID).Take(1)
                    If query1.Count > 0 Then
                        objArticulo.intID = query1.Single().intID + 1
                    End If

                    'Traemos la cantidad
                    Respuesta = ws.ExecuteSQL(Session("Token"), "EXEC [dbo].[IL_ECOM_OV_ART_CAN] 17,'" & itemm & "', '" & objArticulo.dbCantidadPedido & "','" & objArticulo.intU_PORVENTAS & "'")
                    objArticulo.dbCantidad = CDbl(ReadXML(Respuesta.InnerXml, "Quantity"))

                    lstCarrito.Add(objArticulo)
                End If

                Session("lstCarrito") = lstCarrito
            End If

            If bandera = False Then
                'Traemos Hijos
                Dim docRespuesta As New XmlDocument()
                Dim m_nodelist As XmlNodeList
                Dim m_node As XmlNode
                Try
                    Respuesta = ws.ExecuteSQL(Session("Token"), "EXEC [dbo].[IL_ECOM_OV_ART_HIJOS] 17,'" & itemm & "','" & objArticulo.dbCantidad & "','" & objArticulo.dbCantidadPedido & "'")
                    docRespuesta.LoadXml(Respuesta.InnerXml)
                    m_nodelist = docRespuesta.GetElementsByTagName("row")
                    If m_nodelist.Count > 0 Then
                        For Each m_node In m_nodelist
                            'Si encontro articulo, lo agregamos 
                            If m_node.ChildNodes.Item(0).InnerText.ToString <> "" Then
                                agregar(m_node.ChildNodes.Item(3).InnerText.ToString,
                                    m_node.ChildNodes.Item(0).InnerText.ToString,
                                    m_node.ChildNodes.Item(1).InnerText.ToString,
                                    "")
                            End If
                        Next
                    End If
                Catch ex As Exception
                Finally
                    docRespuesta = Nothing
                End Try
                'Fin Hijos
            End If

            mpContentPlaceHolder = CType(Master.FindControl("CP2"), ContentPlaceHolder)
            mpdiv = CType(mpContentPlaceHolder.FindControl("carritonoti"), HtmlGenericControl)
            If mpdiv.InnerText = "" Then
                Session("carritonumitems") = 1
                mpdiv.InnerHtml = "1"
            Else
                Session("carritonumitems") = Session("carritonumitems") + 1
                mpdiv.InnerHtml = mpdiv.InnerText + 1
            End If
            mpdiv.Attributes.Add("style", "visibility:visible")
            actualizacarritochiquito()

        Catch ex As Exception
            ClientScript.RegisterStartupScript(Me.[GetType](), "aleasrt", "alert(' " & ex.Message & " '); ", True)
        Finally
            lstCarrito = Nothing
            objArticulo = Nothing
            ws = Nothing
            GC.Collect()
        End Try

    End Sub

    Private Function VarXml(ByRef xAtt As XmlElement, ByVal strVar As String) As String
        VarXml = xAtt.GetAttribute(strVar)
        If VarXml = Nothing Then VarXml = ""
    End Function

    Private Sub actualizacarritochiquito()
        Dim lstCarrito As New List(Of CE.clsArticulos_CE)
        Dim mystring As String = ""
        Dim restring As String = ""
        Dim totalxarticulo As Double = 0
        Dim totaliva As Double = 0
        Dim totalivafinal As Double = 0
        Dim fechastring = Today.ToString("yyyyMMdd")
        Dim subtotal As Double = 0
        Dim sqldato As String = ""
        Dim tRow As New TableRow()
        Dim tCell As New TableCell()

        Try
            Table1.Rows.Clear()
            tRow = New TableRow()
            tCell = New TableHeaderCell()
            tCell.Text = "Articulo"
            tRow.Cells.Add(tCell)
            tCell = New TableHeaderCell()
            tCell.Text = "Cantidad"
            tRow.Cells.Add(tCell)
            Table1.Rows.Add(tRow)

            If IsNothing(Session("lstCarrito")) Then
                Table1.Visible = False
            Else
                Table1.Visible = True

                lstCarrito = CType(Session("lstCarrito"), List(Of CE.clsArticulos_CE))
                For Each articulo In lstCarrito
                    tRow = New TableRow()
                    tCell = New TableCell()
                    tCell.Text = articulo.strNombreArticulo
                    tRow.Cells.Add(tCell)
                    tCell = New TableCell()
                    tCell.Text = articulo.dbCantidadPedido.ToString

                    tRow.Cells.Add(tCell)
                    Table1.Rows.Add(tRow)
                Next
            End If
        Catch ex As Exception
            ClientScript.RegisterStartupScript(Me.[GetType](), "aleasrt", "alert(' " & ex.Message & " '); ", True)
        Finally
            tRow.Dispose()
            tCell.Dispose()
            lstCarrito = Nothing
        End Try
    End Sub

    Protected Sub botonpagina_ServerClick(sender As Object, e As EventArgs) Handles botonpagina.ServerClick
        Session("pagIni") = idpagina.Value
        buscarFinale()
    End Sub

    Protected Sub aaaaaaaaaaaaaaa_Click(sender As Object, e As EventArgs) Handles aaaaaaaaaaaaaaa.Click
        Dim data As String = ""
        Dim cantidad As String = ""
        Dim codd As String = ""
        Dim nombr As String = ""
        Dim tarjeta As String = ""
        Dim chkRow As CheckBox = Nothing
        Dim canti As TextBox = Nothing

        Try
            For Each row As GridViewRow In mygrid.Rows
                If row.RowType = DataControlRowType.DataRow Then
                    chkRow = TryCast(row.Cells(0).FindControl("CheckBox1"), CheckBox)
                    canti = TryCast(row.Cells(0).FindControl("txtcantidad"), TextBox)

                    If chkRow.Checked Then
                        cantidad = canti.Text
                        codd = row.Cells(2).Text
                        nombr = row.Cells(3).Text
                        tarjeta = row.Cells(4).Text
                        chkRow.Checked = False

                        agregar(cantidad, codd, nombr, tarjeta)
                    End If
                End If
            Next
        Catch ex As Exception
            ClientScript.RegisterStartupScript(Me.[GetType](), "aleasrt", "alert(' " & ex.Message & " '); ", True)
        End Try
    End Sub

    'Protected Sub checar()
    '    mygrid.Rows(0).Cells(0).Text = "a"
    'End Sub

    'Private Sub SetRowData()
    '    Dim rowIndex As Integer = 0
    '    Try


    '    If ViewState("Customers") IsNot Nothing Then
    '        Dim dtCurrentTable As DataTable = DirectCast(ViewState("Customers"), DataTable)
    '        Dim drCurrentRow As DataRow = Nothing
    '        If dtCurrentTable.Rows.Count > 0 Then
    '            For i As Integer = 1 To dtCurrentTable.Rows.Count
    '                Dim TextBoxName As CheckBox = DirectCast(mygrid.Rows(rowIndex).Cells(1).FindControl("CheckBox1"), CheckBox)
    '                Dim TextBoxAge As TextBox = DirectCast(mygrid.Rows(rowIndex).Cells(2).FindControl("txtcantidad"), TextBox)
    '                drCurrentRow = dtCurrentTable.NewRow()
    '                dtCurrentTable.Rows(i - 1)(0) = TextBoxName.Text
    '                dtCurrentTable.Rows(i - 1)(1) = TextBoxAge.Text
    '                rowIndex += 1
    '            Next
    '            'grvStudentDetails.DataSource = dtCurrentTable;
    '            'grvStudentDetails.DataBind();
    '            ViewState("CurrentTable") = dtCurrentTable
    '        End If
    '    Else
    '        Response.Write("ViewState is null")
    '        End If
    '    Catch ex As Exception
    '        ClientScript.RegisterStartupScript(Me.[GetType](), "aleasrt", "alert(' " & ex.Message & " '); ", True)
    '    End Try
    '    'SetPreviousData();
    'End Sub

    'Private Sub agregaralgrid()
    'End Sub

End Class
