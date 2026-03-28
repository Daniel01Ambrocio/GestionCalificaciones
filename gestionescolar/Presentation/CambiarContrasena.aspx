<%@ Page Title="Cambiar contraseña" Language="C#" MasterPageFile="~/Presentation/menu.Master" AutoEventWireup="true" CodeBehind="CambiarContrasena.aspx.cs" Inherits="gestionescolar.Presentation.CambiarContrasena" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container py-4">

        <div class="row">
            <div class="col-12 col-md-9 col-lg-5">

                <div class="card shadow-sm border-0 rounded-4">
                    <div class="card-header bg-dark text-white text-center rounded-top-4">
                        <h4 class="mb-0">Cambiar Contraseña</h4>
                    </div>

                    <div class="card-body p-4">

                        <!-- Contraseña anterior -->
                        <div class="input-group">
                            <span class="input-group-text"><i class="bi bi-lock-fill"></i></span>
                            <asp:TextBox ID="txtAnterior" runat="server" CssClass="form-control"
                                TextMode="Password" placeholder="Ingresa tu contraseña actual"></asp:TextBox>

                            <span class="input-group-text" onclick="togglePassword('txtAnterior', this)" style="cursor: pointer;">
                                <i class="bi bi-eye"></i>
                            </span>
                        </div>

                        <!-- Nueva contraseña -->
                        <div class="input-group">
                            <span class="input-group-text"><i class="bi bi-shield-lock-fill"></i></span>
                            <asp:TextBox ID="txtNueva" runat="server" CssClass="form-control"
                                TextMode="Password" placeholder="Ingresa una nueva contraseña"></asp:TextBox>

                            <span class="input-group-text" onclick="togglePassword('txtNueva', this)" style="cursor: pointer;">
                                <i class="bi bi-eye"></i>
                            </span>
                        </div>

                        <!-- Confirmar contraseña -->
                        <div class="input-group">
                            <span class="input-group-text"><i class="bi bi-check2-circle"></i></span>
                            <asp:TextBox ID="txtConfirmar" runat="server" CssClass="form-control"
                                TextMode="Password" placeholder="Repite la nueva contraseña"></asp:TextBox>

                            <span class="input-group-text" onclick="togglePassword('txtConfirmar', this)" style="cursor: pointer;">
                                <i class="bi bi-eye"></i>
                            </span>
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
    <script>
        function togglePassword(txtId, el) {
            var txt = document.getElementById('<%= txtAnterior.ClientID %>'.replace('txtAnterior', txtId));
            var icon = el.querySelector("i");

            if (txt.type === "password") {
                txt.type = "text";
                icon.classList.remove("bi-eye");
                icon.classList.add("bi-eye-slash");
            } else {
                txt.type = "password";
                icon.classList.remove("bi-eye-slash");
                icon.classList.add("bi-eye");
            }
        }
    </script>
</asp:Content>
