using Digiexpress.ADO;
using Sindiveg.API.Enumeradores;
using Sindiveg.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sindiveg.API.BLL
{
    public class DashboardsBLL : IBLL
    {
        private DbServer dc;

        public DashboardsBLL(DbServer dc)
        {
            this.dc = dc;
        }

        private static List<DateTime[]> CalcularPeriodos(FiltroDashboard f)
        {
            var periodos = new List<DateTime[]>();

            switch ((TipoIntervaloDashboard)f.Intervalo)
            {
                default:
                case TipoIntervaloDashboard.Mensal:
                    DateTime DataMensal = f.DataInicial.Value;
                    while (DataMensal <= f.DataFinal.Value)
                    {
                        if (DataMensal.AddMonths(1) > f.DataFinal.Value)
                        {
                            TimeSpan diferenca = f.DataFinal.Value - DataMensal;
                            int TotalDias = diferenca.Days;
                            var datas = new[] { DataMensal, DataMensal.AddDays(TotalDias) };
                            periodos.Add(datas);
                        }
                        else
                        {
                            var datas = new[] { DataMensal, DataMensal.AddMonths(1).AddDays(-1) };
                            periodos.Add(datas);
                        }
                        DataMensal = DataMensal.AddMonths(1);
                    }
                    break;
                case TipoIntervaloDashboard.Periodo:

                    var intervalo = f.IntervaloDias.GetValueOrDefault(10);

                    DateTime data = f.DataInicial.Value;
                    while (data <= f.DataFinal.Value)
                    {
                        if (data.AddDays(intervalo) > f.DataFinal.Value)
                        {
                            TimeSpan diferenca = f.DataFinal.Value - data;
                            int TotalDias = diferenca.Days;
                            var datas = new[] { data, data.AddDays(TotalDias) };
                            periodos.Add(datas);
                        }
                        else
                        {
                            var datas = new[] { data, data.AddDays(intervalo - 1) };
                            periodos.Add(datas);
                        }
                        data = data.AddDays(intervalo);
                    }
                    break;
            }
            return periodos;
        }


        public List<Dashboard> ListaOcorrencias(FiltroDashboard f, bool Sindiveg, List<int> Empresas, List<int> TipoOcorrencia)
        {
            var str = new StringBuilder()
            .Append("select ")
            .Append("OcorrenciasItens.id as idOcorrenciasItens, Ocorrencias.id as idOcorrencia,  ")
            .Append(" CategoriasProdutos.id as CategoriaProduto, Ocorrencias.BoletimOcorrencia, ")
            .Append("Denunciantes.id as idFonteGeradora,Produtos.id as Produto,Empresas.id as empresa, ")
            .Append("OcorrenciasItens.idDenunciado,OcorrenciasItens.idProduto,Ocorrencias.Data as DataOcorrencia, ")
            .Append("Denunciantes.Nome as Denunciante,Denunciados.Nome as Denunciado, Ocorrencias.Data, ")
            .Append("OcorrenciasItens.Litros,Produtos.Nome as NomeProduto,OcorrenciasItens.Damage,Ocorrencias.idEmpresa, ")
            .Append("Ocorrencias.Status,Locais.Estado as EstadoDenunciado,Locais.Cidade,Ocorrencias.idLocal,Ocorrencias.TipoOcorrencia, ")
            .Append("classesProdutos.id as ClasseProduto,Ocorrencias.idEmpresa ")
            .Append("from OcorrenciasItens ")
            .Append("right join Ocorrencias on Ocorrencias.id = OcorrenciasItens.idOcorrencia ")
            .Append("left join Denunciantes on Denunciantes.id = Ocorrencias.idDenunciante ")
            .Append("left join Denunciados on Denunciados.id = OcorrenciasItens.idDenunciado ")
            .Append("left join Produtos on OcorrenciasItens.idProduto = produtos.id ")
            .Append("left join TiposDenunciados on Denunciados.idTipoDenunciado = TiposDenunciados.id ")
            .Append("left join TiposDenunciantes on TiposDenunciantes.id = Denunciantes.idTipoDenunciante ")
            .Append("left join Locais on Locais.id = Ocorrencias.idLocal ")
            .Append("left join Empresas on Empresas.id = Produtos.idEmpresaProduto ")
            .Append("left join CategoriasProdutos on CategoriasProdutos.id = OcorrenciasItens.idCategoria ")
            .Append("left join ClassesProdutos on ClassesProdutos.id = Produtos.idClasseProduto ")
            .Append("where Ocorrencias.Excluido = 0 and OcorrenciasItens.Excluido = 0 ");

            if (f.Denunciante.HasValue)
                str.AppendFormat("and Denunciantes.id = @idFonteGeradora ");

            if (f.Denunciado.HasValue)
                str.AppendFormat("and OcorrenciasItens.idDenunciado = @idDenunciado ");

            if (f.DataInicial.HasValue)
                str.AppendFormat("and Ocorrencias.Data >= @DataOcorrencia ");

            if (f.DataFinal.HasValue)
                str.AppendFormat("and Ocorrencias.Data <= @DataFinal ");

            if (f.TipoOcorrencia.HasValue)
                str.AppendFormat("and TipoOcorrencia = @TipoOcorrencia ");

            if (f.Produto.HasValue)
                str.AppendFormat("and Produtos.id = @idProduto ");

            if (f.EmpresaProduto.HasValue)
                str.AppendFormat("and Empresas.id = @idEmpresaProduto ");

            if (f.CategoriaProduto.HasValue)
                str.AppendFormat("and CategoriasProdutos.id = @idCategoria ");

            if (f.ClasseProduto.HasValue)
                str.AppendFormat("and classesProdutos.id = @idClasse ");

            if (f.Empresas.HasValue)
                str.AppendFormat("and Ocorrencias.idEmpresa = @idEmpresa ");

            var parametros = new Dictionary<string, object>();
            parametros.Add("idFonteGeradora", f.Denunciante);
            parametros.Add("idDenunciado", f.Denunciado);
            parametros.Add("TipoOcorrencia", f.TipoOcorrencia);
            parametros.Add("DataOcorrencia", f.DataInicial);
            parametros.Add("DataFinal", f.DataFinal);
            parametros.Add("idProduto", f.Produto);
            parametros.Add("idEmpresaProduto", f.EmpresaProduto);
            parametros.Add("idCategoria", f.CategoriaProduto);
            parametros.Add("idClasse", f.ClasseProduto);
            parametros.Add("idEmpresa", f.Empresas);

            var Lista = dc.Consultar<Dashboard>(str.ToString(), parametros).ToList();

            if (Sindiveg)
                return Lista;
            else
                return Lista.Where(x => Empresas.Contains(x.idEmpresa)).Where(x => TipoOcorrencia.Contains(x.TipoOcorrencia)).ToList();
        }

        public List<object> DashboardOcorrencias(FiltroDashboard f, bool Sindiveg, List<int> Empresas, List<int> TipoOcorrencia)
        {
            var Lista = ListaOcorrencias(f, Sindiveg, Empresas, TipoOcorrencia);

            return DashboardOcorrencias(Lista);
        }

        public List<object> DashboardOcorrencias(List<Dashboard> Lista)
        {
            var ListaFinal = Lista.GroupBy(x => x.idOcorrencia).Select(x => new
            {
                x.Key,
                Lista = x.ToList()
            }).OrderBy(x => x.Key);

            var retorno = new List<DashboardOcorrencia>();
            foreach (var item in ListaFinal)
            {
                var Ocorrencia = new DashboardOcorrencia();
                Ocorrencia.id = item.Lista.Select(x => x.idOcorrencia).FirstOrDefault();
                Ocorrencia.Data = item.Lista.Select(x => x.Data).FirstOrDefault();
                Ocorrencia.Estado = item.Lista.Select(x => x.EstadoDenunciado).FirstOrDefault();
                retorno.Add(Ocorrencia);
            }

            var ListaPorPeriodo = new List<object>();

            var datas = retorno.Where(x => x.Data.HasValue).GroupBy(x => x.Data.Value.Year).ToList();

            foreach (var item in datas)
            {
                var valor = retorno.Where(x => x.Data!=null && x.Data.Value.Year == item.Key).Count();
                var chartData = new object[] { item.Key.ToString(), valor, "stroke-color: blue; stroke-opacity: 0.4; stroke-width: 8;fill-color:#00688B" };
                ListaPorPeriodo.Add(chartData);
            }

            var ListaSemData = retorno.Where(x => !x.Data.HasValue).Count();

            if (ListaSemData > 0)
            {
                var indefinido = new object[] { "INDEFINIDO", ListaSemData,
                "stroke-color: blue; stroke-opacity: 0.4; stroke-width: 8;fill-color:#00688B" };
                ListaPorPeriodo.Add(indefinido);
            }
            return ListaPorPeriodo;
        }

        public List<object> DashboardOcorrenciasUF(FiltroDashboard f, bool Sindiveg, List<int> Empresas, List<int> TipoOcorrencia)
        {
            var Lista = ListaOcorrencias(f, Sindiveg, Empresas, TipoOcorrencia);
            return DashboardOcorrenciasUF(Lista);
        }

        public List<object> DashboardOcorrenciasUF(List<Dashboard> Lista)
        {
            var ListaFinal = Lista.GroupBy(x => x.idOcorrencia).Select(x => new
            {
                x.Key,
                Lista = x.ToList()
            }).OrderBy(x => x.Key);

            var retorno = new List<DashboardOcorrencia>();
            foreach (var item in ListaFinal)
            {
                var Ocorrencia = new DashboardOcorrencia();
                Ocorrencia.id = item.Lista.Select(x => x.idOcorrencia).FirstOrDefault();
                Ocorrencia.Data = item.Lista.Select(x => x.Data).FirstOrDefault();
                Ocorrencia.Estado = item.Lista.Select(x => x.EstadoDenunciado).FirstOrDefault();
                Ocorrencia.idLocal = item.Lista.Select(x => x.idLocal).FirstOrDefault();
                retorno.Add(Ocorrencia);
            }

            var estados = retorno.Where(X => X.idLocal != null).GroupBy(x => x.Estado).Select(
               x => new
               {
                   x.Key,
                   Lista = x.ToList()
               }).OrderBy(x => x.Key);

            var ListaPorEstado = new List<object>();

            ListaPorEstado.Add(new object[] { "Estado", "Total" });
            foreach (var item in estados)
            {
                //var chartData = new object[1];
                var valor = retorno.Where(x => x.Estado == item.Key).Count();
                var chartData = new object[] { item.Key.ToString(), valor };
                ListaPorEstado.Add(chartData);
            }

            var ListaSemLocal = retorno.Where(x => !x.idLocal.HasValue).Count();

            if (ListaSemLocal > 0)
            {
                ListaPorEstado.Add(new object[] { "INDEFINIDO", ListaSemLocal });
            }
            return ListaPorEstado;
        }

        public List<object> DashboarLitrosProduto(FiltroDashboard f, bool Sindiveg, List<int> Empresas, List<int> TipoOcorrencia)
        {
            var Lista = ListaOcorrencias(f, Sindiveg, Empresas, TipoOcorrencia);
            return DashboarLitrosProduto(Lista);
        }

        public List<object> DashboarLitrosProduto(List<Dashboard> Lista)
        {
            var estados = Lista.Where(x => x.NomeProduto != null).GroupBy(x => x.NomeProduto).Select(
                          x => new
                          {
                              x.Key,
                              Lista = x.ToList()
                          }).OrderBy(x => x.Key);

            var ListaPorProduto = new List<object>();

            ListaPorProduto.Add(new object[] { "Produto", "Total" });
            foreach (var item in estados)
            {
                var valor = Lista.Where(x => x.NomeProduto == item.Key).Sum(x => x.Litros);
                var chartData = new object[] { item.Key.ToString(), valor };
                ListaPorProduto.Add(chartData);
            }

            var ListaSemProduto = Lista.Where(x => x.NomeProduto == null).Select(x=>x.Litros).Sum();

            if (ListaSemProduto > 0)
            {
                ListaPorProduto.Add(new object[] { "INDEFINIDO", ListaSemProduto });
            }

            return ListaPorProduto;
        }

        public List<object> DashboardRegistroProduto(FiltroDashboard f, bool Sindiveg, List<int> Empresas, List<int> TipoOcorrencia)
        {
            var Lista = ListaOcorrencias(f, Sindiveg, Empresas, TipoOcorrencia);

            return DashboardRegistroProduto(Lista);
        }

        public List<object> DashboardRegistroProduto(List<Dashboard> Lista)
        {
            var produtos = Lista.Where(x => x.NomeProduto != null).GroupBy(x => x.NomeProduto).Select(
              x => new
              {
                  x.Key,
                  Lista = x.ToList()
              }).OrderBy(x => x.Key);

            var ListaPorPeriodo = new List<object>();

            foreach (var item in produtos)
            {
                var valor = Lista.Where(x => x.NomeProduto == item.Key).Count();
                var chartData = new object[] { item.Key.ToString(), valor, "stroke-color: #871B47; stroke-opacity: 0.4; stroke-width: 8; fill-color: red; fill-opacity: 0.8" };
                ListaPorPeriodo.Add(chartData);
            }

            var ListaSemProduto = Lista.Where(x => x.NomeProduto == null).Count();

            if (ListaSemProduto > 0)
            {
                ListaPorPeriodo.Add(new object[] { "INDEFINIDO", ListaSemProduto, "stroke-color: #871B47; stroke-opacity: 0.4; stroke-width: 8; fill-color: red; fill-opacity: 0.8" });
            }

            return ListaPorPeriodo;
        }

        public List<object> DashboardDamage(FiltroDashboard f, bool Sindiveg, List<int> Empresas, List<int> TipoOcorrencia)
        {
            var Lista = ListaOcorrencias(f, Sindiveg, Empresas, TipoOcorrencia);

            return  DashboardDamage(Lista);
        }

        public List<object> DashboardDamage(List<Dashboard> Lista)
        {
            var datas = Lista.Where(x => x.Data != null).GroupBy(x => x.Data.Value.Year).Select(
                  x => new
                  {
                      x.Key,
                      Lista = x.ToList()
                  }).OrderBy(x => x.Key);

            var ListaPorPeriodo = new List<object>();

            foreach (var item in datas)
            {
                var valor = item.Lista.Where(x => x.Data.Value.Year == item.Key).Sum(x => x.Damage);
                var chartData = new object[] { item.Key.ToString(), valor, "stroke-color: blue; stroke-opacity: 0.4; stroke-width: 8; fill-color:#76A7FA; fill-opacity: 0.8" };
                ListaPorPeriodo.Add(chartData);
            }

            var Indefinido = Lista.Where(x => !x.Data.HasValue).Sum(x => x.Damage);
            if(Indefinido > 0)
                ListaPorPeriodo.Add(new object[] { "INDEFINIDO", Indefinido, "stroke-color: blue; stroke-opacity: 0.4; stroke-width: 8; fill-color:#76A7FA; fill-opacity: 0.8" });

            ListaPorPeriodo.Add(new object[] { "Total", Lista.Sum(x => x.Damage), "stroke-color: #871B47; stroke-opacity: 0.4; stroke-width: 8; fill-color: red; fill-opacity: 0.8" });

            return ListaPorPeriodo;

        }

        public List<object> DashboardTipoOcorrencia(FiltroDashboard f, bool Sindiveg, List<int> Empresas, List<int> TipoOcorrencia)
        {
            var Lista = ListaOcorrencias(f, Sindiveg, Empresas, TipoOcorrencia);

            return DashboardTipoOcorrencia(Lista);
        }

        public List<object> DashboardTipoOcorrencia(List<Dashboard> Lista)
        {
            var Ocorrencias = Lista.GroupBy(x => x.idOcorrencia).Select(
                  x => new
                  {
                      x.Key,
                      Lista = x.ToList()
                  }).OrderBy(x => x.Key);

            var ListaPorOcorrencia = new List<object>();

            var retorno = new List<DashboardOcorrencia>();
            foreach (var item in Ocorrencias)
            {
                var Ocorrencia = new DashboardOcorrencia();
                Ocorrencia.TipoOcorrencia = item.Lista.Select(x => x.TipoOcorrencia).FirstOrDefault();
                retorno.Add(Ocorrencia);
            }

            var tipos = retorno.GroupBy(x=>x.TipoOcorrencia).Select(
               x => new
               {
                   x.Key,
                   Lista = x.ToList()
               }).OrderBy(x => x.Key);

            foreach (var item in tipos)
            {
                var Ocorrencia = item.Key == (int)TipoOcorrencia.Contrabando ? "CONTRABANDO" :
                               item.Key == (int)TipoOcorrencia.Descaminho ? "DESCAMINHO" : item.Key == (int)TipoOcorrencia.Falsificacao ? "FALSIFICACAO" :
                               item.Key == (int)TipoOcorrencia.RouboCarga ? "ROUBO DE CARGA" : string.Empty;
                var valor = item.Lista.Where(x => x.TipoOcorrencia == item.Key).Count();
                var chartData = new object[] { Ocorrencia, valor, "stroke-color: #871B47; stroke-opacity: 0.4; stroke-width: 8; fill-color:black; fill-opacity: 0.8" };
                ListaPorOcorrencia.Add(chartData);
            }

            return ListaPorOcorrencia;

        }

        public List<List<object>> DashboardGeral(FiltroDashboard f, bool Sindiveg, List<int> Empresas, List<int> TipoOcorrencia)
        {
            var Lista = ListaOcorrencias(f, Sindiveg, Empresas, TipoOcorrencia);
            var RetornoDashboards = new List<List<object>>();
            RetornoDashboards.Add(DashboardDamage(Lista));
            RetornoDashboards.Add(DashboardRegistroProduto(Lista));
            RetornoDashboards.Add(DashboarLitrosProduto(Lista));
            RetornoDashboards.Add(DashboardOcorrenciasUF(Lista));
            RetornoDashboards.Add(DashboardOcorrencias(Lista));
            RetornoDashboards.Add(DashboardTipoOcorrencia(Lista));

            return RetornoDashboards;
        }
    }
}
