<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="frmConfig.aspx.vb" Inherits="frmConfig" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <link href="Content/tablanocolor.css" rel="stylesheet" />

    <style>
        /*.navbar-top-links > li > a:hover {     background:#9f9   no-repeat 0 -20px;     color:#9f9 }    .navbar-top-links > li > a:hover > .fa { color:#000 }*/

        .fileUpload {
            position: relative;
            overflow: hidden;
            margin: 10px;
        }

            .fileUpload input.upload {
                position: absolute;
                top: 0;
                right: 0;
                margin: 0;
                padding: 0;
                font-size: 20px;
                cursor: pointer;
                opacity: 0;
                filter: alpha(opacity=0);
            }
    </style>

    <style runat="server" id="estilo"></style>

    <div id="page-wrapper">

        <div class="row">
            <div class="titulolink">
                <br />
                <a href="frmMensajesAdm.aspx"><i class="fa fa-home fa-fw"></i></a>>
            <a href="#">Configuracion</a>
                <br />
                <br />
            </div>

            <br />
            <div class="row  row-centered ">
                <div class="col-xs-12 col-sm-4 col-md-4 col-lg-4">

                    <div>Articulos a mostrar:</div>
                    <asp:DropDownList ID="DropDownList1" runat="server">
                        <asp:ListItem Text="Con stock" Value="stock"></asp:ListItem>
                        <asp:ListItem Text="Todos los articulos" Value="all"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-xs-12 col-sm-4 col-md-4 col-lg-4">
                    <div>Cantidad por busqueda:</div>
                    <asp:DropDownList ID="DropDownList2" runat="server">
                        <asp:ListItem Text="5 Articulos" Value="5"></asp:ListItem>
                        <asp:ListItem Text="10 Articulos" Value="10"></asp:ListItem>
                        <asp:ListItem Text="20 Articulos" Value="10"></asp:ListItem>
                        <asp:ListItem Text="50 Articulos" Value="50"></asp:ListItem>
                        <asp:ListItem Text="100 Articulos" Value="100"></asp:ListItem>
                        <asp:ListItem Text="200 Articulos" Value="200"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-xs-12 col-sm-4 col-md-4 col-lg-4">

                    <div>Documento de venta:</div>
                    <asp:DropDownList ID="DropDownList3" runat="server">
                        <asp:ListItem Text="Oferta de ventas" Value="oferta"></asp:ListItem>
                        <asp:ListItem Text="Pedido de cliente" Value="pedido"></asp:ListItem>
                    </asp:DropDownList>
                </div>

            </div>

             <div class="row  row-centered ">
                  <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                <div>Usuario Mensajes:</div>
                <input runat="server" id="txtUsuMensajes" type="text" />
            </div>

                      <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                <div>Usuario Alertas:</div>
                <input runat="server" id="txtUsuAlertas" type="text" />
            </div>

             </div>

            <div class="row  row-centered ">

                <h3>Apariencia <small>   Color y logo</small></h3>
                <div class="col-xs-12 col-sm-6 col-md-6 col-lg-6">
                    <div>Barra Top:</div>
                    <asp:TextBox ID="Color0" runat="server" Text ="#ff0000"></asp:TextBox>
                    <%--<input id="" runat="server" type="color" name="favcolor" value="#ff0000" />--%>
                </div>
                <div class="col-xs-12 col-sm-6 col-md-6 col-lg-6">
                    <div>Info Usuario:</div>
                   <%-- <input id="Color1" runat="server" type="color" name="favcolor" value="#ff0000" />--%>
                    <asp:TextBox ID="Color1" runat="server" Text ="#ff0000"></asp:TextBox>
                </div>

            </div>

            <div class="row  row-centered ">
                 <div class="col-xs-12 col-sm-6 col-md-6 col-lg-6">
                    <div>Menu lateral:</div>
                   <%-- <input id="Color2" runat="server" type="color" name="favcolor" value="#ff0000" />--%>
                     <asp:TextBox ID="Color2" runat="server" Text ="#ff0000"></asp:TextBox>
                </div>
                 <div class="col-xs-12 col-sm-6 col-md-6 col-lg-6">
                    <div>Contenido:</div>
                    <%--<input id="Color3" runat="server" type="color" name="favcolor" value="#ff0000" />--%>
                     <asp:TextBox ID="Color3" runat="server" Text ="#ff0000"></asp:TextBox>
                </div>


            </div>




            <br />

            <div class="row  row-centered ">
                <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                    Logo pagina:<br />

                    <input type="hidden" name="MAX_FILE_SIZE" value="4194304" style="display: block;" />

                    <div class="fileUpload btn btn-primary">
                        <span>Examinar</span>
                        <input id="File1" runat="server" type="file" class="upload" accept="image/*" />
                        <%--<input type="file" class="upload" />--%>


                      
                    </div>

                    <br />
                    <br />


                </div>
            </div>
        </div>
       
       
        
        <div class="row  row-centered ">
             <h3> Descarga Documento electronico<small>   PDF y XML</small></h3>
            <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                <div>Ruta XML:</div>
                <input runat="server" id="inputxml" type="text" />
            </div>
        </div>
        <div class="row  row-centered ">
            <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                <div>Ruta PDF:</div>
                <input runat="server" id="inputpdf" type="text" />
            </div>
             



        </div>

        <%--EMAIL CONFIG--%>

          <div class="row  row-centered ">
              
              <h3> Configuracion EMAIL <small>   SMTP</small></h3><br />
              <asp:Button  Class="btn btn-info btn-md" ID="PruebaEnvioMail" runat="server" Text="Prueba Envio" />
              <br />
              <div id="Errormail" runat="server" ></div>
                <div class="col-xs-12 col-sm-6 col-md-6 col-lg-6">
                    <div>SMTP</div>
                    <input runat="server" id="inputSMTP" type="text" />
                </div>
               <div class="col-xs-12 col-sm-6 col-md-6 col-lg-6">
                    <div>PUERTO</div>
                    <input runat="server" id="inputPUERTO" type="text" />
                </div>
                
            </div>
           <div class="row  row-centered ">
                <div class="col-xs-12 col-sm-6 col-md-6 col-lg-6">
                    <div>Email Username</div>
                    <input runat="server" id="inputUsername" type="text" />
                </div>
                <div class="col-xs-12 col-sm-6 col-md-6 col-lg-6">
                    <div>Email Acount</div>
                    <input runat="server" id="inputAcount" type="text" />
                </div>
                
            </div>
           <div class="row  row-centered ">
                <div class="col-xs-12 col-sm-6 col-md-6 col-lg-6">
                    <div>Email Password</div>
                    <input runat="server" id="inputPassword" type="text" />
                </div>
               <div class="col-xs-12 col-sm-6 col-md-6 col-lg-6">
                    <div>SSL</div>
                    <asp:DropDownList ID="DropDownList4" runat="server">
                        <asp:ListItem Text="SI" Value="SI"></asp:ListItem>
                        <asp:ListItem Text="NO" Value="NO"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                
            </div>
        <br />
        <div class="row  row-centered ">
            <div class="col-xs-12      ">
                <asp:Button type="button" runat="server" ID="confirmar" Text="Guardar configuracion" class="btn btn-success btn-md" OnClientClick="Confirm()" />
            </div>
        </div>
        <br />
        <br />
        <br />
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

