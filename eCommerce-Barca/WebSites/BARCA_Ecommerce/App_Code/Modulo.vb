Imports Microsoft.VisualBasic
Imports System.Xml

Public Class Modulo
    Inherits System.Web.UI.Page

    Public Shared Function getItemPriceDiscountByPolo(cardcode As String, itemcode As String, cantidad As String, session As String) As ArrayList
        Dim fechastring = Today.ToString("yyyyMMdd")
        Dim precioIni As String
        Dim precio As String
        Dim descuento As String
        Dim Respuesta As XmlNode
        Dim ws As DIS.DIServer
        ws = New DIS.DIServer

        Dim listadeprecios As String = ""
        Dim cardcodeList As String = ""
        Dim resutadoarray As New ArrayList
        Dim querypmax As String = ""
        Dim Preciomax As String = ""

        querypmax = "SELECT [dbo].[IL_PMAX] ('" & cardcode & "','" & itemcode & "')as precio "

        Respuesta = ws.ExecuteSQL(session, querypmax)
        Preciomax = ReadXML(Respuesta.InnerXml, "precio")

        resutadoarray.Add(Preciomax)
        resutadoarray.Add("0")

        'Respuesta = ws.ExecuteSQL(session, "select  T1.Price,t3.listnum from OITM T0 inner join ITM1 T1 on T1.itemcode = T0.itemcode inner join OCRD T3 on T3.listnum = T1.pricelist  where T1.ItemCode ='" & itemcode & "' and T3.CardCode='" & cardcode & "'")
        'precioIni = ReadXML(Respuesta.InnerXml, "Price")
        'listadeprecios = ReadXML(Respuesta.InnerXml, "listnum")
        'resutadoarray.Add(precioIni)



        'Dim sqldato As String = "select Discount ,Price from SPP1 where ItemCode='" & itemcode & "' and CardCode ='" & cardcode & "'  and fromdate &lt;='" & fechastring & "' and ( ToDate  &gt;='" & fechastring & "'  or ToDate is null) "
        'Respuesta = ws.ExecuteSQL(session, sqldato)
        'precioIni = ReadXML(Respuesta.InnerXml, "Price")
        'descuento = ReadXML(Respuesta.InnerXml, "Discount")
        'If CInt(precioIni) <> 0 Then
        '    Respuesta = ws.ExecuteSQL(session, "select Discount,Price  from SPP2 where ItemCode='" & itemcode & "' and CardCode ='" & cardcode & "' and Amount='" & cantidad & "'  ")
        '    precio = ReadXML(Respuesta.InnerXml, "Price")

        '    If CInt(precio) <> 0 Then
        '        descuento = ReadXML(Respuesta.InnerXml, "Discount")
        '        resutadoarray.Add(descuento)

        '        resutadoarray.Add(precio)
        '    Else
        '        resutadoarray.Add(descuento)
        '        resutadoarray.Add(precioIni)

        '    End If
        'Else
        '    ' Respuesta = ws.ExecuteSQL(session, "select Discount,Price  from OSPP where ItemCode='" & itemcode & "' and CardCode ='" & cardcode & "'   ")



        '    cardcodeList = "*" & listadeprecios


        '    sqldato = "select Discount ,Price from SPP1 where ItemCode='" & itemcode & "' and CardCode ='" & cardcodeList & "' and fromdate &lt;='" & fechastring & "' and ( ToDate  &gt;='" & fechastring & "'  or ToDate is null) "
        '    Respuesta = ws.ExecuteSQL(session, sqldato)
        '    precioIni = ReadXML(Respuesta.InnerXml, "Price")
        '    descuento = ReadXML(Respuesta.InnerXml, "Discount")
        '    If CInt(precioIni) <> 0 Then
        '        Respuesta = ws.ExecuteSQL(session, "select Discount,Price  from SPP2 where ItemCode='" & itemcode & "' and CardCode ='" & cardcodeList & "' and Amount='" & cantidad & "'  ")
        '        precio = ReadXML(Respuesta.InnerXml, "Price")

        '        If CInt(precio) <> 0 Then
        '            descuento = ReadXML(Respuesta.InnerXml, "Discount")
        '            resutadoarray.Add(descuento)

        '            resutadoarray.Add(precio)
        '        Else
        '            resutadoarray.Add(descuento)
        '            resutadoarray.Add(precioIni)

        '        End If
        '    Else


        '        Respuesta = ws.ExecuteSQL(session, "select  T1.Price from OITM T0 inner join ITM1 T1 on T1.itemcode = T0.itemcode inner join OCRD T3 on T3.listnum = T1.pricelist  where T1.ItemCode ='" & itemcode & "' and T3.CardCode='" & cardcode & "'")

        '        precioIni = ReadXML(Respuesta.InnerXml, "Price")
        '        descuento = ReadXML(Respuesta.InnerXml, "Discount")
        '        Dim calculo As Integer = CInt(precioIni)
        '        If calculo <> 0 Then
        '            resutadoarray.Add(descuento)
        '            resutadoarray.Add(precioIni)
        '        Else
        '            resutadoarray.Add(0)
        '            resutadoarray.Add(resutadoarray(0))
        '        End If

        '    End If









        'End If
        'If resutadoarray(1) = "" Then
        '    resutadoarray(1) = "0"
        'End If

        Return resutadoarray
    End Function

    Public Shared Function ReadXML(Xml As String, NodeName As String) As String
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
