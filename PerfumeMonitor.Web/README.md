# PerfumeMonitor.Web

Aplicação web ASP.NET Core para monitoramento de perfumes com background service.

## Funcionalidades

- **Background Service**: Monitoramento contínuo e automático dos perfumes
- **API REST**: Gerenciamento de perfumes e configurações
- **Interface Web**: Visualização em tempo real do status dos perfumes
- **Notificações WhatsApp**: Integração com Twilio para notificações
- **Configuração Compartilhada**: Compartilha configurações com a aplicação Windows Forms

## Como Executar

### Pré-requisitos
- .NET 8.0 ou superior
- Arquivo `twilio-credentials.json` configurado (se desejar notificações WhatsApp)

### Executar a Aplicação

```bash
cd PerfumeMonitor.Web
dotnet run
```

A aplicação estará disponível em:
- Interface Web: http://localhost:5000
- API: http://localhost:5000/api

### Executar em Produção

```bash
dotnet publish -c Release
cd bin/Release/net8.0/publish
dotnet PerfumeMonitor.Web.dll
```

## API Endpoints

### Perfumes
- `GET /api/perfumes` - Listar todos os perfumes
- `POST /api/perfumes` - Adicionar novo perfume
- `DELETE /api/perfumes/{index}` - Remover perfume
- `POST /api/perfumes/{index}/check` - Verificar perfume manualmente

### Configurações
- `GET /api/config` - Obter configurações atuais
- `PUT /api/config/interval` - Atualizar intervalo de verificação
- `PUT /api/config/whatsapp` - Atualizar configurações WhatsApp
- `GET /api/config/status` - Status do sistema

## Configuração

### Arquivo config.json
```json
{
  "PerfumeUrls": [
    {
      "Name": "Nome do Perfume",
      "Url": "https://exemplo.com/perfume",
      "IsAvailable": false,
      "LastChecked": "2024-01-01T00:00:00",
      "LastStatus": "Não verificado",
      "LastWhatsAppNotification": "0001-01-01T00:00:00"
    }
  ],
  "CheckIntervalMinutes": 1,
  "WhatsApp": {
    "IsEnabled": true,
    "MaxDailyNotifications": 10,
    "CooldownMinutes": 30,
    "ReduceNightTimeChecks": true
  }
}
```

### Arquivo twilio-credentials.json
```json
{
  "AccountSid": "seu_account_sid",
  "AuthToken": "seu_auth_token",
  "FromPhoneNumber": "+14155238886",
  "ToPhoneNumber": "+5511999999999"
}
```

## Logs

A aplicação registra todas as atividades:
- Verificações de perfumes
- Notificações enviadas
- Erros e exceções
- Status das operações

Os logs são exibidos no console durante a execução.

## Integração com Windows Forms

A aplicação web compartilha os mesmos arquivos de configuração (`config.json` e `twilio-credentials.json`) com a aplicação Windows Forms, permitindo que ambas funcionem em paralelo ou alternadamente.

## Recursos do Background Service

- **Monitoramento Contínuo**: Verifica perfumes automaticamente no intervalo configurado
- **Verificação Paralela**: Verifica múltiplos perfumes simultaneamente para maior eficiência
- **Notificações Inteligentes**: Respeita cooldown e configurações de horário noturno
- **Recuperação de Erros**: Continua funcionando mesmo com falhas em perfumes específicos
- **Logs Detalhados**: Registra todas as atividades para acompanhamento

## Interface Web

A interface web oferece:
- **Dashboard em Tempo Real**: Status atualizado automaticamente
- **Cards Visuais**: Cores diferentes para disponível/esgotado/erro
- **Informações do Sistema**: Status das configurações e monitoramento
- **Design Responsivo**: Funciona bem em desktop e mobile
- **Atualização Automática**: Recarrega dados a cada 30 segundos 