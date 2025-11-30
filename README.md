Baixar o .Net 9.0: https://dotnet.microsoft.com/pt-br/download/dotnet/thank-you/sdk-9.0.308-windows-x64-installer

Para rodar o sistema deve acessar o backend 
cd "Av2 Backends"
cd api e rodar o comando > dotnet build após isso se não ocorrer nenhum erro rodar dotnet run


para o front end deve se baixar o node.js : https://nodejs.org/pt/download
O node utilizado foi o v24.11.1

cd "Av2 Frontend"

rodar o comando npm install (caso o npm não esteja instalado)
ao rodar o comando npm --version
deve dar o resultado: 11.6.2


e na pasta do front rodar npm run dev, acessar o link: http://localhost:5173/
cada usuário tem um carrinho diferente


Para a criação de produtos http://localhost:5259/swagger
os produtos são armazenados na pasta DatabaseJson dentro de Av2 Backend
