<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="frmMensajesAdm.aspx.vb" Inherits="frmMensajesAdm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">



    <script src="Scripts/bootstrap-datepicker.min.js"></script>

    <link href="Content/bootstrap-datepicker.min.css" rel="stylesheet" />

    <link href="Content/tablanocolor.css" rel="stylesheet" />
    <style runat="server" id="estilo"></style>



    <div id="page-wrapper">
        <div class="row">



            

            <div class="titulolink">
                <br />
                <a href="frmInicio.aspx"><i class="fa fa-home fa-fw"></i></a>>
            <a href="#">Mensajes</a>
                <br />
                <br />
            </div>



            <script>
                $('.input-group.date').datepicker({

                    autoclose: true, format: 'yyyy-mm-dd'

                });
            </script>

           <%-- <div style="width: 170px; margin-left: auto">
                <button id="enviarpinshimensaje" onclick="lansa()" style="width: 170px; margin-left: auto" type="button" class="btn btn-default">
                    Nuevo mensaje <i class='fa  fa-envelope-o  '></i>
                </button>

            </div>--%>
           
             <div style="width: auto; overflow-x: auto;"> 
                    <div style="min-width: 600px;"> 

                        <asp:Table ID="Table1" class="table abc" runat="server" Style="width: 100%; min-width: 600px">
                        <asp:TableHeaderRow>
                            <asp:TableHeaderCell>Usuario</asp:TableHeaderCell>
                            <asp:TableHeaderCell>Fecha</asp:TableHeaderCell>
                            <asp:TableHeaderCell>Asunto</asp:TableHeaderCell>
                            <asp:TableHeaderCell>Estado</asp:TableHeaderCell>
                            <asp:TableHeaderCell>Leer</asp:TableHeaderCell>

                        </asp:TableHeaderRow>
                    </asp:Table>
                </div>
            </div>








            <input type="text" runat="server" id="actuacan" clientidmode="Static" style="display: none" />
            <input type="text" runat="server" id="actuaid" clientidmode="Static" style="display: none" />
            <button id="secretbutton" runat="server" class="btn btn-default" type="button" style="display: none">dois</button>



            <script>

                function discre(id) {

                    
                    document.getElementById("actuaid").value = id.substring(3, id.length);
                    document.getElementById('<%= secretbutton.ClientID%>').click();
                       //reload(); 
                }
                function lansa() {
                    //alert("aaa");
                    document.getElementById("enviarpinshimensaje").style.display = "none";
                    document.getElementById("quedemonios").style.display = "Block";
                    //document.getElementById('<%= secretbutton.ClientID%>').click(); 
                    //reload(); 
                }
            </script>



        </div>

    </div>





</asp:Content>

