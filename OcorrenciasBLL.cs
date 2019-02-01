using Digiexpress.ADO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Sindiveg.API.Banco;
using Sindiveg.API.Models;
using Sindiveg.API.Validacao;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using Sindiveg.API.Enumeradores;

namespace Sindiveg.API.BLL
{
    public class OcorrenciasBLL : IBLL
    {
        private DbServer dc;
        OcorrenciasValidacao validacao = new OcorrenciasValidacao();
        public OcorrenciasBLL(DbServer dc)
        {
            this.dc = dc;
        }

        public IEnumerable<Ocorrencias> Lista()
        {
            return dc.Lista<Ocorrencias>();
        }

        public IEnumerable<Ocorrencia> ListaOcorrencias(bool Sindiveg, List<int> Empresas, List<int> TipoOcorrencia)
        {
            var str = new StringBuilder()
              .Append("select Ocorrencias.id,status, Data, Fechamento, Ocorrencias.idLOcal, BoletimOcorrencia,TipoOcorrencia, ")
              .Append("Cidade, Estado,Denunciantes.Nome as Denunciante,Ocorrencias.idEmpresa from Ocorrencias ")
              .Append("left join Locais on Locais.id = Ocorrencias.idLocal ")
              .Append("left join Denunciantes on Denunciantes.id = Ocorrencias.idDenunciante ")
              .Append("where Ocorrencias.Excluido = 0 ");

            var lista = dc.Consultar<Ocorrencia>(str.ToString()).ToList().OrderByDescending(x => x.id);

            var ListaRetorno = new List<Ocorrencia>();

            str.Clear();

            str.Append("select o.id, o.idOcorrencia, o.idProduto, o.idDenunciado, Produtos.Nome as Produto, ")
            .Append("Denunciados.Nome as Denunciado,  ")
            .Append("o.Damage, o.Litros, ")
            .Append("Empresas.RazaoSocial,Empresas.NomeFantasia ")
            .Append("from OcorrenciasItens as o ")
            .Append("left join Produtos on o.idProduto = Produtos.id ")
            .Append("left join Empresas on Empresas.id = Produtos.idEmpresaProduto ")
            .Append("left join Denunciados on Denunciados.id = o.idDenunciado ")
            .Append("where o.Excluido = 0; ");

            var ListaOcorrenciasItens = dc.Consultar<OcorrenciaItens>(str.ToString()).ToList();

            foreach (var item in lista)
            {
                var itemOcorrencia = new Ocorrencia
                {
                    id = item.id,
                    Denunciante = item.Denunciante,
                    idEmpresa = item.idEmpresa,
                    DataOcorrencia = item.Data.HasValue ? item.Data.Value.ToString("dd/MM/yyyy") : null,
                    BoletimOcorrencia = item.BoletimOcorrencia,
                    DataFechamento = item.Fechamento.HasValue ? item.Fechamento.Value.ToString("dd/MM/yyyy") : string.Empty,
                    Status = item.Status,
                    LocalOcorrencia = item.Cidade != null && item.Estado != null ? item.Cidade + " - " + item.Estado : string.Empty,
                    TipoOcorrencia = item.TipoOcorrencia,
                    Damage = ListaOcorrenciasItens.Where(x => x.idOcorrencia == item.id).Select(x => x.Damage).Sum(),
                    subListaOcorrenciasItens = ListaOcorrenciasItens.Where(x => x.idOcorrencia == item.id).ToList()
                };

                ListaRetorno.Add(itemOcorrencia);
            }
            if (Sindiveg)
                return ListaRetorno;
            else
                return ListaRetorno.Where(x => Empresas.Contains(x.idEmpresa)).Where(x => TipoOcorrencia.Contains(x.TipoOcorrencia));
        }

        public void FinalizarOcorrencia(int id, bool Sindiveg, List<int> Empresas, List<int> TipoOcorrencia)
        {
            var item = dc.Selecionar<Ocorrencias>(id);
            if (Sindiveg || (Empresas.Contains((int)item.idEmpresa) && TipoOcorrencia.Contains(item.TipoOcorrencia)))
            {
                item.Fechamento = DateTime.Now;
                item.Status = (int)StatusOcorrencia.Concluido;

                dc.Atualizar(item, id);
            }
        }

