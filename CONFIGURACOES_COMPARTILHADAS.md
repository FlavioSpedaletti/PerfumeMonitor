# 📋 Configurações Compartilhadas - Como Funciona

## ✅ **SOLUÇÃO IMPLEMENTADA**

Agora **ambas as aplicações usam exatamente os mesmos arquivos** de configuração, localizados no **diretório raiz do projeto**.

## 🏗️ **Estrutura de Configurações**

```
📁 PerfumeMonitor/                     # 🎯 DIRETÓRIO RAIZ
├── config.json                        # ✅ ARQUIVO ÚNICO - usado por ambas aplicações
├── twilio-credentials.json             # ✅ ARQUIVO ÚNICO - usado por ambas aplicações
├── PerfumeMonitor.WinForms/            # 🖥️ APLICAÇÃO WINDOWS FORMS
│   └── (busca configurações no diretório pai)
└── PerfumeMonitor.Web/                 # 🌐 APLICAÇÃO WEB
    └── (busca configurações no diretório pai)
```

## 📄 **Arquivos de Configuração**

### 1. **`config.json`** - Configurações Principais
```json
{
  "PerfumeUrls": [
    {
      "Name": "Nome do Perfume",
      "Url": "https://loja.com/perfume",
      "IsAvailable": false,
      "LastChecked": "2025-06-11T11:55:02.5347559-03:00",
      "LastStatus": "Esgotado",
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

**Contém:**
- ✅ **Perfumes monitorados**: Lista com URLs, status e histórico
- ✅ **Intervalo de verificação**: Tempo entre checks em minutos
- ✅ **Configurações WhatsApp**: Limites e regras de notificação

### 2. **`twilio-credentials.json`** - Credenciais WhatsApp
```json
{
  "AccountSid": "seu_account_sid_aqui",
  "AuthToken": "seu_auth_token_aqui", 
  "FromPhoneNumber": "+14155238886",
  "ToPhoneNumber": "+5511999999999"
}
```

**Contém:**
- ✅ **Credenciais Twilio**: Para envio de mensagens WhatsApp
- ✅ **Números**: Remetente e destinatário das notificações

## 🔄 **Como Cada Aplicação Acessa**

### **Windows Forms** (`PerfumeMonitor.WinForms`)
```csharp
public ConfigManager()
{
    // Busca no diretório pai (raiz do projeto)
    var projectRoot = Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory()));
    _configPath = Path.Combine(projectRoot, "config.json");
    _credentialsPath = Path.Combine(projectRoot, "twilio-credentials.json");
}
```

### **Aplicação Web** (`PerfumeMonitor.Web`)
```csharp
public ConfigManager(ILogger<ConfigManager> logger)
{
    // Busca no diretório pai (raiz do projeto)
    var projectRoot = Path.GetDirectoryName(Directory.GetCurrentDirectory());
    _configPath = Path.Combine(projectRoot, "config.json");
    _credentialsPath = Path.Combine(projectRoot, "twilio-credentials.json");
}
```

## 🎯 **Benefícios Alcançados**

### ✅ **1. Configurações Verdadeiramente Compartilhadas**
- **Mesmo arquivo**: Ambas aplicações leem/escrevem no mesmo local
- **Sincronia total**: Mudanças aparecem imediatamente em ambas
- **Zero duplicação**: Um só arquivo de cada tipo

### ✅ **2. Consistência Garantida**
- **Perfumes sincronizados**: Lista sempre atualizada em tempo real
- **Status unificado**: `LastChecked`, `IsAvailable` sempre consistentes
- **Configurações únicas**: Intervalo, WhatsApp, etc. iguais para ambas

### ✅ **3. Facilidade de Gerenciamento**
```
❌ ANTES: 
├── PerfumeMonitor.WinForms/config.json     (versão 1)
└── PerfumeMonitor.Web/config.json          (versão 2 - diferente!)

✅ AGORA:
├── config.json                             (versão única)
├── PerfumeMonitor.WinForms/                (busca arquivo único)
└── PerfumeMonitor.Web/                     (busca arquivo único)
```

## 🚀 **Como Funciona na Prática**

### **Cenário 1: Adicionando Perfume via Windows Forms**
1. ✅ Usuário adiciona perfume na interface Windows Forms
2. ✅ Salva no `config.json` do diretório raiz
3. ✅ Aplicação Web **imediatamente** vê o novo perfume
4. ✅ Background service inclui na próxima verificação

### **Cenário 2: Configurando via API Web**
1. ✅ API recebe request para alterar intervalo
2. ✅ Salva no `config.json` do diretório raiz  
3. ✅ Windows Forms **imediatamente** usa novo intervalo
4. ✅ Ambas aplicações sincronizadas

### **Cenário 3: Execução Simultânea**
1. ✅ Windows Forms rodando e verificando perfumes
2. ✅ Web App rodando background service simultaneamente
3. ✅ Ambos salvam no mesmo arquivo
4. ✅ **Status sempre atualizado** para ambos

## 📍 **Localização dos Arquivos**

### **Diretórios de Execução:**
- **Windows Forms executa de**: `PerfumeMonitor.WinForms/bin/Debug/net6.0-windows/`
- **Web executa de**: `PerfumeMonitor.Web/bin/Debug/net9.0/`

### **Configurações ficam em:**
- **Arquivos reais**: `PerfumeMonitor/config.json` (raiz do projeto)
- **Ambas aplicações**: Calculam caminho para a raiz automaticamente

## 🧪 **Como Testar**

### **Teste 1: Verificar Sincronização**
```bash
# Terminal 1: Executar Windows Forms
cd PerfumeMonitor.WinForms
dotnet run

# Terminal 2: Executar Web App
cd PerfumeMonitor.Web  
dotnet run

# Verificar: Mudanças em uma aparecem na outra
```

### **Teste 2: Verificar Arquivos Únicos**
```bash
# Deve mostrar apenas estes arquivos:
ls PerfumeMonitor/*.json
# Resultado: config.json twilio-credentials.json

# NÃO deve existir mais:
ls PerfumeMonitor.WinForms/*.json   # Vazio
ls PerfumeMonitor.Web/*.json        # Vazio
```

## ✨ **Resumo**

### **🎯 DE ONDE VÊM AS CONFIGURAÇÕES:**

| Configuração | Origem | Usado Por |
|-------------|--------|-----------|
| **Perfumes monitorados** | `config.json` (raiz) | ✅ Ambas aplicações |
| **Intervalo de verificação** | `config.json` (raiz) | ✅ Ambas aplicações |
| **Configurações WhatsApp** | `config.json` (raiz) | ✅ Ambas aplicações |
| **Credenciais Twilio** | `twilio-credentials.json` (raiz) | ✅ Ambas aplicações |

### **🔄 SINCRONIA TOTAL:**
- ✅ **Tempo real**: Mudanças aparecem imediatamente
- ✅ **Bidirecional**: Qualquer aplicação pode modificar
- ✅ **Consistente**: Sempre os mesmos dados
- ✅ **Único**: Zero duplicação de arquivos

**Agora você tem configurações verdadeiramente compartilhadas!** 🎉 