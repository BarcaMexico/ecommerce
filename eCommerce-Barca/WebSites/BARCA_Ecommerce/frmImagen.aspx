<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="frmImagen.aspx.vb" Inherits="frmImagen" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


     

    <link href="Content/tablanocolor.css" rel="stylesheet" />
 
    
    <script src="Scripts/bootstrap-datepicker.min.js"></script>

    <link href="Content/bootstrap-datepicker.min.css" rel="stylesheet" />
        <style runat="server" id="estilo"></style>

    <div id="page-wrapper">
        <div class="row"> 
            <div class="titulolink">
                <br />
                <a href="frmInicio.aspx"><i class="fa fa-home fa-fw"></i></a>>
            <a href="#">Usuario</a> 
                <br />
                <br />
            </div> 
            </div>
            <!-- @end .container --> 
        <img class="img-responsive" src="" alt=" " runat ="server" id="imagenperfil" width="460" height="460" style="margin-left:auto;margin-right :auto ;" /> 
        <div style="text-align:center ">
         Imagen:<br /><br />
                    <input type="hidden" name="MAX_FILE_SIZE" value="4194304"  style="   display: block; margin: 0 auto;"/>
                    <input id="File1" runat="server" type="file" accept="image/*"  style="   display: block; margin: 0 auto;"/>
        <br />
            </div>
        <div>
        <button id="subirr" runat="server" class=" btn btn-primary   " type="button"   style="width:200px;   display: block; margin: 0 auto;" >Cambiar imagen</button>
        </div>
          <br /> <br /> <br />
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
</asp:Content>

