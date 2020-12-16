<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="frmFac.aspx.vb" Inherits="frmFac" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">



    <script src="Scripts/bootstrap-datepicker.min.js"></script>

    <link href="Content/bootstrap-datepicker.min.css" rel="stylesheet" />

     <style runat="server" id="estilo"></style>

    <div id="page-wrapper">
        <div class="row">
             
            <%--<link href="Content/tablamaster.css" rel="stylesheet" />--%> 
            <link href="Content/tablanocolor.css" rel="stylesheet" />

            <div class="titulolink">
                <br />
                <a href="frmInicio.aspx"><i class="fa fa-home fa-fw"></i></a>>
            <a href="#">Historial de facturas</a>
                <br />
                <br />
            </div>
             
            <div> 

                <div style="width: 110px; margin-left: auto; margin-right: auto">Búsqueda  por:</div>

                 

                <div style="max-width: 307px; margin-left: auto; margin-right: auto">
                    <asp:RadioButtonList runat="server" ID="radios" onchange="return selectValue();" RepeatLayout="Flow" CssClass="labels">
                        <asp:ListItem    Text="Mostrar facturas vencidas" Value="factuavencida"   ></asp:ListItem>
                        <asp:ListItem  Text="Mostrar facturas vencidas por fecha" Value="factuavencidaxfecha"></asp:ListItem>
                        <asp:ListItem Text="Mostrar facturas por fecha" Value="factuaxfecha"></asp:ListItem>
                    </asp:RadioButtonList>
                </div>


                




                <div class="row row-centered" id="fechas"   >
                    <div class="col-xs-6 col-centered col-fixed">
                        <div style="width: 20px; margin-left: auto; margin-right: auto">De:</div>
                        <div class="input-group date" style="width: 200px; margin-left: auto; margin-right: auto">
                            <input type="text" class="form-control datepicker" runat="server" id="fecha1"><span class="input-group-addon"><i class="glyphicon glyphicon-th"></i></span>
                        </div>
                    </div>
                    <div class="col-xs-6 col-centered col-fixed">
                        <div style="width: 20px; margin-left: auto; margin-right: auto">A:</div>
                        <div class="input-group date" style="width: 200px; margin-left: auto; margin-right: auto">
                            <input type="text" class="form-control datepicker" runat="server" id="fecha2"><span class="input-group-addon"><i class="glyphicon glyphicon-th"></i></span>
                        </div>
                    </div>
                </div>




                <div class="row row-centered">
                    <div class="col-xs-12 col-centered col-fixed">
                        <div style="width: 68px; margin-left: auto; margin-right: auto">


                            <button runat="server" id="buscarfac" type="button" class="btn btn-default" aria-label="Left Align">
                                Filtrar
                            </button>
                        </div>


                    </div>
                </div>













                <script>
                    $('.input-group.date').datepicker({

                        autoclose: true, format: 'yyyy-mm-dd'

                    });

                    function discre(id) {
                        document.getElementById("idpagina").value = id.substring(3, id.length);
                        document.getElementById('<%= botonpagina.ClientID%>').click();
                        //reload(); 
                }

                   
                    function selectValue() {
                        
                        if (document.getElementById("ContentPlaceHolder1_radios_0").checked == true) {
                             
                            $("#ContentPlaceHolder1_radios_0").css('color', 'black');
                            $("#ContentPlaceHolder1_radios_1").css('color', 'grey');
                            $("#ContentPlaceHolder1_radios_2").css('color', 'grey');

                            $("#fechas").hide();

                        } else {
                            $("#ContentPlaceHolder1_radios_0").css('color', 'grey');
                            $("#ContentPlaceHolder1_radios_1").css('color', 'black');
                            $("#ContentPlaceHolder1_radios_2").css('color', 'black');
                            $("#fechas").show();
                        }

                        
                    }
                 

                </script>






                <div style="width: auto; overflow-x: auto;">
                    <div style="min-width: 600px;">

                        <asp:Table ID="Table1" class="table abc" runat="server" Style="width: 100%; min-width: 600px">
                            <asp:TableHeaderRow>
                                <asp:TableHeaderCell>Fecha</asp:TableHeaderCell>
                                <asp:TableHeaderCell>Vencimiento</asp:TableHeaderCell>
                                <asp:TableHeaderCell>Folio</asp:TableHeaderCell>
                                <asp:TableHeaderCell>Moneda</asp:TableHeaderCell>
                                <asp:TableHeaderCell>Cargo</asp:TableHeaderCell>
                                <asp:TableHeaderCell>Abono</asp:TableHeaderCell>
                                <asp:TableHeaderCell>Saldo</asp:TableHeaderCell>
                                <asp:TableHeaderCell>Descarga</asp:TableHeaderCell>
                            </asp:TableHeaderRow>

                        </asp:Table>

                    </div>

                     <div style="text-align: center;" runat="server" id="PaginationDiv"> 
            </div>

                </div>

              <input type="text" runat="server" id="actuacan" clientidmode="Static" style="display: none" />
            <input type="text" runat="server" id="actuaid" clientidmode="Static" style="display: none" />
            
                 <button id="descargaboton" runat="server" class="btn btn-default" type="button" style="display: none">dois</button>
               <button id="descargabotonpdf" runat="server" class="btn btn-default" type="button" style="display: none">dois</button>
                

                  <input type="text" runat="server" id="idpagina" clientidmode="Static" style="display: none" />
                 <button id="botonpagina" runat="server" class="btn btn-default" type="button"   style="display: none">dois</button>

                 


                <script>


                    <%-- function discre(id) {

                        document.getElementById("actuacan").value = 'dis';
                        document.getElementById("actuaid").value = id.substring(3, id.length);
                        document.getElementById('<%= secretbutton.ClientID%>').click();
                        //reload(); 
                    }--%>

                    

                    function descargapdf(id) {

                        document.getElementById("actuacan").value = 'dis';
                        document.getElementById("actuaid").value = id.substring(3, id.length);
                        document.getElementById('<%= descargabotonpdf.ClientID%>').click();

                         //reload(); 
                    }

                    function descarga(id) {

                        document.getElementById("actuacan").value = 'dis';
                        document.getElementById("actuaid").value = id.substring(3, id.length);
                        document.getElementById('<%= descargaboton.ClientID%>').click();

                          //reload(); 
                    }
            </script>
        </div>

             
</asp:Content>

