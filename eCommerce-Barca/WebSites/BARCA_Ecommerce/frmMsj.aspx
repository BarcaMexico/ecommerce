<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="frmMsj.aspx.vb" Inherits="frmMsj" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">



    <script src="Scripts/bootstrap-datepicker.min.js"></script>

    <link href="Content/bootstrap-datepicker.min.css" rel="stylesheet" />



    <style runat="server" id="estilo"></style>


    <div id="page-wrapper">
        <div class="row">



            <link href="Content/chat.css" rel="stylesheet" />

            <div class="titulolink">
                <br />
                <a href="frmInicio.aspx"><i class="fa fa-home fa-fw"></i></a>>
            <a href="#">Mensajes</a>>
            <a href="#">Leer Mensaje</a>
                <br />
                <br />
            </div>



            <script>
                $('.input-group.date').datepicker({

                    autoclose: true, format: 'yyyy-mm-dd'

                });
            </script>







            <div class="container">
                <div style="padding-bottom: 10px" >
                    <button runat="server" id="btnregresar" type="button" class="btn btn-default" aria-label="Left Align">
                        <i class='fa fa-arrow-left '></i>Regresar
                    </button>
                </div>
                <div runat="server" id="mensajesapliados"></div>

                <div>
                    <textarea id="TextArea1" style="max-width: 600px;" rows="3" runat="server" class="form-control"></textarea>
                </div>
                <div style="width: 170px; margin-left: auto">
                    <button runat="server" id="enviarpinshimensaje" style="width: 170px; margin-left: auto" type="button" class="btn btn-default" aria-label="Right Align">
                        Eviar mensaje <i class='fa  fa-envelope-o  '></i>
                    </button>
                </div>



            </div>
            <!-- @end .container -->




            <input type="text" runat="server" id="actuacan" clientidmode="Static" style="display: none" />
            <input type="text" runat="server" id="actuaid" clientidmode="Static" style="display: none" />
            <button id="secretbutton" runat="server" class="btn btn-default" type="button" style="display: none">dois</button>



            <script>
                function confirpedido(id) {

                    document.getElementById("actuacan").value = 'confir';
                    document.getElementById("actuaid").value = id.substring(3, id.length);
                    document.getElementById('<%= secretbutton.ClientID%>').click();
                    //reload();

                }

                function discre(id) {

                    document.getElementById("actuacan").value = 'dis';
                    document.getElementById("actuaid").value = id.substring(3, id.length);
                    document.getElementById('<%= secretbutton.ClientID%>').click();
                    //reload(); 
                }
            </script>



        </div>

    </div>





</asp:Content>

