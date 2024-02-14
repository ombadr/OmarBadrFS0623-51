using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Esercizio
{
    public partial class HomePage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {

            CaricaSale();
            CaricaBigliettiVenduti();
            }
        }

        private void CaricaSale()
        {
            string connString = ConfigurationManager.ConnectionStrings["ConnessioneDBLocale"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT IDSala, NomeSala FROM Sale", conn))
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    ddlSala.DataSource = reader;
                    ddlSala.DataTextField = "NomeSala";
                    ddlSala.DataValueField = "IDSala";
                    ddlSala.DataBind();

                }            
            }
        }

        private void CaricaBigliettiVenduti()
        {
            string connString = ConfigurationManager.ConnectionStrings["ConnessioneDBLocale"].ConnectionString;
            StringBuilder statisticheHtml = new StringBuilder();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                // Query per conteggio totale biglietti per sala
                string querySale = "SELECT S.NomeSala, COUNT(B.IDBiglietto) AS TotaleBiglietti FROM Sale S LEFT JOIN Biglietti B ON S.IDSala = B.IDSala GROUP BY S.NomeSala";
                using (SqlCommand cmdSale = new SqlCommand(querySale, conn))
                {
                    using (SqlDataReader readerSale = cmdSale.ExecuteReader())
                    {
                        while (readerSale.Read())
                        {
                            statisticheHtml.Append($"<li>Numero di biglietti venduti {readerSale["NomeSala"]}: {readerSale["TotaleBiglietti"]}</li>");
                        }
                    }
                }

                // Query per conteggio biglietti ridotti
                string queryRidotti = "SELECT COUNT(*) AS TotaleRidotti FROM Biglietti WHERE TipoBiglietto = 'Ridotto'";
                using (SqlCommand cmdRidotti = new SqlCommand(queryRidotti, conn))
                {
                    int totaleRidotti = (int)cmdRidotti.ExecuteScalar();
                    statisticheHtml.Append($"<li>Numero di biglietti RIDOTTI venduti: {totaleRidotti}</li>");
                }

                // Assumi di avere un controllo Literal con ID litStatistiche per visualizzare le statistiche
                listStatistiche.Text = statisticheHtml.ToString();
            }
        }


        protected void btnSubmit_Click(object sender, EventArgs e)
        {

            string nome = txtNome.Text.Trim();
            string cognome = txtCognome.Text.Trim();
            int idSala = Convert.ToInt32(ddlSala.SelectedValue);
            string tipoBiglietto = ddlTipoBiglietto.SelectedValue;
            int capienzaMax = 120; // Capienza massima sala

            string connString = ConfigurationManager.ConnectionStrings["ConnessioneDBLocale"].ConnectionString;

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    // controllo numero attuale biglietti venduto per sala selezionata

                    string queryConteggio = "SELECT COUNT(*) FROM Biglietti WHERE IDSala = @IDSala";

                    SqlCommand cmdConteggio = new SqlCommand (queryConteggio, conn);
                    cmdConteggio.Parameters.AddWithValue("@IDSala", idSala);
                    int bigliettiVenduti = (int)cmdConteggio.ExecuteScalar();

                    if(bigliettiVenduti >= capienzaMax)
                    {
                        Response.Write("La sala ha raggiunto la capienza massima. Non è possibile vendere altri biglietti");
                        return;
                    }

                    string query = "INSERT INTO Biglietti (Nome, Cognome, IDSala, TipoBiglietto ) VALUES (@Nome, @Cognome, @IDSala, @TipoBiglietto)";

                    using (SqlCommand cmd = new SqlCommand(query, conn) )
                    {
                        cmd.Parameters.AddWithValue("@Nome", nome);
                        cmd.Parameters.AddWithValue("@Cognome", cognome);
                        cmd.Parameters.AddWithValue("@IDSala", idSala);
                        cmd.Parameters.AddWithValue("@TipoBiglietto", tipoBiglietto);

                        cmd.ExecuteNonQuery();
                    }
                }

                Response.Redirect(Request.RawUrl); // reload the page

            } catch (Exception ex) {

                Response.Write($"Si è verificato un errore: {ex.Message}");
            }


            CaricaBigliettiVenduti();
        }
    }
}