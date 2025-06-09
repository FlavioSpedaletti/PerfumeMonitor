# Monitor de Perfumes

Aplicação Windows Forms que monitora automaticamente a disponibilidade de perfumes em sites específicos.

## Funcionalidades

- ✅ Monitoramento automático a cada 5 minutos
- ✅ Execução em background (system tray)
- ✅ Notificações quando perfumes ficam disponíveis
- ✅ Interface para gerenciar URLs de produtos
- ✅ Histórico de verificações
- ✅ Persistência de configurações

## Como usar

1. **Execução**: Ao iniciar, a aplicação vai para o system tray (ícone na bandeja)
2. **Configuração**: Clique com botão direito no ícone → "Configurações" para gerenciar URLs
3. **Monitoramento**: O sistema verifica automaticamente a cada 5 minutos
4. **Notificações**: Balões de notificação aparecem quando produtos ficam disponíveis

## Configuração

- URLs padrão já vêm configuradas para os perfumes Stella Dustin
- Adicione novos produtos através do menu "Configurações"
- As configurações são salvas automaticamente em `config.json`

## Detecção de disponibilidade

A aplicação detecta disponibilidade através de:
- Ausência de botões "ESGOTADO"
- Presença de campos de quantidade
- Presença de botões de compra ativos

## Requisitos

- Windows 10/11
- .NET 6.0 ou superior

## Como testar

1. Execute a aplicação
2. Clique em "Verificar Agora" para testar imediatamente
3. Veja o status na lista principal
4. Configure seus próprios perfumes através do menu "Configurações" 