var htmlEmpresa = `<div class="form-Empresa" id="FormEmpresa">
                            <form onsubmit="SalvarEmpresa(this, event);">
                                <div class="form-group ">
                                    <div class="row">
                        
                                        <div class="col-lg-12 col-xl-12">
                        
                                            <div class="row">
                                                <div class="col-lg-6">
                                                    <label>Razão Social</label>
                                                    <input class="form-control Nome obrigatorios" name="razaoSocial" type="text" required />
                                                </div>
                                                <div class="col-lg-6">
                                                    <label>Nome Fantasia</label>
                                                    <input class="form-control Nome" name="nomeFantasia" type="text" />
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-lg-4">
                                                    <div class="input-group-sm">
                                                        <label>Ramo de Atividade <a href="#" onclick="IncluirRamosAtividade();" title="Incluir" class="fa fa-plus">&nbsp;</a></label>
                                                        <select class="form-control form-control-sm ListaRamosAtividade" id="RamosAtividade" name="idRamoAtividade">
                                                            <option value="">--SELECIONE--</option>          
                                                        </select>
                                                    </div>
                                                </div>
                                                <div class="col-lg-4">
                                                    <label>Telefone</label>
                                                    <input class="form-control telefone" name="telefone" type="text" id="telefone" />
                                                </div>
                                                <div class="col-lg-4">
                                                    <label>CNPJ</label>
                                                    <input class="form-control obrigatorios Cnpj" name="cnpj" type="text" required />
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-lg-5">
                                                    <label>CEP</label>
                                                    <input class="form-control cep" name="cep" type="text" id="cep" />
                                                </div>
                                                <div class="col-lg-5">
                                                    <label>Rua</label>
                                                    <input class="form-control" name="rua" type="text" id="rua" />
                                                </div>
                                                <div class="col-lg-2">
                                                    <label>Numero</label>
                                                    <input class="form-control" name="numero" type="text" id="numero" />
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-lg-5">
                                                    <label>Bairro</label>
                                                    <input class="form-control" name="bairro" type="text" id="bairro" />
                                                </div>
                                                <div class="col-lg-5">
                                                    <label>Cidade</label>
                                                    <input class="form-control" name="cidade" type="text" id="cidade" />
                                                </div>
                                                <div class="col-lg-2">
                                                    <label>Estado</label>
                                                    <input class="form-control" name="estado" type="text" id="uf" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-2">
                                        <button class="btn btn-sm btn-block btn-primary" type="submit" style="margin-top: 20px;">Enviar</button>
                                    </div>
                                    <div class="col-lg-2">
                                        <button class="btn btn-sm btn-block btn-primary" type="button" onclick="FecharFormularioAvulsoEmpresas();" style="margin-top: 20px;">Fechar</button>
                                    </div>
                                </div>
                            </form>
                        </div>`


var RamosAtividade = ` <div class="form-RamosAtividade">
                        <form onsubmit="salvarRamosAtividade(this, event);">
                            <div class="form-group">
                                <label>Ramo de Atividade</label>
                                <input class="form-control obrigatorios" name="Nome" type="text" required id="descricao" />
                            </div>
                            <div class="row">
                                <div class="col-lg-2">
                                    <button class="btn btn-sm btn-block btn-primary" type="submit" style="margin-top: 20px;">Enviar</button>
                                </div>
                                <div class="col-lg-2">
                                    <button class="btn btn-sm btn-block btn-primary" type="button" onclick="FecharRamosAtividadeAvulso();" style="margin-top: 20px;">Voltar</button>
                                </div>
                            </div>
                        </form>
                    </div>`

