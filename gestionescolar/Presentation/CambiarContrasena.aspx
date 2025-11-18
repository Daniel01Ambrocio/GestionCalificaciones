<%@ Page Title="Cambiar contraseña" Language="C#" MasterPageFile="~/Presentation/menu.Master" AutoEventWireup="true" CodeBehind="CambiarContrasena.aspx.cs" Inherits="gestionescolar.Presentation.CambiarContrasena" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container py-4">

        <div class="row">
            <div class="col-12 col-md-7 col-lg-5">

                <div class="card shadow-sm border-0 rounded-4">
                    <div class="card-header bg-dark text-white text-center rounded-top-4">
                        <h4 class="mb-0">Cambiar Contraseña</h4>
                    </div>

                    <div class="card-body p-4">

                        <!-- Contraseña anterior -->
                        <div class="mb-3">
                            <label class="form-label fw-semibold">Contraseña anterior</label>
                            <div class="input-group">
                                <span class="input-group-text"><i class="bi bi-lock-fill"></i></span>
                                <asp:TextBox ID="txtAnterior" runat="server" CssClass="form-control" TextMode="Password" placeholder="Ingresa tu contraseña actual" required></asp:TextBox>
                            </div>
                        </div>

                        <!-- Nueva contraseña -->
                        <div class="mb-3">
                            <label class="form-label fw-semibold">Nueva contraseña</label>
                            <div class="input-group">
                                <span class="input-group-text"><i class="bi bi-shield-lock-fill"></i></span>
                                <asp:TextBox ID="txtNueva" runat="server" CssClass="form-control" TextMode="Password" placeholder="Ingresa una nueva contraseña" required></asp:TextBox>
                            </div>
                        </div>

                        <!-- Confirmar contraseña -->
                        <div class="mb-3">
                            <label class="form-label fw-semibold">Confirmar contraseña</label>
                            <div class="input-group">
                                <span class="input-group-text"><i class="bi bi-check2-circle"></i></span>
                                <asp:TextBox ID="txtConfirmar" runat="server" CssClass="form-control" TextMode="Password" placeholder="Repite la nueva contraseña" required></asp:TextBox>
                            </div>
                        </div>

                        <!-- Botón guardar -->
                        <div class="d-grid">
                            <asp:Button ID="btnGuardar" runat="server" CssClass="btn btn-dark btn-lg" 
                                Text="Guardar cambios" OnClick="btnGuardar_Click" />
                        </div>

                    </div>
                </div>

            </div>
        </div>

    </div>

</asp:Content>
