<%@ Page Title="" Language="C#" MasterPageFile="~/Presentation/menu.Master" AutoEventWireup="true" CodeBehind="SolicitudBaja.aspx.cs" Inherits="gestionescolar.Presentation.SolicitudBaja" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-5">
        <div class="row">
            <div class="col-12 col-md-8 col-lg-7">

                <!-- Encabezado -->
                <div class="text-center mb-4">
                    <h3 class="fw-bold">Solicitar baja de usuario</h3>
                    <p class="text-muted">Complete la información para procesar la solicitud</p>
                </div>

                <!-- Rol -->
                <div class="mb-3 row">
                    <label class="form-label fw-semibold">Rol del usuario</label>
                    <asp:DropDownList
                        ID="ddlRol"
                        runat="server"
                        AutoPostBack="true"
                        OnSelectedIndexChanged="ddlRol_SelectedIndexChanged">
                    </asp:DropDownList>
                </div>


                <!-- Usuario -->
                <div class="mb-3">
                    <label class="form-label fw-semibold">Usuario</label>
                    <asp:DropDownList ID="ddlUsuario" runat="server" CssClass="form-select">
                        <asp:ListItem Text="Seleccione un usuario" Value="" />
                    </asp:DropDownList>

                </div>

                <!-- Motivo -->
                <div class="mb-3">
                    <label class="form-label fw-semibold">Motivo de la baja</label>
                    <asp:TextBox ID="txtMotivo" runat="server" CssClass="form-control"
                        TextMode="MultiLine" Rows="4"
                        placeholder="Describa la razón de la baja..."></asp:TextBox>
                </div>

                <!-- Botones -->
                <div class="d-flex justify-content-between mt-4">
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar"
                        CssClass="btn btn-outline-secondary" OnClick="btnCancelar_Click" />

                    <asp:Button ID="btnEnviar" runat="server" Text="Enviar solicitud"
                        CssClass="btn btn-danger" />
                </div>


            </div>
        </div>
    </div>
</asp:Content>
