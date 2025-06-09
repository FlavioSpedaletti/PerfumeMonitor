# Configuração WhatsApp - Monitor de Perfumes

## Como configurar notificações WhatsApp

Para receber notificações no WhatsApp quando os perfumes estiverem disponíveis, você precisa configurar uma conta no Twilio.

### Passo 1: Criar conta no Twilio

1. Acesse: https://console.twilio.com/
2. Crie uma conta gratuita
3. Verifique seu número de telefone

### Passo 2: Configurar WhatsApp Sandbox

1. No console do Twilio, vá em **Messaging** → **Try it out** → **Send a WhatsApp message**
2. Siga as instruções para conectar seu WhatsApp ao sandbox
3. Envie a mensagem de código para o número do Twilio conforme instruído

### Passo 3: Obter credenciais

No console do Twilio, anote:
- **Account SID**: Encontrado na página principal do console
- **Auth Token**: Clique em "Show" ao lado do Account SID
- **From Number**: Número do WhatsApp Sandbox (ex: +14155238886)
- **To Number**: Seu número de WhatsApp com código do país (ex: +5511999999999)

### Passo 4: Configurar na aplicação

1. Clique com botão direito no ícone do Monitor de Perfumes na bandeja
2. Selecione **"WhatsApp"**
3. Marque **"Habilitar notificações WhatsApp"**
4. Preencha os campos com as credenciais obtidas
5. Clique em **"Salvar"**

### Teste

Para testar se está funcionando:
1. Force uma verificação clicando em "Verificar Agora"
2. Se algum perfume estiver disponível, você receberá uma mensagem no WhatsApp

### Observações importantes

- A conta gratuita do Twilio tem limitações de mensagens
- O WhatsApp Sandbox só funciona com números previamente autorizados
- Para uso em produção, seria necessário aprovar um template de mensagem no Twilio

### Solução de problemas

- Verifique se o número está no formato internacional (+5511999999999)
- Confirme que o WhatsApp Sandbox está ativo
- Verifique as credenciais no console do Twilio 