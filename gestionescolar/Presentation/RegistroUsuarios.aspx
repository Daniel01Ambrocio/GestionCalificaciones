<%@ Page Title="" Language="C#" MasterPageFile="~/Presentation/menu.Master" AutoEventWireup="true" CodeBehind="RegistroUsuarios.aspx.cs" Inherits="gestionescolar.Presentation.RegistroUsuarios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-5">
        <div class="col-12 col-md-8 col-lg-7">
            <!-- Encabezado -->
            <div class="section-header">
                <h3>Registro de usuarios</h3>
            </div>

            <!-- Rol -->
            <div class="mb-3">
                <asp:Label ID="lbrol" runat="server" class="form-label" Text="Rol"></asp:Label>
                <asp:DropDownList ID="ddlRol" runat="server" CssClass="form-select  textbox-delgado" AutoPostBack="false" onchange="toggleCampos()">
                </asp:DropDownList>
                <asp:Button ID="btnRol" runat="server" Text="Seleccionar" CssClass="btn btn-primary" OnClick="btnRol_Click" />
                <asp:Button ID="btnAtras" runat="server" Text="Atras" CssClass="btn btn-danger" OnClick="btnAtras_Click" />
            </div>

            <!-- Nombre -->
            <div class="mb-3" id="nombrediv" runat="server">
                <label class="form-label">Nombre</label>
                <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control textbox-delgado"></asp:TextBox>
            </div>

            <!-- Apellidos -->
            <div class="mb-3" id="apellidopdiv" runat="server">
                <label class="form-label">Apellido Paterno</label>
                <asp:TextBox ID="txtApellidoPaterno" runat="server" CssClass="form-control textbox-delgado"></asp:TextBox>
            </div>
            <div class="mb-3" id="apellidomdiv" runat="server">
                <label class="form-label">Apellido Materno</label>
                <asp:TextBox ID="txtApellidoMaterno" runat="server" CssClass="form-control textbox-delgado"></asp:TextBox>
            </div>

            <!-- Contraseña -->
            <div class="mb-3" id="contradiv" runat="server">
                <label class="form-label">Contraseña</label>
                <asp:TextBox ID="txtPasswor" runat="server"
                    CssClass="form-control textbox-delgado"
                    TextMode="Password"
                    onfocus="mostrarInfo()"
                    onblur="ocultarInfo()">
                </asp:TextBox>
            </div>

            <div class="mb-3 password-info" id="contrainfodiv" runat="server">
                <small class="text-muted">La contraseña debe cumplir con:
                </small>
                <ul class="text-muted">
                    <li>Mínimo 7 caracteres</li>
                    <li>Una mayúscula (A–Z)</li>
                    <li>Una minúscula (a–z)</li>
                    <li>Un número (0–9)</li>
                    <li>Un símbolo (_ @ &)</li>
                </ul>
            </div>
            <!-- Validar Contraseña -->
            <div class="mb-3" id="contravalidiv" runat="server">
                <label class="form-label">Validar contraseña</label>
                <asp:TextBox ID="txtvalicontra" runat="server" CssClass="form-control textbox-delgado" TextMode="Password"></asp:TextBox>
            </div>

            <!-- Periodos -->
            <div class="mb-3" id="periodoinidiv" runat="server">
                <label class="form-label">Periodo de Ingreso</label>
                <asp:TextBox ID="txtPeriodoIngreso" runat="server" CssClass="form-control textbox-delgado" TextMode="Date"></asp:TextBox>
            </div>
            <div class="mb-3" id="periodofindiv" runat="server">
                <label class="form-label">Periodo Fin</label>
                <asp:TextBox ID="txtPeriodoFin" runat="server" CssClass="form-control textbox-delgado" TextMode="Date"></asp:TextBox>
            </div>

            <!-- Status -->
            <div class="mb-3" id="statusdiv" runat="server">
                <label class="form-label">Estatus</label>
                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-select">
                </asp:DropDownList>
            </div>

            <!-- Grupo (solo Maestro y Alumno) -->
            <div class="mb-3" id="grupoDiv" runat="server">
                <label class="form-label">Grupo</label>
                <asp:DropDownList ID="ddlGrupo" runat="server" CssClass="form-select">
                </asp:DropDownList>
            </div>

            <!-- Cédula Profesional (solo Maestro) -->
            <div class="mb-3" id="cedulaDiv" runat="server">
                <label class="form-label">Cédula Profesional</label>
                <asp:TextBox ID="txtCedula" runat="server" CssClass="form-control textbox-delgado"></asp:TextBox>
            </div>

            <asp:Button ID="btnRegistrar" runat="server" Text="Registrar" CssClass="btn btn-primary" OnClick="btnRegistrar_Click" />
        </div>

    </div>
    <script>
        function mostrarInfo() {
            document.getElementById('<%= contrainfodiv.ClientID %>')
                .classList.add('active');
        }

        function ocultarInfo() {
            document.getElementById('<%= contrainfodiv.ClientID %>')
                .classList.remove('active');
        }
    </script>
</asp:Content>
