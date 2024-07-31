# BurgerRoyale.Orders

Esta solução foi desenvolvida como uma API a ser utilizada na gestão de produtos e pedidos de uma lanchonete.

## Tecnologias Utilizadas

- **Linguagem:** C#
- **Framework:** ASP.NET Core
- **Banco de Dados:** SQL Server

## Configuração do Ambiente

Para configurar o ambiente de desenvolvimento, siga os passos abaixo:

1. **Clone o Repositório:**

   ```bash
   git clone https://github.com/FiapSoftwareArquitectureTechChallange/BurgerRoyale.Orders.git
   ```

2. **Navegue até o Diretório do Projeto:**

   ```bash
   cd BurgerRoyale.Orders
   ```

3. **Restaurar Dependências:**

   Utilize o `dotnet` CLI para restaurar as dependências do projeto:

   ```bash
   dotnet restore
   ```

4. **Configurar Variáveis de Ambiente:**

   Crie um arquivo `appsettings.Development.json` na raiz do projeto e adicione as variáveis de ambiente necessárias. Consulte o arquivo `appsettings.json` para ver exemplos de configuração.

## Uso

Para iniciar o serviço, use o seguinte comando:

```bash
dotnet run
```

O serviço estará disponível em `http://localhost:5000` (ou a porta configurada).

## Kubernetes

Este projeto inclui arquivos de configuração para Kubernetes localizados na pasta `Kubernetes`. Siga os passos abaixo para aplicar essas configurações em um cluster Kubernetes local:

1. **Acesse a Pasta Kubernetes:**

   Navegue até o diretório que contém os arquivos de configuração do Kubernetes:

   ```bash
   cd Kubernetes
   ```

2. **Verifique os Arquivos de Configuração:**

   Certifique-se de que as configurações no arquivo `api-deployment.yaml` estão corretas e ajustadas conforme suas necessidades.

3. **Aplicar Configurações no Cluster Kubernetes:**

   Use o comando `kubectl apply` para aplicar as configurações:

   ```bash
   kubectl apply -f api-deployment.yaml
   kubectl apply -f api-svc.yaml
   ```

4. **Verificar o Status dos Pods e Serviços:**

   Após aplicar as configurações, verifique o status dos pods e serviços para garantir que tudo está funcionando conforme o esperado:

   ```bash
   kubectl get pods
   kubectl get services
   ```

5. **Acessar o Serviço:**

   Dependendo da configuração do serviço, você pode precisar expor o serviço ou usar um LoadBalancer. Certifique-se de seguir as instruções específicas de acesso para seu ambiente Kubernetes.

   - Para obter o endereço IP do serviço, use:

     ```bash
     kubectl get svc
     ```
