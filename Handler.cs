using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sindiveg.API.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sindiveg.API.Controllers
{
    public class Handler
    {
        private string diretorioAplicacao;

        public Handler(string diretorioAplicacao)
        {
            this.diretorioAplicacao = diretorioAplicacao;
        }

        public IActionResult Handle<T>(Controller controle, Func<T> action)
        {
            try
            {
                return controle.Ok(action());
            }
            catch (LoginInvalido)
            {
                return controle.Unauthorized();
            }
            catch (ErroValidacao ex)
            {
                return controle.BadRequest(ex.Erros);
            }
            catch (Exception ex)
            {
                CriarLogErro(ex);

                return controle.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        public IActionResult Handle(Controller controle, Action action)
        {
            try
            {
                action();
                return controle.Ok();
            }
            catch (LoginInvalido)
            {
                return controle.Unauthorized();
            }
            catch (ErroValidacao ex)
            {
                return controle.BadRequest(ex.Erros);
            }
            catch (Exception ex)
            {
                CriarLogErro(ex);
                return controle.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        private void CriarLogErro(Exception ex)
        {
            StringBuilder sb = new StringBuilder();

            Exception erro = ex;

            do
            {
                sb.AppendLine(erro.Message);
                erro = erro.InnerException;
            } while (erro != null);

            sb.AppendLine()
                .AppendLine(ex.StackTrace);

            string diretorioErro = Path.Combine(diretorioAplicacao, "LogErro");

            if (!Directory.Exists(diretorioErro))
                Directory.CreateDirectory(diretorioErro);

            string arquivo = $"erro{DateTime.Now.ToString("yyyyMMddHHmmss")}.log";
            arquivo = Path.Combine(diretorioErro, arquivo);

            File.WriteAllText(arquivo, sb.ToString());
        }

        /// <summary>
        /// Remove log de erros depois de uma semana
        /// </summary>
        private void LimparDiretorioErro()
        {
            IEnumerable<string> arquivos = Directory.EnumerateFiles(diretorioAplicacao);

            foreach (string arquivo in arquivos)
            {
                FileInfo file = new FileInfo(arquivo);

                if (file.CreationTime.AddDays(7) < DateTime.Now)
                    file.Delete();

                file = null;
            }
        }
    }
}
