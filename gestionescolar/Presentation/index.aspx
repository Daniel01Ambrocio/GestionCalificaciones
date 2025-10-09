<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="gestionescolar.Presentation.index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <title>Inicio de Sesión - Gestión Escolar</title>
    <link href="~/Styles/buttons.css" rel="stylesheet" type="text/css" />
    <!-- Bootstrap 5 CDN -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>

    <!-- Colores personalizados usando clases inline (no se usa CSS externo) -->
</head>

<body>
    <form id="form1" runat="server">
        <div class="container d-flex justify-content-center align-items-center vh-100">
            <div class="card shadow p-4" style="width: 100%; max-width: 400px;">
                <h3 class="text-center mb-4" style="color: #007BFF;">Gestión Escolar</h3>

                <div class="mb-3">
                    <label for="txtUsuario" class="form-label">Usuario</label>
                    <asp:TextBox ID="txtUsuario" runat="server" CssClass="form-control" placeholder="Ingresa tu usuario" />
                </div>

                <div class="mb-3">
                    <label for="txtPassword" class="form-label">Contraseña</label>
                    <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Contraseña" />
                </div>

                <div class="d-grid">
                    <asp:Button ID="btnLogin" runat="server" Text="Iniciar Sesión"
                        CssClass="btn btn-white-bg text-border-black"
                        OnClick="btnLogin_Click" />

                </div>
            </div>
        </div>
    </form>
</body>
</html>