        public Ocorrencias Selecionar(int id, bool Sindiveg, List<int> Empresas, List<int> TipoOcorrencia)
        {
            var Ocorrencia = dc.Selecionar<Ocorrencias>(id);
            if (Sindiveg || (Empresas.Contains((int)Ocorrencia.idEmpresa) && TipoOcorrencia.Contains(Ocorrencia.TipoOcorrencia)))
                return Ocorrencia;
            else
                return null;
        }

        public Ocorrencias Incluir(Ocorrencias Ocorrencia)
        {
            Ocorrencia.id = dc.Inserir(Ocorrencia);
            return Ocorrencia;
        }

        public Ocorrencias EnviarEmail(int id, UserInfo userInfo, string path)
        {
            var ocorrencia = dc.Selecionar<Ocorrencias>(id);
            ocorrencia.listaOcorrenciasItens = new OcorrenciasItensBLL(dc).Listar(ocorrencia).ToList();
            new PdfBLL(dc).MontarPDF(ocorrencia, userInfo, path);
            var caminho = string.Format("{0}\\wwwroot\\Temp\\ocorrencia_{1}.pdf", path, ocorrencia.id);
            var anexos = new List<AnexoEmail>();
            anexos.Add(new AnexoEmail
            {
                arquivoByteArray = File.ReadAllBytes(caminho),
                nome = "boletim_ocorrencia.pdf"
            });

            var destinatarios = PopulaDestinatario(ocorrencia);

            new EmailBLL(dc).EnviarEmail(String.Format("Boletim de Ocorrência Nº {0}", ocorrencia.id), destinatarios, anexos);
            DeletaArquivoTemp(caminho);
            return ocorrencia;
        }

        private List<String> PopulaDestinatario(Ocorrencias ocorrencia)
        {
            var listaDestinatario = new List<string>();
            var idEmpresa = ocorrencia.idEmpresa.HasValue ? ocorrencia.idEmpresa.Value : 0;
            var listaUsuarios = new UsuariosBLL(dc).Lista().Where(x => x.idEmpresa == idEmpresa);

            foreach (var usuario in listaUsuarios)
            {
                var permissao = new UsuariosPermissoesBLL(dc).SelecionarIdUsuario(usuario.id);
                if (permissao != null)
                {
                    var permissoesTipoOcorrencias = permissao.TipoOcorrencia.Split(',');

                    if (permissoesTipoOcorrencias != null)
                    {
                        if (permissoesTipoOcorrencias.Contains(ocorrencia.TipoOcorrencia.ToString()) && !String.IsNullOrWhiteSpace(usuario.Email))
                        {
                            if (!listaDestinatario.Contains(usuario.Email))
                                listaDestinatario.Add(usuario.Email);
                        }
                    }
                }
            }

            return listaDestinatario;
        }

        private void DeletaArquivoTemp(string caminho)
        {
            File.Delete(caminho);
        }

        public Ocorrencias Atualizar(int id, Ocorrencias Ocorrencia, bool Sindiveg, List<int> Empresas, List<int> TipoOcorrencia)
        {
            if (Sindiveg || (Empresas.Contains((int)Ocorrencia.idEmpresa) && TipoOcorrencia.Contains(Ocorrencia.TipoOcorrencia)))
            {
                validacao.Validacao(Ocorrencia);
                dc.Atualizar(Ocorrencia, id);
            }
            return Ocorrencia;
        }

        public void Excluir(int id, bool Sindiveg, List<int> Empresas, List<int> TipoOcorrencia)
        {
            var Ocorrencia = dc.Selecionar<Ocorrencias>(id);
            if (Sindiveg || (Empresas.Contains((int)Ocorrencia.idEmpresa) && TipoOcorrencia.Contains(Ocorrencia.TipoOcorrencia)))
                dc.Excluir<Ocorrencias>(id);
        }
    }
}
