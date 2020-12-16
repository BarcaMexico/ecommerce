Imports Modulo

Imports System.Xml
Imports System.Globalization

Partial Class frmdiscreadmin
    Inherits System.Web.UI.Page
    Public ws As DIS.DIServer
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            'Busqueda de facturas venciadas Balance
            Dim tRow As New TableRow()
            Dim tCell As New TableCell()
            ws = New DIS.DIServer

            Try

                Dim Respuesta As XmlNode 'busca las estregas con discrepancias de todos los socios
                Dim sqldato As String = "select  T0.DocDate as fecha,  T0.DocNum as entrega  , T1.BaseRef as pedido , T0.CardName as  socio  " &
 "from ODLN T0 inner join DLN1 T1 on T0.DocEntry = T1.DocEntry   where      " &
 "U_IL_estado  ='D'  group by T1.BaseRef ,T0.DocDate  , T0.Address2,T0.TrackNo,T0.DocNum, T0.CardName order by  T0.DocNum asc"

                Respuesta = ws.ExecuteSQL(Session("Token"), sqldato)
                If ReadXML(Respuesta.InnerXml, "factura") <> "0" Then
                    Dim documentum As String = ""
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
                                Session("fcsi") = 0
                                For x = 0 To rootin.ChildNodes.Count - 1
                                    Select Case rootin.ChildNodes(x).Name
                                        Case "fecha"
                                            tCell = New TableCell()
                                            tCell.Text = DateTime.ParseExact((rootin.ChildNodes(x).InnerText), "yyyyMMdd", CultureInfo.InvariantCulture)
                                            tRow.Cells.Add(tCell)
                                        Case "entrega"
                                            tCell = New TableCell()
                                            tCell.Text = rootin.ChildNodes(x).InnerText
                                            documentum = rootin.ChildNodes(x).InnerText
                                            tRow.Cells.Add(tCell)
                                        Case "pedido"
                                            tCell = New TableCell()
                                            tCell.Text = rootin.ChildNodes(x).InnerText
                                            tRow.Cells.Add(tCell)
                                        Case "socio"
                                            tCell = New TableCell()
                                            tCell.Text = rootin.ChildNodes(x).InnerText
                                            tRow.Cells.Add(tCell)

                                            tCell = New TableCell()
                                            tCell.HorizontalAlign = HorizontalAlign.Center
                                            tCell.Text = "<button class='btn btn-info'  id='row" & documentum & "'       onclick='discre( id)'>Ver detalles <i class='fa fa-eye'></i></button>"
                                            tCell.HorizontalAlign = HorizontalAlign.Center
                                            tRow.Cells.Add(tCell)

                                    End Select
                                Next x
                                Table1.Rows.Add(tRow)
                            End If
                        Next s
                    End If
                Else
                    Table1.Visible = False
                End If
            Catch ex As Exception
            End Try
        End If

    End Sub

    Protected Sub secretbutton_ServerClick(sender As Object, e As EventArgs) Handles secretbutton.ServerClick
        ws = New DIS.DIServer

        Try 
            Session("DocNumDis") = actuaid.Value 'abre los detalles de la discrepancia
            Response.Redirect("frmEmbarquesDiscrepAdmin.aspx") 
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


