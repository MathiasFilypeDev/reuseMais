# <h1 align="center"> REUSE MAIS + 🌐♻️</h1>
<h1 align="center">O site, um projeto, um propósito, vários beneficiados. Paixão que move multidões.</h1>

###
<h3>Linguagens e ferramentas</h3>
<div align="center">
  <img src="https://cdn.jsdelivr.net/gh/devicons/devicon/icons/css3/css3-original.svg" height="60" alt="css logo"  />
  <img width="12" />
  <img src="https://cdn.jsdelivr.net/gh/devicons/devicon/icons/javascript/javascript-original.svg" height="60" alt="javascript logo"  />
  <img width="12" />
  <img src="https://skillicons.dev/icons?i=git" height="60" alt="git logo"  />
  <img width="12" />
  <img src="https://skillicons.dev/icons?i=github" height="60" alt="github logo"  />
  <img width="12" />
  <img src="https://cdn.jsdelivr.net/gh/devicons/devicon/icons/html5/html5-original.svg" height="60" alt="html5 logo"  />
  <img width="12" />
  <img src="https://cdn.jsdelivr.net/gh/devicons/devicon/icons/csharp/csharp-original.svg" height="60" alt="csharp logo"  />
  <img width="12" />
  <img src="https://cdn.jsdelivr.net/gh/devicons/devicon/icons/dotnetcore/dotnetcore-original.svg" height="60" alt="dotnetcore logo"  />
  <img width="12" />
  <img src="https://cdn.jsdelivr.net/gh/devicons/devicon/icons/postgresql/postgresql-original.svg" height="60" alt="postgresql logo"  />
</div>

## 🚀 Visão Geral
O **ReusePlus** é um sistema de gestão de estoque voltado para reaproveitamento e sustentabilidade.  
Ele permite:
- Cadastro de usuários com autenticação segura (JWT + BCrypt).  
- Login tradicional (email/senha).  
- Cadastro de itens no estoque.  
- Registro de movimentações (entradas e saídas).  
- Relatórios detalhados com estatísticas e gráficos (Chart.js).  

---

## 🛠️ Tecnologias Utilizadas
- **Backend:** ASP.NET Core 8 + Entity Framework Core  
- **Banco de Dados:** PostgreSQL  
- **Frontend:** HTML + CSS + JavaScript  
- **Autenticação:** JWT + Google Identity (OAuth 2.0)  
- **Gráficos:** Chart.js  

---

## 📂 Estrutura do Projeto

### Backend (`ReusePlusApi`)

```bash
📂 ReusePlusApi
 ├── 📂 bin
 ├── 📂 Controllers
 │    ├── 📄 UsuarioController.cs
 │    ├── 📄 ItemController.cs
 │    ├── 📄 MovimentacaoController.cs
 │    └── 📄 RelatorioController.cs
 ├── 📂 Data
 │    └── 📄 AppDbContext.cs
 ├── 📂 obj
 ├── 📂 Properties
 ├── 📄 .editorconfig
 ├── 📄 appsettings.Development.json
 ├── 📄 appsettings.json
 ├── 📄 Program.cs
 ├── 📄 ReusePlusApi.csproj
 └── 📄 ReusePlusApi.http
```

### Frontend (`Frontend`)

```
📂 Frontend
 ├── 📂 assets
 ├── 📂 css
 ├── 📂 Js
 │    ├── 📄 about.html
 │    ├── 📄 admin.html
 │    ├── 📄 cadastro.html
 │    ├── 📄 cadastroitens.html
 │    ├── 📄 index.html
 │    ├── 📄 login.html
 │    ├── 📄 principal.html
 │    └── 📄 recebimento.html
```
