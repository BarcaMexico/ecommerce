Imports Modulo

Imports System.Xml
Imports System.Globalization

Partial Class frmConfir
    Inherits System.Web.UI.Page
    Public ws As DIS.DIServer
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

            Dim twoDarray(20, 10) As String
             
            'Busqueda de facturas venciadas Balance
            Dim tRow As New TableRow()
            Dim tCell As New TableCell()

            Dim factura As ArrayList = New ArrayList
             
            ws = New DIS.DIServer

             
            Try
                Dim Respuesta As XmlNode
                Dim sqldato As String = "   select DocEntry,DocNum ,DocDate  from ODLN where DocNum  ='" & Session("DocNumDis") & "' "
                Respuesta = ws.ExecuteSQL(Session("Token"), sqldato)


                sqldato = "   select ItemCode ,Dscription ,Quantity ,unitMsr from DLN1 where DocEntry  ='" & ReadXML(Respuesta.InnerXml, "DocEntry") & "' "
                Respuesta = ws.ExecuteSQL(Session("Token"), sqldato)


                Dim doc2 As New XmlDocument()
                doc2.LoadXml(Respuesta.InnerXml)
                System.Diagnostics.Debug.Write(Respuesta.InnerXml & vbCrLf)
                Dim root2 As XmlNode = doc2.FirstChild
                Dim rootin As XmlNode

                root2 = root2.LastChild
                'root2 = root2.FirstChild



                If root2.HasChildNodes Then
                    Dim s As Integer
                    For s = 0 To root2.ChildNodes.Count - 1

                        rootin = root2.ChildNodes(s)

                        If rootin.HasChildNodes Then
                            Dim x As Integer
                            tRow = New TableRow()

                            For x = 0 To rootin.ChildNodes.Count - 1


                                Select Case rootin.ChildNodes(x).Name

                                    Case "ItemCode"
                                        tCell = New TableCell()
                                        tCell.Text = rootin.ChildNodes(x).InnerText
                                        tRow.Cells.Add(tCell)


                                    Case "Dscription"
                                        tCell = New TableCell()
                                        tCell.Text = rootin.ChildNodes(x).InnerText
                                        tRow.Cells.Add(tCell)

                                    Case "Quantity"
                                        tCell = New TableCell()
                                        tCell.Text = CInt(rootin.ChildNodes(x).InnerText)
                                        tRow.Cells.Add(tCell)

                                    Case "unitMsr"
                                        tCell = New TableCell()
                                        tCell.Text = rootin.ChildNodes(x).InnerText
                                        tRow.Cells.Add(tCell)



                                End Select
                            Next x


                            Table1.Rows.Add(tRow)

                        End If
                    Next s
                End If




            Catch ex As Exception
            End Try
        End If

    End Sub


    Protected Sub secretbutton_ServerClick(sender As Object, e As EventArgs) Handles secretbutton.ServerClick

        Try
             
            ws = New DIS.DIServer

             
            Dim Respuesta As XmlNode 
            Dim sqldato As String = "  select DocEntry from ODLN where DocNum  =" & Session("DocNumDis") & " "
            Respuesta = ws.ExecuteSQL(Session("Token"), sqldato) 
            Dim mystring As String = "<env:Envelope xmlns:env='http://schemas.xmlsoap.org/soap/envelope/'>   <env:Header>    <SessionID>" & Session("Token") & "</SessionID>   </env:Header>   <env:Body>    <dis:UpdateObject xmlns:dis='http://www.sap.com/SBO/DIS'>     <BOM>      <BO>       <AdmInfo>        <Object>oDeliveryNotes</Object>       </AdmInfo>       <QueryParams>        <DocEntry>" & ReadXML(Respuesta.InnerXml, "DocEntry") & "</DocEntry>       </QueryParams>     <Documents>                 <row>          <U_IL_estado>C</U_IL_estado>            </row>                </Documents>               </BO>     </BOM>    </dis:UpdateObject>   </env:Body>  </env:Envelope>"
            Dim restring As String = ws.Interact(Session("Token"), mystring) 
            ClientScript.RegisterStartupScript(Me.[GetType](), "aleasrt", "alert(' Se Confirmo " & Session("DocNumDis") & " ');document.location.href='frmEmbarques'; ", True)
             
        Catch ex As Exception

        End Try
         
    End Sub 
    Private Function ReadXML(Xml As String, NodeName As String)
        Dim reader As System.Xml.XmlTextReader = New System.Xml.XmlTextReader(New System.IO.StringReader(Xml))
        Dim valor As String
        valor = ""
        Do While (reader.Read())
            If reader.NodeType = System.Xml.XmlNodeType.Element Then
                If reader.Name.ToUpper = NodeName.ToUpper Then
                    valor = reader.ReadElementContentAsString
                End If
            End If
        Loop
        reader.Close()
        reader.Dispose()
        Return valor
    End Function
     

     

End Class
