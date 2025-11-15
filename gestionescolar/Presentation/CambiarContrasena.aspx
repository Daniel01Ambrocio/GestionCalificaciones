<%@ Page Title="Cambiar contraseña" Language="C#" MasterPageFile="~/Presentation/menu.Master" AutoEventWireup="true" CodeBehind="CambiarContrasena.aspx.cs" Inherits="gestionescolar.Presentation.CambiarContrasena" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Bootstrap (si no está ya en tu MasterPage) -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-5">
        <div class="row justify-content-center">
            <div class="col-12 col-md-8 col-lg-5">

                <div class="card shadow">
                    <div class="card-header text-center bg-primary text-white">
                        <h5 class="mb-0">Cambiar contraseña</h5>
                    </div>

                    <div class="card-body">

                        <div class="mb-3">
                            <label class="form-label">Contraseña anterior</label>
                            <asp:TextBox ID="txtAnterior" runat="server" CssClass="form-control" TextMode="Password" required></asp:TextBox>
                        </div>

                        <div class="mb-3">
                            <label class="form-label">Nueva contraseña</label>
                            <asp:TextBox ID="txtNueva" runat="server" CssClass="form-control" TextMode="Password" required></asp:TextBox>
                        </div>

                        <div class="mb-3">
                            <label class="form-label">Confirmar contraseña</label>
                            <asp:TextBox ID="txtConfirmar" runat="server" CssClass="form-control" TextMode="Password" required></asp:TextBox>
                        </div>

                        <div class="d-grid">
                            <asp:Button ID="btnGuardar" runat="server" CssClass="btn btn-primary" 
                                Text="Guardar cambios" OnClick="btnGuardar_Click" />
                        </div>

                        <asp:Label ID="lblMensaje" runat="server" CssClass="mt-3 d-block text-center"></asp:Label>

                    </div>
                </div>

            </div>
        </div>
    </div>
</asp:Content>
