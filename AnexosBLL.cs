using Sindiveg.API.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Sindiveg.API.BLL
{
    public class AnexosBLL
    {
        private string path;

        public AnexosBLL(string path)
        {
            this.path = path;
        }

        public List<Anexo> ListarArquivos(int id)
        {
            var listaArquivos = new List<Anexo>();

            if (ValidarPath())
            {
                var diretorio = new DirectoryInfo(path);

                foreach (var item in diretorio.GetFiles())
                {
                    Byte[] bytes = File.ReadAllBytes(item.FullName);
                    string base64 = Convert.ToBase64String(bytes);

                    var tipo = AtribuirTipo(item.Extension);

                    listaArquivos.Add(new Anexo
                    {
                        arquivoBase64 = string.Format("data:{0};base64,{1}", tipo, base64),
                        data = item.LastWriteTime.ToString("dd/MM/yyyy HH:mm"),
                        idOcorrenciaItem = id,
                        nome = item.Name,
                        tipo = tipo
                    });
                }
            }

            return listaArquivos;
        }

        private string AtribuirTipo(string extension)
        {
            extension = extension.Replace(".", "");

            if (extension.Equals("jpeg") || extension.Equals("jpg") || extension.Equals("png"))
                return String.Format("image/{0}", extension);

            if (extension.Equals("pdf"))
                return "application/pdf";

            return null;
        }

        public void Salvar(List<Anexo> listaAnexos)
        {
            // há mais validações no Client no Front End -> Anexo.js

            CriarPath();

            if (QuantidadeArquivos() > 0)
            {
                DeletarArquivos();
            }

            foreach (var anexo in listaAnexos)
            {
                if (anexo.arquivoBase64 != null)
                {
                    var header = anexo.tipo;
                    if (header.Contains("jpeg") || header.Contains("jpg") || header.Contains("png") || header.Contains("pdf"))
                        SalvarArquivos(anexo);
                }
            }

        }

        private void SalvarArquivos(Anexo anexo)
        {
            var arquivo = anexo.arquivoBase64.Split(',');
            var base64 = arquivo[1];
            byte[] imagemByteArray = Convert.FromBase64String(base64);
            File.WriteAllBytes(Path.Combine(path, anexo.nome), imagemByteArray.ToArray());
        }

        private void CriarPath()
        {
            if (!ValidarPath())
                Directory.CreateDirectory(path);
        }

        private void DeletarArquivos()
        {
            var diretorio = new DirectoryInfo(path);
            foreach (var item in diretorio.GetFiles())
            {
                item.Delete();
            }
        }

        private bool ValidarPath()
        {
            return Directory.Exists(path);
        }

        private int QuantidadeArquivos()
        {
            if (ValidarPath())
            {
                var diretorio = new DirectoryInfo(path);
                return diretorio.GetFiles().Length;
            }

            return 0;
        }
    }
}
