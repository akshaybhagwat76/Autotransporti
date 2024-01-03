using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utility;
using Newtonsoft.Json;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Operatore loggedUser = PageUtility.getLoggedUser();
            if (loggedUser != null)
                Response.Redirect("~/Dashboard.aspx");

            if (!IsPostBack)
            {
                //divCaptcha.Attributes["data-sitekey"] = Settings.RecaptchaSiteKey;
            }
        }catch(Exception ex)
        {
            BootstrapUI.showNotification(this, "Attenzione", ex.Message, "error", "toast-top-center", true);
        }
    }

    protected void lnkLogin_Click(object sender, EventArgs e)
    {
        try
        {

            if (string.IsNullOrEmpty(txtUser.Value) || string.IsNullOrEmpty(txtPassword.Value))
                return;

            Operatore user = Operatore.login(txtUser.Value, txtPassword.Value);
            if (user != null)
            {
                Session[Settings.K_LOGGED_USER] = user;
                Response.Redirect("~/Dashboard.aspx");
            }
            else
                throw new Exception("Username e/o password non validi!");

        }
        catch (System.Threading.ThreadAbortException ex) { }
        catch (Exception ex)
        {
            BootstrapUI.showNotification(this, "Attenzione", ex.Message, "error", "toast-top-center", true);
        }
        finally
        {
            register_script_recaptcha();
        }
    }
    protected void register_script_recaptcha()
    {

        //ScriptManager.RegisterClientScriptBlock(
        //    divCaptcha,
        //    divCaptcha.GetType(),
        //    "recaptcha",
        //    "grecaptcha.render(" + divCaptcha.ClientID + ", {'sitekey': '" + Settings.RecaptchaSiteKey + "'});",
        //    true
        //    );
    }
}