var Denunciado = `<div class="form-Denunciados" id="FormDenunciados">
                <form onsubmit="SalvarDenunciadosAvulso(this, event);">
                    <div class="form-group">
                        <div class="row">

                            <div class="col-lg-12 col-xl-12">

                                <div class="row">
                                    <div class="col-lg-3">
                                        <label>TIPO <a href="#" onclick="IncluirTiposDenunciados()" title="Incluir" class="fa fa-plus">&nbsp;</a></label>
                                        <select class="form-control form-control-sm obrigatorios" id="ListaTiposDenunciados" name="idTipoDenunciado" required>
                                        <option value="">--SELECIONE--</option>                         
                                        </select>
                                    </div>
                                    <div class="col-lg-9">
                                        <div class="input-group-sm">
                                            <label>NOME</label>
                                            <input class="form-control Nome obrigatorios" name="nome" type="text" required />
                                        </div>
                                    </div>
                                </div>

                                <div class="row formulario">
                                    <div class="col-lg-3">
                                        <div class="input-group-sm">
                                            <div class="input-group-sm" name="tipoDocumento">
                                                <label>DOCUMENTO</label>
                                                <select class="form-control tipoDocumento" name="tipoDocumento">
                                                    <option value="">--SELECIONE--</option>
                                                    <option value="0">CNPJ</option>
                                                    <option value="1">CPF</option>
                                                    <option value="2">RG</option>
                                                </select>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-lg-5">
                                        <div class="input-group-sm">
                                            <label>NUMERO DOCUMENTO</label>
                                            <input class="form-control NumeroDocumento" name="numeroDocumento" type="text" />
                                        </div>
                                    </div>
                                    <div class="col-lg-4">
                                        <div class="input-group-sm">
                                            <label>LOCAL <a href="#" onclick="IncluirSubLocaisDenunciados();" title="Incluir" class="fa fa-plus">&nbsp;</a></label>
                                            <select class="form-control form-control-sm obrigatorios" id="ListaLocaisDenunciados" name="idLocal" required>
                                                <option value="">--SELECIONE--</option>                         
                                            </select>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-2">
                            <button class="btn btn-sm btn-block btn-primary" type="submit" style="margin-top: 20px;">ENVIAR</button>
                        </div>
                        <div class="col-lg-2">
                            <button class="btn btn-sm btn-block btn-primary" type="button" onclick="FecharFormularioAvulso();" style="margin-top: 20px;">FECHAR</button>
                        </div>
                    </div>
                </form>
            </div>`

var TipoDenunciado = `<div class="form-SubTiposDenunciados">
                    <form onsubmit="SalvarTiposDenunciados(this, event);">
                        <div class="form-group">
                        <label for="email">DESCRICAO</label>
                            <input class="form-control obrigatorios" name="descricao" type="text" required id="descricao" />
                        </div>
                        <div class="row">
                            <div class="col-lg-2">
                                 <button class="btn btn-sm btn-block btn-primary" type="submit" style="margin-top: 20px;">ENVIAR</button>
                            </div>
                        <div class="col-lg-2">
                            <button class="btn btn-sm btn-block btn-primary" type="button" onclick="FecharSubFormularioAvulsoDenunciados();" style="margin-top: 20px;">VOLTAR</button>
                        </div>
                        </div>
                 </form>
                </div>`

var LocaisDenunciado = `<div class="form-Locais">
                <form onsubmit="SalvarSubLocaisDenunciados(this, event);">
                    <div class="form-group ">
            
                        <div class="row">
                            <div class="col-lg-9">
                                <div class="input-group-sm">
                                    <label>Cidade</label>
                                    <input class="form-control cidade" name="cidade" type="text" required />
                                </div>
                            </div>
                            <div class="col-lg-3">
                                <div class="input-group-sm">
                                    <label>Estado</label>
                                    <select class="form-control" name="estado" required>
                                        <option value="">--Selecione--</option>
                                        <option value="AC">AC</option>
                                        <option value="AL">AL</option>
                                        <option value="AP">AP</option>
                                        <option value="AM">AM</option>
                                        <option value="BA">BA</option>
                                        <option value="CE">CE</option>
                                        <option value="DF">DF</option>
                                        <option value="ES">ES</option>
                                        <option value="GO">GO</option>
                                        <option value="MA">MA</option>
                                        <option value="MT">MT</option>
                                        <option value="MS">MS</option>
                                        <option value="MG">MG</option>
                                        <option value="PA">PA</option>
                                        <option value="PB">PB</option>
                                        <option value="PR">PR</option>
                                        <option value="PE">PE</option>
                                        <option value="PI">PI</option>
                                        <option value="RJ">RJ</option>
                                        <option value="RN">RN</option>
                                        <option value="RS">RS</option>
                                        <option value="RO">RO</option>
                                        <option value="RR">RR</option>
                                        <option value="SC">SC</option>
                                        <option value="SP">SP</option>
                                        <option value="SE">SE</option>
                                        <option value="TO">TO</option>
                                    </select>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-2">
                            <button class="btn btn-sm btn-block btn-primary" type="submit" style="margin-top: 20px;">ENVIAR</button>
                        </div>
                        <div class="col-lg-2">
                            <button class="btn btn-sm btn-block btn-primary" type="button" onclick="FecharSubFormularioAvulsoDenunciados();" style="margin-top: 20px;">VOLTAR</button>
                        </div>
                    </div>
                </form>
            </div>`

