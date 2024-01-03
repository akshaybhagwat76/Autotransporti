<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Dashboard.aspx.cs" Inherits="Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="chpHeader" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpContent" runat="Server">
    <div class="container-fluid">
        <div class="row">
            <div class="col-6">
                <div class="card">
                    <div class="card-body text-center">
                        <h4 class="font-weight-bolder">Viaggi registrati</h4>
                        <span class="font-weight-bolder custom-red-color" style="font-size: 30px" runat="server" id="spTripCounter">0</span><br />

                    </div>
                    
                </div>
            </div>
            <div class="col-6">
                <div class="card">
                    <div class="card-body text-center">
                        <h4 class="font-weight-bolder">Consumo medio globale</h4>
                        <span class="font-weight-bolder custom-red-color" style="font-size: 30px" runat="server" id="spTotalAverage">0</span><br />
                        <span>lt/100km</span>

                    </div>
                </div>
            </div>

        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="chpScripts" runat="Server">
</asp:Content>

