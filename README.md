# Monitor de Perfumes

Solução modular para monitoramento de produtos com múltiplas aplicações.

## 📁 Estrutura do Projeto

```
PerfumeMonitor/
├── PerfumeMonitor.WinForms/    # Aplicação Windows Forms (atual)
│   ├── Forms/                  # Formulários da aplicação
│   ├── Services/              # Serviços (checagem, notificações)
│   ├── Models/                # Modelos de dados
│   ├── config.json            # Configurações da aplicação
│   └── Program.cs             # Ponto de entrada
├── docs/                      # Documentação (futuros projetos)
└── README.md                  # Este arquivo
```

## 🎯 Aplicações Disponíveis

### PerfumeMonitor.WinForms
Aplicação Windows Forms que monitora automaticamente a disponibilidade de perfumes em sites específicos com notificações WhatsApp.

### Projetos Futuros
- **PerfumeMonitor.WebApi**: API REST para monitoramento via web
- **PerfumeMonitor.Service**: Serviço Windows para execução em background
- **PerfumeMonitor.Web**: Interface web para gerenciamento remoto

## 🚀 Funcionalidades

### Core
- ✅ Monitoramento automático a cada 5 minutos
- ✅ Execução em background (system tray)
- ✅ Interface para gerenciar URLs de produtos
- ✅ Histórico de verificações
- ✅ Persistência de configurações

### Notificações
- ✅ **Notificações WhatsApp** via Twilio
- ✅ Notificações do sistema (balões)
- ✅ Limite diário de notificações WhatsApp (economia de custos)
- ✅ Cooldown entre notificações do mesmo produto
- ✅ Redução noturna de verificações (1h-6h)

### Segurança
- ✅ **Gerenciamento seguro de credenciais** (arquivo separado)
- ✅ Credenciais não incluídas no controle de versão
- ✅ Arquivo de exemplo para configuração

## 🔧 Como usar

### PerfumeMonitor.WinForms
1. **Compilação**: Abra `PerfumeMonitor.sln` no Visual Studio ou compile via CLI
2. **Execução**: Ao iniciar, a aplicação vai para o system tray (ícone na bandeja)
3. **URLs**: Clique com botão direito → "Configurações" para gerenciar produtos
4. **WhatsApp**: Clique com botão direito → "WhatsApp" para configurar notificações

### Configuração WhatsApp (Opcional)
Para receber notificações no WhatsApp:
1. Configure uma conta no Twilio (gratuita)
2. Configure as credenciais na aplicação
3. Consulte `WHATSAPP_SETUP.md` para instruções detalhadas

### Monitoramento
- ✅ Verificação automática a cada 5 minutos
- ✅ Clique em "Verificar Agora" para teste imediato
- ✅ Status visível na interface principal

## ⚙️ Configuração de Produtos

- URLs padrão já vêm configuradas para perfumes Stella Dustin
- Adicione novos produtos através do menu "Configurações"
- As configurações são salvas automaticamente em `config.json`

## 🎯 Detecção de Disponibilidade

A aplicação detecta disponibilidade através de:
- Ausência de botões "ESGOTADO"
- Presença de campos de quantidade
- Presença de botões de compra ativos

## 💰 Otimizações de Custo WhatsApp

- **Limite diário**: Máximo de 10 notificações WhatsApp por dia
- **Cooldown**: 30 minutos entre notificações do mesmo produto
- **Redução noturna**: Menos verificações durante madrugada
- **Economia**: ~85% de redução no consumo de créditos
- Consulte `OTIMIZACOES_WHATSAPP.md` para detalhes completos

## 🔐 Gerenciamento de Credenciais

### Configuração segura
1. Copie `twilio-credentials.example.json` para `twilio-credentials.json`
2. Preencha com suas credenciais reais do Twilio
3. O arquivo real não será incluído no Git (configurado no .gitignore)

### Estrutura das credenciais
```json
{
  "AccountSid": "Seu Account SID do Twilio",
  "AuthToken": "Seu Auth Token do Twilio", 
  "FromNumber": "Número WhatsApp Sandbox",
  "ToNumber": "Seu número com código do país"
}
```

## 📋 Requisitos

- Windows 10/11
- .NET 6.0 ou superior
- Conta Twilio (para notificações WhatsApp - opcional)

## 🧪 Como testar

1. Execute a aplicação
2. Configure URLs através do menu "Configurações"
3. Configure WhatsApp (opcional) através do menu "WhatsApp"
4. Clique em "Verificar Agora" para testar imediatamente
5. Veja o status na lista principal

## 📚 Documentação adicional

- `WHATSAPP_SETUP.md` - Configuração completa do WhatsApp
- `OTIMIZACOES_WHATSAPP.md` - Detalhes das otimizações de custo
- `twilio-credentials.example.json` - Exemplo de configuração de credenciais 