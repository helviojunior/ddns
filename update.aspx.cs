using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Net;
using SafeTrend.DNS;
using SafeTrend.DNS.Client;


namespace DDNSWeb
{
    public partial class update : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            //Permite somente o método POST
            if ((Request.HttpMethod != "GET") || (RouteData.Values["auth_key"] == null) || (RouteData.Values["host_id"] == null) || (Request.Params["REMOTE_ADDR"] == null))
            {
                Response.Status = "412 Precondition Failed";
                Response.StatusCode = 412;
                Response.End();
                return;
            }

            String auth_key = RouteData.Values["auth_key"].ToString();
            String host_id = RouteData.Values["host_id"].ToString();
            String remote_addr = Request.Params["REMOTE_ADDR"];
            String host_name = null;

            //Verifica chave de autenticação
            if (auth_key != "authtest123")
            {
                Response.Status = "403 Access denied";
                Response.StatusCode = 403;
                Response.End();
                return;
            }

            //Cria uma tabela com a listagem de hosts/IDs
            //Essa tabela pode ser pega de uma banco de dados
            Dictionary<String, String> hosts = new Dictionary<string, string>();
            hosts.Add("aae5cd33-5b51-49af-8b10-6e88d5af92a8", "filial1");
            hosts.Add("759eacaa-f2d9-4324-86c4-b599a709890a", "filial2");

            foreach (String key in hosts.Keys)
                if (key == host_id)
                    host_name = hosts[key];

            if (host_name == null)
            {
                Response.Status = "403 Access denied";
                Response.StatusCode = 403;
                Response.End();
                return;
            }

            //Define o nome da zona DNS
            String dnsZone = "teste.com.br";
            IPAddress server = IPAddress.Parse("192.168.254.200");
            
            try
            {

                DNSUpdateMessage message = DNSClient.UpdateRecord(server, dnsZone, host_name, DNSType.A, DNSClass.IN, remote_addr);

                if (message.header.RCODE != DNSReturnCode.Success)
                    throw new Exception("");

                Byte[] bRet = Encoding.UTF8.GetBytes("OK");

                Response.Status = "200 OK";
                Response.StatusCode = 200;
                Response.OutputStream.Write(bRet, 0, bRet.Length);
                Response.OutputStream.Flush();

            }
            catch (Exception ex)
            {

                Byte[] bRet = Encoding.UTF8.GetBytes(ex.Message);

                Response.Status = "500 Internal Server Error";
                Response.StatusCode = 500;
                //Response.OutputStream.Write(bRet, 0, bRet.Length);
                //Response.OutputStream.Flush();
                Response.End();
                return;
            }

        }
    }
}
