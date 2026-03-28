<%@ Page Title="" Language="C#" MasterPageFile="~/Presentation/Principal.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="gestionescolar.Presentation.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container d-flex justify-content-center align-items-center min-vh-100">
        <!-- Responsive: w-100 (ancho completo), mx-3 (margen horizontal en celular), y max-w para desktop -->
        <div class="card shadow p-4 w-100 mx-3" style="max-width: 400px;">
            <h3 class="text-center mb-4 text-primary">Gestión Escolar</h3>

            <div class="mb-3">
                <label for="txtUsuario" class="form-label">Usuario</label>
                <asp:TextBox ID="txtUsuario" runat="server" CssClass="form-control" placeholder="Ingresa tu usuario" />
            </div>

            <div class="mb-3">
                <label for="txtPassword" class="form-label">Contraseña</label>
                <div class="input-group">
                    <span class="input-group-text"><i class="bi bi-lock-fill"></i></span>
                    <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Contraseña" />

                    <span class="input-group-text" onclick="togglePassword('txtPassword', this)" style="cursor: pointer;">
                        <i class="bi bi-eye"></i>
                    </span>
                </div>
            </div>


            <div class="d-grid">
                <asp:Button ID="btnLogin" runat="server" Text="Iniciar Sesión"
                    CssClass="btn btn-white-bg text-border-black"
                    OnClick="btnLogin_Click" />
            </div>

        </div>
    </div>
    <script>
        function togglePassword(txtId, el) {
            var txt = document.getElementById('<%= txtPassword.ClientID %>'.replace('txtPassword', txtId));
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
