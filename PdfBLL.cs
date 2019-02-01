using Digiexpress.ADO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Sindiveg.API.Banco;
using Sindiveg.API.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Sindiveg.API.BLL
{
    public class PdfBLL
    {
        private DbServer dc;
        private PdfPTable tabela;
        private Paragraph paragrafo;
        private PdfPCell coluna;

        public PdfBLL(DbServer dc)
        {
            this.dc = dc;
        }

        public void MontarPDF(Ocorrencias ocorrencia, UserInfo userInfo, string caminhoServidor)
        {
            var usuario = new UsuariosBLL(dc).Selecionar(userInfo.idUsuario.Value);

            var caminho = string.Format("{0}\\wwwroot\\Temp\\ocorrencia_{1}.pdf", caminhoServidor, ocorrencia.id);
            int numerosColunas = 3;

            Document doc = CriarDocumento(userInfo.idEmpresa.Value, "BOLETIM DE OCORRÊNCIA SINDIVEG", numerosColunas, caminho, caminhoServidor);

            var fonteGeradora = new DenunciantesBLL(dc).Selecionar(ocorrencia.idDenunciante.Value);
            var tipoOcorrencia = new TipoOcorrenciaBLL().SelecionarDescricao(ocorrencia.TipoOcorrencia);
            var local = ocorrencia.idLocal.HasValue ? new LocaisBLL(dc).Selecionar(ocorrencia.idLocal.Value) : null;
            var localDescricao = local != null ? string.Format("{0} - {1}", local.Cidade, local.Estado) : "Não Informado";

            paragrafo = Paragrafo(string.Format("Fonte Geradora:\n {0}", fonteGeradora != null ? fonteGeradora.Nome : "Não Informado"));
            coluna = Coluna(paragrafo);
            coluna.Colspan = numerosColunas;
            tabela.AddCell(coluna);

            paragrafo = Paragrafo(string.Format("Tipo de Ocorrência:\n {0}", tipoOcorrencia != null ? tipoOcorrencia : "Não Informado"));
            coluna = Coluna(paragrafo);
            tabela.AddCell(coluna);

            paragrafo = Paragrafo(string.Format("Número do B.O.:\n {0}", !String.IsNullOrWhiteSpace(ocorrencia.BoletimOcorrencia) ? ocorrencia.BoletimOcorrencia : "Não Informado"));
            coluna = Coluna(paragrafo);
            tabela.AddCell(coluna);

            paragrafo = Paragrafo(string.Format("Data do B.O.:\n {0}", ocorrencia.Data.HasValue ? ocorrencia.Data.Value.ToShortDateString() : "Não Informado"));
            coluna = Coluna(paragrafo);
            tabela.AddCell(coluna);

            paragrafo = Paragrafo(string.Format("Local:\n {0}", localDescricao));
            coluna = Coluna(paragrafo);
            coluna.Colspan = numerosColunas;
            tabela.AddCell(coluna);

            QuebraLinhaColuna(numerosColunas);

            paragrafo = ParagrafoTitulo(string.Format("DENÚNCIAS"));
            coluna = Coluna(paragrafo);
            coluna.Colspan = numerosColunas;
            coluna.BackgroundColor = BaseColor.LIGHT_GRAY;
            coluna.Padding = 0;
            coluna.Border = PdfPCell.RIGHT_BORDER | PdfPCell.LEFT_BORDER;
            tabela.AddCell(coluna);

            paragrafo = Paragrafo(" ");
            coluna = Coluna(paragrafo);
            coluna.BackgroundColor = BaseColor.LIGHT_GRAY;
            coluna.Border = PdfPCell.RIGHT_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER;
            coluna.Colspan = numerosColunas;
            coluna.Padding = -5;
            tabela.AddCell(coluna);

            if (ocorrencia.listaOcorrenciasItens != null && ocorrencia.listaOcorrenciasItens.Count > 0)
            {
                foreach (var denuncia in ocorrencia.listaOcorrenciasItens)
                {
                    QuebraLinhaColuna(numerosColunas);
                    paragrafo = ParagrafoTitulo("Denúncia nº " + denuncia.id);
                    paragrafo.Alignment = Element.ALIGN_LEFT;
                    coluna = Coluna(paragrafo);
                    coluna.BackgroundColor = BaseColor.LIGHT_GRAY;
                    coluna.Colspan = numerosColunas;
                    tabela.AddCell(coluna);

                    AdicionarDenuncia(denuncia, numerosColunas);
                }
            }
            else
            {
                paragrafo = Paragrafo("Não há Denúncia Registrada.");
                paragrafo.Alignment = Element.ALIGN_CENTER;
                coluna = Coluna(paragrafo);
                coluna.Colspan = numerosColunas;
                tabela.AddCell(coluna);
            }

            QuebraLinhaColuna(numerosColunas);

            paragrafo = ParagrafoTitulo(String.Format("Link para acessar o sistema: http://sindiveg.digiexpress.com.br/"));
            paragrafo.Alignment = Element.ALIGN_LEFT;
            coluna = Coluna(paragrafo);
            coluna.BackgroundColor = BaseColor.LIGHT_GRAY;
            coluna.Colspan = numerosColunas;
            tabela.AddCell(coluna);

            doc.Add(tabela);
            doc.Close();

            AdicionaRodape(usuario, caminho);
        }

        private void AdicionarDenuncia(OcorrenciaItens denuncia, int numerosColunas)
        {
            var denunciado = denuncia.idDenunciado.HasValue? new DenunciadosBLL(dc).Selecionar(denuncia.idDenunciado.Value) : null;

            paragrafo = Paragrafo(String.Format("Data da Ocorrência:\n {0}", denuncia.data.ToString("dd/MM/yyyy HH:mm")));
            coluna = Coluna(paragrafo);
            tabela.AddCell(coluna);

            paragrafo = Paragrafo(String.Format("Denúnciado:\n {0}", denuncia.Denunciado));
            coluna = Coluna(paragrafo);
            if (!denuncia.TipoOcorrencia.Equals((int)TiposOcorrencias.ROUBOCARGA))
                coluna.Colspan = 2;
            tabela.AddCell(coluna);

            if (denuncia.TipoOcorrencia.Equals((int)TiposOcorrencias.ROUBOCARGA))
            {
                var modoOperacao = denuncia.modoOperacao.HasValue ? new ModusOperandisBLL().SelecionarDescricao(denuncia.modoOperacao.Value) : "Não Informado";
                paragrafo = Paragrafo(String.Format("Modus Operandis:\n {0}", modoOperacao));
                coluna = Coluna(paragrafo);
                tabela.AddCell(coluna);

                var logradouro = denuncia.idLogradouro.HasValue ? new LogradourosBLL(dc).Selecionar(denuncia.idLogradouro.Value).Nome : "Não Informado";
                paragrafo = Paragrafo(String.Format("Logradouro:\n {0}", logradouro));
                coluna = Coluna(paragrafo);
                tabela.AddCell(coluna);

                var complemento = !String.IsNullOrWhiteSpace(denuncia.ComplementoLogradouro) ? denuncia.ComplementoLogradouro : "Não Informado";
                paragrafo = Paragrafo(String.Format("Complemento:\n {0}", complemento));
                coluna = Coluna(paragrafo);
                tabela.AddCell(coluna);

                var locais = denuncia.idLogradouro.HasValue ? new LocaisBLL(dc).Selecionar(denuncia.idLogradouro.Value) : null;
                var cidade = "Não Informado";
                if (locais != null)
                    cidade = String.Format("{0}-{1}", locais.Cidade, locais.Estado);
                paragrafo = Paragrafo(String.Format("Cidade:\n {0}", cidade));
                coluna = Coluna(paragrafo);
                tabela.AddCell(coluna);
            }

            paragrafo = Paragrafo(String.Format("Descrição:\n {0}", !String.IsNullOrWhiteSpace(denuncia.Descricao) ? denuncia.Descricao : "Não Informado"));
            coluna = Coluna(paragrafo);
            coluna.Colspan = numerosColunas;
            tabela.AddCell(coluna);

            var empresas = denuncia.idEmpresa > 0 ? new EmpresasBLL(dc).Selecionar(denuncia.idEmpresa) : null;
            var empresa = "Não Informado";
            if (empresas != null)
                empresa = !String.IsNullOrWhiteSpace(empresas.NomeFantasia) ? empresas.NomeFantasia : empresas.RazaoSocial;

            paragrafo = Paragrafo(String.Format("Empresa:\n {0}", empresa));
            coluna = Coluna(paragrafo);
            tabela.AddCell(coluna);

            var produtos = denuncia.idProduto.HasValue ? new ProdutosBLL(dc).Selecionar(denuncia.idProduto.Value) : null;
            var produto = "Não Informado";
            if (produtos != null)
                produto = produtos.Nome;

            paragrafo = Paragrafo(String.Format("Produto:\n {0}", produto));
            coluna = Coluna(paragrafo);
            tabela.AddCell(coluna);

            var categorias = denuncia.idCategoria.HasValue ? new CategoriasProdutosBLL(dc).Selecionar(denuncia.idCategoria.Value) : null;
            var categoria = "Não Informado";
            if (categorias != null)
                categoria = categorias.Descricao;
            paragrafo = Paragrafo(String.Format("Categoria:\n {0}", categoria));
            coluna = Coluna(paragrafo);
            tabela.AddCell(coluna);


            paragrafo = Paragrafo(String.Format("Litros/KG:\n {0}", !denuncia.Litros.Equals(0) ? denuncia.Litros.ToString() : "Não informado"));
            coluna = Coluna(paragrafo);
            tabela.AddCell(coluna);

            paragrafo = Paragrafo(String.Format("Lote:\n {0}", !String.IsNullOrWhiteSpace(denuncia.Lote) ? denuncia.Lote : "Não informado"));
            coluna = Coluna(paragrafo);
            tabela.AddCell(coluna);

            paragrafo = Paragrafo(String.Format("Perda:\n {0}", denuncia.Damage));
            coluna = Coluna(paragrafo);
            tabela.AddCell(coluna);

        }

        private Document CriarDocumento(int idEmpresa, string titulo, int numeroColunas, string caminho, string caminhoServidor)
        {

            Document doc = new Document(PageSize.A4);
            doc.SetMargins(10, 10, 10, 10);
            doc.AddCreationDate();

            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(caminho, FileMode.Create));
            doc.Open();

            tabela = new PdfPTable(numeroColunas);
            tabela.WidthPercentage = 100;

            paragrafo = ParagrafoTitulo(string.Format("{0}", titulo));
            coluna = Coluna(paragrafo);
            coluna.Colspan = numeroColunas;
            coluna.BackgroundColor = BaseColor.LIGHT_GRAY;
            coluna.Border = iTextSharp.text.Rectangle.NO_BORDER;
            coluna.Padding = 0;
            tabela.AddCell(coluna);

            paragrafo = Paragrafo(" ");
            coluna = Coluna(paragrafo);
            coluna.BackgroundColor = BaseColor.LIGHT_GRAY;
            coluna.Border = iTextSharp.text.Rectangle.NO_BORDER;
            coluna.Colspan = numeroColunas;
            coluna.Padding = -5;
            tabela.AddCell(coluna);


            var caminhoImagem = string.Format("{0}\\wwwroot\\Imagens\\logo-sindiveg.png", caminhoServidor);
            var imagem = File.ReadAllBytes(caminhoImagem);

            Image pic = Image.GetInstance(imagem);
            pic.ScaleToFit(80f, 60f);
            pic.Border = iTextSharp.text.Rectangle.BOX;
            pic.BorderColor = BaseColor.BLACK;
            pic.BorderWidth = 2f;

            coluna = Coluna(pic);
            coluna.HorizontalAlignment = Element.ALIGN_CENTER;
            coluna.Border = PdfPCell.NO_BORDER;
            coluna.HorizontalAlignment = Element.ALIGN_CENTER;
            coluna.VerticalAlignment = Element.ALIGN_MIDDLE;
            coluna.Rowspan = 3;

            tabela.AddCell(coluna);



            var empresa = new EmpresasBLL(dc).Selecionar(idEmpresa);
            var listaEndereco = new List<String>();

            if (!String.IsNullOrWhiteSpace(empresa.Rua))
                listaEndereco.Add(empresa.Rua);

            if (empresa.Numero.HasValue)
                listaEndereco.Add(empresa.Numero.Value.ToString());

            if (!String.IsNullOrWhiteSpace(empresa.Bairro))
                listaEndereco.Add(empresa.Bairro);

            if (!String.IsNullOrWhiteSpace(empresa.Cidade))
                listaEndereco.Add(empresa.Cidade);

            if (!String.IsNullOrWhiteSpace(empresa.Estado))
                listaEndereco.Add(empresa.Estado);

            if (!String.IsNullOrWhiteSpace(empresa.Telefone))
                listaEndereco.Add(empresa.Telefone);

            if (!String.IsNullOrWhiteSpace(empresa.Rua))
                listaEndereco.Add(empresa.Cep);

            var enderecoEmpresa = "Não informado";
            var enderecoEmpresa2 = "Não informado";

            if (listaEndereco.Count >= 5)
            {
                enderecoEmpresa = String.Join(" - ", listaEndereco.GetRange(0, 2));
                enderecoEmpresa2 = String.Join(" - ", listaEndereco.GetRange(2, listaEndereco.Count() - 3));
            }

            paragrafo = Paragrafo(empresa.RazaoSocial);
            paragrafo.Alignment = Element.ALIGN_RIGHT;
            coluna = Coluna(paragrafo);
            coluna.Border = PdfPCell.NO_BORDER;
            coluna.PaddingBottom = 0;
            coluna.Colspan = numeroColunas - 1;
            tabela.AddCell(coluna);

            paragrafo = Paragrafo(enderecoEmpresa);
            paragrafo.Alignment = Element.ALIGN_RIGHT;
            coluna = Coluna(paragrafo);
            coluna.Border = PdfPCell.NO_BORDER;
            coluna.PaddingBottom = 0;
            coluna.Colspan = numeroColunas - 1;
            tabela.AddCell(coluna);

            paragrafo = Paragrafo(enderecoEmpresa2);
            paragrafo.Alignment = Element.ALIGN_RIGHT;
            coluna = Coluna(paragrafo);
            coluna.Border = PdfPCell.NO_BORDER;
            coluna.Colspan = numeroColunas - 1;
            tabela.AddCell(coluna);


            return doc;
        }

        private void AdicionaRodape(Usuarios usuario, string caminho)
        {
            byte[] bytes = File.ReadAllBytes(caminho);
            Font blackFont = FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLACK);
            using (MemoryStream ms = new MemoryStream())
            {
                PdfReader reader = new PdfReader(bytes);
                using (PdfStamper stamper = new PdfStamper(reader, ms))
                {
                    int paginas = reader.NumberOfPages;
                    for (int i = 1; i <= paginas; i++)
                    {
                        ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_LEFT, new Phrase(String.Format("Gerado por: {0}", usuario.Login), blackFont), 10f, 15f, 0);
                        ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_RIGHT, new Phrase(String.Format("Data: {0}", DateTime.Now.ToString("dd/MM/yyyy HH:mm")), blackFont), 510f, 15f, 0);
                        ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_RIGHT, new Phrase(String.Format("Pagina: {0}/{1}",i.ToString(), paginas), blackFont), 568f, 15f, 0);
                    }
                }
                bytes = ms.ToArray();
            }
            File.WriteAllBytes(caminho, bytes);
        }

        private Paragraph ParagrafoTitulo(string texto)
        {
            Paragraph paragrafo = new Paragraph();
            paragrafo.Font = new Font(Font.GetFamilyIndex("Times New Roman"), 11);
            paragrafo.Alignment = Element.ALIGN_CENTER;
            paragrafo.Add(texto);
            return paragrafo;
        }

        private Paragraph Paragrafo(string texto)
        {
            Paragraph paragrafo = new Paragraph();
            paragrafo.Font = new Font(Font.GetFamilyIndex("Times New Roman"), 10);
            paragrafo.Add(texto);
            return paragrafo;
        }

        private Paragraph QuebraLinha()
        {
            var ph = new Paragraph();
            ph.Add(new Chunk("\n"));
            return ph;
        }

        private void QuebraLinhaColuna(int numerosColunas)
        {
            paragrafo = Paragrafo(" ");
            coluna = Coluna(paragrafo);
            coluna.Colspan = numerosColunas;
            coluna.Padding = -1;
            tabela.AddCell(coluna);
        }

        private PdfPCell Coluna(IElement elemento)
        {
            PdfPCell coluna = new PdfPCell();
            coluna.AddElement(elemento);
            coluna.Padding = 5;
            return coluna;
        }
    }
}
