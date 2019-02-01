using Digiexpress.ADO;
using Digiexpress.Mail;
using Sindiveg.API.Banco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Sindiveg.API.BLL
{
    public class EncaminhamentoBLL : IBLL
    {
        private DbServer dc;

        public EncaminhamentoBLL(DbServer dc)
        {
            this.dc = dc;
        }

        public IEnumerable<Encaminhamento> Lista()
        {
            return dc.Lista<Encaminhamento>();
        }

        public Encaminhamento Selecionar(int id)
        {
            var CategoriaProduto = dc.Selecionar<Encaminhamento>(id);
            return CategoriaProduto;
        }

        public Encaminhamento Incluir(Encaminhamento Encaminhamento)
        {
            var email = new eMail(Encaminhamento.Remetente, Encaminhamento.Destinatario);
            email.Mensagem = Encaminhamento.Mensagem;
            email.Assunto = Encaminhamento.Assunto;
            email.AdicionarDestinatario(Encaminhamento.Destinatario);
            email.MensagemHTML = true;
            email.SSL = true;
            email.Login = "Levis_maciel@hotmail.com";
            email.Senha = "24052716";
            email.ServidorSMTP = "Smtp.live.com";
            email.Porta = 587;
            email.Enviar();

            return Encaminhamento;
        }

        public Encaminhamento Atualizar(int id, Encaminhamento CategoriaProduto)
        {
            dc.Atualizar(CategoriaProduto, id);

            return CategoriaProduto;
        }

        public void Excluir(int id)
        {
            dc.Excluir<Encaminhamento>(id);
        }
    }
}
