using Sindiveg.API.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Sindiveg.API.BLL
{
    public class ModusOperandisBLL
    {
        public string SelecionarDescricao(int enumerador)
        {
            switch (enumerador)
            {
                case (int)ModusOperandis.Movimento:
                    return GetDescription(ModusOperandis.Movimento);
                case (int)ModusOperandis.ParadoPortaCliente:
                    return GetDescription(ModusOperandis.ParadoPortaCliente);
                case (int)ModusOperandis.ParadoPostoCombustivel:
                    return GetDescription(ModusOperandis.ParadoPostoCombustivel);
                case (int)ModusOperandis.ParadoPostoFiscal:
                    return GetDescription(ModusOperandis.ParadoPostoFiscal);
                default:
                    return null;
            }
        }

        private string GetDescription(ModusOperandis enumTipoOcorrencia)
        {
            FieldInfo oFieldInfo = enumTipoOcorrencia.GetType().GetField(enumTipoOcorrencia.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])oFieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes.Length > 0)
                return attributes[0].Description;
            else
                return enumTipoOcorrencia.ToString();
        }
    }
}