var htmlProduto = `<div class="form-Produtos" id="FormProdutos">
                        <form onsubmit="SalvarProdutosAvulso(this, event);">
                            <div class="form-group ">
                                <div class="row">
                    
                                    <div class="col-lg-12 col-xl-12">
                    
                                        <div class="row">
                                            <div class="col-lg-12">
                                                <div class="input-group-sm">
                                                    <label>NOME</label>
                                                    <input class="form-control obrigatorios" name="nome" type="text" required />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-lg-6">
                                                <div class="input-group-sm">
                                                    <label>EMPRESA <a href="#" onclick="IncluirProdutoEmpresa();" title="Incluir" class="fa fa-plus">&nbsp;</a></label>
                                                    <select class="form-control form-control-sm ListaEmpresas obrigatorios"  name="idEmpresaProduto" required>
                                                        <option value="">--SELECIONE--</option>    
                                                    </select>
                                                </div>
                                            </div>
                                            <div class="col-lg-6">
                                                <div class="input-group-sm">
                                                    <label>CLASSE <a href="#" onclick="IncluirProdutoClasse();" title="Incluir" class="fa fa-plus">&nbsp;</a></label>
                                                    <select class="form-control form-control-sm obrigatorios" id="ListaClassesProdutos" name="idClasseProduto" required>
                                                        <option value="">--SELECIONE--</option>    
                                                    </select>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-2">
                                        <button class="btn btn-sm btn-block btn-primary" type="submit" style="margin-top: 20px;">ENVIAR</button>
                                    </div>
                                    <div class="col-lg-2">
                                        <button class="btn btn-sm btn-block btn-primary" type="button" onclick="FecharFormularioProdutos();" style="margin-top: 20px;">FECHAR</button>
                                    </div>
                                </div>
                            </div>
                    </form>
                    </div>`

var ProdutoEmpresa = `<div class="form-SubProdutosEmpresa" id="FormProdutoEmpresa">
                        <form onsubmit="SalvarProdutoEmpresa(this, event);">
                            <div class="form-group ">
                                <div class="row">
                
                                    <div class="col-lg-12 col-xl-12">
                
                                        <div class="row">
                                            <div class="col-lg-6">
                                                <label>RAZAO SOCIAL</label>
                                                <input class="form-control obrigatorios" name="razaoSocial" type="text" required />
                                            </div>
                                            <div class="col-lg-6">
                                                <label>NOME FANTASIA</label>
                                                <input class="form-control " name="nomeFantasia" type="text" />
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-lg-4">
                                                <div class="input-group-sm">
                                                    <label>RAMO DE ATIVIDADE <a href="#" onclick="IncluirRamosAtividadeProdutoEmpresa();" title="Incluir" class="fa fa-plus">&nbsp;</a></label>
                                                    <select id="RamoAtividadeEmpresa" class="form-control form-control-sm ListaRamosAtividade" name="idRamoAtividade">
                                                     <option value="">--SELECIONE--</option>   
                                                    </select>
                                                </div>
                                            </div>
                                            <div class="col-lg-4">
                                                <label>TELEFONE</label>
                                                <input class="form-control telefone" name="telefone" type="text" id="telefone" />
                                            </div>
                                            <div class="col-lg-4">
                                                <label>CNPJ</label>
                                                <input class="form-control obrigatorios Cnpj" name="cnpj" type="text" required />
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-lg-5">
                                                <label>CEP</label>
                                                <input class="form-control cep" name="cep" type="text" id="cep" />
                                            </div>
                                            <div class="col-lg-5">
                                                <label>Rua</label>
                                                <input class="form-control " name="rua" type="text" id="rua" />
                                            </div>
                                            <div class="col-lg-2">
                                                <label>NUMERO</label>
                                                <input class="form-control " name="numero" type="text" id="numero" />
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-lg-5">
                                                <label>BAIRRO</label>
                                                <input class="form-control " name="bairro" type="text" id="bairro" />
                                            </div>
                                            <div class="col-lg-5">
                                                <label>CIDADE</label>
                                                <input class="form-control " name="cidade" type="text" id="cidade" />
                                            </div>
                                            <div class="col-lg-2">
                                                <label>ESTADO</label>
                                                <input class="form-control " name="estado" type="text" id="uf" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-2">
                                    <button class="btn btn-sm btn-block btn-primary" type="submit" style="margin-top: 20px;">ENVIAR</button>
                                </div>
                                <div class="col-lg-2">
                                    <button class="btn btn-sm btn-block btn-primary" type="button" onclick="FecharSubFormularioAvulsoProdutos();" style="margin-top: 20px;">VOLTAR</button>
                                </div>
                            </div>
                        </form>
                </div>`

