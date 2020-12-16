<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="frmOrden.aspx.vb" Inherits="frmOrden" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script src="Scripts/bootstrap-datepicker.min.js"></script>

    <link href="Content/bootstrap-datepicker.min.css" rel="stylesheet" />

    <script src="Scripts/checkbox-x.min.js"></script>
    <link href="Content/theme-krajee-flatblue.min.css" rel="stylesheet" />
    <link href="Content/checkbox-x.min.css" rel="stylesheet" />
    <style runat="server" id="estilo"></style>

    <div id="page-wrapper">
        <div class="row">
            <style>
                input[type=checkbox] {
                    /* Double-sized Checkboxes */
                    -ms-transform: scale(1.4); /* IE */
                    -moz-transform: scale(1.4); /* FF */
                    -webkit-transform: scale(1.42); /* Safari and Chrome */
                    -o-transform: scale(1.4); /* Opera */
                    padding: 10px;
                }

                /* ROUNDED ONE */
                .roundedOne {
                    width: 28px;
                    height: 28px;
                    background: #fcfff4;
                    background: -webkit-linear-gradient(top, #fcfff4 0%, #dfe5d7 40%, #b3bead 100%);
                    background: -moz-linear-gradient(top, #fcfff4 0%, #dfe5d7 40%, #b3bead 100%);
                    background: -o-linear-gradient(top, #fcfff4 0%, #dfe5d7 40%, #b3bead 100%);
                    background: -ms-linear-gradient(top, #fcfff4 0%, #dfe5d7 40%, #b3bead 100%);
                    background: linear-gradient(top, #fcfff4 0%, #dfe5d7 40%, #b3bead 100%);
                    filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#fcfff4', endColorstr='#b3bead',GradientType=0 );
                    margin: 20px auto;
                    -webkit-border-radius: 50px;
                    -moz-border-radius: 50px;
                    border-radius: 50px;
                    -webkit-box-shadow: inset 0px 1px 1px white, 0px 1px 3px rgba(0,0,0,0.5);
                    -moz-box-shadow: inset 0px 1px 1px white, 0px 1px 3px rgba(0,0,0,0.5);
                    box-shadow: inset 0px 1px 1px white, 0px 1px 3px rgba(0,0,0,0.5);
                    position: relative;
                }

                    .roundedOne label {
                        cursor: pointer;
                        position: absolute;
                        width: 20px;
                        height: 20px;
                        -webkit-border-radius: 50px;
                        -moz-border-radius: 50px;
                        border-radius: 50px;
                        left: 4px;
                        top: 4px;
                        -webkit-box-shadow: inset 0px 1px 1px rgba(0,0,0,0.5), 0px 1px 0px rgba(255,255,255,1);
                        -moz-box-shadow: inset 0px 1px 1px rgba(0,0,0,0.5), 0px 1px 0px rgba(255,255,255,1);
                        box-shadow: inset 0px 1px 1px rgba(0,0,0,0.5), 0px 1px 0px rgba(255,255,255,1);
                        background: -webkit-linear-gradient(top, #222 0%, #45484d 100%);
                        background: -moz-linear-gradient(top, #222 0%, #45484d 100%);
                        background: -o-linear-gradient(top, #222 0%, #45484d 100%);
                        background: -ms-linear-gradient(top, #222 0%, #45484d 100%);
                        background: linear-gradient(top, #222 0%, #45484d 100%);
                        filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#222', endColorstr='#45484d',GradientType=0 );
                    }

                        .roundedOne label:after {
                            -ms-filter: "progid:DXImageTransform.Microsoft.Alpha(Opacity=0)";
                            filter: alpha(opacity=0);
                            opacity: 0;
                            content: '';
                            position: absolute;
                            width: 16px;
                            height: 16px;
                            background: #00bf00;
                            background: -webkit-linear-gradient(top, #00bf00 0%, #009400 100%);
                            background: -moz-linear-gradient(top, #00bf00 0%, #009400 100%);
                            background: -o-linear-gradient(top, #00bf00 0%, #009400 100%);
                            background: -ms-linear-gradient(top, #00bf00 0%, #009400 100%);
                            background: linear-gradient(top, #00bf00 0%, #009400 100%);
                            -webkit-border-radius: 50px;
                            -moz-border-radius: 50px;
                            border-radius: 50px;
                            top: 2px;
                            left: 2px;
                            -webkit-box-shadow: inset 0px 1px 1px white, 0px 1px 3px rgba(0,0,0,0.5);
                            -moz-box-shadow: inset 0px 1px 1px white, 0px 1px 3px rgba(0,0,0,0.5);
                            box-shadow: inset 0px 1px 1px white, 0px 1px 3px rgba(0,0,0,0.5);
                        }

                        .roundedOne label:hover::after {
                            -ms-filter: "progid:DXImageTransform.Microsoft.Alpha(Opacity=30)";
                            filter: alpha(opacity=30);
                            opacity: 0.3;
                        }

                    .roundedOne input[type=checkbox]:checked + label:after {
                        -ms-filter: "progid:DXImageTransform.Microsoft.Alpha(Opacity=100)";
                        filter: alpha(opacity=100);
                        opacity: 1;
                    }

                /*aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa*/
                .abc table {
                    background: #f5f5f5;
                    border-collapse: separate;
                    box-shadow: inset 0 1px 0 #fff;
                    font-size: 12px;
                    line-height: 24px;
                    margin: 30px auto;
                    text-align: left;
                    width: 800px;
                }

                th {
                    background: url(http://jackrugile.com/images/misc/noise-diagonal.png), linear-gradient(#777, #444);
                    border-left: 1px solid #555;
                    border-right: 1px solid #777;
                    border-top: 1px solid #555;
                    border-bottom: 1px solid #333;
                    box-shadow: inset 0 1px 0 #999;
                    color: #fff;
                    font-weight: bold;
                    padding: 10px 15px;
                    position: relative;
                    text-shadow: 0 1px 0 #000;
                }

                    th:after {
                        background: linear-gradient(rgba(255,255,255,0), rgba(255,255,255,.08));
                        content: '';
                        display: block;
                        height: 25%;
                        left: 0;
                        margin: 1px 0 0 0;
                        position: absolute;
                        top: 25%;
                        width: 100%;
                    }

                    th:first-child {
                        border-left: 1px solid #777;
                        box-shadow: inset 1px 1px 0 #999;
                    }

                    th:last-child {
                        box-shadow: inset -1px 1px 0 #999;
                    }

                td {
                    border-right: 1px solid #fff;
                    border-left: 1px solid #e8e8e8;
                    border-top: 1px solid #fff;
                    border-bottom: 1px solid #e8e8e8;
                    padding: 10px 15px;
                    position: relative;
                    transition: all 300ms;
                }

                    td:first-child {
                        box-shadow: inset 1px 0 0 #fff;
                    }

                    td:last-child {
                        border-right: 1px solid #e8e8e8;
                        box-shadow: inset -1px 0 0 #fff;
                    }

                tr {
                }

                    tr:nth-child(odd) td {
                        background: #f1f1f1;
                    }

                    tr:last-of-type td {
                        box-shadow: inset 0 -1px 0 #fff;
                    }

                        tr:last-of-type td:first-child {
                            box-shadow: inset 1px -1px 0 #fff;
                        }

                        tr:last-of-type td:last-child {
                            box-shadow: inset -1px -1px 0 #fff;
                        }

                tbody:hover td {
                }

                tbody:hover tr:hover td {
                    color: #444;
                }

                .row {
                    margin-right: 0px;
                    margin-left: 0px;
                }

                .row-centered {
                    text-align: center;
                }

                .col-centered {
                    display: inline-block;
                    float: none;
                    /* reset the text-align */
                    text-align: left;
                    /* inline-block space fix */
                    margin-right: -4px;
                }

                .col-fixed {
                    /* custom width */
                    width: 220px;
                }

                .col-min {
                    /* custom min width */
                    min-width: 320px;
                }

                .col-max {
                    /* custom max width */
                    max-width: 320px;
                }

                /* visual styles */


                h1 {
                    margin: 40px 0px 20px 0px;
                    color: #95c500;
                    font-size: 28px;
                    line-height: 34px;
                    text-align: center;
                }

                [class*="col-"] {
                    padding-top: 10px;
                    padding-bottom: 15px;
                }

                    [class*="col-"]:before {
                        display: block;
                        position: relative;
                        margin-bottom: 8px;
                        font-family: sans-serif;
                        font-size: 10px;
                        letter-spacing: 1px;
                        color: #658600;
                        text-align: left;
                    }

                .item {
                    width: 100%;
                    height: 100%;
                    border: 1px solid #cecece;
                    padding: 16px 8px;
                    background: #ededed;
                    background: -webkit-gradient(linear, left top, left bottom,color-stop(0%, #f4f4f4), color-stop(100%, #ededed));
                    background: -moz-linear-gradient(top, #f4f4f4 0%, #ededed 100%);
                    background: -ms-linear-gradient(top, #f4f4f4 0%, #ededed 100%);
                }

                /* content styles */
                .item {
                    display: table;
                }

                .content {
                    display: table-cell;
                    vertical-align: middle;
                    text-align: center;
                }

                    .content:before {
                        content: "Content";
                        font-family: sans-serif;
                        font-size: 12px;
                        letter-spacing: 1px;
                        color: #747474;
                    }



                /* centering styles for jsbin */
            </style>

            <link href="Content/ordenestilo.css" rel="stylesheet" />

            <div class="titulolink">
                <br />
                <a href="frmInicio.aspx"><i class="fa fa-home fa-fw"></i></a>>
            <a id="linkaqui" href="#">Orden de venta</a>
                <br />
                <br />
            </div>

            <div class="row  ">
                <div class="col-xs-12 col-sm-12  col-md-12 col-lg-4 ">
                    <div class="input-group" style="max-width: 300px">
                        <%--barra de busqueda--%>
                        <input type="text" runat="server" id="barrabusqueda" class="form-control" onkeypress="runScript(event)" placeholder="Buscar por..." />
                        <span class="input-group-btn">
                            <button id="btnbuscar" runat="server" class="btn btn-default" type="button"><i class="fa fa-search"></i></button>
                        </span>
                    </div>
                </div>

                <div class="col-xs-12 col-sm-12  col-md-12 col-lg-8 " runat="server" id="minicarrito">
                    <div style="width: auto; overflow-x: auto;">
                        <div style="min-width: 500px;">
                            <asp:Table ID="Table1" class="table abc" runat="server" Style="width: 100%; min-width: 500px">
                                <asp:TableHeaderRow>
                                    <asp:TableHeaderCell>Articulo</asp:TableHeaderCell>
                                    <asp:TableHeaderCell>Cantidad</asp:TableHeaderCell>
                                </asp:TableHeaderRow>
                            </asp:Table>

                        </div>
                    </div>
                </div>

            </div>

            <%--++++++++++++++++++++++++++++++++++++++++--%>
            <div runat="server" id="articuloslista"></div>
            <%--+++++++++++++++++++++++++++++++++++++++++++++++--%>

            <div class="row  ">
                <div class="col-xs-12 col-sm-12  col-md-12 col-lg-12 " runat="server" id="Div1">
                    <asp:GridView ID="mygrid" runat="server" Width="100%" Style="width: 100%;"
                        ShowFooter="False" AutoGenerateColumns="False"
                        CellPadding="4" ForeColor="#333333"
                        GridLines="None">
                        <Columns>
                            <asp:TemplateField HeaderText="Select">
                                <ItemTemplate>
                                    <asp:CheckBox ID="CheckBox1" Style="zoom: 1.5;" runat="server" />
                                    <%--  <input type="checkbox" id="checkprueba" name="change" runat="server" value="1" data-toggle="checkbox-x" data-size="xl" data-three-state="false" />--%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cantidad">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtcantidad" runat="server" size="4" Text="1"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="codigo" HeaderText="Codigo" />
                            <asp:BoundField DataField="nombre" HeaderText="Nombre" />
                            <asp:BoundField DataField="tarjetafab" HeaderText="Tarjeta Fabricación" />
                        </Columns>
                    </asp:GridView>
                    <br />
                    <asp:Button ID="aaaaaaaaaaaaaaa" runat="server" Text="AGREGAR" class="btn  btn-large  btn-success" Style="float: right; height: 50px; width: 250px" />
                </div>
                <div class="col-xs-12 col-sm-6  col-md-6 col-lg-6 ">
                    <asp:DropDownList ID="DropDownList1" runat="server">
                        <asp:ListItem Text="Todos" Value="all"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <input type="text" runat="server" id="txtwharehouse" clientidmode="Static" style="display: none" />
            <input type="text" runat="server" id="articuno" clientidmode="Static" style="display: none" />
            <input type="text" runat="server" id="articprecio" clientidmode="Static" style="display: none" />
            <input type="text" runat="server" id="articnom" clientidmode="Static" style="display: none" />
            <input type="text" runat="server" id="idpagina" clientidmode="Static" style="display: none" />
            <%--<button id="secretbutton" runat="server" class="btn btn-default" type="button" onserverclick="agregar" style="display: none">dois</button>--%>
            <button id="botonpagina" runat="server" class="btn btn-default" type="button" style="display: none">dois</button>

            <%-- <button id="Button1" runat="server" class="btn btn-default" type="button" onclick="envioid(id)"  >mete id a textbox</button>
            <button id="Button2" runat="server" class="btn btn-default" type="button" onclick="envioid(id)" >mete id a textbox</button>
            <button id="Button3" runat="server" class="btn btn-default" type="button" onclick="envioid(id)" >mete id a textbox</button>--%>

            <div style="text-align: center;" runat="server" id="PaginationDiv">
            </div>
            <div style="text-align: center;" runat="server" id="errorDiv">
            </div>

            <script type="text/javascript">
                function imgError(image) {
                    image.onerror = "";
                    image.src = "data:image/png;base64,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";
                    return true;
                }
                function discre(id) {
                    document.getElementById("idpagina").value = id.substring(3, id.length);
                    document.getElementById('<%= botonpagina.ClientID%>').click();
                    //reload(); 
                }

                function runScript(e) {
                    if (e.keyCode == 13) {
                        document.getElementById('<%= btnbuscar.ClientID%>').click();
                    }
                }
                //Called this method on any button click  event for Testing
                function envioid(as) {
                    //PageMethods.agregar(Param1);
                    // alert(as.toString()) 
                    do {
                        var cantidad = parseInt(window.prompt("Por favor introduzca la cantidad", ""), 10);
                    } while (cantidad > 10000 || cantidad < 1);
                    document.getElementById("articuno").value = (as.substring(4, as.length));
                    document.getElementById("articprecio").value = cantidad;
                    document.getElementById("articnom").value = document.getElementById("title" + (as.substring(4, as.length))).innerHTML;

                    if (isNaN(cantidad) == false) {

                    }
                    //document.getElementById("articprecio").value = document.getElementById("precio"+(as.substring(4, as.length  ))).innerHTML;
                    //elem.value = "My default value";
                }
            </script>
        </div>
    </div>
</asp:Content>

