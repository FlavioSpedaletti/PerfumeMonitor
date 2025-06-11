# Deploy da Aplicação Web no Azure

Este documento explica como fazer o deploy da aplicação PerfumeMonitor.Web no Azure App Service.

## Estratégia de Configuração

A aplicação foi desenvolvida com uma estratégia híbrida para funcionar tanto localmente quanto na nuvem:

### Desenvolvimento Local
- Usa arquivos compartilhados na raiz do projeto (`config.json` e `twilio-credentials.json`)
- Permite sincronização automática entre aplicação web e Windows Forms

### Produção (Azure)
- Usa arquivos locais do projeto ou variáveis de ambiente
- Fallback automático quando arquivos compartilhados não estão disponíveis

## Opções de Configuração no Azure

### Opção 1: Usando Arquivos de Configuração
Os arquivos `config.json` e `twilio-credentials.json` estão incluídos no projeto e serão copiados automaticamente durante a publicação.

**Vantagens:**
- Funciona imediatamente após o deploy
- Não requer configuração adicional

**Desvantagens:**
- Credenciais ficam no código (menos seguro)
- Configurações ficam estáticas

### Opção 2: Usando Variáveis de Ambiente (Recomendado)
Configure as seguintes variáveis de ambiente no Azure App Service:

#### Credenciais do Twilio:
- `TWILIO_ACCOUNT_SID`: Seu Account SID do Twilio
- `TWILIO_AUTH_TOKEN`: Seu Auth Token do Twilio  
- `TWILIO_FROM_NUMBER`: Número do WhatsApp Business (formato: +5511999999999)
- `TWILIO_TO_NUMBER`: Seu número pessoal (formato: +5511999999999)

#### Como configurar no Azure Portal:
1. Acesse o Azure Portal
2. Navegue até seu App Service
3. Vá em **Configuration** > **Application settings**
4. Adicione as variáveis de ambiente listadas acima
5. Salve as configurações
6. Reinicie a aplicação

## Ordem de Precedência da Configuração

A aplicação tenta carregar as configurações na seguinte ordem:

1. **Arquivos compartilhados** (desenvolvimento local)
2. **Arquivos locais do projeto** (publicação)
3. **Variáveis de ambiente** (Azure App Service)
4. **Configuração padrão** (fallback)

## Deploy Step-by-Step

### 1. Preparação
```bash
# Navegue até o diretório da aplicação web
cd PerfumeMonitor.Web

# Compile o projeto para verificar se está tudo OK
dotnet build
```

### 2. Publicação via Visual Studio
1. Clique com botão direito no projeto `PerfumeMonitor.Web`
2. Selecione **Publish**
3. Escolha **Azure App Service**
4. Configure sua assinatura e grupo de recursos
5. Clique em **Publish**

### 3. Publicação via CLI
```bash
# Publique para Azure
dotnet publish -c Release
az webapp up --name seu-app-name --resource-group seu-resource-group
```

### 4. Configurar Variáveis de Ambiente (Recomendado)
No Azure Portal, configure as variáveis de ambiente conforme descrito acima.

## Verificação do Deploy

Após o deploy, verifique:

1. **Logs da aplicação**: Procure por mensagens como:
   - "ConfigManager usando config.json em: [caminho]"
   - "Credenciais do Twilio carregadas do arquivo/das variáveis de ambiente"

2. **Funcionalidade**: Acesse `/status` para ver o dashboard de monitoramento

3. **Background Service**: Verifique se o serviço está rodando e fazendo verificações automáticas

## Troubleshooting

### Problema: Arquivos de configuração não encontrados
**Solução**: Configure as variáveis de ambiente no Azure App Service

### Problema: Credenciais do Twilio inválidas
**Solução**: Verifique se as variáveis de ambiente estão configuradas corretamente

### Problema: Background service não está funcionando
**Solução**: Verifique os logs da aplicação para identificar possíveis erros

## Segurança

- **Nunca commite credenciais reais** nos arquivos de configuração
- **Use sempre variáveis de ambiente** em produção
- **Mantenha as credenciais do Twilio seguras**
- **Configure HTTPS** no Azure App Service 