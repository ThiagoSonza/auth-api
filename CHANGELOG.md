# Changelog

Todas as alterações notáveis deste projeto serão documentadas neste arquivo.

## [4.0.0] - 2025-08-20

- Created Worker project to handle email notifications for user actions.
- Implemented consumers for Forgot Password, Register User, and Resend Confirmation emails.
- Added email sending functionality using SMTP with EmailSender service.
- Introduced EmailTemplateRenderer and EmailTemplateRendererBuilder for rendering email templates.
- Updated Api project to include new Worker project references and configurations.
- Enhanced application configuration for messaging settings.
- Updated application dependencies to support new features and improvements.

## [3.0.0] - 2025-08-18

- Implemented ConfirmMultiFactorTelemetry to track code validation and user status.
- Added DisableMultiFactorTelemetry for tracking the disabling of two-factor authentication.
- Created EnableMultiFactorTelemetry to log enabling of two-factor authentication.
- Developed GenerateQrCodeTelemetry to monitor QR code generation events.
- Introduced GetStatusTelemetry to track the status of two-factor authentication for users.
- Added ForgotPasswordTelemetry to log events related to password reset requests.
- Implemented ResetPasswordTelemetry to track password reset actions.
- Created CreateRoleTelemetry to log role creation and failure events.
- Added DeleteRoleTelemetry to monitor role deletion actions.
- Developed GetRoleByNameTelemetry to track retrieval of roles by name.
- Introduced GetRolesTelemetry to log retrieval of all roles.
- Implemented GetUserInfoTelemetry to track user information retrieval.
- Added ManageUserInfoTelemetry to log user information updates.
- Created RegisterUserTelemetry to monitor user registration events.
- Updated AppDbContextFactory to support environment-specific configuration.

## [2.1.1] - 2025-08-08

- Atualizados os comandos **DisableMultiFactor** e **EnableMultiFactor** para retornarem resultados detalhados.
- Aprimorados os endpoints **DisableMultiFactor** e **EnableMultiFactor** com melhor tratamento de respostas e documentação OpenAPI.
- Modificados o comando e o endpoint **GenerateQrCode** para retornarem respostas estruturadas.
- Refinados o comando e o endpoint **GetStatus** para retornarem resultados booleanos para o status de MFA.
- Ajustados o comando e o endpoint **ForgotPassword** para fornecerem respostas do tipo string.
- Revisados o comando e o endpoint **ResetPassword** para simplificar a lógica e as respostas de redefinição de senha.
- Criados o comando e o endpoint **CreateRole** para retornarem respostas estruturadas de criação de função (role).
- Implementados o comando e o endpoint **DeleteRole** para lidarem com exclusão de função (role) com respostas detalhadas.
- Aprimorados os endpoints **GetRoleByName** e **GetRoles** para retornarem dados específicos de funções (roles).
- Atualizados os endpoints **UserInfo** para retornarem informações estruturadas do usuário.
- Refatorados o comando e o endpoint **RegisterUser** para retornarem detalhes do registro de usuário.
- Introduzidas classes de resposta para gerenciamento de funções (roles) e usuários, a fim de padronizar as respostas da API.

## [2.1.0] - 2025-08-01

Segue a tradução do texto:

- Atualizados os arquivos **launchSettings.json** e **appsettings** para garantir consistência e clareza.
- Modificado o **Application.csproj** para incluir os pacotes necessários do **Entity Framework Core** para operações com banco de dados.
- Introduzido o **AppDbContextFactory** para criação de contexto de banco de dados em tempo de design.
- Criado o **UserDomainMapping** e ajustados os mapeamentos existentes para usar **UserDomain** em vez de **IdentityUser**.
- Adicionados arquivos de migração inicial para criação do esquema do banco de dados.
- Garantido que todos os mapeamentos estejam configurados corretamente para a nova entidade **UserDomain**.

## [2.0.0] - 2025-07-31

-

## [1.0.0] - 2025-07-31

- Configuração inicial da integração com ASP.NET Identity
- Adicionada funcionalidade de registro e login de usuários
- Implementada autorização baseada em papéis
- Adicionados recursos de redefinição de senha e confirmação de e-mail
- Melhorias no tratamento de erros e validação

---

> Este changelog segue o padrão do [Keep a Changelog](https://keepachangelog.com/pt-BR/1.0.0/) e adere ao [Versionamento Semântico](https://semver.org/lang/pt-BR/).
