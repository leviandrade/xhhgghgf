using Sindiveg.API.Enumeradores;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Sindiveg.API.BLL
{
    public class TipoOcorrenciaBLL
    {
        public string SelecionarDescricao(int enumerador)
        {
            switch (enumerador)
            {
                case (int)TipoOcorrencia.Contrabando:
                    return GetDescription(TipoOcorrencia.Contrabando);
                case (int)TipoOcorrencia.Descaminho:
                    return GetDescription(TipoOcorrencia.Descaminho);
                case (int)TipoOcorrencia.Falsificacao:
                    return GetDescription(TipoOcorrencia.Falsificacao);
                case (int)TipoOcorrencia.RouboCarga:
                    return GetDescription(TipoOcorrencia.RouboCarga);
                default:
                    return null;
            }
        }

        private string GetDescription(TipoOcorrencia enumTipoOcorrencia)
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
