using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Digiexpress.ADO;
using Digiexpress.Mail;
using Sindiveg.API.Models;

namespace Sindiveg.API.BLL
{
    public class EmailBLL
    {
        private DbServer dc;


        public EmailBLL(DbServer dc)
        {
            this.dc = dc;
        }

        public void EnviarEmail(string assunto, List<String> destinatarios, List<AnexoEmail> anexos)
        {
            var config = new ConfiguracoesBLL(dc).Lista().FirstOrDefault();
            var mail = new eMail(config.Email, "Sindiveg");

            if (destinatarios.Count() > 0)
            {
                foreach (var anexo in anexos)
                    mail.AdicionarAnexo(anexo.arquivoByteArray, anexo.nome, MediaTypeNames.Application.Octet);

                foreach (var destinatario in destinatarios)
                    mail.AdicionarDestinatario(destinatario);

                mail.Login = config.Email;
                mail.Senha = Base64ToString(config.Senha);
                mail.ServidorSMTP = config.ServidorSmtp;
                mail.Assunto = assunto;
                mail.Porta = config.Porta;
                mail.SSL = config.Ssl;
                mail.Mensagem = config.MensagemEmail;
                mail.Enviar();
            }
        }

        private string Base64ToString(string base64)
        {
            byte[] data = Convert.FromBase64String(base64);
            return Encoding.UTF8.GetString(data);
        }
    }
}