var RamosAtividadeProdutoEmpresa = ` <div class="form-RamosAtividade">
                        <form onsubmit="salvarRamosAtividadeProdutoEmpresa(this, event);">
                            <div class="form-group ">
                                <label for="email">Ramo de Atividade</label>
                                <input class="form-control obrigatorios" name="Nome" type="text" required id="descricao" />
                            </div>
                            <div class="row">
                                <div class="col-lg-2">
                                    <button class="btn btn-sm btn-block btn-primary" type="submit" style="margin-top: 20px;">Enviar</button>
                                </div>
                                <div class="col-lg-2">
                                    <button class="btn btn-sm btn-block btn-primary" type="button" onclick="FecharRamosAtividadeProdutoEmpresa();" style="margin-top: 20px;">Voltar</button>
                                </div>
                            </div>
                        </form>
                    </div>`

var ProdutoClasse = `<div class="form-SubClasseProduto">
                      <form onsubmit="SalvarProdutoClasse(this, event);">
                          <div class="form-group ">
                              <label for="email">DESCRICAO</label>
                              <input class="form-control obrigatorios" name="descricao" type="text" required id="descricao" />
                          </div>
                          <div class="row">
                              <div class="col-lg-2">
                                  <button class="btn btn-sm btn-block btn-primary" type="submit" style="margin-top: 20px;">ENVIAR</button>
                              </div>
                              <div class="col-lg-2">
                                  <button class="btn btn-sm btn-block btn-primary" type="button" onclick="FecharSubFormularioAvulsoProdutos();" style="margin-top: 20px;">VOLTAR</button>
                              </div>
                          </div>
                      </form>
                  </div>`

var DenunciantesLocais = `
    <div class="form-SubDenunciantesLocais">
    <form onsubmit="SalvarSubLocais(this, event);">
        <div class="form-group ">

            <div class="row">
                <div class="col-lg-9">
                    <div class="input-group-sm">
                        <label>CIDADE</label>
                        <input class="form-control cidade" name="cidade" type="text" required />
                    </div>
                </div>
                <div class="col-lg-3">
                    <div class="input-group-sm">
                        <label>ESTADO</label>
                        <select class="form-control" name="estado" required>
                            <option value="">--SELECIONE--</option>
                            <option value="AC">AC</option>
                            <option value="AL">AL</option>
                            <option value="AP">AP</option>
                            <option value="AM">AM</option>
                            <option value="BA">BA</option>
                            <option value="CE">CE</option>
                            <option value="DF">DF</option>
                            <option value="ES">ES</option>
                            <option value="GO">GO</option>
                            <option value="MA">MA</option>
                            <option value="MT">MT</option>
                            <option value="MS">MS</option>
                            <option value="MG">MG</option>
                            <option value="PA">PA</option>
                            <option value="PB">PB</option>
                            <option value="PR">PR</option>
                            <option value="PE">PE</option>
                            <option value="PI">PI</option>
                            <option value="RJ">RJ</option>
                            <option value="RN">RN</option>
                            <option value="RS">RS</option>
                            <option value="RO">RO</option>
                            <option value="RR">RR</option>
                            <option value="SC">SC</option>
                            <option value="SP">SP</option>
                            <option value="SE">SE</option>
                            <option value="TO">TO</option>
                        </select>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-lg-2">
                <button class="btn btn-sm btn-block btn-primary" type="submit" style="margin-top: 20px;">ENVIAR</button>
            </div>
            <div class="col-lg-2">
                <button class="btn btn-sm btn-block btn-primary" type="button" onclick="FecharSubFormularioAvulsoDenunciantes();" style="margin-top: 20px;">VOLTAR</button>
            </div>
        </div>
    </form>
</div>`

var TiposDenunciantes = `<div class="form-SubTiposDenunciantes">
                        <form onsubmit="SalvarTiposDenunciantes(this, event);">
                        <div class="form-group">
                            <label for="email">DESCRICAO</label>
                            <input class="form-control obrigatorios" name="descricao" type="text" required id="descricao" />
                      </div>
                    <div class="row">
                        <div class="col-lg-2">
                            <button class="btn btn-sm btn-block btn-primary" type="submit" style="margin-top: 20px;">ENVIAR</button>
                         </div>
                    <div class="col-lg-2">
                        <button class="btn btn-sm btn-block btn-primary" type="button" onclick="FecharSubFormularioAvulsoDenunciantes();" style="margin-top: 20px;">VOLTAR</button>
                    </div>
                    </div>
            </form>
</div>`

