<%@ Page Title="" Language="C#" MasterPageFile="~/Presentation/menu.Master" AutoEventWireup="true" CodeBehind="FinInicioPeriodo.aspx.cs" Inherits="gestionescolar.Presentation.FinInicioPeriodo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-5">
        <div class="col-12 col-md-11 col-lg-11">
            <!-- Encabezado -->
            <div class="section-header">
                <h3>Terminar/comenzar periodo escolar</h3>
            </div>

            <!-- Contenedor principal del formulario  -->
            <div id="contform">

                <!-- Lista de grupos -->
                <div class="col-md-8">
                    <div class="card">
                        <div class="card-header bg-primary text-white">
                            Lista de grupos del periodo escolar actual
                        </div>
                        <div class="card-body p-0">
                            <asp:GridView runat="server" ID="gvGrupos" CssClass="table table-striped table-bordered mb-0"
                                AutoGenerateColumns="False" GridLines="None" DataKeyNames="IDGrupo">
                                <Columns>
                                    <asp:BoundField DataField="grado" HeaderText="Grado" />
                                    <asp:BoundField DataField="grupo" HeaderText="Grupo" />
                                    <asp:BoundField DataField="anio" HeaderText="Año" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>

                <!-- Formulario -->
                <div class="col-md-4 mt-5">
                    <asp:Button 
                        runat="server" 
                        Text="Finalizar y Crear Nuevo Periodo" 
                        CssClass="btn btn-success btn-block" 
                        ID="btnTerminarComenzar" 
                        OnClientClick="return confirm('Esta acción terminará el periodo actual y generará automáticamente un nuevo periodo. Solo los alumnos aprobados serán asignados al nuevo año que les corresponde. ¿Deseas continuar?');" OnClick="btnTerminarComenzar_Click" />
                </div>
                <!-- fin row -->
            </div>
        </div>

        <!-- fin contform -->
    </div>
</asp:Content>
