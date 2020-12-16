<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="frmInicio.aspx.vb" Inherits="frmInicio" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

   
            <div id="page-wrapper">
                <div class="row">
                     
  
    
                    <link href="Content/tablanocolor.css" rel="stylesheet" />
      
  

    <div   style="width :auto; overflow-x: auto;  ">


         <div class="titulolink" >
             <br />
            <a  href ="frmInicio.aspx"><i class="fa fa-home fa-fw"></i></a>>
            <a    href ="#">Saldos pendientes</a>
            <br />
            <br />
            </div>

<div style="width: auto; overflow-x: auto;"> 
                    <div style="min-width: 600px;"> 

                        <asp:Table ID="Table1" class="table abc" runat="server" Style="width: 100%; min-width: 600px">
   <asp:TableHeaderRow >
       
        
       
   </asp:TableHeaderRow>

    </asp:Table>

     </div></div>



     </div>


           
                </div>

            </div>





    </asp:Content>

