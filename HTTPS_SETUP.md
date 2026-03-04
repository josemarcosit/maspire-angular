# HTTPS Local - Guia de Configuração

## O que foi feito

A aplicação já está 100% configurada para HTTPS local em desenvolvimento. Aqui está o que foi implementado:

### Backend (.NET)
- Porta HTTPS: `https://localhost:7257`
- Certificado auto-assinado gerado e confiável no Windows
- Arquivo: [launchSettings.json](Properties/launchSettings.json)

### Frontend (Angular)
- Arquivo de configuração centralizado: [environment.ts](ClientApp/src/app/shared/config/environment.ts)
- Todos os serviços (Auth e Vehicle) usam HTTPS
- Proxy Angular configurado para HTTPS
- Arquivo: [proxy.conf.js](ClientApp/proxy.conf.js)

## Como usar

### 1. Iniciar o Backend
```bash
cd h:\Github\angular-vega
dotnet run
```
A aplicação abrirá em `https://localhost:7257`

### 2. Iniciar o Frontend (desenvolvimento)
```bash
cd ClientApp
npm start
```
Angular rodará em `http://localhost:4200` e fará proxy das requisições `/api/*` para `https://localhost:7257`

## Segurança - HTTPS

O certificado de desenvolvimento foi gerado com:
```bash
dotnet dev-certs https --trust
```

Este certificado:
- É auto-assinado e confiável no Windows
- É válido apenas para `localhost`
- Expira após 1 ano
- Está armazenado no Certificate Store do Windows

## Configurações HTTPS

Se precisar regenerar o certificado:
```bash
# Remover certificado antigo
dotnet dev-certs https --clean

# Gerar novo certificado e confiar
dotnet dev-certs https --trust

# Verificar se está funcionando
dotnet dev-certs https --check --trust
```

## Ambientes

### Desenvolvimento (local)
```typescript
// ClientApp/src/app/shared/config/environment.ts
export const environment = {
  apiUrl: 'https://localhost:7257',
  production: false
};
```

### Produção
Para produção, altere o arquivo `environment.ts`:
```typescript
export const environment = {
  apiUrl: 'https://seu-dominio.com', // seu domínio
  production: true
};
```

## URLs Base

Todos os serviços usam a configuração centralizada:

- **Auth Service**: `${environment.apiUrl}/api/auth`
- **Vehicle Service**: `${environment.apiUrl}/api/vehicles`
- **Other APIs**: Adicione novos serviços com `${environment.apiUrl}/api/...`

## Troubleshooting

### Erro: "The remote certificate is invalid"
**Solução**: Regenere o certificado:
```bash
dotnet dev-certs https --clean
dotnet dev-certs https --trust
```

### Certificado não é confiável
**Solução**: Execute como admin e rode:
```bash
dotnet dev-certs https --trust
```

### CORS Error com HTTPS
Verifique se `launchSettings.json` tem a porta HTTPS correta (7257)

## Arquivo de Configuração

**Localização**: `ClientApp/src/app/shared/config/environment.ts`

Use este arquivo para:
- Centralizar URLs de API
- Alternar entre dev e produção
- Adicionar outras configurações globais

## Resultado

Agora sua aplicação:
- Usa HTTPS em desenvolvimento local
- Tem certificado confiável (sem avisos)
- Está pronta para HTTPS em produção
- Segue boas práticas de segurança

