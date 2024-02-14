<%@ Page Title="" Language="C#" MasterPageFile="~/Template.Master" AutoEventWireup="true" CodeBehind="HomePage.aspx.cs" Inherits="Esercizio.HomePage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-5">
        <div class="row">
            <div class="col">
            <h2 class="text-start">Vendita biglietti</h2>             
                    <div class="mb-3">
                        <label for="txtNome" class="form-label">Nome</label>
                        <asp:TextBox ID="txtNome" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                    <div class="mb-3">
                        <label for="txtCognome" class="form-label">Cognome</label>
                        <asp:TextBox ID="txtCognome" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                    <div class="mb-3">
                        <label for="ddlSala" class="form-label">Sala</label>
                        <asp:DropDownList ID="ddlSala" CssClass="form-select" runat="server">
                        </asp:DropDownList>
                    </div>
                    <div class="mb-3">
                        <label for="ddlTipoBiglietto" class="form-label">Tipo Biglietto</label>
                        <asp:DropDownList ID="ddlTipoBiglietto" CssClass="form-select" runat="server">
                            <asp:ListItem Text="Intero" Value="Intero"></asp:ListItem>
                            <asp:ListItem Text="Ridotto" Value="Ridotto"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <asp:Button ID="btnSubmit" CssClass="btn btn-primary" runat="server" Text="Compra Biglietto" OnClick="btnSubmit_Click" />
            </div>
            <div class="col ">

                <h2 class="text-start">Statistiche</h2>
                <asp:Literal ID="listStatistiche" runat="server" />
               
            </div>
        </div>
    </div>
</asp:Content>