var htmlDenunciantes = `<div class="form-Denunciantes" id="FormularioDenunciantes">
                    <form onsubmit="SalvarDenuncianteAvulso(this, event);">
                        <div class="form-group" id="FormularioDenunciantes">
                            <div class="row">

                                <div class="col-lg-12 col-xl-12">

                                    <div class="row">
                                        <div class="col-lg-3">
                                            <label>TIPO <a href="#" onclick="IncluirTiposDenunciantes();" title="Incluir" class="fa fa-plus">&nbsp;</a></label>
                                            <select class="form-control form-control-sm obrigatorios" id="ListaTiposDenunciantes" name="idTipoDenunciante" required>
                                            <option value="">--SELECIONE--</option>      
                                            </select>
                                        </div>
                                        <div class="col-lg-9">
                                            <div class="input-group-sm">
                                                <label>NOME</label>
                                                <input class="form-control obrigatorios" name="nome" type="text" id="NomeDenunciante" value="" required />
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row formulario">
                                        <div class="col-lg-3">
                                            <div class="input-group-sm">
                                <div class="input-group-sm" name="tipoDocumento" id="TipoDocumento">
                                                    <label>DOCUMENTO</label>
                                                    <select class="form-control tipoDocumento" name="tipoDocumento">
                                                        <option value="">--SELECIONE--</option>
                                                        <option value="0">CNPJ</option>
                                                        <option value="1">CPF</option>
                                                        <option value="2">RG</option>
                                                    </select>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-lg-5">
                                            <div class="input-group-sm">
                                                <label>NUMERO DOCUMENTO</label>
                                                <input class="form-control NumeroDocumento" name="numeroDocumento" id="NumeroDocumento" type="text" />
                                            </div>
                                        </div>
                                        <div class="col-lg-4">
                                            <div class="input-group-sm">
                                                <label>LOCAL <a href="#" onclick="IncluirSubLocais();" title="Incluir" class="fa fa-plus">&nbsp;</a></label>
                                <select class="form-control form-control-sm obrigatorios" id="ListaLocaisDenunciantes" name="idLocal" id="Local" required>
                             <option value="">--SELECIONE--</option>     
                                </select>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-2">
                                <button class="btn btn-sm btn-block btn-primary" type="submit" style="margin-top: 20px;">ENVIAR </button>
                            </div>
                            <div class="col-lg-2">
                                <button class="btn btn-sm btn-block btn-primary" type="button" onclick="FecharFormularioAvulsoOcorrencias();" style="margin-top: 20px;">FECHAR</button>
                            </div>
                        </div>
                    </form>
                </div>`


var Logradouro = `<div class="form-Logradouro">
                        <form onsubmit="SalvarLogradouro(this, event);">
                        <div class="form-group">
                            <label for="email">LOGRADOURO</label>
                            <input class="form-control" name="nome" type="text" required id="Logradouro" />
                      </div>
                    <div class="row">
                        <div class="col-lg-2">
                            <button class="btn btn-sm btn-block btn-primary" type="submit" style="margin-top: 20px;">ENVIAR</button>
                         </div>
                    <div class="col-lg-2">
                        <button class="btn btn-sm btn-block btn-primary" type="button" onclick="FacharAvulso();" style="margin-top: 20px;">FECHAR</button>
                    </div>
                    </div>
                     </form>
                </div>`

var observacao =` <div id="conteudoimpressao">
    <label>OBSERVACAO</label>
    <input class="form-control" name="observacao" id="observacao" type="text" required />
    <div class="col-lg-2">
        <button class="btn btn-sm btn-block btn-primary" onclick="imprimirDamage()" style="margin-top: 20px;">IMPRIMIR</button>
    </div>
</div>`

var observacaoGeral = `<div class="form-graficos">
<form onsubmit="imprimirGeral(this, event);">
    <label>PERDA</label>
    <input class="form-control" name="perda" id="perda" type="text" /><br/>

    <label>OCORRENCIAS POR UF</label>
    <input class="form-control" name="ocorrenciasUF" id="ocorrenciasUF" type="text" /><br/>

    <label>QTD. LITROS POR PRODUTO</label>
    <input class="form-control" name="litrosProduto" id="litrosProduto" type="text" /><br/>

    <label>QUANTIDADE DE OCORRENCIAS</label>
    <input class="form-control" name="qtdOcorrencias" id="qtdOcorrencias" type="text" /><br/>

    <label>REGISTRO POR PRODUTO</label>
    <input class="form-control" name="registroProduto" id="registroProduto" type="text" /><br/>

    <label>TIPO DE OCORRENCIA</label>
    <input class="form-control" name="tipoOcorrencia" id="tipoOcorrencia" type="text" />

    <div class="col-lg-2">
        <button class="btn btn-sm btn-block btn-primary" type="submit" style="margin-top: 20px;">IMPRIMIR</button>
    </div>
</form>
</div>`