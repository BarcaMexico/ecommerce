<%@ Page Title="" Language="VB" MasterPageFile="~/master_blanco.master" AutoEventWireup="false" CodeFile="frmLogin.aspx.vb" Inherits="frmLogin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="blanco" Runat="Server">
    <link href="Content/loginestilo.css" rel="stylesheet" />
    <link href="Content/menufix.css" rel="stylesheet" />
 
  <div class="row">
    <div class="Absolute-Center is-Responsive">

        <style runat="server" id="estilo"></style>
         
        <div class="col-sm-12 col-md-10 col-md-offset-1">
        
          <div class="form-group input-group" >
            <span class="input-group-addon"><i class="glyphicon glyphicon-user"></i></span>
            <input class="form-control" runat="server"    id="usu" type="text" name='username' placeholder="username"/>          
          </div>
          <div class="form-group input-group">
            <span class="input-group-addon"><i class="glyphicon glyphicon-lock"></i></span>
            <input class="form-control" runat="server" id="pass" type="password" name='password' placeholder="password"/>     
          </div>
         <%-- <div class="checkbox">
            <label>
              <input type="checkbox"/>Recordar usuario.
            </label>
          </div>--%>
          <div class=" ">
            <asp:Button ID="Button1" runat="server" Text="Entrar" class="btn btn-def btn-block btn-success "/>      
          </div>
         
        <%--  <div class="form-group text-center">
            <a href="#">¿No puedes acceder a tu cuenta? </a>  
          </div>--%>

          <div class="form-group text-center">
            <div runat="server" id="Alertamail" style ="display:none " class="alert alert-danger" role="alert"><b>Usuario o contraseña incorrectos.</b></div> 
          </div>   
      </div>  
         
    </div>  
       
            
  </div>
 





</asp:Content>

