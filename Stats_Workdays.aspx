<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Stats_Workdays.aspx.cs" Inherits="admin_Stats_Workdays" ClientIDMode="AutoID" %>

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
                            <h3 class="page-title text-truncate text-dark font-weight-medium mb-1">Giornate lavorative</h3>
                        </div>

                    </div>
                </div>
                <div class="card">
                    <div class="card-body p-3">

                        <div class="d-flex">
                            <span class="font-weight-medium">Visualizza statistiche in base al periodo selezionato</span>
                            <div class="ml-2">
                                <input type="text" runat="server" id="txtStart" class="form-control pickdate w-auto" />
                            </div>
                            <div class="ml-2">
                                <input type="text" runat="server" id="txtEnd" class="form-control pickdate w-auto" />
                            </div>
                            <div class="ml-2">
                                <asp:LinkButton runat="server" ID="lnkApplyFilter" OnClick="lnkApplyFilter_Click" CssClass="btn btn-primary"><i class="fa fa-filter"></i> Applica</asp:LinkButton></div>

                        </div>
                    </div>
                </div>
                <div class="table-responsive">

                    <table class="table table-bordered" id="tabledata">
                        <thead>
                            <tr>
                                <th>Autista</th>
                                <th class="text-right">Partenze</th>
                                <th class="text-right">Feriali</th>
                                <th class="text-right">Festivi</th>
                                <th class="text-right">Festivi speciali</th>
                                <th class="text-right">Rientri</th>
                                <th class="text-right">Totale km</th>
                                <th></th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater runat="server" ID="rpStats">
                                <ItemTemplate>
                                    <tr>
                                        <td><%# Eval("driver")  %></td>
                                        <td class="text-right"><%# Eval("partenze")  %></td>
                                        <td class="text-right"><%# Eval("feriali")  %></td>
                                        <td class="text-right"><%# Eval("festive")  %></td>
                                        <td class="text-right"><%# Eval("festiveSpeciali")  %></td>
                                        <td class="text-right"><%# Eval("rientri")  %></td>
                                        <td class="text-right"><%# Eval("km")  %></td>
                                        <td class="text-center">
                                            <asp:LinkButton runat="server" ID="lnkDetail" CssClass="fa fa-1-5x fa-search" dname='<%# Eval("driver") %>' did='<%# Eval("driverId") %>' OnClick="lnkDetail_Click"></asp:LinkButton></td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div>

                <div id="modalDetail" class="modal" tabindex="-1" role="dialog" aria-hidden="true" style="display: none;">
                    <div class="modal-dialog modal-xl">
                        <div class="modal-content">
                            <div class="modal-header">
                                <h4>Dettaglio viaggi <strong class="custom-red-color font-weight-bolder">
                                    <asp:Literal runat="server" ID="litDetailDriver"></asp:Literal></strong></h4>
                                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                            </div>
                            <div class="modal-body p-2">
                                <div class="mb-2">
                                    <div class="card-body p-3">
                                        <asp:Repeater runat="server" ID="rpDriverDetails" OnItemDataBound="rpDriverDetails_ItemDataBound">
                                            <ItemTemplate>
                                                <div class="row border-bottom mb-3">
                                                    <div class="col-2">
                                                        <span class="font-weight-bold"><i class="fa fa-flag"></i> Partenza</span><br />
                                                        <strong class="custom-red-color"><%# ((Viaggio)Container.DataItem).Inizio.ToLocalTime().ToShortDateString()  %></strong>
                                                        <br />
                                                        <span class="font-weight-bold"><i class="fa fa-location-arrow"></i> Rientro</span><br />
                                                        <strong class="custom-red-color"><%# ((Viaggio)Container.DataItem).Fine.HasValue ? ((Viaggio)Container.DataItem).Fine.Value.ToLocalTime().ToShortDateString() : "" %></strong>
                                                    </div>
                                                    <div class="col-2">
                                                        <span class="font-weight-bold">Targa</span><br />
                                                        <span><%# ((Viaggio)Container.DataItem).Targa  %></span>
                                                        <div runat="server" visible="<%# !string.IsNullOrEmpty(((Viaggio)Container.DataItem).TargaRimorchio) %>" class="mt-1">
                                                            <span class="font-weight-bold">Targa rimorchio</span><br />
                                                            <span><%# ((Viaggio)Container.DataItem).TargaRimorchio  %></span>
                                                        </div>

                                                    </div>
                                                    <div class="col-4">
                                                        <div class="d-flex">
                                                            <div class="text-center">
                                                                <span class="font-weight-bold">GG. lavorativi</span><br />
                                                                <strong class="custom-red-color">
                                                                    <asp:Literal runat="server" ID="litWorkingDays" Text="0"></asp:Literal></strong>
                                                            </div>
                                                            <div class="text-center flex-grow-1">
                                                                <span class="font-weight-bold">Festivi</span><br />
                                                                <strong class="custom-red-color">
                                                                    <asp:Literal runat="server" ID="litWorkingFestivi" Text="0"></asp:Literal></strong>
                                                            </div>
                                                            
                                                        </div>
                                                        <div class="d-flex">
                                                            <div class="text-center">
                                                                <span class="font-weight-bold">Festivi speciali</span><br />
                                                                <strong class="custom-red-color">
                                                                    <asp:Literal runat="server" ID="litWorkingFestiviSpeciali" Text="0"></asp:Literal></strong>
                                                            </div>
                                                            <div class="text-center flex-grow-1">
                                                                <span class="font-weight-bold">Rientro</span><br />
                                                                <strong class="custom-red-color">
                                                                    <asp:Literal runat="server" ID="litWorkingRientro" Text="0"></asp:Literal></strong>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-4">
                                                        <h6 class="font-weight-medium">Rifornimenti</h6>
                                                        <asp:Repeater runat="server" ID="rpRefuel" DataSource="<%# ((Viaggio)Container.DataItem).Rifornimenti  %>">
                                                            <ItemTemplate>
                                                                <div class="d-flex mb-2">
                                                                    <small class="mr-2"><%# ((Rifornimento)Container.DataItem).Data.ToLocalTime().ToShortDateString()  %></small>
                                                                    <span class="mr-2 flex-grow-1 text-right custom-red-color"><%# ((Rifornimento)Container.DataItem).LtCarburante  %> <small>lt</small></span>
                                                                    <span class="mr-2 flex-grow-1 text-right"><small>€/lt</small> <%# ((Rifornimento)Container.DataItem).CostoCarburante.HasValue ? Math.Round(((Rifornimento)Container.DataItem).CostoCarburante.Value, 2).ToString() : "n.d."  %></span>
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                    </div>
                                                    <div class="col-12 bg-light-danger text-right p-2">
                                                        <span>Tariffa totale <strong class="custom-red-color font-weight-bolder"><asp:Literal runat="server" ID="litTotalFee" Text="0,00 €"></asp:Literal></strong></span>
                                                    </div>
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </div>
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

