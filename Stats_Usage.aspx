<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Stats_Usage.aspx.cs" Inherits="admin_Stats_Usage" ClientIDMode="AutoID" %>

<asp:Content ID="Content1" ContentPlaceHolderID="chpHeader" runat="Server">
    <link href="<%= ResolveUrl(string.Format("~/assets/libs/datatables.net-bs4/css/dataTables.bootstrap4.min.css")) %>" rel="stylesheet" />
    <link href="assets/libs/pickadate/lib/themes/default.css" rel="stylesheet" />
    <link href="assets/libs/pickadate/lib/themes/default.date.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpContent" runat="Server">
    <div class="container-fluid">
        <asp:UpdateProgress runat="server" ID="updProgWrap" AssociatedUpdatePanelID="updContent" DisplayAfter="500">
            <ProgressTemplate>
                <div class="text-center" style="width: 100%; height: 100%; position: absolute; background-color: #343a406b; z-index: 99999; top: 0px; left: 0px">
                    <div class="p-5 text-center" style="background: #fff; width: 50%; left: 25%; margin-top: 20%; border-radius: 10px; position: absolute">
                        <h3>Attendere...</h3>
                        <span class="fa fa-4x fa-cog fa-spin"></span>
                    </div>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>



        <asp:UpdatePanel runat="server" ID="updContent">
            <ContentTemplate>
                <div class="page-breadcrumb p-0 mb-3">
                    <div class="row">
                        <div class="col-10 align-self-center">
                            <h3 class="page-title text-truncate text-dark font-weight-medium mb-1">Consumi</h3>
                        </div>

                    </div>
                </div>
                <div class="d-flex">
                    <div class="w-25" style="margin: 0 auto">
                        <div class="card">
                            <div class="card-body text-center">
                                <h4 class="font-weight-bolder">Consumo medio globale</h4>
                                <span class="font-weight-bolder custom-red-color" style="font-size: 30px" runat="server" id="spTotalAverage">0</span><br />
                                <span>lt/100km</span>

                            </div>
                            <hr />
                            <small class="p-2">Il dato fa riferimento a tutti i viaggi registrati nel sistema</small>
                        </div>
                    </div>
                </div>
                <div class="card">
                    <div class="card-body p-3">


                        <div class="row">
                            <div class="col-4">
                                <span class="font-weight-medium">Visualizza consumi medi in base al periodo selezionato</span>
                                <div class="d-flex mb-3">
                                    
                                    <div class="ml-2">
                                        <input type="text" runat="server" id="txtStart" class="form-control pickdate w-auto" style="width:120px !important" />
                                    </div>
                                    <div class="ml-2">
                                        <input type="text" runat="server" id="txtEnd" class="form-control pickdate w-auto" style="width:120px !important" />
                                    </div>
                                    <div class="ml-2">
                                        <asp:LinkButton runat="server" ID="lnkApplyFilter" OnClick="lnkApplyFilter_Click" CssClass="btn btn-primary"><i class="fa fa-filter"></i> Applica</asp:LinkButton>
                                    </div>

                                </div>
                                <div class="card">
                                    <div class="card-body text-center">
                                        <h4 class="font-weight-bolder">Consumo medio</h4>
                                        <span class="font-weight-bolder custom-red-color" style="font-size: 30px" runat="server" id="spPeriodAverage">0</span><br />
                                        <span>lt/100km</span>

                                    </div>

                                </div>
                            </div>
                            <div class="col-8">
                                <div class="table-responsive">

                                    <table class="table table-bordered" id="tabledata">
                                        <thead>
                                            <tr>
                                                <th>Autista</th>
                                                <th class="text-right">Km percorsi</th>
                                                <th class="text-right">Consumo medio lt/100km</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:Repeater runat="server" ID="rpStats">
                                                <ItemTemplate>
                                                    <tr>
                                                        <td><%# Eval("driver")  %></td>
                                                        <td class="text-right"><%# Eval("km")  %></td>
                                                        <td class="text-right"><%# Eval("average")  %></td>
                                                        
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>



            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="chpScripts" runat="Server">
    <script src="assets/libs/datatables.net/js/jquery.dataTables.min.js"></script>
    <script src="assets/libs/pickadate/lib/picker.js"></script>
    <script src="assets/libs/pickadate/lib/picker.date.js"></script>
    <script src="assets/libs/pickadate/lib/translations/it_IT.js"></script>
    <script type="text/javascript">
        function pageLoad() {
            $(".pickdate").pickadate({
                format: "dd/mm/yyyy",
                formatSubmit: "dd/mm/yyyy",
                editable: false
            });
            $("#tabledata").DataTable({
                searching : false,
                pageLength: 25,
                stateSave: true,
                stateDuration: -1,
                responsive: true,
                language: {
                    url: 'assets/libs/datatables.net/lang/it.json'
                }
            });
        }
    </script>
</asp:Content>

